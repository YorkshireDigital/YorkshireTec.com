namespace YorkshireTec.Api.Infrastructure
{
    using System;
    using System.Configuration;
    using System.Text.RegularExpressions;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.SimpleAuthentication;
    using NHibernate;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.NHibernate;
    using YorkshireDigital.Data.Services;
    using YorkshireTec.Api.Infrastructure.Helpers;

    public class AuthenticationCallbackProvider : IAuthenticationCallbackProvider
    {
        private readonly UserService userService;

        public AuthenticationCallbackProvider()
        {
            ISessionFactory sessionFactory;

            var inMemoryDb = bool.Parse(ConfigurationManager.AppSettings["Database:InMemory"]);
            if (inMemoryDb)
            {
                NHibernate.Cfg.Configuration configuration;
                sessionFactory = NHibernateSessionFactoryProvider.BuildInMemorySessionFactory(out configuration);
            }
            else
            {
                sessionFactory = NHibernateSessionFactoryProvider.BuildSessionFactory(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
            }
            
            var requestSession = sessionFactory.OpenSession();
            userService = new UserService(requestSession);
        }

        public dynamic Process(NancyModule nancyModule, AuthenticateCallbackData model)
        {
            User loggedInUser = null;
            var returnUrl = GetReturnUrl(model.ReturnUrl);

            if (nancyModule.Context.CurrentUser != null)
            {
                loggedInUser = userService.GetUser(nancyModule.Context.CurrentUser.UserName);
            }
            if (model.Exception == null)
            {
                var authenticatedClient = model.AuthenticatedClient;
                var userInfo = authenticatedClient.UserInformation;
                var providerName = model.AuthenticatedClient.ProviderName;

                var user = userService.GetUserByIdentity(providerName, userInfo.UserName);

                // Are they already logged in?
                if (loggedInUser != null)
                {
                    // Has this account signed up before?
                    if (user == null)
                    {
                        // No - Link the accounts
                        userService.LinkIdentity(Provider.FromAuthenticatedClient(authenticatedClient), loggedInUser);
                    }
                    // Redirect back to the account page
                    return nancyModule.Response.AsRedirect("~/account");
                }
                // No one is logged in
                // But already exist
                if (user != null)
                {
                    // Just log them in
                    return nancyModule.LoginAndRedirect(user.Id, null, returnUrl);
                }
                // We've not seen this user before!
                // Has the email signed up with another account?
                if (userService.EmailAlreadyRegistered(userInfo.Email))
                {
                    // Link the accounts
                    var existingUser = userService.GetUser(userInfo.Email);
                    userService.LinkIdentity(Provider.FromAuthenticatedClient(authenticatedClient), existingUser);
                    // Log them in
                    return nancyModule.LoginAndRedirect(existingUser.Id, null, returnUrl);
                }
                // New user!! HOZZAAARRR!!!
                // Register them
                var newUser = userService.SaveUser(User.FromAuthenticatedClient(authenticatedClient));
                var updateText = string.Format("{0} just signed up at {1}. Go {0}!", newUser.Name, nancyModule.Context.Request.Url.SiteBase);
                SlackHelper.PostToSlack(new SlackUpdate { channel = "#website", icon_emoji = ":yorks:", username = "New User", text = updateText });
                // Log them in and forward them to the welcome page
                return nancyModule.LoginAndRedirect(newUser.Id, null, "~/account/welcome");
            }
            return 500;
        }

        public static string GetReturnUrl(string uriString)
        {
            if (!Uri.IsWellFormedUriString(uriString, UriKind.Absolute))
            {
                return "~/";
            }
            var matches = Regex.Matches(new Uri(uriString).Query, "returnUrl=([^;]+)");
            return matches.Count > 0 ? string.Format("~/{0}", matches[0].Groups[1].Value.TrimStart('/')) : "~/";
        }

        public dynamic OnRedirectToAuthenticationProviderError(NancyModule nancyModule, string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
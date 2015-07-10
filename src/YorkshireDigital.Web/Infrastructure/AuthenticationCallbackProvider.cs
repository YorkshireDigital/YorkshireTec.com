namespace YorkshireDigital.Web.Infrastructure
{
    using System;
    using System.Configuration;
    using System.Text.RegularExpressions;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.SimpleAuthentication;
    using NHibernate;
    using SimpleAuthentication.Core;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.NHibernate;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Infrastructure.Helpers;

    public class AuthenticationCallbackProvider : IAuthenticationCallbackProvider
    {
        private readonly UserService userService;
        private readonly ISession requestSession;
        public AuthenticationCallbackProvider()
        {
            var sessionFactory = NHibernateSessionFactoryProvider.BuildSessionFactory(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
            requestSession = sessionFactory.OpenSession();
            
            userService = new UserService(requestSession);
        }

        public dynamic Process(NancyModule nancyModule, AuthenticateCallbackData model)
        {
            requestSession.BeginTransaction();
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

                // If they are already logged in then they're linking accounts on the account page
                if (loggedInUser != null)
                {
                    return LinkAccounts(nancyModule, user, authenticatedClient, loggedInUser);
                }

                // User is already registered
                if (user != null)
                {
                    return nancyModule.LoginAndRedirect(user.Id, null, returnUrl);
                }

                // New user. Have we seen this email before?
                if (userService.EmailAlreadyRegistered(userInfo.Email))
                {
                    // Link the accounts
                    var existingUser = userService.GetUser(userInfo.Email);
                    userService.LinkIdentity(Provider.FromAuthenticatedClient(authenticatedClient), existingUser);
                    requestSession.Transaction.Commit();
                    return nancyModule.LoginAndRedirect(existingUser.Id, null, returnUrl);
                }

                // New user
                var newUser = userService.SaveUser(User.FromAuthenticatedClient(authenticatedClient));
                SlackHelper.PostNewUserUpdate(newUser.Username, newUser.Name, newUser.Email, false, nancyModule.Context.Request.Url.SiteBase);
                requestSession.Transaction.Commit();
                // Log them in and forward them to the welcome page
                return nancyModule.LoginAndRedirect(newUser.Id, null, "~/account/welcome");
            }
            return 500;
        }

        public dynamic OnRedirectToAuthenticationProviderError(NancyModule nancyModule, string errorMessage)
        {
            throw new NotImplementedException();
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

        private dynamic LinkAccounts(INancyModule nancyModule, User user, IAuthenticatedClient authenticatedClient,
            User loggedInUser)
        {
            // Has this account signed up before?
            if (user == null)
            {
                // No - Link the accounts
                userService.LinkIdentity(Provider.FromAuthenticatedClient(authenticatedClient), loggedInUser);
                requestSession.Transaction.Commit();
            }
            // Redirect back to the account page
            return nancyModule.Response.AsRedirect("~/account");
        }
    }
}
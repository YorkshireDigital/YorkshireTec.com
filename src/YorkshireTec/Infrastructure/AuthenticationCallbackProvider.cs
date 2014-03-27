namespace YorkshireTec.Infrastructure
{
    using System.Text.RegularExpressions;
    using global::Raven.Client;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.SimpleAuthentication;
    using System;
    using YorkshireTec.Raven;
    using YorkshireTec.Raven.Domain.Account;
    using YorkshireTec.Raven.Repositories;

    public class AuthenticationCallbackProvider : IAuthenticationCallbackProvider
    {
        private readonly UserRepository userRepository;

        public AuthenticationCallbackProvider()
        {
            var store = RavenSessionProvider.DocumentStore;
            userRepository = new UserRepository(store.OpenSession());
        }

        public dynamic Process(NancyModule nancyModule, AuthenticateCallbackData model)
        {
            User loggedInUser = null;
            var returnUrl = GetReturnUrl(model.ReturnUrl);

            if (nancyModule.Context.CurrentUser != null)
            {
                loggedInUser = userRepository.GetUser(nancyModule.Context.CurrentUser.UserName);
            }
            if (model.Exception == null)
            {
                var authenticatedClient = model.AuthenticatedClient;
                var userInfo = authenticatedClient.UserInformation;
                var providerName = model.AuthenticatedClient.ProviderName;

                var user = userRepository.GetUserByIdentity(providerName, userInfo.UserName);

                // Are they already logged in?
                if (loggedInUser != null)
                {
                    // Has this account signed up before?
                    if (user == null)
                    {
                        // No - Link the accounts
                        userRepository.LinkIdentity(Provider.FromAuthenticatedClient(authenticatedClient), loggedInUser);
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
                if (userRepository.EmailAlreadyRegistered(userInfo.Email))
                {
                    // Link the accounts
                    var existingUser = userRepository.GetUser(userInfo.Email);
                    userRepository.LinkIdentity(Provider.FromAuthenticatedClient(authenticatedClient), existingUser);
                    // Log them in
                    return nancyModule.LoginAndRedirect(existingUser.Id, null, returnUrl);
                }
                // New user!! HOZZAAARRR!!!
                // Register them
                var newUser = userRepository.SaveUser(User.FromAuthenticatedClient(authenticatedClient));
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

        public IDocumentSession DocumentSessionProvider { get; set; }
    }
}
namespace YorkshireTec.Infrastructure
{
    using global::Raven.Client;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.SimpleAuthentication;
    using System;
    using SimpleAuthentication.Core.Exceptions;
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

            if (nancyModule.Context.CurrentUser != null)
            {
                loggedInUser = userRepository.GetUser(nancyModule.Context.CurrentUser.UserName);
            }
            if (model.Exception == null)
            {
                var authenticatedClient = model.AuthenticatedClient;
                var userInfo = authenticatedClient.UserInformation;
                var providerName = model.AuthenticatedClient.ProviderName;

                var user = userRepository.GetUserByIdentity(providerName, userInfo.Id);

                // User with that identity doesn't exist, check if a user is logged in
                if (user == null)
                {
                    if (loggedInUser != null)
                    {
                        // User is logged in so link the Identity
                        userRepository.LinkIdentity(Provider.FromAuthenticatedClient(authenticatedClient), loggedInUser);
                        // As the user is already logged in then they must be on the accounts page
                        return nancyModule.Response.AsRedirect("~/account/#identityProviders");
                    }
                    // No user is logged - check if they have registered before
                    if (userRepository.EmailAlreadyRegistered(userInfo.Email))
                    {
                        // This email has been registered before so link to that account
                        var existingUser = userRepository.GetUser(userInfo.Email);
                        userRepository.LinkIdentity(Provider.FromAuthenticatedClient(authenticatedClient), existingUser);
                        // Log the existing user in
                        return nancyModule.LoginAndRedirect(existingUser.Id, null, model.ReturnUrl);
                    }
                    // Email hasn't been registered so create a new user
                    var newUser = userRepository.AddUser(User.FromAuthenticatedClient(authenticatedClient));
                    // Log the new user in
                    return nancyModule.LoginAndRedirect(newUser.Id, null, model.ReturnUrl);
                }
                if (loggedInUser == null)
                {
                    // This account has already been registered so just need to log them in
                    return nancyModule.LoginAndRedirect(user.Id, null, model.ReturnUrl);
                }
                if (user != loggedInUser)
                {
                    // This provider has already been registered to a different user account. Kick Off!
                    throw new AuthenticationException("This provider is already linked to another user account");
                }
            }
            return GetResponse(nancyModule, model);
        }

        public dynamic OnRedirectToAuthenticationProviderError(NancyModule nancyModule, string errorMessage)
        {
            throw new NotImplementedException();
        }

        private static Response GetResponse(INancyModule nancyModule, AuthenticateCallbackData model)
        {
            var response = model.ReturnUrl != null
                ? nancyModule.Response.AsRedirect("~" + model.ReturnUrl)
                : nancyModule.Response.AsRedirect("~");
            return response;
        }

        public IDocumentSession DocumentSessionProvider { get; set; }
    }
}
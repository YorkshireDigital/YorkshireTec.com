namespace YorkshireDigital.Web.Account.Modules
{
    using System;
    using System.Configuration;
    using Nancy;
    using Nancy.Helpers;
    using Nancy.Security;
    using YorkshireDigital.Data.Helpers;
    using YorkshireDigital.Data.Services;

    public class AccountSsoModule : NancyModule
    {
        public AccountSsoModule(IUserService userService) 
            : base("/account/sso")
        {
            this.RequiresAuthentication();

            var discourseSecret = ConfigurationManager.AppSettings["SSO_Discourse_Key"];
            var discourseDomain = ConfigurationManager.AppSettings["SSO_Discourse_Domain"];
            var discourseHelper = new DiscourseHelper(discourseSecret);

            Get["/discourse"] = _ =>
            {
                var sso = Request.Query.sso;
                var sig = Request.Query.sig;

                // Validate request
                if (string.IsNullOrEmpty(sso) || string.IsNullOrEmpty(sig))
                    throw new ArgumentNullException("sso", "The request does not contain the required parameters");

                var payload64 = HttpUtility.UrlDecode(sso);

                string checksum = discourseHelper.GetHash(payload64);
                if (checksum != sig)
                    return HttpStatusCode.Forbidden;

                var nonce = discourseHelper.GetNonceFromPayload(payload64);
                var user = userService.GetUser(Context.CurrentUser.UserName);
                string redirectUrl = discourseHelper.GetRedirectUrl(discourseDomain, user.Name, user.Id.ToString(), user.Email, user.Username, nonce);

                return Response.AsRedirect(redirectUrl);     
            };
        }
    }
}
namespace YorkshireTec.Infrastructure
{
    using System;
    using System.Configuration;
    using Nancy;

    public class SecurityHooks
    {
        /// <summary>
        /// Creates a hook to be used in a pipeline before a route handler to ensure that
        /// the requested feature is enabled.
        /// </summary>
        /// <param name="feature">The feature the must be enabled</param>
        /// <returns>Hook that returns an Unauthorized response if not authenticated in,
        /// null otherwise</returns>
        public static Func<NancyContext, Response> RequiresFeature(string feature)
        {
            bool enabled;
            bool.TryParse(ConfigurationManager.AppSettings[string.Format("Feature_{0}", feature)], out enabled);

            return (ctx) =>
            {
                Response response = null;
                if (!enabled)
                {
                    response = new Response { StatusCode = HttpStatusCode.NotFound };
                }
                return response;
            };
        }
    }
}
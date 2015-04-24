namespace YorkshireDigital.Web.Infrastructure
{
    using Nancy;
    using Nancy.Extensions;

    public static class NancyExtensions
    {
        /// <summary>
        /// This module requires a feature is enabled
        /// </summary>
        /// <param name="module">Module to enable</param>
        /// <param name="featureName">The name of the feature</param>
        public static void RequiresFeature(this INancyModule module, string featureName)
        {
            module.AddBeforeHookOrExecute(SecurityHooks.RequiresFeature(featureName), "Requires Feature");
        }
    }
}
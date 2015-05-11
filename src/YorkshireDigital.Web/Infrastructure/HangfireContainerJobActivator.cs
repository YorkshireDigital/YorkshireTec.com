namespace YorkshireDigital.Web.Infrastructure
{
    using System;
    using Hangfire;
    using Nancy.TinyIoc;

    public class HangfireContainerJobActivator : JobActivator
    {
        private readonly TinyIoCContainer container;

        public HangfireContainerJobActivator(TinyIoCContainer container)
        {
            this.container = container;
        }

        public override object ActivateJob(Type type)
        {
            return container.Resolve(type);
        }
    }
}
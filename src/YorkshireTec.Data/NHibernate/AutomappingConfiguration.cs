namespace YorkshireDigital.Data.NHibernate
{
    using System;
    using FluentNHibernate.Automapping;

    public class AutomappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.Namespace != null && type.Namespace.StartsWith("YorkshireDigital.Data.Domain") && !type.Namespace.Contains("Enums");
        }
    }
}

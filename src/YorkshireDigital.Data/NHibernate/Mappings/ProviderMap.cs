namespace YorkshireDigital.Data.NHibernate.Mappings
{
    using FluentNHibernate.Mapping;
    using YorkshireDigital.Data.Domain.Account;

    public class ProviderMap : ClassMap<Provider>
    {
        public ProviderMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Name);
            Map(x => x.PublicToken);
            Map(x => x.SecretToken);
            Map(x => x.ExpiresOn);
            Map(x => x.Username);
        }
    }
}

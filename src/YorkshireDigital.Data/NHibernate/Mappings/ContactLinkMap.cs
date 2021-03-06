﻿namespace YorkshireDigital.Data.NHibernate.Mappings
{
    using FluentNHibernate.Mapping;
    using YorkshireDigital.Data.Domain.Group;

    public class ContactLinkMap : ClassMap<ContactLink>
    {
        public ContactLinkMap()
        {
            Id(x => x.Id);

            Map(x => x.Type);
            Map(x => x.Value);
        }
    }
}

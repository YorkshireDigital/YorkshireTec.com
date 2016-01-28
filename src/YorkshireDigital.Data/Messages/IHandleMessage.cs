using System;
using NHibernate;
using YorkshireDigital.Data.Services;

namespace YorkshireDigital.Data.Messages
{
    public interface IHandleMeetupRequest
    {
        void Handle(ISession session, IMeetupService meetupService);
    }
}

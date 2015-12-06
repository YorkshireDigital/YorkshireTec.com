using System;
using YorkshireDigital.Data.Services;

namespace YorkshireDigital.Data.Messages
{
    public interface IHandleMeetupRequest : IDisposable
    {
        void Handle(IMeetupService meetupService);
    }
}

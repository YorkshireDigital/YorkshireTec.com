using System;
using NHibernate;
using YorkshireDigital.Data.Services;

namespace YorkshireDigital.Data.Messages
{
    public class TextMessage : IHandleMeetupRequest
    {
        public TextMessage(string input)
        {
            Message = input;
        }

        public string Message { get; set; }

        public void Dispose()
        {
            
        }

        public void Handle(ISession session, IMeetupService meetupService)
        {
            Console.WriteLine(Message);
        }
    }
}

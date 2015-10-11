using EasyNetQ;
using System;
using YorkshireDigital.Data.Messages;

namespace YorkshireDigital.MessageQueue.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                var input = "";
                Console.WriteLine("Enter a message. 'Quit' to quit.");
                while ((input = Console.ReadLine()) != "Quit")
                {
                    bus.Publish<IHandleMeetupRequest>(new TextMessage(input));
                }
            }
        }
    }
}

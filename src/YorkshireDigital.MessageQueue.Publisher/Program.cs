using EasyNetQ;
using System;
using System.Configuration;
using YorkshireDigital.Data.Messages;

namespace YorkshireDigital.MessageQueue.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MessageQueue"];
            if (connectionString == null || connectionString.ConnectionString == string.Empty)
            {
                throw new Exception("easynetq connection string is missing or empty");
            }
            using (var bus = RabbitHutch.CreateBus(connectionString.ConnectionString))
            {
                var input = "";
                Console.WriteLine("Enter a message. 'Quit' to quit.");
                while ((input = Console.ReadLine()) != "Quit")
                {
                    try
                    {
                        bus.Publish<IHandleMeetupRequest>(new TextMessage(input));
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                        Console.WriteLine("---------------------");
                        Console.WriteLine(exception.StackTrace);
                        Console.ReadLine();
                    }
                    
                }
            }
        }
    }
}

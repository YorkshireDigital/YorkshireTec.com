using EasyNetQ;
using System;
using System.Configuration;

public class BusBuilder
{
    public static IBus CreateMessageBus()
    {
        var connectionString = ConfigurationManager.ConnectionStrings["MessageQueue"];
        if (connectionString == null || connectionString.ConnectionString == string.Empty)
        {
            throw new Exception("easynetq connection string is missing or empty");
        }

        return RabbitHutch.CreateBus(connectionString.ConnectionString);
    }
}
﻿using System;
using System.Configuration;
using EasyNetQ;
using Serilog;
using YorkshireDigital.Data.Messages;

namespace YorkshireDigital.Data.Tasks
{
    public class EventSyncTask
    {
        public void Execute(string eventId)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MessageQueue"];
            if (connectionString == null || connectionString.ConnectionString == string.Empty)
            {
                throw new Exception("easynetq connection string is missing or empty");
            }

            Log.Information("Processing EventSyncTask for EventId " + eventId);

            var message = new EventSyncMessage(eventId);

            using (var bus = RabbitHutch.CreateBus(connectionString.ConnectionString))
            {
                bus.Publish<IHandleMeetupRequest>(message);
            }
            Log.Information("Processing Complete");
        }
    }
}

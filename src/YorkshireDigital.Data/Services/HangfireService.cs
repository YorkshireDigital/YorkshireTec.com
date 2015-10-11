using Hangfire;
using System;
using System.Linq.Expressions;
using YorkshireDigital.Data.Tasks;

namespace YorkshireDigital.Data.Services
{
    public interface IHangfireService
    {
        void AddEventSyncTask(string eventSyncName);
        void AddOrUpdateJob<T>(string recurringJobId, Expression<Action<T>> methodCall, Func<string> cronExpression);
        void RemoveJobIfExists(string jobId);
        void Trigger(string jobId);
    }

    public class HangfireService : IHangfireService
    {
        public void AddEventSyncTask(string eventSyncName)
        {
            AddOrUpdateJob<EventSyncTask>(eventSyncName, x => x.Execute(eventSyncName), Cron.Hourly);
        }

        public void RemoveJobIfExists(string jobId)
        {
            RecurringJob.RemoveIfExists(jobId);
        }

        public void Trigger(string jobId)
        {
            RecurringJob.Trigger(jobId);
        }

        public void AddOrUpdateJob<T>(string recurringJobId, Expression<Action<T>> methodCall, Func<string> cronExpression)
        {
            RecurringJob.AddOrUpdate<T>(recurringJobId, methodCall, cronExpression);
        }
    }
}

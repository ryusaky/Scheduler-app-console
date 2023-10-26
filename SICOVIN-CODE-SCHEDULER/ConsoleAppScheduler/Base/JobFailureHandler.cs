using ConsoleAppScheduler.Base.Common;
using Microsoft.Extensions.Logging;
using Quartz;

namespace ConsoleAppScheduler.Base
{
    public class JobFailureHandler : IJobListener
    {
        public string Name => "FailJobListener";
        private int _hourRetry;
        private int _minutesRetry;
        private readonly ILogger<QuartzHostedService> _logger;
        public JobFailureHandler(int hour, int minutes, ILogger<QuartzHostedService> logger)
        {
            _hourRetry= hour;
            _minutesRetry= minutes;
            _logger = logger;
        }

        Task IJobListener.JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(null);
        }

        Task IJobListener.JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken)
        {
            //if (!context.JobDetail.JobDataMap.Contains(Constants.NumTriesKey))
            //{
            //    context.JobDetail.JobDataMap.Put(Constants.NumTriesKey, 0);
            //}

            //var numberTries = context.JobDetail.JobDataMap.GetIntValue(Constants.NumTriesKey);
            //context.JobDetail.JobDataMap.Put(Constants.NumTriesKey, ++numberTries);

            return Task.FromResult<object>(null);
        }

        async Task IJobListener.JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken)
        {
            if (jobException == null)
            {
                return;
            }

            if (_hourRetry<=0 && _minutesRetry<=0)
            {
                _hourRetry= 0;
                _minutesRetry= 30;
            }
            DateTime dateNow = DateTime.Now;
            _logger.LogInformation($"hours of retry: {_hourRetry}, minutes of retry: {_minutesRetry}");
            dateNow = dateNow.AddHours(_hourRetry);
            dateNow = dateNow.AddMinutes(_minutesRetry);
            _logger.LogInformation($"Next Execution Date: {dateNow.ToString()}");
            var trigger = TriggerBuilder
                .Create()
                .WithIdentity(Guid.NewGuid().ToString(), Constants.TriggerGroup)
                .StartAt(dateNow)
                .Build();

            _logger.LogInformation($"Job with ID and type: {context.JobDetail.Key}, {context.JobDetail.JobType} has thrown the exception: {jobException}. Running again in {_hourRetry*60+_minutesRetry} minutes.");

            await context.Scheduler.RescheduleJob(context.Trigger.Key, trigger);
        }
    }
}

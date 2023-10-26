using Quartz;
using SICOVIN_CODE_SCHEDULER.Base.Common;

namespace SICOVIN_CODE_SCHEDULER.Base
{
    public class JobFailureHandler : IJobListener
    {
        public string Name => "FailJobListener";

        Task IJobListener.JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(null);
        }

        Task IJobListener.JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken)
        {
            if (!context.JobDetail.JobDataMap.Contains(Constants.NumTriesKey))
            {
                context.JobDetail.JobDataMap.Put(Constants.NumTriesKey, 0);
            }

            var numberTries = context.JobDetail.JobDataMap.GetIntValue(Constants.NumTriesKey);
            context.JobDetail.JobDataMap.Put(Constants.NumTriesKey, ++numberTries);

            return Task.FromResult<object>(null);
        }

        async Task IJobListener.JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken)
        {
            if (jobException == null)
            {
                return;
            }

            var numTries = context.JobDetail.JobDataMap.GetIntValue(Constants.NumTriesKey);

            if (numTries > Constants.MaxRetries)
            {
                Console.WriteLine($"Job with ID and type: {0}, {1} has run {2} times and has failed each time.",
                    context.JobDetail.Key, context.JobDetail.JobType, Constants.MaxRetries);

                return;
            }
            var trigger = TriggerBuilder
                .Create()
                .WithIdentity(Guid.NewGuid().ToString(), Constants.TriggerGroup)
                .StartAt(DateTime.Now.AddSeconds(Constants.WaitInterval * numTries))
                .Build();

            Console.WriteLine($"Job with ID and type: {0}, {1} has thrown the exception: {2}. Running again in {3} seconds.",
                context.JobDetail.Key, context.JobDetail.JobType, jobException, Constants.WaitInterval * numTries);

            await context.Scheduler.RescheduleJob(context.Trigger.Key, trigger);
        }
    }
}

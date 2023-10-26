using Quartz;
using Quartz.Spi;

namespace SICOVIN_CODE_SCHEDULER.Base
{
    public class QuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<JobSchedule> _jobSchedules;
        public QuartzHostedService(
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory,
            IEnumerable<JobSchedule> jobSchedules)
        {
            _schedulerFactory= schedulerFactory;
            _jobFactory= jobFactory;
            _jobSchedules = jobSchedules;
        }
        public IScheduler Scheduler { get; private set; }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory= _jobFactory;
            foreach (var jobSchedule in _jobSchedules)
            {
                var job = CreateJob(jobSchedule);
                var trigger = CreateTrigger(jobSchedule);
                Scheduler.ListenerManager.AddJobListener(new JobFailureHandler());
                await Scheduler.ScheduleJob(job, trigger, cancellationToken);
            }
            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);
        }
        private static IJobDetail CreateJob(JobSchedule schedule)
        {
            var jobType = schedule.JobType;
            return JobBuilder
                .Create(jobType)
                .WithIdentity(schedule.JobId)
                .WithDescription(jobType.Name)
                .UsingJobData("MaxRetries",5)
                .Build();
        }
        private static ITrigger CreateTrigger(JobSchedule schedule)
        {
            return schedule.ExecuteOnce
                ? TriggerBuilder
                    .Create()
                    .WithIdentity($"{schedule.JobId}.trigger")
                    //.WithCronSchedule(schedule.CronExpression)
                    .WithDescription(schedule.CronExpression)
                    .StartNow()
                    .WithPriority(1)
                    .Build()
                : TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.JobId}.trigger")
                .WithCronSchedule(schedule.CronExpression)
                .WithDescription(schedule.CronExpression)
                .Build();
        }
    }
}

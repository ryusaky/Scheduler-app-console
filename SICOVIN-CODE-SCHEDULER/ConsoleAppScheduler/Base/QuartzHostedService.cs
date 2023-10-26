using Quartz.Spi;
using Quartz;
using Microsoft.Extensions.Hosting;
using ConsoleAppScheduler.Base.Tools;
using Microsoft.Extensions.Logging;

namespace ConsoleAppScheduler.Base
{
    public class QuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<JobSchedule> _jobSchedules;
        private readonly ILogger<QuartzHostedService> _logger;
        public QuartzHostedService(
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory,
            IEnumerable<JobSchedule> jobSchedules,
            ILogger<QuartzHostedService> logger)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _jobSchedules = jobSchedules;
            _logger = logger;
        }
        public IScheduler Scheduler { get; private set; }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;
            foreach (var jobSchedule in _jobSchedules)
            {
                var job = CreateJob(jobSchedule);
                var trigger = CreateTrigger(jobSchedule,_logger);
                Scheduler.ListenerManager.AddJobListener(new JobFailureHandler(jobSchedule.HourRetry,jobSchedule.MinutesRetry, _logger));
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
                .AddDataInJob(schedule.Data)
                //.UsingJobData("MaxRetries", 5)
                .Build();
        }
        private static ITrigger CreateTrigger(JobSchedule schedule,ILogger<QuartzHostedService> logger)
        {
            logger.LogInformation($"Tarea : {schedule.JobId} CronExpressión: {schedule.CronExpression}");
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

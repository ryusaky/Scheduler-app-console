using Quartz;
using SICOVIN_CODE_SCHEDULER.Base;
using SICOVIN_CODE_SCHEDULER.Database;

namespace SICOVIN_CODE_SCHEDULER.Jobs
{
    public class DatabaseQueryJob : IJob
    {
        private readonly ILogger<DatabaseQueryJob> _logger;
        private readonly IServiceProvider _provider;
        private readonly IEnumerable<JobSchedule> _jobSchedules;
        public DatabaseQueryJob(IServiceProvider provider, 
            ILogger<DatabaseQueryJob> logger,
            IEnumerable<JobSchedule> jobSchedules)
        {
            _provider = provider;
            _logger = logger;
            _jobSchedules = jobSchedules;
        }

        public Task Execute(IJobExecutionContext context)
        {
            using(var scope = _provider.CreateScope())
            {

                var dbContext = scope.ServiceProvider.GetService<SICOVINDbContext> ();
                var jobs = dbContext.JobDescriptions.ToList();
                foreach(var myjob in jobs)
                {
                    _jobSchedules.Append(new JobSchedule(
                        jobType: typeof(ReadDataJob),
                        executeOnce: true,
                        cronExpression: "0/5 * * * * ?")
                    );
                    //_services.AddSingleton<ReadDataJob>();
                    //_services.AddSingleton(new JobSchedule(
                    //jobType: typeof(ReadDataJob),
                    //cronExpression: "0/10 * * * * ?")
                    //);
                    _logger.LogInformation($"{myjob.JobName}");
                }

            }
            return Task.CompletedTask;
        }
    }
}

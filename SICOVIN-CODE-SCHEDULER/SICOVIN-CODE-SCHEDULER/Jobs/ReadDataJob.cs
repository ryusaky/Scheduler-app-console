using Quartz;

namespace SICOVIN_CODE_SCHEDULER.Jobs
{
    public class ReadDataJob : IJob
    {
        private readonly ILogger<ReadDataJob> _logger;
        public ReadDataJob(ILogger<ReadDataJob> logger)
        {
            _logger= logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"Executando job {context.JobDetail.Key.Name}");
            return Task.CompletedTask;
        }
    }
}

using Microsoft.Extensions.Logging;
using Quartz;

namespace ConsoleAppScheduler.Jobs
{
    public class PrintConsoleJob : IJob
    {
        private readonly ILogger<PrintConsoleJob> _logger;
        public PrintConsoleJob(ILogger<PrintConsoleJob> logger)
        {
            _logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"Executando job:  {context.JobDetail.Key.Name}");
            return Task.CompletedTask;
        }
    }
}

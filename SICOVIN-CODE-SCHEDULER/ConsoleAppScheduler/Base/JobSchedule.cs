namespace ConsoleAppScheduler.Base
{
    public class JobSchedule
    {
        public JobSchedule(Type jobType, string cronExpression, IDictionary<string, object> data, int hourRetry = 0, int minutesRetry=0, bool executeOnce = false)
        {
            JobType = jobType;
            CronExpression = cronExpression;
            ExecuteOnce = executeOnce;
            JobId = Guid.NewGuid().ToString();
            HourRetry = hourRetry;
            MinutesRetry= minutesRetry;
            Data = data;
        }
        public Type JobType { get; set; }
        public string CronExpression { get; set; }
        public bool ExecuteOnce { get; set; }
        public string JobId { get; set; }
        public int HourRetry { get; set; }
        public int MinutesRetry { get; set; }
        public IDictionary<string, object> Data { get;set; }
    }
}

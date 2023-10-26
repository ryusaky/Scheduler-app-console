namespace SICOVIN_CODE_SCHEDULER.Base
{
    public class JobSchedule
    {
        public JobSchedule(Type jobType, string cronExpression, bool executeOnce = false)
        {
            JobType= jobType;
            CronExpression= cronExpression;
            ExecuteOnce= executeOnce;
            JobId = Guid.NewGuid().ToString();
        }
        public Type JobType { get; set; }
        public string CronExpression { get; set; }
        public bool ExecuteOnce { get; set; }
        public string JobId { get; set; }
    }
}

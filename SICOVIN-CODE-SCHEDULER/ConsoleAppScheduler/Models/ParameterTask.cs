namespace ConsoleAppScheduler.Models
{
    public class ParameterTask
    {
        public Guid IdMeasurePoint { get; set; }
        public string JobId { get; set; }
        public string TipoPuntoMedicion { get; set; }
        public int ExecutionHours { get; set; }
        public int ExecutionMinutes { get; set; }
        public int ExceptionExecutionHourInterval { get; set; }
        public int ExceptionExecutionMinuteInterval { get; set; }
        public string TagExecutionWeekDays { get; set; }
        public Guid IdBalance { get; set; }
        public string EntityNameCode { get; set; }
    }
}

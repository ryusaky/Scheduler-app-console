namespace SICOVIN_CODE_SCHEDULER.Models
{
    public class ParameterTask
    {
        public string JobId { get; set; }
        public string TipoPuntoMedicion { get; set; }
        public int ExecutionHours { get; set; }
        public int ExecutionMinutes { get; set; }
        public int ExceptionExecutionHourInterval { get; set; }
        public int ExceptionExecutionMinuteInterval { get; set; }
        public string TagExecutionWeekDays { get; set; }
    }
}

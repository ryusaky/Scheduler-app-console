namespace ConsoleAppScheduler.Models
{
    public class MedicionPIRequest
    {
        public string IdMeasurePoint { get; set; }
        public string TagRequested { get; set; }
        public double TagValue { get; set; }
        public string DateDiaGas { get; set; }
        public int ExceptionCode { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string IdBalance { get; set; }
        public string EntityNameCode { get; set; }

    }
}

namespace ConsoleAppScheduler.Models
{
    public class DataTagEnergiaTask
    {
        public string IdMeasurePoint { get; set; }
        public DateTime DiaGas { get; set; }
        public string Tag { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public double Value { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
    }
}

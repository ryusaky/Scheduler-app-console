namespace ConsoleAppScheduler.Models
{
    public class HistoryDataModel
    {
        public bool IsHistoryData { get; set; }
        public List<Measures> Measures { get; set; }
    }
    public class Measures
    {
        public string EntityName { get; set; }
        public string EntityType { get; set; }
        public string EntityNameCode { get; set; }
        public Guid EntityId { get; set; }
        public List<MeasurePoint> MeasurePoints { get; set; }
        public List<PeriodRead> PeriodReads { get; set; }
    }
    public class MeasurePoint
    {
        public string TagRequested { get; set; }
        public Guid IdMeasurePoint { get; set; }
    }
    public class PeriodRead
    {
        public Guid IdBalance { get; set; }
        public List<short> DaysOfCharge { get; set; }   
        public byte MaxDay { get; set; }
        public byte MinDay { get; set; }
        public byte Month { get; set; }
        public short Year { get; set; }
    }
}

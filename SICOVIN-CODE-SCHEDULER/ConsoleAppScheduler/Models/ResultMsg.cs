
namespace ConsoleAppScheduler.Models
{
    public class ResultMsg<TO>
        where TO : class
    {
        public ResultMsg() => Success = true;
        public TO Value { get; set; }
        public string Message { get; set; }
        public List<string> Messages { get; set; }
        public bool Success { get; set; }
        public short ErrorCode { get; set; }
        public string Environment { get; set; }
    }
}

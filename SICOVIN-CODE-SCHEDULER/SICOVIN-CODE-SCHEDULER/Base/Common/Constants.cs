namespace SICOVIN_CODE_SCHEDULER.Base.Common
{
    public class Constants
    {
        public static readonly int WaitInterval = 5; // Seconds
        public static readonly int MaxRetries = 3;
        public static readonly string NumTriesKey = "numTriesKey"; // Can be anything
        public static readonly string TriggerGroup = "failTriggerGroup"; // Can be anything
        public static readonly string EveryYear = "0 0 0 ? * * 2022/1";
        public static readonly string EverySecond = "0 0/1 * 1/1 * ? *";

        public class StoreProcedures
        {
            public static readonly string SP_API_GET_LOAD_PARAMETERS = "spAPI_Get_Load_Parameters";
        }
    }
}

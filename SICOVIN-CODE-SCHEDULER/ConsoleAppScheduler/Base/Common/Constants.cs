namespace ConsoleAppScheduler.Base.Common
{
    public class Constants
    {
        public static readonly int WaitInterval = 5; // Seconds
        public static readonly int MaxRetries = 3;
        public static readonly string NumTriesKey = "numTriesKey"; // Can be anything
        public static readonly string TriggerGroup = "failTriggerGroup"; // Can be anything
        public static readonly string EveryYear = "0 0 0 ? * * 2022/1";
        public static readonly string EverySecond = "0 0/1 * 1/1 * ? *";
        public class BooleanValues
        {
            public static readonly string STRING_TRUE = "1";
            public static readonly short SHORT_TRUE = 1;
            public static readonly string STRING_FALSE = "0";
            public static readonly short SHORT_FALSE = 0;
        }
        public class StoreProcedures
        {
            //public static readonly string SP_API_GET_LOAD_PARAMETERS = "spAPI_Get_Load_Parameters";
            public static readonly string SP_API_GET_LOAD_PARAMETERS = "spJOB_PI_Get_LoadParameters";
            public static readonly string SP_API_GET_TAG_VOLUMEN = "spIE_GetTagVolumen";
            public static readonly string SP_API_GET_TAG_ENERGIA = "spIE_GetTagEnergia";
            //public static readonly string SP_API_INSERT_MEASURES = "spAPI_Insert_Measures";
            public static readonly string SP_API_INSERT_MEASURES = "spJOB_Add_PI_Measures";
            public static readonly string SP_API_INSERT_MEASURES_TEST = "spAPI_Insert_Measures_test";
        }
        public static class FormatDates
        {
            public const string DB_DATE_TIME = "yyyy-MM-dd HH:mm:ss";
            public const string DB_DATE = "yyyy-MM-dd";
            public const string LATIN_DATE = "dd/MM/yyyy";
            public const string LATIN_DATE_HOUR = "dd/MM/yyyy hh:mm";
            public const string LATIN_DATE_HOUR_24 = "dd/MM/yyyy HH:mm";
            public const string LATIN_DATE_TIME = "dd/MM/yyyy HH:mm:ss";
            public const string LATIN_TIME_HOUR_24 = "HH:mm";
            public const string LATIN_TIME_HOUR_SECOND_24 = "HH:mm:ss";
            public const string LATIN_TIME_HOUR_12 = "hh:mm tt";
            public const string LATIN_DATE_TIME_FFF = "dd/MM/yyyy HH:mm:ss.fff";
        }
        public static class Environment
        {
            public const string ENVIRONMENT_SYSTEM = "ASPNETCORE_ENVIRONMENT";
            public const string ENV_DEVELOPMENT = "Development";
            public const string ENV_PRODUCTION = "Production";
            public const string ENV_QA = "Qa";
        }
        public static class TypeMeasure
        {
            public const string ENERGIA = "Energia";
            public const string VOLUMEN = "Volumen";
        }
    }
}

using Quartz;

namespace ConsoleAppScheduler.Base.Tools
{
    public static class JobBuilderHelper
    {
        public static JobBuilder AddDataInJob(this JobBuilder builder, IDictionary<string,object> map)
        {
            JobDataMap datamap = new ();
            
            foreach (KeyValuePair<string,object> entry in map)
            {
                datamap.Add(entry);
                //JobData data = (JobData)entry.Value;
                //switch (data.TypeData)
                //{
                //    case GlobalEnum.TypeDataJob.INTEGER:
                //        builder.UsingJobData(entry.Key, int.Parse(data.Value));break;
                //    case GlobalEnum.TypeDataJob.DATE:
                //        builder.UsingJobData(entry.Key,DateHelper.StringToDateFormat(data.Value));break;
                //    default:
                //        builder.UsingJobData(entry.Key, data.Value);break;
                //}

            }
            builder.UsingJobData(datamap);
            return builder;
        }
    }
}

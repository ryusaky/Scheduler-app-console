using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using SICOVIN_CODE_SCHEDULER.Jobs;
using SICOVIN_CODE_SCHEDULER.Database;
using Microsoft.EntityFrameworkCore;
using SICOVIN_CODE_SCHEDULER.Base.TemplateMapper;
using System.Data;
using SICOVIN_CODE_SCHEDULER.Models;
using SICOVIN_CODE_SCHEDULER.Base.Common;

namespace SICOVIN_CODE_SCHEDULER.Base
{
    public static class QuartzNetConfiguration
    {
        public static async Task ConfigureJobs(this IServiceCollection services)
        {
            services.AddSingleton<QuartzJobRunner>();
            //services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            try
            {
                var dbContext = serviceProvider.GetRequiredService<SICOVINDbContext>();
                var jobs = new List<ParameterTask>();
                using (IDataTemplate dataTemplate = new SqlServerDataTemplate(dbContext.Database.GetDbConnection()))
                {
                    await dataTemplate.ComplexStoreProcedureAsync(Constants.StoreProcedures.SP_API_GET_LOAD_PARAMETERS, new DataParameter[]
                    {
                            new DataParameter()
                            {
                                Name= "@LastName",
                                Direction=ParameterDirection.Input,
                                Type= SqlDbType.NVarChar,
                                Value= "Paz"
                            },
                            new DataParameter()
                            {
                                Name="@FirstName",
                                Direction=ParameterDirection.Input,
                                Type= SqlDbType.NVarChar,
                                Value="Luis"
                            }
                    }, typeExcec: Common.GlobalEnum.TypeExecuteStoreSP.Read);
                    var result = dataTemplate.MapperReaderStoreProcedure(r => new ParameterTask
                    {
                        JobId = Guid.NewGuid().ToString(),
                        TipoPuntoMedicion = !r.IsDBNull(r.GetOrdinal("TIPO PUNTO MEDICION")) ? r.GetString(r.GetOrdinal("TIPO PUNTO MEDICION")) : string.Empty,
                        ExecutionHours = !r.IsDBNull(r.GetOrdinal("iExecutionHour")) ? r.GetInt32(r.GetOrdinal("iExecutionHour")) : default,
                        ExecutionMinutes = !r.IsDBNull(r.GetOrdinal("iExecutionMinute")) ? r.GetInt32(r.GetOrdinal("iExecutionMinute")) : default,
                        ExceptionExecutionHourInterval = !r.IsDBNull(r.GetOrdinal("iExceptionExecutionHourInterval")) ? r.GetInt32(r.GetOrdinal("iExceptionExecutionHourInterval")) : default,
                        ExceptionExecutionMinuteInterval = !r.IsDBNull(r.GetOrdinal("iExceptionExecutionMinuteInterval")) ? r.GetInt32(r.GetOrdinal("iExceptionExecutionMinuteInterval")) : default,
                        TagExecutionWeekDays = !r.IsDBNull(r.GetOrdinal("sTagExecutionWeekdays")) ? r.GetString(r.GetOrdinal("sTagExecutionWeekdays")) : string.Empty,
                    });

                    jobs = result.ToList();
                    foreach (var job in jobs)
                    {
                        logger.LogInformation($"Job with name {job.JobId}");
                        services.AddSingleton<ReadDataJob>();
                        services.AddSingleton(new JobSchedule(
                            jobType: typeof(ReadDataJob),
                            cronExpression: "0/5 * * * * ?"));
                    }
                }
                //var jobs = dbContext.Database.ExecuteSqlAsync.FromSql($"spAPI_Get_Load_Parameters {"Luis"} {"Paz"}").ToList();
                
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ha ocurrido un error");
            }
            //services.AddSingleton<HelloWorldJob>();
            //services.AddSingleton(new JobSchedule(
            //    jobType: typeof(HelloWorldJob),
            //    cronExpression: "0/5 * * * * ?"));
            //services.AddScoped<DatabaseQueryJob>();
            //services.AddSingleton(new JobSchedule(
            //    jobType: typeof(DatabaseQueryJob),
            //    executeOnce: true,
            //    cronExpression: "0/5 * * * * ?"));
            services.AddHostedService<QuartzHostedService>();


        }
    }
}

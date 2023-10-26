using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using ConsoleAppScheduler.Base.Common;
using ConsoleAppScheduler.Models;
using ConsoleAppScheduler.DataBase;
using ConsoleAppScheduler.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ConsoleAppScheduler.Base.Tools;
using ConsoleAppScheduler.SPManager;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Microsoft.EntityFrameworkCore;

namespace ConsoleAppScheduler.Base
{
    public static class QuartzNetConfiguration
    {
        public static async Task ConfigureJobs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SICOVINDbContext>(
                options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("SICOVINDBCnx"));
                }
            );
            services.AddDbContext<PIDbContext>(
                options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("PIDBCnx"));
                }
            );
            services.AddSingleton<QuartzJobRunner>();
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            var _logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            var dbContext = serviceProvider.GetRequiredService<SICOVINDbContext>();
            var jobs = new List<ParameterTask>();
            var HistoryData = configuration.GetSection("HistoryMeasures").Get<HistoryDataModel>();
            if (HistoryData != null && HistoryData.IsHistoryData && HistoryData.Measures.Count > 0)
            {
                HistoryData.Measures.ForEach(m =>
                {
                    m.MeasurePoints.ForEach(mp =>
                    {
                        m.PeriodReads.ForEach(pr =>
                        {
                            IDictionary<string, object> data = new Dictionary<string, object>
                    {
                            {
                                "Year",
                                new JobData
                                {
                                    TypeData = GlobalEnum.TypeDataJob.STRING,
                                    Value= pr.Year.ToString()
                                }
                            },
                            {
                                "Month",
                                new JobData
                                {
                                    TypeData = GlobalEnum.TypeDataJob.STRING,
                                    Value = pr.Month.ToString()
                                }
                            },
                            {
                                "MinDay",
                                new JobData
                                {
                                    TypeData = GlobalEnum.TypeDataJob.STRING,
                                    Value= pr.MinDay.ToString()
                                }
                            },
                            {
                                "MaxDay",
                                new JobData
                                {
                                    TypeData = GlobalEnum.TypeDataJob.STRING,
                                    Value= pr.MaxDay.ToString()
                                }
                            },
                            {
                                "Tag",
                                new JobData
                                {
                                    TypeData = GlobalEnum.TypeDataJob.STRING,
                                    Value = mp.TagRequested.ToString()
                                }
                            },
                            {
                                "IdMeasurePoint",
                                new JobData
                                {
                                    TypeData = GlobalEnum.TypeDataJob.GUID,
                                    Value = mp.IdMeasurePoint.ToString()
                                }
                            },
                            {
                                "IdBalance",
                                new JobData
                                {
                                    TypeData = GlobalEnum.TypeDataJob.GUID,
                                    Value = pr.IdBalance.ToString()
                                }
                            },
                            {
                                "EntityNameCode",
                                new JobData
                                {
                                    TypeData = GlobalEnum.TypeDataJob.STRING,
                                    Value = m.EntityNameCode
                                }
                            }
                    };
                            if (pr.DaysOfCharge != null
                            && pr.DaysOfCharge.Any())
                            {
                                data.Add("DaysOfCharge",
                                    new JobData
                                    {
                                        TypeData = GlobalEnum.TypeDataJob.STRING,
                                        Value = string.Join(",", pr.DaysOfCharge)
                                    }
                                 );
                                services.AddSingleton<ChargeListDaysJob>();
                                services.AddSingleton(new JobSchedule(
                                    jobType: typeof(ChargeListDaysJob),
                                    //cronExpression: "0/5 * * * * ?",
                                    cronExpression: "Se ejecuta una sola vez, cambiar el history a false despues de migrar",
                                    data: data,
                                    executeOnce: true
                                ));
                            }
                            else
                            {
                                services.AddSingleton<ReadHistoryDataJob>();
                                services.AddSingleton(new JobSchedule(
                                    jobType: typeof(ReadHistoryDataJob),
                                    //cronExpression: "0/5 * * * * ?",
                                    cronExpression: "Se ejecuta una sola vez, cambiar el history a false despues de migrar",
                                    //hourRetry: 0,
                                    //minutesRetry: 30,
                                    data: data,
                                    executeOnce: true
                                ));
                            }

                        });
                    });
                });
            }
            else
            {
                var parameters = await SPReadDataParameters.ReadParameters(dbContext);
                foreach (var job in parameters)
                {
                    DayOfWeek[] daysOfWeek = CronExpressionHelper.StringDaysToListEnum(job.TagExecutionWeekDays);
                    //string cronExpression = CronExpressionHelper.AtHourAndMinuteOnGivenDaysOfWeek(11, 58, daysOfWeek);
                    string cronExpression = CronExpressionHelper.AtHourAndMinuteOnGivenDaysOfWeek(job.ExecutionHours, job.ExecutionMinutes, daysOfWeek);
                    IDictionary<string, object> data = new Dictionary<string, object>
                        {
                            {
                                "DateSearch",
                                new JobData
                                {
                                    TypeData = GlobalEnum.TypeDataJob.STRING,
                                    Value = DateHelper.ToLatinFormat(DateTime.Now)
                                }
                            },
                            {
                                "Tag",
                                new JobData
                                {
                                    TypeData = GlobalEnum.TypeDataJob.STRING,
                                    Value = job.TipoPuntoMedicion
                                }
                            },
                            {
                                "IdMeasurePoint",
                                new JobData
                                {
                                    TypeData = GlobalEnum.TypeDataJob.GUID,
                                    Value = job.IdMeasurePoint.ToString()
                                }
                            },
                            {
                                "IdBalance",
                                new JobData
                                {
                                    TypeData = GlobalEnum.TypeDataJob.GUID,
                                    Value = job.IdBalance.ToString()
                                }
                            },
                            {
                                "EntityNameCode",
                                new JobData
                                {
                                    TypeData = GlobalEnum.TypeDataJob.STRING,
                                    Value = job.EntityNameCode
                                }
                            }
                        };
                    services.AddSingleton<ReadDataJob>();
                    services.AddSingleton(new JobSchedule(
                        jobType: typeof(ReadDataJob),
                        //cronExpression: "0/5 * * * * ?",
                        cronExpression: cronExpression,
                        hourRetry: job.ExceptionExecutionHourInterval,
                        minutesRetry: job.ExceptionExecutionMinuteInterval,
                        data: data
                    ));
                }
            }

            services.AddSingleton<PrintConsoleJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(PrintConsoleJob),
                cronExpression: "0 0 2 ? * * *",
                hourRetry: 0,
                minutesRetry:30
            ));

            services.AddHostedService<QuartzHostedService>();
            services.AddLogging(builder =>
            {
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.File(path: "\\Logs\\schedulerlog.log", restrictedToMinimumLevel: LogEventLevel.Error,
                            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.ffff}|{TenantName}|{RequestId}|{SourceContext}|{Level:u3}|{Message:lj}{NewLine}{Exception}[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                            rollingInterval: RollingInterval.Day)
                .CreateLogger();
            });
        }
    }
}

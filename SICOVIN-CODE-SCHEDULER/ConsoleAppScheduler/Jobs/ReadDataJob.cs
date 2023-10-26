using ConsoleAppScheduler.Base.Tools;
using ConsoleAppScheduler.DataBase;
using ConsoleAppScheduler.Models;
using ConsoleAppScheduler.SPManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using static ConsoleAppScheduler.Base.Common.Constants;

namespace ConsoleAppScheduler.Jobs
{
    public class ReadDataJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<ReadDataJob> _logger;
        public ReadDataJob(IServiceProvider provider,
            ILogger<ReadDataJob> logger)
        {
            _provider = provider;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _provider.CreateScope();
            var dbPIContext = scope.ServiceProvider.GetService<PIDbContext>();
            var dbSICOVINContext = scope.ServiceProvider.GetService<SICOVINDbContext>();
            string dateSearch = ((JobData)context.JobDetail.JobDataMap.Get("DateSearch")).Value;
            string tag = ((JobData)context.JobDetail.JobDataMap.Get("Tag")).Value;
            string idMeasurePoint = ((JobData)context.JobDetail.JobDataMap.Get("IdMeasurePoint")).Value;
            string idBalance = ((JobData)context.JobDetail.JobDataMap.Get("IdBalance")).Value;
            string entityNameCode = ((JobData)context.JobDetail.JobDataMap.Get("EntityNameCode")).Value;
            if (tag.Contains(TypeMeasure.ENERGIA))
            {
                var valueEnergy = (await SPReadDataPI.ReadDataEnergia(dbPIContext, dateSearch, tag,_logger)).FirstOrDefault();
                if (valueEnergy != null && !string.IsNullOrEmpty(valueEnergy.Tag))
                {
                    await SPInsertDataMeasures.InsertData(dbSICOVINContext, new MedicionPIRequest
                    {
                        DateDiaGas = DateHelper.ToLatinFormat(valueEnergy.DiaGas),
                        IdMeasurePoint = idMeasurePoint,
                        Description = valueEnergy.Description,
                        ExceptionCode = valueEnergy.Status,
                        Status = valueEnergy.Status,
                        TagValue = valueEnergy.Value,
                        TagRequested = valueEnergy.Tag,
                        IdBalance = idBalance,
                        EntityNameCode = entityNameCode
                    },_logger);
                }

                //foreach (var item in listEnergy)
                //{
                //    Console.WriteLine($"CONSULTA ENERGIA: " +
                //        $"\nDIAGAS: {item.DiaGas},\n" +
                //        $"Tag: {item.Tag},\n" +
                //        $"FECHAINI: {item.FechaIni},\n" +
                //        $"FECHAFIN: {item.FechaFin},\n" +
                //        $"VALUE: {item.Value},\n" +
                //        $"ESTATUS: {item.Status},\n" +
                //        $"DESCRIPCION: {item.Description}");
                //}

            }
            else
            {
                var valueVolumen = (await SPReadDataPI.ReadDataVolumen(dbPIContext, dateSearch, tag,_logger)).FirstOrDefault();
                if (valueVolumen != null && !string.IsNullOrEmpty(valueVolumen.Tag))
                {
                    await SPInsertDataMeasures.InsertData(dbSICOVINContext, new MedicionPIRequest
                    {
                        DateDiaGas = DateHelper.ToLatinFormat(valueVolumen.DiaGas),
                        IdMeasurePoint = idMeasurePoint,
                        Description = valueVolumen.Description,
                        ExceptionCode = valueVolumen.Status,
                        Status = valueVolumen.Status,
                        TagValue = valueVolumen.Value,
                        TagRequested = valueVolumen.Tag,
                        IdBalance = idBalance,
                        EntityNameCode = entityNameCode
                    }, _logger);
                }
                //foreach (var item in listVolumen)
                //{
                //    Console.WriteLine($"CONSULTA VOLUMEN: " +
                //        $"\nDIAGAS: {item.DiaGas},\n" +
                //       $"Tag: {item.Tag},\n" +
                //       $"FECHA: {item.Fecha},\n" +
                //       $"VALUE: {item.Value},\n" +
                //       $"ESTATUS: {item.Status},\n" +
                //       $"DESCRIPCION: {item.Description}");
                //}
            }

        }
    }
}

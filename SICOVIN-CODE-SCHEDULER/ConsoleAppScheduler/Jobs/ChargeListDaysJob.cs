﻿using ConsoleAppScheduler.Base.Tools;
using ConsoleAppScheduler.DataBase;
using ConsoleAppScheduler.Models;
using ConsoleAppScheduler.SPManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using static ConsoleAppScheduler.Base.Common.Constants;

namespace ConsoleAppScheduler.Jobs
{
    public class ChargeListDaysJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<ChargeListDaysJob> _logger;

        public ChargeListDaysJob(IServiceProvider provider,
            ILogger<ChargeListDaysJob> logger)
        {
            _provider = provider;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _provider.CreateScope();
            var dbPIContext = scope.ServiceProvider.GetService<PIDbContext>();
            var dbSICOVINContext = scope.ServiceProvider.GetService<SICOVINDbContext>();
            string month = ((JobData)context.JobDetail.JobDataMap.Get("Month")).Value;
            string year = ((JobData)context.JobDetail.JobDataMap.Get("Year")).Value;
            string tag = ((JobData)context.JobDetail.JobDataMap.Get("Tag")).Value;
            string idMeasurePoint = ((JobData)context.JobDetail.JobDataMap.Get("IdMeasurePoint")).Value;
            string idBalance = ((JobData)context.JobDetail.JobDataMap.Get("IdBalance")).Value;
            string entityNameCode = ((JobData)context.JobDetail.JobDataMap.Get("EntityNameCode")).Value;
            List<short> daysForCharge = (((JobData)context.JobDetail.JobDataMap.Get("DaysOfCharge")).Value.Split(',')).Select(e => short.Parse(e)).ToList();
            if (tag.Contains(TypeMeasure.ENERGIA))
            {
                foreach(short i in daysForCharge)
                {
                    var dateMeasureRead = $"{month}/{i}/{year}";
                    var valueEnergy = (await SPReadDataPI.ReadDataEnergia(dbPIContext, dateMeasureRead, tag, _logger)).FirstOrDefault();
                    _logger.LogInformation($"Lectura correspondiente al día {i:00}/{month:00}/{year} con formato({dateMeasureRead}) y tag ({tag})");
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
                        }, _logger);
                    }
                    else
                    {
                        string msgError = $"No se obtuvo medición de ENERGIA con los siguientes parametros: {{Fecha: {dateMeasureRead}, Tag: {tag}}}";
                        _logger.LogError(msgError);
                        throw new Exception(msgError);
                    }
                }
            }
            else
            {
                foreach(short i in daysForCharge)
                {
                    var dateMeasureRead = $"{month}/{i}/{year}";
                    var valueVolumen = (await SPReadDataPI.ReadDataVolumen(dbPIContext, dateMeasureRead, tag, _logger)).FirstOrDefault();
                    _logger.LogInformation($"Lectura correspondiente al día {i:00}/{month:00}/{year} con formato({dateMeasureRead}) y tag ({tag})");
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
                    else
                    {
                        string msgError = $"No se obtuvo medición de VOLUMEN con los siguientes parametros: {{Fecha: {dateMeasureRead}, Tag: {tag}}}";
                        _logger.LogError(msgError);
                        throw new Exception(msgError);
                    }
                }
            }

        }
    }
}

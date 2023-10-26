using ConsoleAppScheduler.Base.Common;
using ConsoleAppScheduler.Base.TemplateMapper;
using ConsoleAppScheduler.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using Quartz;
using System.Data;
using static ConsoleAppScheduler.Base.Common.Constants;

namespace ConsoleAppScheduler.SPManager
{
    public class SPReadDataPI
    {
        public async static Task<List<DataTagEnergiaTask>> ReadDataEnergia(DbContext dbContext, string dateSearch, string tag, ILogger<IJob> logger)
        {
            IQueryable<DataTagEnergiaTask> result = default;
            try
            {
                using IDataTemplate dataTemplate = new SqlServerDataTemplate(dbContext.Database.GetDbConnection());
                await dataTemplate.ComplexStoreProcedureAsync(StoreProcedures.SP_API_GET_TAG_ENERGIA, new DataParameter[]
                {
                            new DataParameter
                            {
                                Name="@fechaInicial",
                                Direction=ParameterDirection.Input,
                                Type=SqlDbType.VarChar,
                                //Value=((JobData)context.JobDetail.JobDataMap.Get("DateSearch")).Value
                                Value=dateSearch
                            },
                            new DataParameter
                            {
                                Name="@tag",
                                Direction= ParameterDirection.Input,
                                Type=SqlDbType.VarChar,
                                //Value=((JobData)context.JobDetail.JobDataMap.Get("Tag")).Value
                                Value=tag
                            }
                }, typeExcec: GlobalEnum.TypeExecuteStoreSP.Read);

                result = dataTemplate.MapperReaderStoreProcedure(r => new DataTagEnergiaTask
                {
                    DiaGas = !r.IsDBNull(r.GetOrdinal("dia_gas")) ? r.GetDateTime(r.GetOrdinal("dia_gas")) : default,
                    Tag = !r.IsDBNull(r.GetOrdinal("tag")) ? r.GetString(r.GetOrdinal("tag")) : string.Empty,
                    FechaIni = !r.IsDBNull(r.GetOrdinal("fecha_Ini")) ? r.GetDateTime(r.GetOrdinal("fecha_Ini")) : default,
                    FechaFin = !r.IsDBNull(r.GetOrdinal("fecha_Fin")) ? r.GetDateTime(r.GetOrdinal("fecha_Fin")) : default,
                    Value = !r.IsDBNull(r.GetOrdinal("value")) ? r.GetDouble(r.GetOrdinal("value")) : default,
                    Status = !r.IsDBNull(r.GetOrdinal("estatus")) ? r.GetInt32(r.GetOrdinal("estatus")) : default,
                    Description = !r.IsDBNull(r.GetOrdinal("descripcion")) ? r.GetString(r.GetOrdinal("descripcion")) : string.Empty
                });

            }
            catch (Exception ex)
            {
                string msgError = $"No se pudo leer la medición de {(tag.Contains(TypeMeasure.ENERGIA) ? TypeMeasure.ENERGIA : TypeMeasure.VOLUMEN)} con los siguientes parametros: {{Fecha: {dateSearch}, Tag: {tag}, Exception: {ex.Message}}}";
                logger.LogDebug(msgError);
                throw new Exception($"Read Data ENERGIA-PI with parameters:\n {{DateSearch: {dateSearch}, Tag: {tag}}}", ex);
            }
            return result.ToList();
        }
        public async static Task<List<DataTagVolumenTask>> ReadDataVolumen(DbContext dbContext, string dateSearch, string tag, ILogger<IJob> logger)
        {
            IQueryable<DataTagVolumenTask> result = default;
            try
            {
                using IDataTemplate dataTemplate = new SqlServerDataTemplate(dbContext.Database.GetDbConnection());
                await dataTemplate.ComplexStoreProcedureAsync(StoreProcedures.SP_API_GET_TAG_VOLUMEN, new DataParameter[]
                {
                            new DataParameter
                            {
                                Name="@fechaInicial",
                                Direction=ParameterDirection.Input,
                                Type=SqlDbType.VarChar,
                                Value=dateSearch
                            },
                            new DataParameter
                            {
                                Name="@tag",
                                Direction= ParameterDirection.Input,
                                Type=SqlDbType.VarChar,
                                Value=tag
                            }
                }, typeExcec: GlobalEnum.TypeExecuteStoreSP.Read);
                result = dataTemplate.MapperReaderStoreProcedure(r => new DataTagVolumenTask
                {
                    DiaGas = !r.IsDBNull(r.GetOrdinal("dia_gas")) ? r.GetDateTime(r.GetOrdinal("dia_gas")) : default,
                    Tag = !r.IsDBNull(r.GetOrdinal("tag")) ? r.GetString(r.GetOrdinal("tag")) : string.Empty,
                    Fecha = !r.IsDBNull(r.GetOrdinal("fecha")) ? r.GetDateTime(r.GetOrdinal("fecha")) : default,
                    Value = !r.IsDBNull(r.GetOrdinal("value")) ? r.GetDouble(r.GetOrdinal("value")) : default,
                    Status = !r.IsDBNull(r.GetOrdinal("estatus")) ? r.GetInt32(r.GetOrdinal("estatus")) : default,
                    Description = !r.IsDBNull(r.GetOrdinal("descripcion")) ? r.GetString(r.GetOrdinal("descripcion")) : string.Empty
                });
            }
            catch (Exception ex)
            {
                string msgError = $"No se pudo leer la medición de {(tag.Contains(TypeMeasure.ENERGIA) ? TypeMeasure.ENERGIA : TypeMeasure.VOLUMEN)} con los siguientes parametros: {{Fecha: {dateSearch}, Tag: {tag}, Exception: {ex.Message}}}";
                logger.LogDebug(msgError);
                throw new Exception($"Read Data VOLUMEN-PI with parameters:\n {{DateSearch: {dateSearch}, Tag: {tag}}}", ex);
            }

            return result.ToList();
        }
    }
}

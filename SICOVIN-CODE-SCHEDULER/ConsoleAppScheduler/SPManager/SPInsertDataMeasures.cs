using Azure;
using ConsoleAppScheduler.Base.Common;
using ConsoleAppScheduler.Base.TemplateMapper;
using ConsoleAppScheduler.Jobs;
using ConsoleAppScheduler.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using System.Data;
using static ConsoleAppScheduler.Base.Common.Constants;

namespace ConsoleAppScheduler.SPManager
{
    public class SPInsertDataMeasures
    {
        public async static Task InsertData(DbContext dbContext, MedicionPIRequest medicion, ILogger<IJob> logger)
        {
            IQueryable<ResultMsg<string>> result = default;
            try
            {
                using IDataTemplate dataTemplate = new SqlServerDataTemplate(dbContext.Database.GetDbConnection());
                await dataTemplate.ComplexStoreProcedureAsync(Constants.StoreProcedures.SP_API_INSERT_MEASURES, new DataParameter[]
                {
                            new DataParameter
                            {
                                Name="@idMeasurePoint",
                                Direction=ParameterDirection.Input,
                                Type=SqlDbType.VarChar,
                                Value=medicion.IdMeasurePoint
                            },
                            new DataParameter
                            {
                                Name="@sTagRequested",
                                Direction= ParameterDirection.Input,
                                Type=SqlDbType.VarChar,
                                Value=medicion.TagRequested
                            },
                            new DataParameter
                            {
                                Name="@fTagValue",
                                Direction= ParameterDirection.Input,
                                Type=SqlDbType.Float,
                                Value=medicion.TagValue.ToString()
                            },
                            new DataParameter
                            {
                                Name="@DateDiaGas",
                                Direction= ParameterDirection.Input,
                                Type=SqlDbType.VarChar,
                                Value= medicion.DateDiaGas
                            },
                            new DataParameter
                            {
                                Name="@sExceptionCode",
                                Direction= ParameterDirection.Input,
                                Type=SqlDbType.Int,
                                Value=medicion.ExceptionCode.ToString()
                            },
                            new DataParameter
                            {
                                Name="@sStatus",
                                Direction= ParameterDirection.Input,
                                Type=SqlDbType.Int,
                                Value=medicion.Status.ToString()
                            },
                            new DataParameter
                            {
                                Name="@Description",
                                Direction= ParameterDirection.Input,
                                Type=SqlDbType.VarChar,
                                Value=medicion.Description
                            },
                            new DataParameter
                            {
                                Name="@idBalance",
                                Direction= ParameterDirection.Input,
                                Type=SqlDbType.VarChar,
                                Value=medicion.IdBalance
                            }
                            /**,
                            new DataParameter
                            {
                                Name="@sEntityNameCode",
                                Direction= ParameterDirection.Input,
                                Type=SqlDbType.VarChar,
                                Value=medicion.EntityNameCode
                            }**/
                }, typeExcec: GlobalEnum.TypeExecuteStoreSP.Read);
                result = dataTemplate.MapperReaderStoreProcedure(r => new ResultMsg<string>
                {
                    Value = r.IsDBNull(r.GetOrdinal("sResultCode")) ? default : r.GetString(r.GetOrdinal("sResultCode")),
                    ErrorCode = 0,
                    Message = !r.IsDBNull(r.GetOrdinal("sResultMsg")) ? r.GetString(r.GetOrdinal("sResultMsg")) : string.Empty,
                    Messages = new List<string>
                    {
                        $"Entidad: ({medicion.EntityNameCode}), Fecha: {medicion.DateDiaGas}",
                        "Variables Adicionales:",
                        medicion.TagValue.ToString(),
                        medicion.IdBalance,
                        medicion.IdMeasurePoint,
                        medicion.Status.ToString(),
                        medicion.Description
                    }
                });
                if(result!=null && result.First().Value != "1")
                {
                    logger.LogError(JsonConvert.SerializeObject(result.First()));
                }
                else
                {
                    logger.LogInformation($"Result: {JsonConvert.SerializeObject(result.First())}");
                }
            }
            catch(Exception ex)
            {
                string msgError = $"No se inserto medición de {(medicion.TagRequested.Contains(TypeMeasure.ENERGIA) ? TypeMeasure.ENERGIA : TypeMeasure.VOLUMEN) } con los siguientes parametros: {{Fecha: {medicion.DateDiaGas}, Tag: {medicion.TagRequested}, Valor: {medicion.TagValue}, Exception: {ex.Message}}}";
                logger.LogDebug(msgError);
                throw new Exception(JsonConvert.SerializeObject(medicion), ex);
            }
            
        }
    }
}

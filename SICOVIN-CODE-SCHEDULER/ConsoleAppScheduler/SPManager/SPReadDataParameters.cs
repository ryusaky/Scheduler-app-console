using Azure;
using ConsoleAppScheduler.Base.Common;
using ConsoleAppScheduler.Base.TemplateMapper;
using ConsoleAppScheduler.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleAppScheduler.SPManager
{
    public class SPReadDataParameters
    {
        public async static Task<List<ParameterTask>> ReadParameters(DbContext context)
        {
            IQueryable<ParameterTask> result = default;
            try
            {
                using IDataTemplate dataTemplate = new SqlServerDataTemplate(context.Database.GetDbConnection());
                await dataTemplate.ComplexStoreProcedureAsync(Constants.StoreProcedures.SP_API_GET_LOAD_PARAMETERS,
                     typeExcec: GlobalEnum.TypeExecuteStoreSP.Read);

                result = dataTemplate.MapperReaderStoreProcedure(r => new ParameterTask
                {
                    IdMeasurePoint = !r.IsDBNull(r.GetOrdinal("idMeasurePoint")) ? r.GetGuid(r.GetOrdinal("idMeasurePoint")) : default,
                    JobId = Guid.NewGuid().ToString(),
                    TipoPuntoMedicion = !r.IsDBNull(r.GetOrdinal("TIPO PUNTO MEDICION")) ? r.GetString(r.GetOrdinal("TIPO PUNTO MEDICION")) : string.Empty,
                    ExecutionHours = !r.IsDBNull(r.GetOrdinal("iExecutionHour")) ? r.GetInt32(r.GetOrdinal("iExecutionHour")) : default,
                    ExecutionMinutes = !r.IsDBNull(r.GetOrdinal("iExecutionMinute")) ? r.GetInt32(r.GetOrdinal("iExecutionMinute")) : default,
                    ExceptionExecutionHourInterval = !r.IsDBNull(r.GetOrdinal("iExceptionExecutionHourInterval")) ? r.GetInt32(r.GetOrdinal("iExceptionExecutionHourInterval")) : default,
                    ExceptionExecutionMinuteInterval = !r.IsDBNull(r.GetOrdinal("iExceptionExecutionMinuteInterval")) ? r.GetInt32(r.GetOrdinal("iExceptionExecutionMinuteInterval")) : default,
                    TagExecutionWeekDays = !r.IsDBNull(r.GetOrdinal("sTagExecutionWeekdays")) ? r.GetString(r.GetOrdinal("sTagExecutionWeekdays")) : string.Empty,
                    IdBalance = !r.IsDBNull(r.GetOrdinal("idBalance")) ? r.GetGuid(r.GetOrdinal("idBalance")) : default,
                    EntityNameCode = !r.IsDBNull(r.GetOrdinal("sEntityNameCode")) ? r.GetString(r.GetOrdinal("sEntityNameCode")) : string.Empty

                });
            }
            catch(Exception ex)
            {
                throw new Exception($"{Constants.StoreProcedures.SP_API_GET_LOAD_PARAMETERS}", ex);
            }
            return result?.ToList();
        }
    }
}

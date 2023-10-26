using SICOVIN_CODE_SCHEDULER.Base.Common;
using System.Data.Common;

namespace SICOVIN_CODE_SCHEDULER.Base.TemplateMapper
{
    public interface IDataTemplate : IDisposable
    {
        void ComplexStoreProcedure(string nameStoreProcedure,
            DataParameter[] parameters,
            GlobalEnum.TypeExecuteStoreSP typeExcec = GlobalEnum.TypeExecuteStoreSP.Normal);

        Task ComplexStoreProcedureAsync(string nameStoreProcedure,
           DataParameter[] parameters,
           GlobalEnum.TypeExecuteStoreSP typeExcec = GlobalEnum.TypeExecuteStoreSP.Normal);

        Task ComplexStoreProcedureAsync(string nameStoreProcedure,
           GlobalEnum.TypeExecuteStoreSP typeExcec = GlobalEnum.TypeExecuteStoreSP.Normal);

        void AddParameters(DbCommand command, DataParameter[] parameters);
        IQueryable<T> MapperReaderStoreProcedure<T>(Func<DbDataReader, T> rowMapperDelegate);
    }
}

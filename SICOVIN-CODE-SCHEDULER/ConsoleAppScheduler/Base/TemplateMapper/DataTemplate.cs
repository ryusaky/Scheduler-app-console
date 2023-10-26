using System.Data.Common;
using System.Data;
using static ConsoleAppScheduler.Base.Common.GlobalEnum;

namespace ConsoleAppScheduler.Base.TemplateMapper
{
    public class DataTemplate : IDataTemplate
    {
        public DbConnection Connection { get; private set; }
        public DbCommand Command { get; private set; }
        public DbDataReader Reader { get; private set; }
        public DbTransaction Transaction { get; private set; }
        protected bool IsContext { get; set; }
        public object ValueEscalar { get; set; }
        public int RowsAfected { get; set; }
        public DataTemplate(DbConnection connection)
        {
            IsContext = false;
            Connection = connection;
            if (connection == null)
            {
                throw new Exception("No se pudo establecer la conexión");
            }
            connection.Open();
        }
        public DataTemplate(DbConnection connection, bool open)
        {
            IsContext = false;
            Connection = connection;
            if (connection == null)
            {
                throw new Exception("No se pudo establecer la conexión");
            }
            if (open)
                connection.Open();
        }
        public virtual void AddParameters(DbCommand command, DataParameter[] parameters)
        {
            foreach (DataParameter param in parameters)
            {
                DbParameter parameter = command.CreateParameter();
                parameter.ParameterName = param.Name;
                parameter.DbType = (DbType)param.Type;
                parameter.Value = param.Value;
                parameter.Direction = param.Direction;
                command.Parameters.Add(parameter);
            }
        }
        public virtual void ComplexStoreProcedure(string nameStoreProcedure, DataParameter[] parameters,
            TypeExecuteStoreSP typeExcec = TypeExecuteStoreSP.Normal)
        {
            Command = Connection.CreateCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandTimeout = 600;
            Command.CommandText = nameStoreProcedure;
            AddParameters(Command, parameters);
            switch (typeExcec)
            {
                case TypeExecuteStoreSP.Normal:
                    RowsAfected = Command.ExecuteNonQuery();
                    break;
                case TypeExecuteStoreSP.Escalar:
                    ValueEscalar = Command.ExecuteScalar();
                    break;
                case TypeExecuteStoreSP.Read:
                    Reader = Command.ExecuteReader();
                    break;
            }
        }

        public virtual async Task ComplexStoreProcedureAsync(string nameStoreProcedure, DataParameter[] parameters,
            TypeExecuteStoreSP typeExcec = TypeExecuteStoreSP.Normal)
        {
            Command = Connection.CreateCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandTimeout = 600;
            Command.CommandText = nameStoreProcedure;
            AddParameters(Command, parameters);
            switch (typeExcec)
            {
                case TypeExecuteStoreSP.Normal:
                    RowsAfected = await Command.ExecuteNonQueryAsync();
                    break;
                case TypeExecuteStoreSP.Escalar:
                    ValueEscalar = await Command.ExecuteScalarAsync();
                    break;
                case TypeExecuteStoreSP.Read:
                    Reader = await Command.ExecuteReaderAsync();
                    break;
            }
        }

        public virtual async Task ComplexStoreProcedureAsync(string nameStoreProcedure,
            TypeExecuteStoreSP typeExcec = TypeExecuteStoreSP.Normal)
        {
            Command = Connection.CreateCommand();
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandText = nameStoreProcedure;
            switch (typeExcec)
            {
                case TypeExecuteStoreSP.Normal:
                    RowsAfected = await Command.ExecuteNonQueryAsync();
                    break;
                case TypeExecuteStoreSP.Escalar:
                    ValueEscalar = await Command.ExecuteScalarAsync();
                    break;
                case TypeExecuteStoreSP.Read:
                    Reader = await Command.ExecuteReaderAsync();
                    break;
            }
        }

        public void Dispose()
        {
            if (!IsContext)
            {
                Transaction?.Dispose();

                if (Reader != null)
                {
                    Reader.Close();
                    Reader.Dispose();
                }

                Command?.Dispose();

                Connection?.Close();
            }
        }
        public virtual IQueryable<T> MapperReaderStoreProcedure<T>(Func<DbDataReader, T> rowMapperDelegate)
        {
            if (Reader != null)
            {
                List<T> result = new List<T>();
                while (Reader.Read())
                {
                    result.Add(rowMapperDelegate(Reader));
                }
                return result.AsQueryable();
            }
            return new List<T>().AsQueryable();
        }
    }
}

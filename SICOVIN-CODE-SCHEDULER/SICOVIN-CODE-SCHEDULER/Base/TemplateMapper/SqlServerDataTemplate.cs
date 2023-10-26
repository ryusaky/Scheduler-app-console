using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace SICOVIN_CODE_SCHEDULER.Base.TemplateMapper
{
    public class SqlServerDataTemplate : DataTemplate
    {
        public SqlServerDataTemplate(DbConnection connection) : base(connection)
        {
        }

        public SqlServerDataTemplate(DbConnection connection, bool open) : base(connection, open)
        {
        }
        public override void AddParameters(DbCommand command, DataParameter[] parameters)
        {
            foreach (DataParameter param in parameters)
            {
                SqlCommand cmd = (SqlCommand)command;
                SqlParameter parameter = cmd.CreateParameter();
                parameter.ParameterName = param.Name;
                parameter.SqlDbType = (SqlDbType)param.Type;
                parameter.Direction = param.Direction;
                if (param.IsNullable != null)
                {
                    parameter.IsNullable = param.IsNullable.Value;
                }
                if (param.Size != null)
                {
                    parameter.Size = param.Size.Value;
                }
                if (param.DefaultValue != null)
                {
                    parameter.Value = param.DefaultValue;
                }
                if (param.Direction == ParameterDirection.Input)
                {
                    parameter.Value = param.Value;
                }
                command.Parameters.Add(parameter);
            }
        }
    }
}

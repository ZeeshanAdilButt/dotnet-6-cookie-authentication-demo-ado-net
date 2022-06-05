using CookiesAuthenticationPOC.Controllers;
using CookiesAuthenticationPOC.Infrastructure.Extensions;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace CookiesAuthenticationPOC.Infrastructure
{

    public class AccountInfrastructure : IAccountInfrastructure
    {
        public AccountInfrastructure(IConfiguration configuration)
        {

            ConnectionString = configuration["ConnectionString"];

        }
        private string authenticateProcedureName = "usp_AuthenticateUser";

        public string ConnectionString { get; }




        public async Task<bool> IsAuthenticated(User user)
        {

            string result = null;

            var parameters = new List<DbParameter>
            {
                this.GetParameter("Email", user.Email),
                this.GetParameter("Password", user.Password)
            };

            using (var dataReader = await this.ExecuteReader(parameters, authenticateProcedureName, CommandType.StoredProcedure))
            {
                if (dataReader != null && dataReader.HasRows)
                {
                    if (dataReader.Read())
                    {

                        result = dataReader.GetStringValue("UserID");

                    }
                }
            }

            return result != null;
        }


        #region helper methods TODO: refactor to some base class
        private DbConnection GetConnection()
        {
            DbConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;

        }

        private DbCommand GetCommand(DbConnection connection, string commandText, CommandType commandType, List<DbParameter> parameters)
        {
            var command = connection.CreateCommand();

            command.CommandText = commandText;
            command.CommandType = commandType;

            if (parameters != null && parameters.Count > 0)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            return command;

        }
        protected async Task<DbDataReader> ExecuteReader(List<DbParameter> parameters, string commandText, CommandType commandType = CommandType.StoredProcedure)
        {
            DbDataReader ds;

            try
            {
                //using (var connection = this.GetConnection())
                //{
                    var connection = this.GetConnection();
                    var cmd = this.GetCommand(connection, commandText, commandType, parameters);

                    ds = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                //}
            }
            catch (Exception ex)
            {
                //TODO; log
                throw ex;
            }

            return ds;

        }

        protected DbParameter GetParameter(string parameterName, object parameterValue)
        {
            DbParameter parameterObject = new SqlParameter(parameterName, parameterValue ?? DBNull.Value);

            parameterObject.Direction = ParameterDirection.Input;

            return parameterObject;
        }

        #endregion
    }
}

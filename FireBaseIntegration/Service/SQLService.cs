using FireBaseIntegration.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using static Google.Api.Gax.Grpc.Gcp.AffinityConfig.Types;

namespace FireBaseIntegration.Service
{
    public class SQLService
    {
        private readonly IConfiguration _configuration;
        public SQLService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public async Task<SQLPayload> InsertDestinationData(Destination des)
        {
            SQLPayload payload = new SQLPayload();
            string constring = _configuration["DataConnection:ConnectionString"];
            SqlCommand sqlCommand = new SqlCommand($"INSERT INTO [dbo].[Destination] ([Id], [Code], [lineNo], [location], [New_Quantity], [User], [Update_Date], [Update_Time])    SELECT      NEXT VALUE FOR destination_sequence , '{des.Code}', '{des.lineNo}', '{des.location}', '{des.New_Quantity}', '{des.User}', '{des.Update_Date.ToString("yyyy-MM-dd")}', '{des.Update_Time.ToString("yyyy-MM-dd")}'");

            using (var sql1 = new SqlConnection(constring))
            {
                try
                {
                    sql1.Open();
                    sqlCommand.Connection = sql1;
                    await sqlCommand.ExecuteNonQueryAsync();
                    payload.status = true;
                }
                catch (Exception ex)
                {
                    payload.sqlissue = ex.Message;
                    payload.status = false;
                }
                finally
                {
                    sql1.Close();
                    sqlCommand = null;
                }
            }
            return payload;
        }
    }
}

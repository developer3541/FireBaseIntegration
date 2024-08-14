using FireBaseIntegration.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Data;
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
            SqlCommand sqlcmd = new SqlCommand($"SELECT * FROM [dbo].[Destination] WHERE [Code] = '{des.Code}' AND [lineNo] = '{des.lineNo}' ");
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand sqlCommand = new SqlCommand();
            using (var sql = new SqlConnection(constring))
            {
                try
                {
                    sql.Open();
                    sqlcmd.Connection = sql;
                    adapter.SelectCommand = sqlcmd;
                    adapter.Fill(dt);
                }
                catch (Exception ex)
                {
                    payload.sqlissue = ex.Message;
                    payload.status = false;
                }
                finally
                {
                    sql.Close();
                    sqlcmd = null;
                }
            }
            if (dt.Rows.Count == 1)
            {
                sqlCommand = new SqlCommand($"UPDATE [dbo].[Destination] SET [location] = '{des.location}', [New_Quantity]='{des.New_Quantity}', [User]='{des.User}', [Update_Date]='{des.Update_Date.ToString("yyyy-MM-dd")}', [Update_Time] = '{des.Update_Time.ToString("yyyy-MM-dd HH:mm")}' WHERE [Code] = '{des.Code}' and [lineNo]= '{des.lineNo}'");
            }
            else
            {
                sqlCommand = new SqlCommand($"INSERT INTO [dbo].[Destination] ([Id], [Code], [lineNo], [location], [New_Quantity], [User], [Update_Date], [Update_Time])    SELECT      NEXT VALUE FOR destination_sequence , '{des.Code}', '{des.lineNo}', '{des.location}', '{des.New_Quantity}', '{des.User}', '{des.Update_Date.ToString("yyyy-MM-dd")}', '{des.Update_Time.ToString("yyyy-MM-dd HH:mm")}'");
            }

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

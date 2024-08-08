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
        public async Task<bool> InsertDestinationData(Destination des)
        {
            Destination dest = des;
            string constring = _configuration["DataConnection:ConnectionString"];
            SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO Destination (Code, lineNo, location, New_Quantity, User, Update_Date, Update_Time)
                                     VALUES ( @Code, @LineNo, @Location, @New_Quantity, @User, @Update_Date, @Update_Time)");
            sqlCommand.Parameters.AddWithValue("@Code", dest.Code ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@LineNo", dest.lineNo ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@Location", dest.location ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@New_Quantity", dest.New_Quantity ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@User", dest.User ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@Update_Date", dest.Update_Date);
            sqlCommand.Parameters.AddWithValue("@Update_Time", dest.Update_Time);

            using (var sql1 = new SqlConnection(constring))
            {
                try
                {
                    sql1.Open();
                    sqlCommand.Connection = sql1;
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    sql1.Close();
                    sqlCommand = null;
                }
            }
            return true;
        }
    }
}

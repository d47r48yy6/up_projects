using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class DB_Conn
    {
        private readonly string _connection;
        public DB_Conn()
        {
        }

        public DB_Conn(IConfiguration configuration)
        {
            _connection = configuration.GetConnectionString("DefaultConnection");
        }
        public string GetConnection()
        {
            return Startup.ConnectionString;

        }

        //////////////////////////////////////////////////////////////////////////////////////Common procedure for create/delete/update
        public static string CUDProcedureExecute(string proc, object param)
        {
            string msg = "";
             DB_Conn c1 = new DB_Conn();
            try
            {                
                using (var conn = new SqlConnection(c1.GetConnection()))
                {
                    var alm = conn.QueryFirstOrDefault(proc, param, commandType: System.Data.CommandType.StoredProcedure);
                    msg = alm.Message;
                }             
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            
            finally
            {
                c1 = null;
            }

            return msg;
        }
        public static List<T> SelectProcedureExecute<T>(string proc, object param)
        {
            var result = (dynamic)null;
            DB_Conn c1 = new DB_Conn();
            try
            {
                using (var conn = new SqlConnection(c1.GetConnection()))
                {
                    result = conn.Query<T>(proc, param, commandType: System.Data.CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                c1 = null;
            }
            return result;
        }
    }
}

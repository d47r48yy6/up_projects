using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class Unit
    {
        DB_Conn c1 = new DB_Conn();
        public int Id { get; set; }
        public string Value { get; set; }
        ///////////////////////////////////////////////////////////Get Unit List.......
        public List<Unit> GetAllUnits(object param)
        {
            List<Unit> result = new List<Unit>();

            try
            {
                using (var conn = new SqlConnection(c1.GetConnection()))
                {

                    result = conn.Query<Unit>("[GetUnits]", param, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch
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

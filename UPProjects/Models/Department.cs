using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class Department
    {
        DB_Conn c1 = new DB_Conn();
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        ///////////////////////////////////////////////////////////Get Deparment List.......
        public List<Department> GetAllDepartment()
        {
            List<Department> result = new List<Department>();
            try
            {
                using (var conn = new SqlConnection(c1.GetConnection()))
                {

                    result = conn.Query<Department>("GetDepartments", null, commandType: CommandType.StoredProcedure).ToList();
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

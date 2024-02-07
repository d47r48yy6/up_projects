using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class MPR
    {

        public int Id { get; set; }
      
        [Display(Name = "Financial Year")]
        [Required, Range(1, int.MaxValue, ErrorMessage = "Please select Financial Year")]

        public string FinancialYear { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Please select Select")]

        [Display(Name = "Month")]
        public string Month { get; set; }

        [Required(ErrorMessage = "Please select File")]
        [Display(Name = "File")]
        public IFormFile file { get; set; }

        public string FilePath { get; set; }
        public string IsApproved  { get; set; }
        public string UploadOn { get; set; }
        public string UnitName { get; set; }
       

    }
 
    public class MPRDAL
    {
        private readonly string _connection;

        public MPRDAL()
        {
        }

        public MPRDAL(IConfiguration configuration)
        {
            _connection = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<List<MPR>> GetModuleAsync(string StoredProc, object parameter)
        {
            var Result = (dynamic)null;
            using (var con = new SqlConnection(_connection))
            {
                await con.OpenAsync();
                Result = await con.QueryAsync<MPR>(StoredProc, parameter, null, null, commandType: CommandType.StoredProcedure);
                await con.CloseAsync();
            }
            return Result;
        }
        public  List<MPR> GetListofPendingMPR(string StoredProc, object parameter)
        {
            var Result = (dynamic)null;
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    con.Open();
                    Result = con.Query<MPR>(StoredProc, parameter, null, commandType: CommandType.StoredProcedure).ToList();
                    con.Close();
                }
            }
            catch (Exception ex)
            { throw ex; }
            return Result;
        }
    }
}

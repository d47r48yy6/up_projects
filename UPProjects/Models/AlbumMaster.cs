using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace UPProjects.Models
{
    public class AlbumMaster
    {
         DB_Conn c1 = new DB_Conn();
        public int Id { get; set; }
        [DisplayName("Album Title (in English)")]
        [Required(ErrorMessage ="Please enter album title")]
        [StringLength(200, ErrorMessage = "Please enter Album Title.", MinimumLength = 1)]
        public string AlbumTitle { get; set; }

        [DisplayName("Album Title (in Hindi)")]
        [StringLength(200, ErrorMessage = "Please enter Album.", MinimumLength = 1)]
        public string HNAlbumTitle { get; set; }
        public string UserId { get; set; }
        public string UserIP { get; set; }
        //////////////////////////////////////////////////////////////////////////////////////Get All Album List 
        public List<AlbumMaster> GetAllAlbumList()
        {
            List<AlbumMaster> result = new List<AlbumMaster>();
            using (var conn = new SqlConnection(c1.GetConnection()))
            {
                result = conn.Query<AlbumMaster>("[GetAllAlbum]", null, commandType: CommandType.StoredProcedure).ToList();
            }
            return result;
        }
        //////////////////////////////////////////////////////////////////////////////////////Get  Album By Id 
        public AlbumMaster GetAlbumById(int Id)
        {
            List<AlbumMaster> result = new List<AlbumMaster>();
            AlbumMaster result1 = new AlbumMaster();
            var param = new { Id = Id };
            using (var conn = new SqlConnection(c1.GetConnection()))
            {
                result = conn.Query<AlbumMaster>("[GetAllAlbumById]", param, commandType: CommandType.StoredProcedure).ToList();
                result1 = result.FirstOrDefault();
            }
            return result1;
        }

        /////////////////////////////////////////////////////////////////////////////////////////Delete Project
        public string DeleteAlbumData(int Id,string userid)
        {
            string result = "NA";
            var param = new { Id = Id, UserId=userid, UserIP=AppCommonMethod.GetIP()};
            using (var conn = new SqlConnection(c1.GetConnection()))
            {
                try
                {
                   var m= conn.QueryFirstOrDefault("[DeleteAlbum]", param, commandType: CommandType.StoredProcedure);
                    if(m.statuscode=="t")
                    {
                        result = "Album Title has been deleted.";
                    }
                    else
                    {
                        result = "Album not deleted because it has photos associated with it.";
                    }
                  
                }
                catch
                {
                    result = "Album Title not deleted.";
                }


            }
            return result;
        }


    }
}

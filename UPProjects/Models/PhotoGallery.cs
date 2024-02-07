using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace UPProjects.Models
{
    public class PhotoGallery
    {
        DB_Conn c1 = new DB_Conn();
        public string Id { get; set; }

        //[Required(ErrorMessage = "Please enter Title.")]
        //[StringLength(50, ErrorMessage = "Please enter Title.", MinimumLength = 1)]
        //[DataType(DataType.Text)]
        //[Display(Name = "Title (Max Size: 50 Character)")]
        public string Title { get; set; }
       // [Required(ErrorMessage = "Please enter Description.")]
        [StringLength(100, ErrorMessage = "Please enter Description.", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [Display(Name = "Description about Photo (Max Size: 100 Character)")]
        public string Description { get; set; }       
        public string FileName { get; set; }
        public IFormFile file { get; set; }

        public string AlbumTitleName { get; set; }
        public string HNAlbumTitleName { get; set; }

        public int AlbumId { get; set; }

        ///////////////////////////////////////////////////////////Get Deparment List.......
        public List<AlbumTitle> GetAllAlbum()
        {
            List<AlbumTitle> result = new List<AlbumTitle>();
            try
            {
                using (var conn = new SqlConnection(c1.GetConnection()))
                {

                    result = conn.Query<AlbumTitle>("GetAlbumGallary", null, commandType: CommandType.StoredProcedure).AsList();
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
    public class AlbumTitle
    {
        public int Id { get; set; }
        public string  Value { get; set; }
    }


}

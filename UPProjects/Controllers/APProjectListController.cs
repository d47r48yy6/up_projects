using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPProjects.Entities;
using UPProjects.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace UPProjects.Controllers
{
    [Authorize(AuthenticationSchemes = AuthSchemes)]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class APProjectListController : Controller
    {
        private const string AuthSchemes =
        CookieAuthenticationDefaults.AuthenticationScheme + "," +
        JwtBearerDefaults.AuthenticationScheme;
        private readonly DAL dAL;
        private readonly AppCommonMethod acm;
        private readonly IWebHostEnvironment _env;
        public APProjectListController(DAL _dAL, AppCommonMethod _acm, IWebHostEnvironment env)
        {
            dAL = _dAL;
            acm = _acm;
            _env = env;

        }

        [HttpPost]
      
        public async Task<List<ProjectDetails>> ProjectList(ExpandoObject expando)
        {
            //var data = (dynamic)null;
            List<ProjectDetails> data = new List<ProjectDetails>();
            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new {
                    UserId = expandoDict["UserId"].ToString()==""?null:expandoDict["UserId"].ToString()
                };
                data =  dAL.GetProjectList("App_GetProjectList", param);
                if(data.Count<=0)
                {
                    data = new List<ProjectDetails>();
                }
                
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "","");
            }
            return data;
        }

        [HttpPost]

        public async Task<List<ProjectDetails>> ProjectListById(ExpandoObject expando)
        {
            //var data = (dynamic)null;
            List<ProjectDetails> data = new List<ProjectDetails>();
            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    ProjectId = expandoDict["ProjectId"].ToString() == "" ? null : expandoDict["ProjectId"].ToString(),
                  
                };
                data = dAL.GetProjectList("App_GetComleteProjectList", param);

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }

        [HttpPost]
        public async Task<Object> ProgressPhotoUploadPhoto(ExpandoObject expando)
        {
            Result result = new Result();
            var innerresult = (dynamic)null;
            string FileName1 = "";
            IFormFile file = (dynamic)null;
            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var description = expandoDict["description"].ToString();
                var FileName = expandoDict["FileName"].ToString();
                var UserId = expandoDict["UserId"].ToString();
                var UserIP = expandoDict["IPAddress"].ToString();
                var UnitId = expandoDict["UnitId"].ToString();
                var ZoneId = expandoDict["ZoneId"].ToString();
                var Category = expandoDict["Category"].ToString();
                var ProjectId = expandoDict["ProjectId"].ToString();
                var Latitude = expandoDict["Latitude"].ToString();
                var Longtitude = expandoDict["Longitude"].ToString();

                //  FileName1 = FileName.Split('.')[0] + DateTime.Now.Ticks + "." + FileName.Split('.')[1].ToString();
                var unqid = Guid.NewGuid();
                FileName1 = FileName;



                var param = new
                {

                    Description = description == "" ? null : description,
                    FileName = unqid+".jpg",
                    UserId = UserId,
                    IPAddress = UserIP == "" ? null : UserIP,
                    UnitId = UserId == "" ? null : UnitId,
                    ZoneId = ZoneId == "" ? null : ZoneId,
                    Category = Category,
                    ProjectId = ProjectId,
                    Lat = Latitude,
                    Longi = Longtitude
                };


                innerresult = dAL.QueryWithExecuteAsync("App_InsertProgressUpload", param);
                if (Convert.ToInt32(innerresult.Status) > 0)
                {
                   
                    
                    
                    if (FileName1 != null)
                    {
                        string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/ProgressPhoto");
                        //if (!Directory.Exists(RootFolderPath))
                        //{
                        //    Directory.CreateDirectory(RootFolderPath);
                        //}
                        //  string FilePath = Path.Combine(RootFolderPath, FileName1);
                        var Ids = Convert.ToString(innerresult.Status);

                       
                        SaveBase64ImagesMultiple(Ids, unqid.ToString(), FileName1.ToString());




                    }

                }

                result.Status = "T";
                result.Message = innerresult.Message;



            }
            catch (Exception ex)
            {
                result.Status = "F";
                result.Message = innerresult.Message;
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }

            return result;
        }

        public string SaveBase64ImagesMultiple(string id,string rand, string images)
        {
            try
            {
                var folderPath = Path.Combine(_env.WebRootPath, "Upload/ProgressPhoto/"+ id);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

               
                System.IO.File.WriteAllBytes(Path.Combine(folderPath, rand + ".jpg"), Convert.FromBase64String(images));
               
                return "Images Uploaded Successfully";
            }
            catch (Exception ex)
            {
                return "No Image Uploaded";
            }

        }

        public class Result
        {
            public string Status { get; set; }
            public string Message { get; set; }
        }


    }
}
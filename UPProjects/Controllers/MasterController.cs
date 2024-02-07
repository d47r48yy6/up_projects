using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UPProjects.Models;

namespace UPProjects.Controllers
{
    public class MasterController : Controller
    {
        private readonly DAL dAL;
        private readonly AppCommonMethod acm;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration configuration;

        public MasterController(DAL _dAL, AppCommonMethod _acm, IWebHostEnvironment env, IConfiguration _configuration)
        {
            dAL = _dAL;
            acm = _acm;
            _env = env;
            configuration = _configuration;

        }
        [Authorize(Roles = "admin,unit,gm")]
        public IActionResult ProjectMaster(int id)
        {
            ProjectMaster mt = new ProjectMaster();
            var unitparam = new { UnitId = acm.CheckZero(this.User.Claims.First(c => c.Type == "UnitId").Value.ToString()), ZoneId = acm.CheckZero(this.User.Claims.First(c => c.Type == "ZoneId").Value.ToString()), Role = this.User.Claims.First(c => c.Type == "Role").Value.ToString() };
            try
            {
                ViewBag.List = mt.GetAllProjectList(unitparam);
                if (id == 0 || id == null)
                {

                }
                else
                {
                    mt = mt.GetProjectById(id);
                }
            }         
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(mt);
        }

        
        public JsonResult BindDepartment()
        {
            Department mt = new Department();
            var data = (dynamic)null;
            try
            {
                 data = mt.GetAllDepartment();
            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }         
            return Json(data);
        }
        public JsonResult BindUnits()
        {
            Unit ut = new Unit();
            List<Unit> data = new List<Unit>();
            try
            {
                var unitparam = new { UnitId = acm.CheckZero(this.User.Claims.First(c => c.Type == "UnitId").Value.ToString()), ZoneId = acm.CheckZero(this.User.Claims.First(c => c.Type == "ZoneId").Value.ToString()), Role = this.User.Claims.First(c => c.Type == "Role").Value.ToString() };
                data = ut.GetAllUnits(unitparam);

            }
            
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }
        public JsonResult BindMonth()
        {
            ProjectMaster mt = new ProjectMaster();
            List<ProgressMonth> pm = new List<ProgressMonth>();
            var data = (dynamic)null;
            try
            {
                  data = mt.GetProgressMonth();
            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }          
            return Json(data);
        }
        public JsonResult BindYear()
        {
            ProjectMaster mt = new ProjectMaster();
            List<ProgressYear> pm = new List<ProgressYear>();
            var data = (dynamic)null;
            try
            {
                data = mt.GetProgressYear();
            }
              catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }
        public async Task<JsonResult> GetProjects(string unit)
        {
            List<CommonSelect> objProjects = new List<CommonSelect>();
            var projectparam = new { UnitId = unit };
            try
            {
                objProjects = await dAL.BindSelect("GetProjects", projectparam);
                objProjects.Insert(0, new CommonSelect { Id = "0", Value = "Select Project" });
            }
          
           catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(objProjects);
        }
        public async Task<JsonResult> GetHNProjects(string unit)
        {
            List<CommonSelect> objProjects = new List<CommonSelect>();
            var projectparam = new { UnitId = unit };
            try
            {
                objProjects = await dAL.BindSelect("GetHNProjects", null);
                objProjects.Insert(0, new CommonSelect { Id = "0", Value = "Select Project" });
            }

            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(objProjects);
        }
        public JsonResult SaveProject(ProjectMaster obj)
        {
            dynamic msg = (null);
           
            ProjectMaster mt = new ProjectMaster();
            var UserRole = this.User.Claims.First(c => c.Type == "Role").Value.ToString();
            try
            {
                msg = mt.InsertProjectMaster(obj, UserRole);
            }
          
             catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(msg);
        }
        public JsonResult DeleteProject(int Id)
        {
            dynamic msg = (null);
            var UserRole = this.User.Claims.First(c => c.Type == "Role").Value.ToString();
            ProjectMaster mt = new ProjectMaster();
            try
            {
                msg = mt.DeleteProjects(Id, UserRole);
            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }

         
            return Json(msg);
        }

        [Authorize(Roles = "admin,unit")]
        public IActionResult AlbumMaster(int id)
        {
            AlbumMaster mt = new AlbumMaster();
            try
            {
                ViewBag.List = mt.GetAllAlbumList();
                if (id == 0 || id == null)
                {
                    ViewBag.ButtonText = "Submit";
                }
                else
                {
                    mt = mt.GetAlbumById(id);
                    ViewBag.ButtonText = "Update";
                }
            }
           
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(mt);
        }
        [HttpPost]
        public async Task<IActionResult> AlbumMaster(AlbumMaster obj)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            if (ModelState.IsValid)
            {
                try
                {

                    var param = new
                    {
                        Id = obj.Id,
                        AlbumTitle = obj.AlbumTitle,
                        HNAlbumTitle=obj.HNAlbumTitle,
                        UserId = this.User.Claims.First(c => c.Type == "Role").Value.ToString(),
                        UserIP = HttpContext.Connection.RemoteIpAddress.ToString()

                    };
                    innerresult = dAL.QueryWithExecuteAsync("InsertAlbum", param);
                   

                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "New Progress Photo Upload"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                    await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }
       
         public JsonResult DeleteAlbum(int Id)
          {
            dynamic msg = (null);
            AlbumMaster mt = new AlbumMaster();
            var UserRole = this.User.Claims.First(c => c.Type == "Role").Value.ToString();
            try
            {
                msg = mt.DeleteAlbumData(Id, UserRole);
            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }

          
            return Json(msg);
        }

        [Authorize(Roles = "admin,srv")]
        public IActionResult NewsMaster(int id)
        {
            NewsMaster nt = new NewsMaster();
            List<NewsMaster> data = new List<NewsMaster>();          
            var param = new { Id = id };
            try
            {
                if (id == 0 || id == null)
                {
                    nt.Id = 0;
                    ViewBag.ButtonText = "Submit";
                }
                else
                {
                    data = DB_Conn.SelectProcedureExecute<NewsMaster>("GetNewsDetails", param);
                    nt = data.FirstOrDefault();
                    ViewBag.ButtonText = "Update";
                }
            }
            
             catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(nt);
        }
        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public IActionResult NewsMaster(NewsMaster photoup)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            var newscont = "";
           
            
           if (ModelState.IsValid)
            {
                try
                {
                    if (photoup.file != null)
                    {
                        FileName = photoup.file.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + photoup.file.FileName.Split('.')[1].ToString();
                        FileName = photoup.file.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + photoup.file.FileName.Split('.')[1].ToString();
                        var extension = Path.GetExtension(photoup.file.FileName);

                        ///Find Length in Kb and takes two deciaml points
                        var size = (photoup.file.Length) / 1024;
                        

                        var param = new
                        {
                            Id = photoup.Id,
                            NewsTitle = photoup.NewsTitle,
                            HNNewsTitle = photoup.HNNewsTitle,
                            NewsContent = FileName,
                            HNNewsContent = FileName,
                            FileType = extension,
                            FileSize = size,
                            ContentType = photoup.ContentType,
                            UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("Role").Value,
                            UserIP = HttpContext.Connection.RemoteIpAddress.ToString(),
                        };
                        innerresult = dAL.QueryWithExecuteAsync("InsertNews", param);
                    }
                    else
                    {
                        var param = new
                        {
                            Id = photoup.Id,
                            NewsTitle = photoup.NewsTitle,
                            HNNewsTitle = photoup.HNNewsTitle,
                            NewsContent =photoup.NewsContent,
                            HNNewsContent = photoup.NewsContent,
                            ContentType = photoup.ContentType,
                            UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("Role").Value,
                            UserIP = HttpContext.Connection.RemoteIpAddress.ToString(),
                        };
                        innerresult = dAL.QueryWithExecuteAsync("InsertNews", param);
                    }
                       
                  
                    
                        if (photoup.file != null)
                        {
                            string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/NewsContent/" + innerresult.Status);
                            if (!Directory.Exists(RootFolderPath))
                            {
                                Directory.CreateDirectory(RootFolderPath);
                            }
                            string FilePath = Path.Combine(RootFolderPath, FileName);
                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                photoup.file.CopyTo(stream);
                            }
                        
                    }

                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                     acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }

        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> GetNewsMaster()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id =0};
                data = await dAL.QueryAsync("GetNewsDetails", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }
        public JsonResult DeleteNews(int Id)
        {
            dynamic msg = (null);
            var UserRole = this.User.Claims.First(c => c.Type == "Role").Value.ToString();
            var param = new { Id = Id, UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value, UserIP = HttpContext.Connection.RemoteIpAddress.ToString() };
            try
            {
                msg = DB_Conn.CUDProcedureExecute("DeleteNews", param);
            }
          
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(msg);
        }

        [Authorize(Roles = "admin,srv")]
        public IActionResult ServicesMaster(int id)
        {
            ServiceMaster nt = new ServiceMaster();
            List<ServiceMaster> data = new List<ServiceMaster>();

            var param = new { Id = id };
            try
            {
                if (id == 0 || id == null)
                {
                    nt.Id = 0;
                    ViewBag.ButtonText = "Submit";
                }
                else
                {
                    data = DB_Conn.SelectProcedureExecute<ServiceMaster>("[GetServicesDetails]", param);
                    nt = data.FirstOrDefault();
                    ViewBag.ButtonText = "Update";
                }
            }
           
             catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(nt);
        }
        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public IActionResult ServicesMaster(ServiceMaster photoup)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (photoup.file != null)
                        FileName = photoup.file.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + photoup.file.FileName.Split('.')[1].ToString();
                    var param = new
                    {
                        Id = photoup.Id,
                        Title = photoup.Title,
                        HNTitle=photoup.HNTitle,
                        FileName = FileName,
                        UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        UserIP = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    innerresult = dAL.QueryWithExecuteAsync("InsertServices", param);
                    if (Convert.ToInt32(innerresult.Status) > 0)
                    {
                        if (photoup.file != null)
                        {
                            string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/ServicesPhotos/" + innerresult.Status);
                            if (!Directory.Exists(RootFolderPath))
                            {
                                Directory.CreateDirectory(RootFolderPath);
                            }
                            string FilePath = Path.Combine(RootFolderPath, FileName);
                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                photoup.file.CopyTo(stream);
                            }
                        }
                    }

                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                     acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }
        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> GetServiceMaster()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                data = await dAL.QueryAsync("[GetServicesDetails]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        public JsonResult DeleteServices(int Id)
        {
            dynamic msg = (null);
            var param = new { Id = Id, UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value, UserIP = HttpContext.Connection.RemoteIpAddress.ToString() };
            try
            {
                msg = DB_Conn.CUDProcedureExecute("DeleteServices", param);
            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }

         
            return Json(msg);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Slider Master
        [Authorize(Roles = "admin,srv")]
        public IActionResult SliderMaster(int id)
        {
            SliderMaster nt = new SliderMaster();
            List<SliderMaster> data = new List<SliderMaster>();

            var param = new { Id = id };
            try
            {
                if (id == 0 || id == null)
                {
                    nt.Id = 0;
                    ViewBag.ButtonText = "Submit";
                }
                else
                {
                    data = DB_Conn.SelectProcedureExecute<SliderMaster>("[GetSliderDetails]", param);
                    nt = data.FirstOrDefault();
                    ViewBag.ButtonText = "Update";
                }
            }
           
             catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(nt);
        }

        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public IActionResult SliderMaster(SliderMaster photoup)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (photoup.file != null)
                        FileName = photoup.file.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + photoup.file.FileName.Split('.')[1].ToString();
                    var param = new
                    {
                        Id = photoup.Id,
                        MainTitle=photoup.MainTitle,
                        Title = photoup.Title,
                        HNMainTitle=photoup.HNMainTitle,
                        HNTitle=photoup.HNTitle,
                        FileName = FileName,
                        UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        UserIP = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    innerresult = dAL.QueryWithExecuteAsync("InsertSlider", param);
                    if (Convert.ToInt32(innerresult.Status) > 0)
                    {
                        if (photoup.file != null)
                        {
                            string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/SliderPhotos/" + innerresult.Status);
                            if (!Directory.Exists(RootFolderPath))
                            {
                                Directory.CreateDirectory(RootFolderPath);
                            }
                            string FilePath = Path.Combine(RootFolderPath, FileName);
                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                photoup.file.CopyTo(stream);
                            }
                        }
                    }

                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                     acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }

        public async Task<IActionResult> SetSliderPriority()
        {
            List<SliderMaster> data = new List<SliderMaster>();
            try
            {
                var param = new { Id = 0 };
               // data = await dAL.QueryAsync("[GetSliderDetails]", param);
                data = await dAL.SelectProcedureExecute<SliderMaster>("[GetSliderDetailsForPriority]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(data);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateBannerPriority(string Id, string Priority)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            try
            {



                var param = new
                {
                    Id = Id,
                    Priority = Priority == "" ? null : Priority,
                    UserId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value,
                    IPAddress = HttpContext.Connection.RemoteIpAddress.ToString()
                };
                innerresult = dAL.QueryWithExecuteAsync("UpdateBannerPriority", param);
                //var DynamicResult = new
                //{
                //    innerresult = innerresult,
                //    status = true,
                //    eventKey = "Insert"
                //};
               // Result = new { data = "", dynamicResult = DynamicResult };

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
            }
            return Json(innerresult);
        }

        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> GetSliderMaster()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                data = await dAL.QueryAsync("[GetSliderDetails]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        public JsonResult DeleteSlider(int Id)
        {
            dynamic msg = (null);
            var param = new { Id = Id, UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value, UserIP = HttpContext.Connection.RemoteIpAddress.ToString() };
            try
            {
                msg = DB_Conn.CUDProcedureExecute("DeleteSlider", param);
            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
     
            return Json(msg);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Slider Our Project master
        [Authorize(Roles = "admin,srv")]
        public IActionResult OurProjectMaster(int id)
        {
            OurProjectMaster nt = new OurProjectMaster();
            List<OurProjectMaster> data = new List<OurProjectMaster>();

            var param = new { Id = id };
            try
            {
                if (id == 0 || id == null)
                {
                    nt.Id = 0;
                    ViewBag.ButtonText = "Submit";
                }
                else
                {
                    data = DB_Conn.SelectProcedureExecute<OurProjectMaster>("[GetOurProjectDetails]", param);
                    nt = data.FirstOrDefault();
                    ViewBag.ButtonText = "Update";
                }
            }
           
             catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(nt);
        }
        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public IActionResult OurProjectMaster(OurProjectMaster photoup)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (photoup.file != null)
                        FileName = photoup.file.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + photoup.file.FileName.Split('.')[1].ToString();
                    var param = new
                    {
                        Id = photoup.Id,
                        ProjectId=photoup.ProjectId,
                        FileName = FileName,
                        UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        UserIP = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    innerresult = dAL.QueryWithExecuteAsync("InsertOurProject", param);
                    if (Convert.ToInt32(innerresult.Status) > 0)
                    {
                        if (photoup.file != null)
                        {
                            string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/OurProjectPhotos/" + innerresult.Status);
                            if (!Directory.Exists(RootFolderPath))
                            {
                                Directory.CreateDirectory(RootFolderPath);
                            }
                            string FilePath = Path.Combine(RootFolderPath, FileName);
                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                photoup.file.CopyTo(stream);
                            }
                        }
                    }

                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                     acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }

        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> GetOurProjectMaster()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                data = await dAL.QueryAsync("[GetOurProjectDetails]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        public  JsonResult DeleteOurProject(int Id)
        {
            dynamic msg = (null);
            var param = new { Id = Id, UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value, UserIP = HttpContext.Connection.RemoteIpAddress.ToString() };
            try
            {
                msg = DB_Conn.CUDProcedureExecute("DeleteOurProject", param);
            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
        

            return Json(msg);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////reset password
        [Authorize(Roles = "admin,srv")]
        public IActionResult ResetPassword()
        {
           
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> GetAllUser()
        {
            var data = (dynamic)null;
            try
            {
                data = await dAL.QueryAsync("[GetAllUser]",null);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        public JsonResult ResetuserPassword(int Id, string UserPassword)
        {          
            var param = new { Id = Id, @Password = MD5Encryption.getMd5Hash(UserPassword), UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value, IpAddress = HttpContext.Connection.RemoteIpAddress.ToString() };
            string msg = msg = DB_Conn.CUDProcedureExecute("ResetUserPassword", param);
            return Json(msg);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Web Directory master
        [Authorize(Roles = "admin,srv")]
        public IActionResult WebDirectoryMaster(int id)
        {
            WebDirectoryMaster nt = new WebDirectoryMaster();
            List<WebDirectoryMaster> data = new List<WebDirectoryMaster>();

            var param = new { Id = id };
            try
            {
                if (id == 0 || id == null)
                {
                    nt.Id = 0;
                    ViewBag.ButtonText = "Submit";
                }
                else
                {
                    data = DB_Conn.SelectProcedureExecute<WebDirectoryMaster>("[SelectWebDirectory]", param);
                    nt = data.FirstOrDefault();
                    ViewBag.ButtonText = "Update";
                }
            }
            
             catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(nt);
        }
        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public IActionResult WebDirectoryMaster(WebDirectoryMaster webobj)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            if (ModelState.IsValid)
            {
                try
                {
                    var param = new
                    {
                        Id = webobj.Id,
                        Name= webobj.Name,
                        Position= webobj.Position,
                        HNName=webobj.HNName,
                        HNPosition=webobj.HNPosition,
                        Address= webobj.Address,
                        HNAddress=webobj.HNAddress,
                        TelephoneNo = webobj.TelephoneNo,
                        CugNo= webobj.CugNo,
                        MobileNo= webobj.MobileNo,
                        EmailId= webobj.EmailId,
                        Categoryid= webobj.CategoryId,
                        ZoneId= webobj.ZoneId,
                        UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        UserIP = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    innerresult = dAL.QueryWithExecuteAsync("InsertWebDirectory", param);
                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                     acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }

        public async Task<JsonResult> GetCategory()
        {
            List<CommonSelect> objProjects = new List<CommonSelect>();
            try
            {
                objProjects = await dAL.BindSelect("GetWebDirectoryCategory", null);
                objProjects.Insert(0, new CommonSelect { Id = "0", Value = "Select Category" });
            }
            
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(objProjects);
        }

        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> GetWebDirectoryMaster()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                data = await dAL.QueryAsync("[SelectWebDirectory]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        public JsonResult DeleteWebDirectory(int Id)
        {
            dynamic msg = (null);
            var param = new { Id = Id, UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value, IpAddress = HttpContext.Connection.RemoteIpAddress.ToString() };
            try
            {
                msg = DB_Conn.CUDProcedureExecute("DeleteWebDirectory", param);
            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
       
            return Json(msg);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Video Master
        [Authorize(Roles = "admin,srv")]
        public IActionResult VideoMaster(int id)
        {
            VideoMaster nt = new VideoMaster();
            List<VideoMaster> data = new List<VideoMaster>();

            var param = new { Id = id };
            try
            {
                if (id == 0 || id == null)
                {
                    nt.Id = 0;
                    ViewBag.ButtonText = "Submit";
                }
                else
                {
                    data = DB_Conn.SelectProcedureExecute<VideoMaster>("[GetVideoDetails]", param);
                    nt = data.FirstOrDefault();
                    ViewBag.ButtonText = "Update";
                }
            }
           
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(nt);
        }
        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public IActionResult VideoMaster(VideoMaster photoup)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (photoup.file != null)
                        FileName = photoup.file.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + photoup.file.FileName.Split('.')[1].ToString();
                    var param = new
                    {
                        Id = photoup.Id,
                        Title = photoup.Title,
                        FileName = FileName,
                        UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        UserIP = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    innerresult = dAL.QueryWithExecuteAsync("InsertVideo", param);
                    if (Convert.ToInt32(innerresult.Status) > 0)
                    {
                        if (photoup.file != null)
                        {
                            string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/Videos/" + innerresult.Status);
                            if (!Directory.Exists(RootFolderPath))
                            {
                                Directory.CreateDirectory(RootFolderPath);
                            }
                            string FilePath = Path.Combine(RootFolderPath, FileName);
                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                photoup.file.CopyTo(stream);
                            }
                        }
                    }

                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                     acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }


        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> GetVideoMaster()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                data = await dAL.QueryAsync("[GetVideoDetails]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        public JsonResult DeleteVideos(int Id)
        {
            dynamic msg = (null);
            var param = new { Id = Id, UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value, UserIP = HttpContext.Connection.RemoteIpAddress.ToString() };
            try
            {
                msg = DB_Conn.CUDProcedureExecute("[DeleteVideo]", param);
            }
   
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(msg);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Feedback
        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public IActionResult FeedbackReport()
        {
            List<FeedbackMaster> data = new List<FeedbackMaster>();
            try
            {
                data = DB_Conn.SelectProcedureExecute<FeedbackMaster>("[GetAllFeedback]", null);
            }

            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(data);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Technical Sanction.
        [Authorize(Roles = "gm")]
        public IActionResult TechnicalSanction(int id)
        {
            TechnicalSanction nt = new TechnicalSanction();
            List<TechnicalSanction> data = new List<TechnicalSanction>();
            var param = new { Id = id };
            try
            {
                if (id == 0 || id == null)
                {
                    nt.Id = 0;
                    ViewBag.ButtonText = "Submit";
                }
                else
                {
                    data = DB_Conn.SelectProcedureExecute<TechnicalSanction>("GetTechnicalSanctionDetails", param);
                    nt = data.FirstOrDefault();
                    ViewBag.ButtonText = "Update";
                }
            }

            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(nt);
        }

        [HttpPost]
        [Authorize(Roles = "gm")]
        public IActionResult TechnicalSanction(TechnicalSanction photoup)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (photoup.file != null)
                        FileName = photoup.file.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + photoup.file.FileName.Split('.')[1].ToString();
                    var param = new
                    {
                        Id = photoup.Id,
                        FileName = FileName,
                        UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        UserIP = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    innerresult = dAL.QueryWithExecuteAsync("InsertTechnicalSanction", param);
                    if (Convert.ToInt32(innerresult.Status) > 0)
                    {
                        if (photoup.file != null)
                        {
                            string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/TechnicalSanction/" + innerresult.Status);
                            if (!Directory.Exists(RootFolderPath))
                            {
                                Directory.CreateDirectory(RootFolderPath);
                            }
                            string FilePath = Path.Combine(RootFolderPath, FileName);
                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                photoup.file.CopyTo(stream);
                            }
                        }
                    }

                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                     acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }


        [HttpGet]
        [Authorize(Roles = "gm")]
        public async Task<IActionResult> GetTechnicalSanction()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                data = await dAL.QueryAsync("[GetTechnicalSanctionDetails]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }
        public JsonResult DeleteTechnicalSanction(int Id)
        {
            dynamic msg = (null);
            var param = new { Id = Id, UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value, UserIP = HttpContext.Connection.RemoteIpAddress.ToString() };
            try
            {
                msg = DB_Conn.CUDProcedureExecute("[DeleteTechnicalSanction]", param);
            }

            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(msg);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  CM Announcements.........
        [Authorize(Roles = "admin,srv")]
        public IActionResult CMAnnouncement(int id)
        {
            CMAnnouncement nt = new CMAnnouncement();
            List<CMAnnouncement> data = new List<CMAnnouncement>();
            var param = new { Id = id };
            try
            {
                if (id == 0 || id == null)
                {
                    nt.Id = 0;
                    ViewBag.ButtonText = "Submit";
                }
                else
                {
                    data = DB_Conn.SelectProcedureExecute<CMAnnouncement>("GetCMAnnouncementDetails", param);
                    nt = data.FirstOrDefault();
                    ViewBag.ButtonText = "Update";
                }
            }

            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(nt);
        }
        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public IActionResult CMAnnouncement(CMAnnouncement photoup)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (photoup.file != null)
                        FileName = photoup.file.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + photoup.file.FileName.Split('.')[1].ToString();
                    var param = new
                    {
                        Id = photoup.Id,
                        FileName = FileName,
                        UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        UserIP = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    innerresult = dAL.QueryWithExecuteAsync("InsertCMAnnouncement", param);
                    if (Convert.ToInt32(innerresult.Status) > 0)
                    {
                        if (photoup.file != null)
                        {
                            string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/Announcements/" + innerresult.Status);
                            if (!Directory.Exists(RootFolderPath))
                            {
                                Directory.CreateDirectory(RootFolderPath);
                            }
                            string FilePath = Path.Combine(RootFolderPath, FileName);
                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                photoup.file.CopyTo(stream);
                            }
                        }
                    }

                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                     acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }
        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> GetCMAnnouncement()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                data = await dAL.QueryAsync("[GetCMAnnouncementDetails]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        public JsonResult DeleteCMAnnouncement(int Id)
        {
            dynamic msg = (null);
            var param = new { Id = Id, UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value, UserIP = HttpContext.Connection.RemoteIpAddress.ToString() };
            try
            {
                msg = DB_Conn.CUDProcedureExecute("[DeleteCMAnnouncement]", param);
            }

            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(msg);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  Media Gallery.........
         [Authorize(Roles = "admin,srv")]
        public IActionResult MediaGallery(int id)
        {
            MediaGallery nt = new MediaGallery();
            List<MediaGallery> data = new List<MediaGallery>();

            var param = new { Id = id };
            try
            {
                if (id == 0 || id == null)
                {
                    nt.Id = 0;
                    ViewBag.ButtonText = "Submit";
                }
                else
                {
                    data = DB_Conn.SelectProcedureExecute<MediaGallery>("[GetMediaGalleryDetails]", param);
                    nt = data.FirstOrDefault();
                    ViewBag.ButtonText = "Update";
                }
            }

            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(nt);
        }
        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public IActionResult MediaGallery(MediaGallery photoup)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (photoup.file != null)
                        FileName = photoup.file.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + photoup.file.FileName.Split('.')[1].ToString();
                    var param = new
                    {
                        Id = photoup.Id,
                        Title = photoup.Title,
                        HNTitle = photoup.HNTitle,
                        FileName = FileName,
                        UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        UserIP = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    innerresult = dAL.QueryWithExecuteAsync("InsertMediaGallery", param);
                    if (Convert.ToInt32(innerresult.Status) > 0)
                    {
                        if (photoup.file != null)
                        {
                            string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/MediaGallery/" + innerresult.Status);
                            if (!Directory.Exists(RootFolderPath))
                            {
                                Directory.CreateDirectory(RootFolderPath);
                            }
                            string FilePath = Path.Combine(RootFolderPath, FileName);
                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                photoup.file.CopyTo(stream);
                            }
                        }
                    }

                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                     acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }

        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> GetMediaGalleryDetails()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                data = await dAL.QueryAsync("[GetMediaGalleryDetails]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        public JsonResult DeleteMediaGallery(int Id)
        {
            dynamic msg = (null);
            var param = new { Id = Id, UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value, UserIP = HttpContext.Connection.RemoteIpAddress.ToString() };
            try
            {
                msg = DB_Conn.CUDProcedureExecute("DeleteMediaGallery", param);
            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }


            return Json(msg);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  Performa.........
        [Authorize(Roles = "admin,srv")]
        public IActionResult Proforma(int id)
        {
            Proforma nt = new Proforma();
            List<Proforma> data = new List<Proforma>();

            var param = new { Id = id };
            try
            {
                if (id == 0 || id == null)
                {
                    nt.Id = 0;
                    ViewBag.ButtonText = "Submit";
                }
                else
                {
                    data = DB_Conn.SelectProcedureExecute<Proforma>("[GetProformaDetails]", param);
                    nt = data.FirstOrDefault();
                    ViewBag.ButtonText = "Update";
                }
            }

            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(nt);
        }
        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public IActionResult Proforma(Proforma photoup)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (photoup.file != null)
                        FileName = photoup.file.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + photoup.file.FileName.Split('.')[1].ToString();
                    var param = new
                    {
                        Id = photoup.Id,
                        Title = photoup.Title,
                        HNTitle=photoup.HNTitle,

                        FileName = FileName,
                        UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        UserIP = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    innerresult = dAL.QueryWithExecuteAsync("InsertProforma", param);
                    if (Convert.ToInt32(innerresult.Status) > 0)
                    {
                        if (photoup.file != null)
                        {
                            string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/Proforma/" + innerresult.Status);
                            if (!Directory.Exists(RootFolderPath))
                            {
                                Directory.CreateDirectory(RootFolderPath);
                            }
                            string FilePath = Path.Combine(RootFolderPath, FileName);
                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                photoup.file.CopyTo(stream);
                            }
                        }
                    }

                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                     acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }

        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> GetProformaDetails()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                data = await dAL.QueryAsync("[GetProformaDetails]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        public JsonResult DeleteProforma(int Id)
        {
            dynamic msg = (null);
            var param = new { Id = Id, UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value, UserIP = HttpContext.Connection.RemoteIpAddress.ToString() };
            try
            {
                msg = DB_Conn.CUDProcedureExecute("DeleteProforma", param);
            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }


            return Json(msg);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  EOI.........
        [Authorize(Roles = "admin,srv")]
        public IActionResult EOI(int id)
        {
            EOI eoi = new EOI();
            List<EOI> data = new List<EOI>();

            var param = new { Id = id };
            try
            {
                if (id == 0 || id == null)
                {
                    eoi.Id = 0;
                    ViewBag.ButtonText = "Submit";
                }
                else
                {
                    data = DB_Conn.SelectProcedureExecute<EOI>("[GetEOIDetails]", param);
                    eoi = data.FirstOrDefault();
                    ViewBag.ButtonText = "Update";
                }
            }

            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(eoi);
        }
        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public IActionResult EOI(EOI photoup)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (photoup.file != null)
                        FileName = photoup.file.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + photoup.file.FileName.Split('.')[1].ToString();
                    var param = new
                    {
                        Id = photoup.Id,
                        Title = photoup.Title,
                        HNTitle=photoup.HNTitle,
                        FileName = FileName,
                        UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        UserIP = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    innerresult = dAL.QueryWithExecuteAsync("InsertEOI", param);
                    if (Convert.ToInt32(innerresult.Status) > 0)
                    {
                        if (photoup.file != null)
                        {
                            string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/EOI/" + innerresult.Status);
                            if (!Directory.Exists(RootFolderPath))
                            {
                                Directory.CreateDirectory(RootFolderPath);
                            }
                            string FilePath = Path.Combine(RootFolderPath, FileName);
                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                photoup.file.CopyTo(stream);
                            }
                        }
                    }

                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                     acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }

        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> GetEOIDetails()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                data = await dAL.QueryAsync("[GetEOIDetails]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        public JsonResult DeleteEOI(int Id)
        {
            dynamic msg = (null);
            var param = new { Id = Id, UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value, UserIP = HttpContext.Connection.RemoteIpAddress.ToString() };
            try
            {
                msg = DB_Conn.CUDProcedureExecute("DeleteEOI", param);
            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }


            return Json(msg);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  EOI.........
        [Authorize(Roles = "admin,srv")]
        public IActionResult GOS(int id)
        {
            GOS eoi = new GOS();
            List<GOS> data = new List<GOS>();

            var param = new { Id = id };
            try
            {
                if (id == 0 || id == null)
                {
                    eoi.Id = 0;
                    ViewBag.ButtonText = "Submit";
                }
                else
                {
                    data = DB_Conn.SelectProcedureExecute<GOS>("[GetGOSDetails]", param);
                    eoi = data.FirstOrDefault();
                    ViewBag.ButtonText = "Update";
                }
            }

            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(eoi);
        }
        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public IActionResult GOS(EOI photoup)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (photoup.file != null)
                        FileName = photoup.file.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + photoup.file.FileName.Split('.')[1].ToString();
                         
                    var extension = Path.GetExtension(photoup.file.FileName);

                    ///Find Length in Kb and takes two deciaml points
                    var size = (photoup.file.Length) / 1024;

                    var param = new
                    {
                        Id = photoup.Id,
                        Title = photoup.Title,
                        HNTitle=photoup.HNTitle,
                        GOSDate = photoup.GOSDate,
                        FileName = FileName,
                        FileSize=size,
                        FileType=extension,
                        UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        UserIP = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    innerresult = dAL.QueryWithExecuteAsync("InsertGOS", param);
                    if (Convert.ToInt32(innerresult.Status) > 0)
                    {
                        if (photoup.file != null)
                        {
                            string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/GOS/" + innerresult.Status);
                            if (!Directory.Exists(RootFolderPath))
                            {
                                Directory.CreateDirectory(RootFolderPath);
                            }
                            string FilePath = Path.Combine(RootFolderPath, FileName);
                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                photoup.file.CopyTo(stream);
                            }
                        }
                    }

                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                     acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }

        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> GetGOSDetails()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                data = await dAL.QueryAsync("[GetGOSDetails]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        public JsonResult DeleteGOS(int Id)
        {
            dynamic msg = (null);
            var param = new { Id = Id, UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value, UserIP = HttpContext.Connection.RemoteIpAddress.ToString() };
            try
            {
                msg = DB_Conn.CUDProcedureExecute("DeleteGOS", param);
            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }


            return Json(msg);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  Financial.........
        [Authorize(Roles = "admin,srv")]
        public IActionResult Financial(int id)
        {
            FinancialMaster nt = new FinancialMaster();
            List<FinancialMaster> data = new List<FinancialMaster>();

            var param = new { Id = id };
            try
            {
                if (id == 0 || id == null)
                {
                    nt.Id = 0;
                    ViewBag.ButtonText = "Submit";
                }
                else
                {
                    data = DB_Conn.SelectProcedureExecute<FinancialMaster>("[GetFinancialDetails]", param);
                    nt = data.FirstOrDefault();
                    ViewBag.ButtonText = "Update";
                }
            }

            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(nt);
        }
        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public IActionResult Financial(FinancialMaster fdata)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                   
                    var param = new
                    {
                        Id = fdata.Id,
                        Year = fdata.Year,
                        WorkDone=fdata.WorkDone,
                        NetProfit=fdata.NetProfit,
                        CumulativeReservers=fdata.CumulativeReservers,
                        UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        UserIP = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    innerresult = dAL.QueryWithExecuteAsync("InsertFinancial", param);

                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                     acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }
        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> GetFinancialDetails()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                data = await dAL.QueryAsync("[GetFinancialDetails]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }
        public JsonResult DeleteFinancial(int Id)
        {
            dynamic msg = (null);
            var param = new { Id = Id, UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value, UserIP = HttpContext.Connection.RemoteIpAddress.ToString() };
            try
            {
                msg = DB_Conn.CUDProcedureExecute("DeleteFinancial", param);
            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }


            return Json(msg);
        }

        public async Task<JsonResult> GetFinancialYear()
        {
            List<CommonSelect> objProjects = new List<CommonSelect>();
            try
            {
                objProjects = await dAL.BindSelect("GetFinancialYear", null);
                objProjects.Insert(0, new CommonSelect { Id = "0", Value = "Select Financial Year" });
            }

            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(objProjects);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Minister Master
        [Authorize(Roles = "admin,srv")]
        public IActionResult MinisterMaster(int id)
        {
            MinisterMaster nt = new MinisterMaster();
            List<MinisterMaster> data = new List<MinisterMaster>();

            var param = new { Id = id };
            try
            {
                if (id == 0 || id == null)
                {
                    nt.Id = 0;
                    ViewBag.ButtonText = "Submit";
                }
                else
                {
                    data = DB_Conn.SelectProcedureExecute<MinisterMaster>("[GetMinisterDetails]", param);
                    nt = data.FirstOrDefault();
                    ViewBag.ButtonText = "Update";
                }
            }

            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(nt);
        }

        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public IActionResult MinisterMaster(MinisterMaster photoup)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (photoup.file != null)
                        FileName = photoup.file.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + photoup.file.FileName.Split('.')[1].ToString();
                    var param = new
                    {
                        Id = photoup.Id,
                        MainTitle = photoup.MainTitle,
                        SubTitle = photoup.SubTitle,
                        HNMainTitle = photoup.HNMainTitle,
                        HNSubTitle = photoup.HNSubTitle,
                        Category=photoup.Category,
                        FileName = FileName,
                        UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        UserIP = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    innerresult = dAL.QueryWithExecuteAsync("InsertMinister", param);
                    if (Convert.ToInt32(innerresult.Status) > 0)
                    {
                        if (photoup.file != null)
                        {
                            string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/MinisterPhotos/" + innerresult.Status);
                            if (!Directory.Exists(RootFolderPath))
                            {
                                Directory.CreateDirectory(RootFolderPath);
                            }
                            string FilePath = Path.Combine(RootFolderPath, FileName);
                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                photoup.file.CopyTo(stream);
                            }
                        }
                    }

                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                     acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }

        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> GetMinisterMaster()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                data = await dAL.QueryAsync("[GetMinisterDetails]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);

            }
            return Json(data);
        }

        public JsonResult DeleteMinisterMaster(int Id)
        {
            dynamic msg = (null);
            var param = new { Id = Id, UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value, UserIP = HttpContext.Connection.RemoteIpAddress.ToString() };
            try
            {
                msg = DB_Conn.CUDProcedureExecute("DeleteMinister", param);
            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }

            return Json(msg);
        }

        //////////////////////////////////////////////////////////////////////////////////DeparmentMaster
        [Authorize(Roles = "admin,unit,hq")]
        public IActionResult DepartMaster(int id)
        {
            DepartmentMaster nt = new DepartmentMaster();
            List<DepartmentMaster> data = new List<DepartmentMaster>();

            var param = new { Id = id };
            try
            {
                if (id == 0 || id == null)
                {
                    nt.ID = 0;
                    ViewBag.ButtonText = "Submit";
                }
                else
                {
                    data = DB_Conn.SelectProcedureExecute<DepartmentMaster>("[GetDeparmentList]", param);
                    nt = data.FirstOrDefault();
                    ViewBag.ButtonText = "Update";
                }
            }

            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(nt);
        }

        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public IActionResult DepartMaster(DepartmentMaster photoup)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                  
                    var param = new
                    {
                        Id = photoup.ID,
                        departmentname = photoup.DepartmentName.Trim(),                 
                        UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        IpAddress = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    innerresult = dAL.QueryWithExecuteAsync("InserUpdateDepartment", param);
                   

                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                    acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }
        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> GetDepartmentMaster()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                data = await dAL.QueryAsync("[GetDeparmentList]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);

            }
            return Json(data);
        }

        public JsonResult DeleteDepartment(int Id)
        {
            dynamic msg = (null);
            var param = new { Id = Id, UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value, IpAddress= HttpContext.Connection.RemoteIpAddress.ToString() };
            try
            {
                msg = DB_Conn.CUDProcedureExecute("DeleteDepartment", param);
            }
            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }

            return Json(msg);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  CM Announcements.........
        [Authorize(Roles = "admin,srv")]
        public IActionResult MonthlyCompiled(int id)
        {
            MonthlyCompileReport nt = new MonthlyCompileReport();
            List<MonthlyCompileReport> data = new List<MonthlyCompileReport>();
            var param = new { Id = id };
            try
            {
                if (id == 0 || id == null)
                {
                    nt.Id = 0;
                    ViewBag.ButtonText = "Submit";
                }
                else
                {
                    data = DB_Conn.SelectProcedureExecute<MonthlyCompileReport>("GetMonthlyCompiledDetails", param);
                    nt = data.FirstOrDefault();
                    ViewBag.ButtonText = "Update";
                }
            }

            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(nt);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////Monthly compiled Report
        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public IActionResult MonthlyCompiled(MonthlyCompileReport photoup)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (photoup.file != null)
                        FileName = photoup.file.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + photoup.file.FileName.Split('.')[1].ToString();
                    var param = new
                    {
                        Id = photoup.Id,
                        FileName = FileName,
                        UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        UserIP = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    innerresult = dAL.QueryWithExecuteAsync("InsertMonthlyCompileReport", param);
                    if (Convert.ToInt32(innerresult.Status) > 0)
                    {
                        if (photoup.file != null)
                        {
                            string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/MonthlyCompiled/" + innerresult.Status);
                            if (!Directory.Exists(RootFolderPath))
                            {
                                Directory.CreateDirectory(RootFolderPath);
                            }
                            string FilePath = Path.Combine(RootFolderPath, FileName);
                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                photoup.file.CopyTo(stream);
                            }
                        }
                    }

                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                    acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }
        [HttpGet]
        [Authorize(Roles = "admin,hq")]
        public async Task<IActionResult> GetMonthlyCompiled()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                data = await dAL.QueryAsync("[GetMonthlyCompiledDetails]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        public JsonResult DeleteCompiled(int Id)
        {
            dynamic msg = (null);
            var param = new { Id = Id, UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value, UserIP = HttpContext.Connection.RemoteIpAddress.ToString() };
            try
            {
                msg = DB_Conn.CUDProcedureExecute("[DeleteMonthlyCompileReport]", param);
            }

            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(msg);
        }


       
        public async Task<IActionResult> SurveyorMaster(string Id)
        {
           

               
                SurveyorMaster result = new SurveyorMaster();
                List<CommonSelect> obj = new List<CommonSelect>();
               

                try
                {
                    if ((Convert.ToInt32(Id) > 0))
                    {
                        ViewBag.buttontext = "Update";
                        var param = new { Id = Id };
                        result = dAL.SelectModel<SurveyorMaster>("GetSurveyorDetail", param);
                    }
                    else
                    {
                        ViewBag.buttontext = "Submit";
                       
                    }
                    ////////////////////////////////Get District List//////////////////////////////// 
                    obj = await dAL.BindSelect("GetDistrictDropDownList", null);
                    obj.Insert(0, new CommonSelect { Id = "0", Value = "Select District" });
                    result.DistrictList = new SelectList(obj, "Id", "Value", 0);

               
                }
                catch (Exception ex)
                {
                    await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
                return View(result);
            }


        [HttpPost]
        [Authorize(Roles = "admin,unit,gm,hq")]
        public IActionResult SurveyorMaster(SurveyorMaster master)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            if (ModelState.IsValid)
            {
                try
                {

                    var param = new
                    {
                        Id =master.Id,
                        DistictId = master.DistictId,
                        SurveyorName = master.SurveyorName,
                        MobileNumber = master.MobileNumber,
                        EmailId = master.EmailId,
                        UnitId= ((ClaimsIdentity)this.User.Identity).FindFirst("OfficeId").Value,
                        UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        IpAddress = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    Result = dAL.QueryWithExecuteAsync("InsertSurveyorDetail", param);


                   

                }
                catch (Exception ex)
                {
                    acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }

        [HttpGet]
        [Authorize(Roles = "admin,hq,gm,unit")]
        public async Task<IActionResult> GetSurveyorMaster()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                data = await dAL.QueryAsync("[GetSurveyorDetail]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        public IActionResult DeleteSurveyor(string Id)
        {
            var Result = (dynamic)null;
          
          
                try
                {

                    var param = new
                    {
                        Id =Id,
                       UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        IpAddress = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    Result = dAL.QueryWithExecuteAsync("DeleteSurveyorData", param);




                }
                catch (Exception ex)
                {
                    acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            
            return Json(Result);
        }

        [HttpGet]
        [Authorize(Roles = "admin,hq,gm,unit")]
        public async Task<IActionResult> ServeyDetail()
        {
            SurveyorMaster result = new SurveyorMaster();
            List<CommonSelect> obj = new List<CommonSelect>();

            ////////////////////////////////Get District List//////////////////////////////// 
            obj = await dAL.BindSelect("GetDistrictDropDownList", null);
            obj.Insert(0, new CommonSelect { Id = "0", Value = "Select District" });
            result.DistrictList = new SelectList(obj, "Id", "Value", 0);
            return View(result);
        }

       
        public async Task<IActionResult> BindTehsilbyDistrict(string Id)
        {
            List<ResultSet> data = new List<ResultSet>();

            try
            {
                
                var param = new
                {
                    District = Id
                };
                data = dAL.SelectModelList<ResultSet>("App_GetTehsilByDistrict", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Json(data);
        }



        public async Task<IActionResult> BindBlockByDistrictTehsil(string dist,string tehsil)
        {
            List<ResultSet> data = new List<ResultSet>();

            try
            {

                var param = new
                {
                    District =dist=="0"?null:dist,
                    Tehsil=tehsil=="0"?null:tehsil
                };
                data = dAL.SelectModelList<ResultSet>("App_GetBlockList", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Json(data);
        }


        public async Task<IActionResult> BindGramSabhaByTehsilBlock(string tehsil, string blockid)
        {
            List<ResultSet> data = new List<ResultSet>();

            try
            {

                var param = new
                {
                   
                    Block = blockid == "0" ? null : blockid,
                    Tehsil = tehsil == "0" ? null : tehsil,
                };
                data = dAL.SelectModelList<ResultSet>("App_GetGramSabha", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Json(data);
        }

        public async Task<IActionResult> GetSurveryCompleteList(string dist,string tehsil,string block,string gram,string surveyDate)
        {
            var data = (dynamic)null;
            try
            {
                var role = ((ClaimsIdentity)this.User.Identity).FindFirst("Role").Value;
                var zoneId= ((ClaimsIdentity)this.User.Identity).FindFirst("OfficeId").Value;

                var param = new { 
                    DistrictId=dist=="0"?null:dist,
                    TehsilId=tehsil=="0"?null:tehsil,
                    BlockId= block=="0"? null: block,
                    Gramsabha= gram=="0"?null: gram,
                    surveyDate=string.IsNullOrEmpty(surveyDate) ? "" : DateTime.ParseExact(surveyDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                    role=role,
                    zone= zoneId,
                    UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                };
                data = await dAL.QueryAsync("[GetSurveryDetailsByFilter]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

      
        public async Task<IActionResult> GetCompleteDetails(string Id)
        {
           SurveryList data = new SurveryList();

            try
            {
               
                var param = new
                {
                    Id =Id,

                };
                data = dAL.SelectModel<SurveryList>("GetSurveryListById", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return View(data);
        }

        public async Task<IActionResult> PMList()
        {
            var UserRole = this.User.Claims.First(c => c.Type == "Role").Value.ToString();
            var ZoneIds = this.User.Claims.First(c => c.Type == "ZoneId").Value.ToString();
            var UnitIds = this.User.Claims.First(c => c.Type == "UnitId").Value.ToString();
            ViewBag.UserRole = UserRole;
            ViewBag.ZoneId1 = ZoneIds;
            ViewBag.UnitId1 = UnitIds;

            PMList result = new PMList();
            List<CommonSelect> obj = new List<CommonSelect>();

            ////////////////////////////////Get District List//////////////////////////////// 
            obj = await dAL.BindSelect("GetZone", null);
            obj.Insert(0, new CommonSelect { Id = "0", Value = "Select Zone" });
            result.ZoneList = new SelectList(obj, "Id", "Value", 0);
            return View(result);

        }

        public async Task<IActionResult> GetPMListByZoneUnit(string ZoneId, string UnitId)
        {
            var data = (dynamic)null;
            try
            {
                var param = new
                {
                    ZoneId = ZoneId == "0" ? null : ZoneId,
                    UnitId = UnitId == "0" ? null : UnitId,
                    
                };
                data = await dAL.QueryAsync("[GetPmDetailByZoneUnit]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        public class ResultSet
        {

            public string Id { get; set; }
            public string Value { get; set; }

        }
        public ActionResult UpdateSurveyorDetails()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetSurveyorDetails()
        {
            var data = (dynamic)null;
            try
            {
                data = await dAL.QueryAsync("[getSurveyDetails]", null);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }
        [HttpPost]
        public async Task<IActionResult> InsertUpdateSurvey([FromBody] ExpandoObject expando)
        {
            var result = (dynamic)null;

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var objd = expandoDict["objd"];

                DataTable dt = (DataTable)JsonConvert.DeserializeObject(objd.ToString(), (typeof(DataTable)));
                 
                
                var identity = (ClaimsIdentity)User.Identity;

                var param = new
                {
                    Reference = dt,
                };

                result = await dAL.QueryAsync("[InsUpdSurveyDetails]", param);
            }
            catch(Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);

            }
            return Json(result);

        }

        public async Task<IActionResult> CreateContractor(int Id)
        {
            CreateContractor objprof = new CreateContractor();

            try
            {
                if (Id > 0)
                {
                    var param = new
                    {
                        Id = Id
                    };
                    objprof = dAL.SelectModel<CreateContractor>("getContractorInformation", param);
                }
                List<CommonSelect> obj1 = new List<CommonSelect>();

                ////////////////////////////////Get Zone List//////////////////////////////// 
                obj1 = await dAL.BindSelect("GetZone", null);
                obj1.Insert(0, new CommonSelect { Id = "0", Value = "Select Zone" });
                objprof.ZoneList = new SelectList(obj1, "Id", "Value", 0);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
            }
            return View(objprof);
        }
        public IActionResult ContractorList()
        {
            return View();
        }


        public IActionResult SupervisorList()
        {
            return View();
        }

        public IActionResult SiteList(string ContractorId,string DistrictId,string TehsilId,string BlockId,string GramsabhaId,string WorkStatus)
        {
            TehsilId = TehsilId == "null" ? "0" : TehsilId;
            BlockId = BlockId == "null" ? "0" : BlockId;
            GramsabhaId = GramsabhaId == "null" ? "0" : GramsabhaId;
            ViewBag.ContractorId = ContractorId;
            ViewBag.DistrictId = DistrictId;
            ViewBag.TehsilId = TehsilId;
            ViewBag.BlockId = BlockId;
            ViewBag.GramsabhaId = GramsabhaId;
            ViewBag.WorkStatus = WorkStatus;
            return View();
        }

        public IActionResult TotalSiteList()
        {
            return View();
        }
        public IActionResult AprroveIssueSiteList(string type)
        {
            ViewBag.type = type;
            return View();
        }
        public IActionResult ViewCompletedetails()
        {
            return View();
        }
        public IActionResult DistrictContractors()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetDistrictList()
        {
            List<CommonSelect> obj = new List<CommonSelect>();

            obj = await dAL.BindSelect("BindDistrictsDD", null);
            // obj.Insert(0, new CommonSelect { Id = "0", Value = "Select District" });
            ViewBag.DistrictList = new SelectList(obj, "Id", "Value");
            return null;
        }

        [HttpPost]

        public async Task<IActionResult> SubmitContractor(CreateContractor obj)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            DataTable dt = new DataTable();
            dt.Columns.Add("DistrictId");
            for (int i = 0; i < obj.Districts.Count(); i++)
            {
                dt.Rows.Add(obj.Districts[i]);
            }
            string ApplicantPhoto = "";
            string PANFile = "";
            string GSTFile = "";
            string randomnum = "";
            try
            {
           

                if (obj.PANFile != null)
                {
                    randomnum = GenerateToken(6);
                    PANFile = randomnum + "_" + DateTime.Now.Ticks + "." + obj.PANFile.FileName.Split('.')[1].ToString();

                    // Image file size check

                    int maximumsize = Convert.ToInt32(configuration["ImageMaximumSize"]);


                    //if (obj.PANFile.Length > maximumsize)
                    //{
                    //    var Res = new
                    //    {
                    //        innerresult = "Upload PAN File Size must be less than or equal to 50 kb.",
                    //        status = false,
                    //        eventKey = "Size Error"
                    //    };
                    //    Result = new { data = "", dynamicResult = Res };
                    //    return Json(Result);
                    //}



                }


                if (obj.GSTFile != null)
                {
                    randomnum = GenerateToken(6);
                    GSTFile = randomnum + "_" + DateTime.Now.Ticks + "." + obj.GSTFile.FileName.Split('.')[1].ToString();

                    // Image file size check

                    int maximumsize = Convert.ToInt32(configuration["ImageMaximumSize"]);


                    //if (obj.GSTFile.Length > maximumsize)
                    //{
                    //    var Res = new
                    //    {
                    //        innerresult = "Upload GST File Size must be less than or equal to 50 kb.",
                    //        status = false,
                    //        eventKey = "Size Error"
                    //    };
                    //    Result = new { data = "", dynamicResult = Res };
                    //    return Json(Result);
                    //}



                }
                AppCommonMethod objAppCommonMethod = new AppCommonMethod();
                var Pass = RandomPassword();
                var param = new
                {
                    Id = obj.Id,
                    FirmName = obj.FirmName,
                    ApplicantName = obj.ApplicantName,
                    ApplicantPhoto = ApplicantPhoto,
                    OfficeAddress = obj.OfficeAddress,
                    Mobile = obj.Mobile,
                    Email = obj.Email,
                    PAN = obj.PAN,
                    PANFile = PANFile,
                    GST = obj.GST,
                    GSTFile = GSTFile,
                    ZoneId=obj.ZoneId,
                    UnitId=obj.UnitId,
                    IPAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
                    Password = objAppCommonMethod.EncryptMD5(Pass),
                    Reference = dt

                };
                innerresult = await dAL.QueryAsync("[InsUpdContractorInformation]", param);
                if (Convert.ToInt32(innerresult[0].Status) > 0)
                {
                    if (Convert.ToInt32(innerresult[0].Status) == 1)
                    {
                        innerresult[0].Message = "Contractor Details Added Successfully. Login Credentials for Contractor are UserId : " + obj.Mobile + ", " + "Password : " + Pass;
                    }
                 
                    if (obj.PANFile != null)
                    {
                        string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/NewContractor/PAN/");
                        if (!Directory.Exists(RootFolderPath))
                        {
                            Directory.CreateDirectory(RootFolderPath);
                        }
                        string FilePath = Path.Combine(RootFolderPath, PANFile);
                        using (var stream = new FileStream(FilePath, FileMode.Create))
                        {
                            obj.PANFile.CopyTo(stream);
                        }
                    }

                    if (obj.GSTFile != null)
                    {
                        string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/NewContractor/GST/");
                        if (!Directory.Exists(RootFolderPath))
                        {
                            Directory.CreateDirectory(RootFolderPath);
                        }
                        string FilePath = Path.Combine(RootFolderPath, GSTFile);
                        using (var stream = new FileStream(FilePath, FileMode.Create))
                        {
                            obj.GSTFile.CopyTo(stream);
                        }
                    }
                }
                var DynamicResult = new
                {
                    innerresult = innerresult,
                    status = true,
                    eventKey = "New Contractor Added"
                };
                Result = new { data = "", dynamicResult = DynamicResult };
                if (obj.Id == "" || obj.Id == null)
                {
                    ModelState.Clear();
                }

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
            }

            return Json(Result);
        }



        [HttpGet]
        public async Task<IActionResult> GetContractorList()
        {
            var data = (dynamic)null;
            try
            {
            
                data = await dAL.QueryAsync("GetContractorList", null);

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetDistrictListCheckBox()
        {
            var data = (dynamic)null;
            try
            {

                data = await dAL.QueryAsync("BindDistrictsDD", null);

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetDistrictListByContractor(string Id)
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = Id };
                data = await dAL.QueryAsync("GetDistrictListByContractor", param);

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetSupervisorList()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { ContractorId = '0' };
                data = await dAL.QueryAsync("getSupervisorMaster", param);

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

       

        [HttpPost]
        public async Task<IActionResult> GetWorkProgressDashboardCount(string ContratorId, string DistrictId, string SupervisorId, string TehsilId, string BlockId, string GramSabhaId)
        {
            var Result = (dynamic)null;
            try
            {
                var param = new
                {
                    DistrictId = DistrictId == "0" ? null : DistrictId,
                    ContratorId = ContratorId == "0" ? null : ContratorId,
                    SupervisorId = SupervisorId == "0" ? null : SupervisorId,
                    TehsilId = TehsilId == "0" ? null : TehsilId,
                    BlockId = BlockId == "0" ? null : BlockId,
                    GramSabhaId = GramSabhaId == "0" ? null : GramSabhaId

                };
                Result = dAL.Query("GetWorkProgressDashboardCount", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            // return model
            return Json(Result);
        }

        [HttpPost]
        public async Task<IActionResult> GetAprrovedIssueDashboardCount(string ContratorId, string DistrictId, string SupervisorId, string TehsilId, string BlockId, string GramSabhaId)
        {
            var Result = (dynamic)null;
            try
            {
                var param = new
                {
                    DistrictId = DistrictId == "0" ? null : DistrictId,
                    ContratorId = ContratorId == "0" ? null : ContratorId,
                    SupervisorId = SupervisorId == "0" ? null : SupervisorId,
                    TehsilId = TehsilId == "0" ? null : TehsilId,
                    BlockId = BlockId == "0" ? null : BlockId,
                    GramSabhaId = GramSabhaId == "0" ? null : GramSabhaId

                };
                Result = dAL.Query("GetAprroveIssueDashboardCount", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            // return model
            return Json(Result);
        }

        [HttpGet]
        public async Task<object> GetSitesFromWorkStatus(string DistrictId, string ContratorId, string TehsilId, string BlockId, string GramSabhaId, string WorkStatus)
         {
            var Result = (dynamic)null;

            try
            {
                //var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    ContractorId=ContratorId=="0"?null:ContratorId,
                    WorkStatus = WorkStatus == "" ? null : WorkStatus,
                    DistrictId = DistrictId == "0" ? null : DistrictId,
                    TehsilId = TehsilId == "0" ? null : TehsilId,
                    BlockId = BlockId == "0" ? null : BlockId,
                    GramSabhaId = GramSabhaId == "0" ? null : GramSabhaId,
                };
                Result = dAL.Query("GetSitesFromWorkStatus", CommandType.StoredProcedure, param);

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Result;
        }

       

        public async Task<object> GetSitesFromWorktype(string type)
        {
            var Result = (dynamic)null;

            try
            {
                //var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    type = type
                };
                Result = dAL.Query("GetSitesFromWorktype", CommandType.StoredProcedure, param);

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Result;
        }

        public async Task<object> GetCompleteSiteDetails(string SiteId, string SupervisorId)
        {
            var Result = (dynamic)null;

            try
            {
                //var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    SiteId = SiteId,
                    SupervisorId = SupervisorId
                };
                Result = dAL.Query("GetCompleteSiteDetails", CommandType.StoredProcedure, param);
              
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Result;
        }

        [HttpGet]
        public async Task<IActionResult> GetDistrictWiseContractors()
        {
            var data = (dynamic)null;
            try
            {

                data = await dAL.QueryAsync("getDistrictWiseContractors", null);

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

      
        public async Task<object> App_GetDistrictListByContractorId(string ContractorId)
        {
            List<ResultSet> data = new List<ResultSet>();

            try
            {

                var param = new
                {
                    Id = ContractorId

                };
                data = dAL.SelectModelList<ResultSet>("App_GetDistrictListByContractorLogin", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }


        public async Task<object> App_GetTehsilList(string DistrictId)
        {
            List<ResultSet> data = new List<ResultSet>();

            try
            {
             
                var param = new
                {
                    District = DistrictId
                };
                data = dAL.SelectModelList<ResultSet>("App_GetTehsilByDistrict", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }

        public async Task<object> App_GetBlockList(string DistrictId,string TehsilId)
        {
            List<ResultSet> data = new List<ResultSet>();

            try
            {
                var param = new
                {
                    District = DistrictId,
                    Tehsil = TehsilId
                };
                data = dAL.SelectModelList<ResultSet>("App_GetBlockList", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }


        public async Task<object> App_GetGramSabhaList(string BlockId, string TehsilId)
        {
            List<ResultSet> data = new List<ResultSet>();
            ResultSet d = new ResultSet();
            d.Id = "0";
            d.Value = "Others";

            try
            {
                var param = new
                {
                    Block = BlockId,
                    Tehsil = TehsilId
                };

                data = dAL.SelectModelList<ResultSet>("App_GetGramSabha", param);
                data.Insert(0, d);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }

        public async Task<object> App_GetUrbanListByDistrictId(string DistrictId)
        {
            List<ResultSet> data = new List<ResultSet>();

            try
            {

                
                var param = new
                {
                    DistrictId = DistrictId
                };
                data = dAL.SelectModelList<ResultSet>("GetUrbanMasterListByDistrictId", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }

        public string GenerateToken(int NoOfDigit)
        {
            try
            {
                //var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                var chars = "0123456789";
                var stringChars = new char[NoOfDigit];
                var random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var finalString = new String(stringChars);
                return finalString.ToLower();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Generates a random password.  
        // 4-LowerCase + 4-Digits + 2-UpperCase  
        public string RandomPassword()
        {
            var passwordBuilder = new StringBuilder();

            // 4-Letters lower case   
            passwordBuilder.Append(RandomString(4, true));

            // 4-Digits between 1000 and 9999  
            passwordBuilder.Append(RandomNumber(1000, 9999));

            // 2-Letters upper case  
            passwordBuilder.Append(RandomString(2));
            return passwordBuilder.ToString();
        }
        // Instantiate random number generator.  
        private readonly Random _random = new Random();


        // Generates a random string with a given size.    
        public string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
        // Generates a random number within a range.      
        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

        [Authorize(Roles = "admin,gm")]
        public IActionResult WorkProgressDashboard()
        {

            return View();
        }



        /////////////////////////////////Super visor list by Contractor Id////////////////////////////////////
        public async Task<IActionResult> GetSupervisorListById(string Id)
        {
            List<Supervisor> data = new List<Supervisor>();
            try
            {
                var param = new { ContractorId = Id };
                data = dAL.SelectModelList<Supervisor>("getSupervisorMaster", param);

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(data);
        }
        [Authorize(Roles = "admin")]
        public IActionResult SchoolWorkHandover()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadSchoolWorkHandover()
        {
            var resultdata = (dynamic)null;
            var Result = (dynamic)null;
            DataTable dataTable = new DataTable();
            string result = string.Empty;
            try
            {
                long size = 0;
                var file = Request.Form.Files;
                var Id = Request.Form["Id"].ToString();
                var filename = ContentDispositionHeaderValue
                                .Parse(file[0].ContentDisposition)
                                .FileName
                                .Trim('"');
                int lastIndex = filename.LastIndexOf('.');
                var ext = filename.Substring(lastIndex + 1);
                var tempFileName = Id + "SchoolWorkHandover-" + DateTime.Now.Ticks + "." + ext;
                string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/SchoolWorkHandover/");
                if (!Directory.Exists(RootFolderPath))
                {
                    Directory.CreateDirectory(RootFolderPath);
                }
                string FilePath = Path.Combine(RootFolderPath, tempFileName);
                DirectoryInfo directory = new DirectoryInfo(RootFolderPath);

                foreach (FileInfo f in directory.GetFiles())
                {
                    if (System.IO.File.Exists(FilePath))
                    {
                        System.IO.File.Delete(FilePath);
                    }
                }
                size += file[0].Length;
                using (FileStream fs = System.IO.File.Create(FilePath))
                {
                    file[0].CopyTo(fs);
                    fs.Flush();
                }
                result = FilePath;
                var stream = new FileStream(FilePath, FileMode.Open);
                IExcelDataReader reader = null;
                if (filename.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                var result1 = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });
                dataTable = result1.Tables[0];
                DataTable dt = DefineSchoolDataTable();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (dataTable.Rows[i][1].ToString().Length > 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr["DistrictName"] = dataTable.Rows[i][1].ToString();
                        dr["BlockName"] = dataTable.Rows[i][2].ToString();
                        dr["VillageName"] = dataTable.Rows[i][3].ToString();
                        dr["SchoolName"] = dataTable.Rows[i][4].ToString();
                        dr["SchoolCode"] = dataTable.Rows[i][5].ToString();
                        dr["DetailsUnder100Days"] = dataTable.Rows[i][6].ToString();
                        dr["DetailsPrior100Days"] = dataTable.Rows[i][7].ToString();
                        dr["CreatedBy"] = this.User.Claims.First(c => c.Type == "UserId").Value.ToString();
                        dr["CreatedIP"] = HttpContext.Connection.RemoteIpAddress.ToString();
                        dt.Rows.Add(dr);
                    }
                }
                var param = new
                {
                 SchoolRef = dt
                   
                };
                
                resultdata = dAL.QueryWithExecuteAsync("InsertSchoolWorkHandover", param);
                Result = new { resultdata = resultdata };
                reader.Close();

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(Result);
        }
        

        [Authorize(Roles = "admin")]
        public IActionResult AnganwadiWorkHandover()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadAnganwadiWorkHandover()
        {
            var resultdata = (dynamic)null;
            var Result = (dynamic)null;
            DataTable dataTable = new DataTable();
            string result = string.Empty;
            try
            {
                long size = 0;
                var file = Request.Form.Files;
                var Id = Request.Form["Id"].ToString();
                var filename = ContentDispositionHeaderValue
                                .Parse(file[0].ContentDisposition)
                                .FileName
                                .Trim('"');
                int lastIndex = filename.LastIndexOf('.');
                var ext = filename.Substring(lastIndex + 1);
                var tempFileName = Id + "AnganwadiWorkHandover-" + DateTime.Now.Ticks + "." + ext;
                string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/AnganwadiWorkHandover/");
                if (!Directory.Exists(RootFolderPath))
                {
                    Directory.CreateDirectory(RootFolderPath);
                }
                string FilePath = Path.Combine(RootFolderPath, tempFileName);
                DirectoryInfo directory = new DirectoryInfo(RootFolderPath);

                foreach (FileInfo f in directory.GetFiles())
                {
                    if (System.IO.File.Exists(FilePath))
                    {
                        System.IO.File.Delete(FilePath);
                    }
                }
                size += file[0].Length;
                using (FileStream fs = System.IO.File.Create(FilePath))
                {
                    file[0].CopyTo(fs);
                    fs.Flush();
                }
                result = FilePath;
                var stream = new FileStream(FilePath, FileMode.Open);
                IExcelDataReader reader = null;
                if (filename.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                var result1 = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });
                dataTable = result1.Tables[0];
                DataTable dt = DefineAnganwadiDataTable();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (dataTable.Rows[i][1].ToString().Length > 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr["DistrictName"] = dataTable.Rows[i][1].ToString();
                        dr["BlockName"] = dataTable.Rows[i][2].ToString();
                        dr["VillageName"] = dataTable.Rows[i][3].ToString();
                        dr["AnganwadiName"] = dataTable.Rows[i][4].ToString();
                        dr["DetailsUnder100Days"] = dataTable.Rows[i][5].ToString();
                        dr["DetailsPrior100Days"] = dataTable.Rows[i][6].ToString();
                        dr["CreatedBy"] = this.User.Claims.First(c => c.Type == "UserId").Value.ToString();
                        dr["CreatedIP"] = HttpContext.Connection.RemoteIpAddress.ToString();
                        dt.Rows.Add(dr);
                    }
                }
                var param = new
                {
                    AnganwadiRef = dt

                };
                resultdata = dAL.QueryWithExecuteAsync("InsertAnganwadiWorkHandover", param);
                Result=  new { resultdata = resultdata };
                reader.Close();

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(Result);
        }
        [Authorize(Roles = "admin")]
        public IActionResult TotalSchoolWorkHandover()
        {

            return View();
        }
        public async Task<IActionResult> GetSchoolWorkHandoverList()
        {
            var UserRole = this.User.Claims.First(c => c.Type == "Role").Value.ToString();
            var data = (dynamic)null;
            try
            {
                var param = new
                {

                    Type = "S",
                 
                };

                data = await dAL.QueryAsync("[GetSchoolAnganwadiList]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }
        [Authorize(Roles = "admin")]
        public IActionResult TotalAnganwadiWorkHandover()
        {

            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult GetMap()
        {

            return View();
        }

        public async Task<IActionResult> GetlAnganwadiWorkHandoverList()
        {
            var UserRole = this.User.Claims.First(c => c.Type == "Role").Value.ToString();
            var data = (dynamic)null;
            try
            {
                var param = new
                {

                    Type = "A",

                };

                data = await dAL.QueryAsync("[GetSchoolAnganwadiList]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSchoolHandover(string RowData)
        {
            var list = (dynamic)null;
            try
            {
                
                var param = new {RowData};
                list = await dAL.QueryAsync("DeleteSchoolHandover", param);
            }
            catch (Exception ex)
            {

                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(list);
        }
        [HttpGet]
        public IActionResult GetLocation(string dist)
        {
            List<Location> result = new List<Location>();
            var p = new
            {
                dist = dist
            };
            result = dAL.SelectModelList<Location>("[GetLocation]", p);
            return Json(result);
        }

        [HttpGet]
        public IActionResult GetMapDistrict()
        {
            List<DropDown> result = new List<DropDown>();
            result = dAL.SelectModelList<DropDown>("[GetDistrictMap]", null);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAnganwadiHandover(string RowData)
        {
            var list = (dynamic)null;
            try
            {

                var param = new { RowData };
                list = await dAL.QueryAsync("DeleteAnganwadiHandover", param);
            }
            catch (Exception ex)
            {

                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(list);
        }
        public DataTable DefineSchoolDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("DistrictName"));
            dt.Columns.Add(new DataColumn("BlockName"));
            dt.Columns.Add(new DataColumn("VillageName"));
            dt.Columns.Add(new DataColumn("SchoolName"));
            dt.Columns.Add(new DataColumn("SchoolCode"));
            dt.Columns.Add(new DataColumn("DetailsUnder100Days"));
            dt.Columns.Add(new DataColumn("DetailsPrior100Days"));
            dt.Columns.Add(new DataColumn("CreatedBy"));
            dt.Columns.Add(new DataColumn("CreatedIP"));
            return dt;
        }
        public DataTable DefineAnganwadiDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("DistrictName"));
            dt.Columns.Add(new DataColumn("BlockName"));
            dt.Columns.Add(new DataColumn("VillageName"));
            dt.Columns.Add(new DataColumn("AnganwadiName"));
            dt.Columns.Add(new DataColumn("DetailsUnder100Days"));
            dt.Columns.Add(new DataColumn("DetailsPrior100Days"));
            dt.Columns.Add(new DataColumn("CreatedBy"));
            dt.Columns.Add(new DataColumn("CreatedIP"));
            return dt;
        }



        [Authorize(Roles = "admin,srv")]
        public IActionResult ProjectCount(string msg)
        {
            Projects_Count project = new Projects_Count();
            List<Projects_Count> data = new List<Projects_Count>();
            try
            {
                // data = await dAL.QueryAsync("[getSurveyDetails]", null);

                data = DB_Conn.SelectProcedureExecute<Projects_Count>("[GetProjectCount]", null);
                if (data != null)
                {
                    project = data.FirstOrDefault();
                }

            }

            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            var msgStatus = "";
            if (msg == "success")
            {
                msgStatus = "Projects counts has been successfully updated.";
            }
            if (msg == "failed")
            {
                msgStatus = "Unable to updated.";
            }
            ViewBag.msg = msgStatus;
            return View(project);

        }

        [Authorize(Roles = "admin,srv")]
        [HttpPost]
        public IActionResult ProjectCount(Projects_Count pc)
        {
            var Result = (dynamic)null;
            Projects_Count project = new Projects_Count();
            List<Projects_Count> data = new List<Projects_Count>();
            try
            {

                var param = new
                {
                    id = 1,
                    CompletedProjects = pc.CompletedProjects,
                    HandoverProjects = pc.HandoverProjects,
                    NonHandoverProjects = pc.NonHandoverProjects,
                    TotalOngoingProjects = pc.TotalOngoingProjects,
                    TotalProjects = pc.TotalProjects,
                    LastUpdateBy = ((ClaimsIdentity)this.User.Identity).FindFirst("Role").Value,
                    UpdateIP = HttpContext.Connection.RemoteIpAddress.ToString(),
                };

                var innerresult = dAL.QueryWithExecuteAsync("UpdateProjectCount", param);

                if (innerresult != null)
                {
                    var check = innerresult.ToString();

                    if (check.Contains("True"))
                    {
                        return RedirectToAction("ProjectCount", new { msg = "success" });
                    }
                    else
                    {
                        return RedirectToAction("ProjectCount", new { msg = "failed" });
                    }
                }

            }
            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(project);
        }

        [Authorize(Roles = "admin,srv")]
        public IActionResult Board_of_Directors(int id)
        {
            List<BoardOfDirectors> data = new List<BoardOfDirectors>();
            BoardOfDirectors nt = new BoardOfDirectors();
            var param = new { Id = id };
            try
            {
                if (id == 0 || id == null)
                {
                    nt.ID = 0;
                    ViewBag.ButtonText = "Submit";
                }
                else
                {
                    data = DB_Conn.SelectProcedureExecute<BoardOfDirectors>("[BoardOfDirectorDetails]", null);
                    nt = data.Where(a => a.ID == id).FirstOrDefault();
                    ViewBag.ButtonText = "Update";
                }
            }

            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(nt);
        }
        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public IActionResult Board_of_Directors(BoardOfDirectors Bd)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {

                    var param = new
                    {
                        ID = Bd.ID,
                        HNDesignation = Bd.HNDesignation,
                        HNDirectorAddress = Bd.HNDirectorAddress,
                        HNDirectorName = Bd.HNDirectorName,
                        Designation = Bd.Designation,
                        DirectorAddress = Bd.DirectorAddress,
                        DirectorName = Bd.DirectorName,
                        CreateBy = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        CreatedIp = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    innerresult = dAL.QueryWithExecuteAsync("[InsertBoardOfDirector]", param);


                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                    acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }


        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> GetBoardOfDirectors()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                data = await dAL.QueryAsync("[BoardOfDirectorDetails]", null);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public IActionResult DeleteBoardOfDirector(int Id)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;

            if (ModelState.IsValid)
            {
                try
                {

                    var param = new
                    {
                        ID = Id,
                        UpdateBy = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,

                    };
                    innerresult = dAL.QueryWithExecuteAsync("[DeleteBoardOfDirector]", param);


                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                    acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }

        public async Task<IActionResult> SetDirectorsPriority()
        {
            List<BoardOfDirectors> data = new List<BoardOfDirectors>();
            try
            {
                var param = new { Id = 0 };
                // data = await dAL.QueryAsync("[GetSliderDetails]", param);
                data = await dAL.SelectProcedureExecute<BoardOfDirectors>("[BoardOfDirectorDetails]", null);

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(data);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateDirectorPriority(string Id, string Priority)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            try
            {



                var param = new
                {
                    Id = Id,
                    Priority = Priority == "" ? null : Priority,
                    UserId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value,
                    IPAddress = HttpContext.Connection.RemoteIpAddress.ToString()
                };
                innerresult = dAL.QueryWithExecuteAsync("UpdateDirectorPriority", param);

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
            }
            return Json(innerresult);
        }


        [Authorize(Roles = "admin,srv")]
        public IActionResult UploadMasterDocuments()
        {
            Projects_Count project = new Projects_Count();
            List<Projects_Count> data = new List<Projects_Count>();
            try
            {
                // data = await dAL.QueryAsync("[getSurveyDetails]", null);

                data = DB_Conn.SelectProcedureExecute<Projects_Count>("[GetProjectCount]", null);
                if (data != null)
                {
                    project = data.FirstOrDefault();
                }

            }

            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(project);
        }

        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public IActionResult UploadMasterDocuments(IFormFile Docfile, int type)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (Docfile.FileName != null)
                        FileName = Docfile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + Docfile.FileName.Split('.')[1].ToString();
                    var param = new
                    {
                        type = type,
                        FileName = FileName,
                        LastUpdateBy = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        UpdateIP = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    innerresult = dAL.QueryWithExecuteAsync("UpdateDocuments", param);
                    if (Convert.ToInt32(innerresult.Status) > 0)
                    {
                        if (Docfile != null)
                        {

                            string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/MasterDocuments/");
                            if (!Directory.Exists(RootFolderPath))
                            {
                                Directory.CreateDirectory(RootFolderPath);
                            }
                            string FilePath = Path.Combine(RootFolderPath, FileName);
                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                Docfile.CopyTo(stream);
                            }
                        }
                    }

                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Insert"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                    acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }


    }

    public class DropDown
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }
    public class Location
    {
        public string Id { get; set; }
        public string LocationName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
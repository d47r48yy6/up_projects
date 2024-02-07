using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using UPProjects.Models;

namespace UPProjects.Controllers
{
    public class ProgressPhotoUploadController : Controller
    {
        private readonly DAL dAL;
        private readonly AppCommonMethod acm;
        private readonly IWebHostEnvironment _env;
        
        public ProgressPhotoUploadController(DAL _dAL, AppCommonMethod _acm, IWebHostEnvironment env)
        {
            dAL = _dAL;
            acm = _acm;
            _env = env;
        
        }
        [Authorize(Roles = "admin,unit,gm")]
        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetProjects(string unit)
        {
            List<CommonSelect> objProjects = new List<CommonSelect>();
            var projectparam = new { UnitId = unit };
            objProjects =await dAL.BindSelect("GetProjects", projectparam);
            objProjects.Insert(0, new CommonSelect { Id = "0", Value = "Select Project" });
            return Json(objProjects);
        }
        [Authorize(Roles = "admin,unit,gm,srv")]
        public async Task<IActionResult> ProgressPhotoUpload(int id)
        {
            ProgressPhotoUpload objtender = new ProgressPhotoUpload();
            try
            {
                // show/hide month div
                ViewBag.divYear = 0;    
                //  Bind Year
                List<CommonSelect> obj = new List<CommonSelect>();
                obj = await dAL.BindSelect("GetProgressYearNew", null);
                obj.Insert(0, new CommonSelect { Id = "0", Value = "Select Year" });
                /// Bind Units
                List<CommonSelect> objunits = new List<CommonSelect>();
                var unitparam = new { UnitId =acm.CheckZero(this.User.Claims.First(c => c.Type == "UnitId").Value.ToString()), ZoneId = acm.CheckZero(this.User.Claims.First(c => c.Type == "ZoneId").Value.ToString()), Role = this.User.Claims.First(c => c.Type == "Role").Value.ToString() };
                objunits = await dAL.BindSelect("GetUnits", unitparam);
                objunits.Insert(0, new CommonSelect { Id = "0", Value = "Select Unit" });
                if (id > 0)
                {
                    var param = new { Id = id, UserId = this.User.Claims.First(c => c.Type == "UserId").Value.ToString(),
                        Role = this.User.Claims.First(c => c.Type == "Role").Value.ToString()
                    };
                    objtender = dAL.GetProgressPhotoDetails("GetProgressPhotoUpload", param);
                    ViewBag.ButtonText = "Update";
                    List<CommonSelect> objProjects = new List<CommonSelect>();
                    var projectparam = new { UnitId = this.User.Claims.First(c => c.Type == "UnitId").Value.ToString(), ZoneId = this.User.Claims.First(c => c.Type == "ZoneId").Value.ToString(), Role = this.User.Claims.First(c => c.Type == "Role").Value.ToString() };
                    objProjects = await dAL.BindSelect("GetProjects", projectparam);
                    objProjects.Insert(0, new CommonSelect { Id = "0", Value = "Select Project" });
                    objtender.Projects = new SelectList(objProjects, "Id", "Value", objtender.ProjectId);
                    if (objtender.Category == "2")
                    {
                        ViewBag.divYear = 1;
                        objtender.Years = new SelectList(obj, "Id", "Value", objtender.Year);
                        List<CommonSelect> objmonth = new List<CommonSelect>();
                        var paramyear = new { year = objtender.Year };
                        objmonth = await dAL.BindSelect("GetProgressMonth", paramyear);
                        objmonth.Insert(0, new CommonSelect { Id = "0", Value = "Select Month" });
                        objtender.Months= new SelectList(objmonth, "Id", "Value", objtender.Month);
                    }
                    else
                        objtender.Years = new SelectList(obj, "Id", "Value", 0);
                    objtender.Units = new SelectList(objunits, "Id", "Value", objtender.UnitId);
                }
                else
                {
                    objtender.Units = new SelectList(objunits, "Id", "Value", 0);
                    objtender.Years = new SelectList(obj, "Id", "Value", 0);
                    ViewBag.ButtonText = "Submit";
                }
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(objtender);
        }

        [HttpPost]
        [Authorize(Roles = "admin,unit,gm,srv")]
        public async Task<IActionResult> ProgressPhotoUpload(ProgressPhotoUpload photoup)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if(photoup.file!=null)
                    {

                        foreach(var x in photoup.file)
                        {
                            if (photoup.file != null)
                             FileName = "NA";
                             FileName =x.FileName.Split('.')[0] + DateTime.Now.Ticks + "." +x.FileName.Split('.')[1].ToString();
                            var param = new
                            {
                                Id = photoup.Id,
                                Category = photoup.Category,
                                Title = photoup.Title,
                                Description = photoup.Description,
                                Year = photoup.Year == "0" ? null : photoup.Year,
                                Month = photoup.Month,
                                FileName = FileName,
                                UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                                IPAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
                                UnitId = photoup.UnitId,
                                ZoneId = ((ClaimsIdentity)this.User.Identity).FindFirst("ZoneId").Value,
                                Role = ((ClaimsIdentity)this.User.Identity).FindFirst("Role").Value,
                                ProjectId = photoup.ProjectId
                            };
                            HttpContext.Connection.RemoteIpAddress.ToString();
                            innerresult = dAL.QueryWithExecuteAsync("InsertProgressPhotoUpload", param);
                            if (Convert.ToInt32(innerresult.Status) > 0)
                            {
                                if (x != null)
                                {
                                    string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/ProgressPhoto/" + innerresult.Status);
                                    if (!Directory.Exists(RootFolderPath))
                                    {
                                        Directory.CreateDirectory(RootFolderPath);
                                    }
                                    string FilePath = Path.Combine(RootFolderPath, FileName);
                                    using (var stream = new FileStream(FilePath, FileMode.Create))
                                    {
                                        x.CopyTo(stream);
                                    }
                                }
                            }

                            var DynamicResult = new
                            {
                                innerresult = innerresult,
                                status = true,
                                eventKey = "New Progress Photo Upload"
                            };
                            Result = new { data = "", dynamicResult = DynamicResult };
                        }

                    }
                    else
                    {
                        var param = new
                        {
                            Id = photoup.Id,
                            Category = photoup.Category,
                            Title = photoup.Title,
                            Description = photoup.Description,
                            Year = photoup.Year == "0" ? null : photoup.Year,
                            Month = photoup.Month,
                            FileName = FileName,
                            UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                            IPAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
                            UnitId = photoup.UnitId,
                            ZoneId = ((ClaimsIdentity)this.User.Identity).FindFirst("ZoneId").Value,
                            Role = ((ClaimsIdentity)this.User.Identity).FindFirst("Role").Value,
                            ProjectId = photoup.ProjectId
                        };
                        
                        innerresult = dAL.QueryWithExecuteAsync("InsertProgressPhotoUpload", param);
                       

                        var DynamicResult = new
                        {
                            innerresult = innerresult,
                            status = true,
                            eventKey = "New Progress Photo Upload"
                        };
                        Result = new { data = "", dynamicResult = DynamicResult };
                    }
                      
                  
                  

                }
                catch (Exception ex)
                {
                    await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }

        [Authorize(Roles = "admin,unit,gm,srv")]
        public IActionResult ProgressPhotoUploadList()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "admin,unit,gm,srv")]
        public async Task<IActionResult> ProgressPhotosUploadList()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { UserId = this.User.Claims.First(c => c.Type == "UserId").Value.ToString(), Role = this.User.Claims.First(c => c.Type == "Role").Value.ToString() };
                data = await dAL.QueryAsync("GetProgressPhotoUploadbyLogin", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        [HttpGet]
        [Authorize(Roles = "admin,unit,gm,srv")]
        public async Task<IActionResult> DeleteProgressPhotosUpload(int id)
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = id, UserId = this.User.Claims.First(c => c.Type == "UserId").Value, Role = this.User.Claims.First(c => c.Type == "Role").Value, IPAddress = HttpContext.Connection.RemoteIpAddress.ToString() };
                data = await dAL.QueryAsync("DeleteProgressPhotoUpload", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }
        [Authorize(Roles = "admin,srv")]
        public IActionResult PendingProgressPhoto()
        {
            return View();
        }


        [HttpGet]
        [Authorize (Roles ="admin,srv")]
        public async Task<IActionResult> GetPendingListofProgressPhotos()
        {
            var Result = (dynamic)null;
            try
            {
                Result =await dAL.QueryAsync("GetPendingListofProgressPhotos", null);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(Result);
        }
        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> Approve(string Id)
        {
            var Result = (dynamic)null;
            try
            {
                var param = new { Id=Id,UserId=(this.User.Claims.First (c=> c.Type=="UserId").Value),IPAddress=AppCommonMethod.GetIP(),Action="A" };
                Result = await dAL.ExecuteAsync("ApproveRejectProgressPhoto", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(Result);
        }

        [HttpPost]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> Reject(string Id)
        {
            var Result = (dynamic)null;
            try
            {
                var param = new { Id = Id, UserId = (this.User.Claims.First(c => c.Type == "UserId").Value), IPAddress = AppCommonMethod.GetIP(), Action = "R" };
                Result = await dAL.ExecuteAsync("ApproveRejectProgressPhoto", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(Result);
        }

        [HttpGet]
        [Authorize(Roles = "admin,unit,gm,srv")]
        public async Task<JsonResult> BindMonth(string year) {
            var Result = (dynamic)null;
            try
            {
                var param = new { year=year };
                Result = await dAL.QueryAsync("GetProgressMonth", param);                
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(Result);
        }
    }
}
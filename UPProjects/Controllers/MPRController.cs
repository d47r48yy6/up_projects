using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPProjects.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace UPProjects.Controllers
{
    [Authorize]
    public class MPRController : Controller
    {
        private readonly DAL dal;
        private readonly AppCommonMethod acm;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly MPRDAL mprdal; 
        public MPRController(DAL _dal, AppCommonMethod _acm, MPRDAL _mprdal, IHostingEnvironment _hostingEnvironment)
        {
            dal = _dal;
            acm = _acm;
            mprdal = _mprdal;
            hostingEnvironment = _hostingEnvironment;
        }
        [Authorize(Roles = "unit")]
        public IActionResult MPR()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetMPRList()
        {
            var Result = (dynamic)null;
            try
            {
                TempData.Remove("Id");
                TempData.Remove("FinancialYear");
                TempData.Remove("Month");

                await YearList();
                await MonthList();

                List<MPR> mp = new List<MPR>();
                var innerresult = (dynamic)null;
                var param = new { UnitId = this.User.Claims.First(c => c.Type == "UnitId").Value.ToString() };
                mp = await mprdal.GetModuleAsync("getMPR", param);
                ViewBag.MPRList = innerresult;
                var DynamicResult = new { status = true, innerresult = innerresult };

                MPR mp1 = new MPR();
               
                var re = await mprdal.GetModuleAsync("getMPR", param);
                if(re.Count>0)
                mp1 = re.ElementAt(0);

                Result = new { 
                data = await RenderViewAsString.RenderViewAsync(this, "_MPRList", mp, true), dynamicResult = DynamicResult,
                data1 = await RenderViewAsString.RenderViewAsync(this, "_AddMPR", mp1, true)
                };
                return Json(Result);

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
            }
            return Json(Result);
            //return Json(Result, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<IActionResult> MPR(MPR formdata)
        {
            var Result = (dynamic)null;
            if (ModelState.IsValid)
            {
                try
                {
                    string mFileName = (dynamic)null;
                    string FinancialYear = formdata.FinancialYear;
                    string mMonth = formdata.Month;
                    var innerresult = (dynamic)null;
                    var RootFolderPath = Path.Combine(hostingEnvironment.WebRootPath, "Upload/MPR");
                    mFileName = formdata.file.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + formdata.file.FileName.Split('.')[1].ToString();
                    if (!Directory.Exists(RootFolderPath))
                    {
                        Directory.CreateDirectory(RootFolderPath);
                    }
                    using (var fileStream = new FileStream(Path.Combine(RootFolderPath, mFileName), FileMode.Create))
                    {
                        formdata.file.CopyTo(fileStream);
                    }
                    var param = new
                    {
                        Id = TempData["Id"] != null ? TempData["Id"] : 0,
                        FinYear = FinancialYear,
                        Month = mMonth,
                        FileName = mFileName,
                        UploadBy = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value,
                        UploadIPAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
                        UnitId=(this.User.Claims.First(c=> c.Type=="UnitId").Value.ToString())
                    };
                    innerresult = await dal.QueryAsync("InsertUpdateMPR", param);
                    if (TempData["Id"] != null)
                    {
                        TempData.Remove("Id");
                    }
                    Result = new { innerresult= innerresult };
                  //  ViewBag.MPRList = await mprdal.GetModuleAsync("getMPR", null);
                  //  Result = new { data = await RenderViewAsString.RenderViewAsync(this, "_MPRList", ViewBag.MPRList, true) };
                    return Json(Result);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return Json(Result);
        }


        public async Task<IActionResult> Edit(int? Id)
        {
            var Result = (dynamic)null;
            MPR mp = new MPR();
            try
            {
                if (Id != 0)
                {
                    var param = new { Id = Id, UnitId = this.User.Claims.First(c => c.Type == "UnitId").Value.ToString() };
                    var re = await mprdal.GetModuleAsync("getMPR", param);
                    mp = re.ElementAt(0);
                    TempData["Id"] = mp.Id;
                    TempData["FinancialYear"] = mp.FinancialYear.Trim();
                    TempData["Month"] = mp.Month.Trim();
                    await YearList();
                    await MonthList();
                    Result = new { data = await RenderViewAsString.RenderViewAsync(this, "_AddMPR", mp, true)};
                }
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
            }
            return Json(Result);
        }

        public async Task YearList()
        {
            ViewBag.YearList = new SelectList(
                                       new List<SelectListItem>
                                       {
                                        new SelectListItem { Text = "Select", Value = "0" },
                                        new SelectListItem { Text = "2018-19", Value = "2018-19" },
                                        new SelectListItem { Text = "2019-20", Value = "2019-20"},
                                       }, "Value", "Text", TempData["FinancialYear"]);

        }
        public async Task MonthList()
        {
            ViewBag.MonthList = new SelectList(
                                     new List<SelectListItem>
                                     {
                                        new SelectListItem { Text = "Select", Value = "0" },
                                        new SelectListItem { Text = "April", Value = "April" },
                                        new SelectListItem { Text = "May", Value = "May"},
                                         new SelectListItem { Text = "June", Value = "June" },
                                        new SelectListItem { Text = "July", Value = "July"},
                                         new SelectListItem { Text = "August", Value = "August" },
                                        new SelectListItem { Text = "September", Value = "September"},
                                          new SelectListItem { Text = "October", Value = "October" },
                                        new SelectListItem { Text = "November", Value = "November"},
                                          new SelectListItem { Text = "December", Value = "December" },
                                        new SelectListItem { Text = "January", Value = "January"},
                                         new SelectListItem { Text = "February", Value = "February"},
                                          new SelectListItem { Text = "March", Value = "March"}
                                     }, "Value", "Text", TempData["Month"]);

        }



        [Authorize(Roles = "admin,gm")]
        public IActionResult PendingMPR()
        {
            List<MPR> objMPR = new List<MPR>();
            try
            {
                var param = new { OfficeId = this.User.Claims.First(c => c.Type == "OfficeId").Value.ToString(), Role = this.User.Claims.First(c => c.Type == "Role").Value.ToString()
                ,UserId = this.User.Claims.First(c => c.Type == "UserId").Value.ToString()};
                objMPR = mprdal.GetListofPendingMPR("GetNewMPRtoApprove", param);
            }
            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(objMPR);
        }

        [Authorize(Roles = "admin,gm")]
        public IActionResult ApproveMPR(int? Id)
        {          
            var res = (dynamic)null;          
            try
            {
                if (Id != 0)
                {
                    var param = new { Id = Id, UserId = this.User.Claims.First(c => c.Type == "UserId").Value.ToString(),
                        OfficeId = this.User.Claims.First(c => c.Type == "OfficeId").Value.ToString(),
                        Role = this.User.Claims.First(c => c.Type == "Role").Value.ToString(),
                        IPAddress = AppCommonMethod.GetIP()
                    };
                    string result =  dal.Executesync("ApproveMPR", param);
                    if(result=="Success")
                        result = "Approve";
                    res = new
                    {
                        result= result
                    };
                   }                
            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
            }
            return Json(res);
        }

        [Authorize(Roles = "admin,gm")]
        public IActionResult DisApproveMPR(int? Id)
        {
            var res = (dynamic)null;
            try
            {
                if (Id != 0)
                {
                    var param = new
                    {
                        Id = Id,
                        UserId = this.User.Claims.First(c => c.Type == "UserId").Value.ToString(),
                        OfficeId = this.User.Claims.First(c => c.Type == "OfficeId").Value.ToString(),
                        Role = this.User.Claims.First(c => c.Type == "Role").Value.ToString(),
                        IPAddress = HttpContext.Connection.RemoteIpAddress.ToString()
                    };
                    string result = dal.Executesync("DisApproveMPR", param);
                    if (result == "Success")
                        result = "Reject";
                    res = new
                    {
                        result = result
                    };
                }
            }
            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
            }
            return Json(res);
        }

        public async Task<IActionResult> MPRMaster(int Id)
        {
            var UserId = this.User.Claims.First(c => c.Type == "UserName").Value.ToString();       
            ViewBag.user = UserId;
            MPRMaster mt = new MPRMaster();
            try
            {
                ViewBag.List = mt.GetAllMPRList();
                if (Id == 0 || Id == null)
                {

                }
                else
                {
                    mt = mt.GetMPRMaterById(Id);
                }
            }
          
             catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
            }

            return View(mt);
        }

        public async Task<JsonResult> BindMonth(string year)
        {
            ProjectMaster mt = new ProjectMaster();
            List<ProgressMonth> pm = new List<ProgressMonth>();
            var data = (dynamic)null;
            try
            {
                data = mt.GetProgressMonth(year);
            }
            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
            }

          
            return Json(data);
        }

        public async Task<JsonResult> BindYear()
        {
            ProjectMaster mt = new ProjectMaster();
            List<ProgressYear> pm = new List<ProgressYear>();
            var data = (dynamic)null;
            try
            {
                 data = mt.GetProgressYearNPR();
            }
            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
            }
            
            return Json(data);
        }
        public async Task<JsonResult> SaveMPR(MPRMaster obj)
        {
            dynamic msg = (null);
            var UserRole = this.User.Claims.First(c => c.Type == "Role").Value.ToString();
            var unitid = this.User.Claims.First(c => c.Type == "UnitId").Value.ToString();
            MPRMaster mt = new MPRMaster();
            try
            {
                msg = mt.InsertMPRMaster(obj, UserRole, unitid);
            }
           
            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
            }
            return Json(msg);
        }
        public JsonResult DeleteMPR(int Id)
        {
            dynamic msg = (null);
            MPRMaster mt = new MPRMaster();
            var UserRole = this.User.Claims.First(c => c.Type == "Role").Value.ToString();
            var unitid = this.User.Claims.First(c => c.Type == "UnitId").Value.ToString();
            msg = mt.DeleteMPRMaster(Id, UserRole);
            return Json(msg);
        }

        public IActionResult MPRReport()
        {
            var UserRole = this.User.Claims.First(c => c.Type == "Role").Value.ToString();
            var ZoneId = this.User.Claims.First(c => c.Type == "ZoneId").Value.ToString();
            var UnitId = this.User.Claims.First(c => c.Type == "UnitId").Value.ToString();
            ViewBag.UserRole = UserRole;
            ViewBag.ZoneId = ZoneId;
            ViewBag.UnitId = UnitId;
            return View();
        }

        public JsonResult BindZone()
        {
            MPRMaster ut = new MPRMaster();
            List<Zone> data = new List<Zone>();
            data = ut.GetAllZone();
            return Json(data);
        }

        public JsonResult BindUnits(string ZoneId)
        {
            MPRMaster ut = new MPRMaster();
            List<UnitByZone> data = new List<UnitByZone>();
            data = ut.GetUnitsByZone(Convert.ToInt32(ZoneId));
            return Json(data);
        }
        public ActionResult MPRReportByUnit(string UnitId)
        {
            MPRMaster mt = new MPRMaster();
            List<MPRMaster> data = new List<MPRMaster>();
            data = mt.GetAllReportByUnit(UnitId);
            return View(data);
        }




    }
}
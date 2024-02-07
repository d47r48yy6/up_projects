using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UPProjects.Models;

namespace UPProjects.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly DAL dAL;
        private readonly AppCommonMethod acm;
        private readonly IWebHostEnvironment _env;

        public UserController(DAL _dAL, AppCommonMethod _acm, IWebHostEnvironment env)
        {
            dAL = _dAL;
            acm = _acm;
            _env = env;
        }

        [Authorize(Roles ="unit")]
        public IActionResult Dashboard()
        {
           
            return View();
        }
        [Authorize(Roles = "gm")]
        public async Task<IActionResult> GMDashboard()
        {
            PMList result = new PMList();
            List<CommonSelect> obj = new List<CommonSelect>();
            var UserRole = this.User.Claims.First(c => c.Type == "Role").Value.ToString();
            var ZoneIds = this.User.Claims.First(c => c.Type == "ZoneId").Value.ToString();
            var UnitIds = this.User.Claims.First(c => c.Type == "UnitId").Value.ToString();
            ViewBag.UserRole = UserRole;
            ViewBag.ZoneId1 = ZoneIds;
            ViewBag.UnitId1 = UnitIds;

            ////////////////////////////////Get District List//////////////////////////////// 
            obj = await dAL.BindSelect("GetDistrictDropDownList", null);
            obj.Insert(0, new CommonSelect { Id = "0", Value = "Select District" });
            result.DistrictList = new SelectList(obj, "Id", "Value", 0);


            List<CommonSelect> obj1 = new List<CommonSelect>();

            ////////////////////////////////Get District List//////////////////////////////// 
            obj1 = await dAL.BindSelect("GetZone", null);
            obj1.Insert(0, new CommonSelect { Id = "0", Value = "Select Zone" });
            result.ZoneList = new SelectList(obj1, "Id", "Value", 0);


            return View(result);
        }
        
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> HQDashboard()
        {
            PMList result = new PMList();
            List<CommonSelect> obj = new List<CommonSelect>();
            var UserRole = this.User.Claims.First(c => c.Type == "Role").Value.ToString();
            var ZoneIds = this.User.Claims.First(c => c.Type == "ZoneId").Value.ToString();
            var UnitIds = this.User.Claims.First(c => c.Type == "UnitId").Value.ToString();
            ViewBag.UserRole = UserRole;
            ViewBag.ZoneId1 = ZoneIds;
            ViewBag.UnitId1 = UnitIds;

            ////////////////////////////////Get District List//////////////////////////////// 
            obj = await dAL.BindSelect("GetDistrictDropDownList", null);
            obj.Insert(0, new CommonSelect { Id = "0", Value = "Select District" });
            result.DistrictList = new SelectList(obj, "Id", "Value", 0);


            List<CommonSelect> obj1 = new List<CommonSelect>();

            ////////////////////////////////Get Zone List//////////////////////////////// 
            obj1 = await dAL.BindSelect("GetZone", null);
            obj1.Insert(0, new CommonSelect { Id = "0", Value = "Select Zone" });
            result.ZoneList = new SelectList(obj1, "Id", "Value", 0);


            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> GetDashBoardSchool(string DistrictId,string Date, string ZoneId,string UnitId,string Type)
        {
            DashBoardCount result = new DashBoardCount();
            try
            {
                var param = new
                {
                    DistrictId = DistrictId=="0"?null: DistrictId,
                    Date = string.IsNullOrEmpty(Date) ? "" : DateTime.ParseExact(Date.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                   
                    ZoneId = ZoneId=="0"?null: ZoneId,
                    UnitId= UnitId=="0"?null: UnitId,
                    Type= Type==""?null:Type

                };
                result =dAL.SelectModel<DashBoardCount>("GetDashboardCount", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            // return model
            return Json(result);
        }

        public async Task<IActionResult> SurveyList(string Sitetype, string type, string DistrictId, string Date, string ZoneId, string UnitId)
        {
            ViewBag.SiteType = Sitetype;
            ViewBag.type = type;
            ViewBag.DistrictId = DistrictId;
            ViewBag.Date = Date;
            ViewBag.ZoneId = ZoneId;
            ViewBag.UnitId = UnitId;
            return View();
        }

        public async Task<IActionResult> GetSurveryCompletedList(string Sitetype, string type, string DistrictId, string Date, string ZoneId, string UnitId)
        {
            var UserRole = this.User.Claims.First(c => c.Type == "Role").Value.ToString();
            var data = (dynamic)null;
            try
            {
                var t = "";
                if(Sitetype=="S")
                {
                    t = "School";
                }
               else if(Sitetype=="A")
                {
                    t = "Anganwadi";
                }
                else if(Sitetype=="AS")
                {
                    t = "Aashram";
                }
                else
                {
                    t = "T";
                }
                var param = new
                {
                    
                    DistrictId = DistrictId == "0" ? null : DistrictId,
                    Date = string.IsNullOrEmpty(Date) ? "" : DateTime.ParseExact(Date.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                    ZoneId = ZoneId == "0" ? null : ZoneId,
                    UnitId = UnitId == "0" ? null : UnitId,
                    Sitetype = t=="T"?null:t,
                    type = type,
                    Role= UserRole
                };
                data = await dAL.QueryAsync("[GetSurveyListByDashboard]", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        public IActionResult SurveyDetails()
        {
            List<SurveyListByUnit> result = new List<SurveyListByUnit>();
           
            try
            {
                result = DB_Conn.SelectProcedureExecute<SurveyListByUnit>("[GetSurveyListByZoneUnitDistrict]", null);
                result = result.OrderByDescending(x => x.ZoneId).OrderByDescending(x=>x.UnitId).ToList();
               
            }

            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return View(result);
        }


        //////////////Update Survey Status///////////////////////////////////////////////

        public async Task<object> UpdateSurveyStatus(string Id, string st)
        {
            var innerresult = (dynamic)null;
           

            try
            {
              
                var param = new
                {
                    Id =Id,
                    status=st,
                    UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                    IpAddress = HttpContext.Connection.RemoteIpAddress.ToString()


                };
                innerresult = dAL.QueryWithExecuteAsync("UpdateSurveyStatus", param);


              


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
               
            }
            return innerresult;
        }

        [Authorize(Roles = "admin,hq,gm,unit")]
        public IActionResult RejectSurveyList()
        {
            return View();
        }


        
        public async Task<IActionResult> GetSurveryRejectedList()
        {

            var data = (dynamic)null;
            try
            {
                var zone = ((ClaimsIdentity)this.User.Identity).FindFirst("OfficeId").Value;


                var param = new
                {
                    
                    ZoneId =zone=="0"?null:zone

                };
                data = await dAL.QueryAsync("[GetRejectedSurveyList]", param);
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



        public class DashBoardCount
        {
            public string TotalVisited { get; set; }
            public string ElectricityAvailibity { get; set; }
            public string HandPumpAvailibity { get; set; }
            public string WhetherTapWater { get; set; }
            public string WhetherRedMark { get; set; }
            public string Feasibility { get; set; }
            public string NonFeasibiliy { get; set; }
            public string WorkHandover { get; set; }
            public string SchoolWorkHandover { get; set; }
            public string AnganwadiWorkHandover { get; set; }
            public string TotalWorkHandover { get; set; }
        }
    }
}
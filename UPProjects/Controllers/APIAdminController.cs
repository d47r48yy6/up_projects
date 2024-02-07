using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPProjects.Models;

namespace UPProjects.Controllers
{
    [Authorize(AuthenticationSchemes = AuthSchemes)]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class APIAdminController : ControllerBase
    {
        private const string AuthSchemes =
       CookieAuthenticationDefaults.AuthenticationScheme + "," +
       JwtBearerDefaults.AuthenticationScheme;
        private readonly DAL dAL;
        private readonly AppCommonMethod acm;
        private readonly IWebHostEnvironment _env;
        public APIAdminController(DAL _dAL, AppCommonMethod _acm, IWebHostEnvironment env)
        {
            dAL = _dAL;
            acm = _acm;
            _env = env;

        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<object> App_GetDashBoardAdmin(ExpandoObject expando)
        {
            
            DashBoardCountWholeDetails result = new DashBoardCountWholeDetails();
            DashBoardCount school = new DashBoardCount();
            DashBoardCount anganwadi = new DashBoardCount();
            DashBoardCount aashram = new DashBoardCount();
            DashBoardCount totalsurvey = new DashBoardCount();
            
           var expandoDict = expando as IDictionary<string, object>;
            var t = "";
            var DistrictId = expandoDict["DistrictId"].ToString();
            var Date = expandoDict["Date"].ToString(); 
            var ZoneId = expandoDict["ZoneId"].ToString();
            var UnitId = expandoDict["UnitId"].ToString();
            try
            {
                ///////////////////////////////////////For School Counting//////////////////////////////
                var schoolparam = new
                {

                    DistrictId = DistrictId==""?null: DistrictId,
                    Date = Date == "" ? null : Date,
                    ZoneId = ZoneId == "" ? null : ZoneId,
                    UnitId = UnitId == "" ? null : UnitId,
                    Type = "School"
                };
                school = dAL.SelectModel<DashBoardCount>("GetDashboardCount", schoolparam);


                ///////////////////////////////////////For Anganwadi Counting//////////////////////////////
                var anganwadiparam = new
                {

                    DistrictId = DistrictId == "" ? null : DistrictId,
                    Date = Date == "" ? null : Date,
                    ZoneId = ZoneId == "" ? null : ZoneId,
                    UnitId = UnitId == "" ? null : UnitId,
                    Type = "Anganwadi"
                };
                anganwadi = dAL.SelectModel<DashBoardCount>("GetDashboardCount", anganwadiparam);


                ///////////////////////////////////////For Aashram Counting//////////////////////////////
                var aashramparam = new
                {

                    DistrictId = DistrictId == "" ? null : DistrictId,
                    Date = Date == "" ? null : Date,
                    ZoneId = ZoneId == "" ? null : ZoneId,
                    UnitId = UnitId == "" ? null : UnitId,
                    Type = "Aashram"
                };
                aashram = dAL.SelectModel<DashBoardCount>("GetDashboardCount", aashramparam);


                ///////////////////////////////////////For Total Counting//////////////////////////////
                var totalparam = new
                {

                    DistrictId = DistrictId == "" ? null : DistrictId,
                    Date = Date == "" ? null : Date,
                    ZoneId = ZoneId == "" ? null : ZoneId,
                    UnitId = UnitId == "" ? null : UnitId,
                    Type = t == "" ? null : t
                };
                totalsurvey = dAL.SelectModel<DashBoardCount>("GetDashboardCount", totalparam);

                //////////////////////////////////////Group all/////////////////////////////////////////

                result.SchoolTotalVisited = school.TotalVisited;
                result.SchoolElectricityAvailibity = school.ElectricityAvailibity;
                result.SchoolHandPumpAvailibity = school.HandPumpAvailibity;
                result.SchoolWhetherTapWater = school.WhetherTapWater;
                result.SchoolWhetherRedMark = school.WhetherRedMark;
                result.SchoolFeasibility = school.Feasibility;
                result.SchoolNonFeasibiliy = school.NonFeasibiliy;
                result.SchoolWorkHandover = school.SchoolWorkHandover;


                result.AnganwadiTotalVisited = anganwadi.TotalVisited;
                result.AnganwadiElectricityAvailibity = anganwadi.ElectricityAvailibity;
                result.AnganwadiHandPumpAvailibity = anganwadi.HandPumpAvailibity;
                result.AnganwadiWhetherTapWater = anganwadi.WhetherTapWater;
                result.AnganwadiWhetherRedMark = anganwadi.WhetherRedMark;
                result.AnganwadiFeasibility = anganwadi.Feasibility;
                result.AnganwadiNonFeasibiliy = anganwadi.NonFeasibiliy;
                result.AnganwadiWorkHandover = anganwadi.AnganwadiWorkHandover;



                result.AashramTotalVisited = aashram.TotalVisited;
                result.AashramElectricityAvailibity = aashram.ElectricityAvailibity;
                result.AashramHandPumpAvailibity = aashram.HandPumpAvailibity;
                result.AashramWhetherTapWater = aashram.WhetherTapWater;
                result.AashramWhetherRedMark = aashram.WhetherRedMark;
                result.AashramFeasibility = aashram.Feasibility;
                result.AashramNonFeasibiliy = aashram.NonFeasibiliy;
                result.AashramWorkHandover = aashram.WorkHandover;



                result.TotalVisited = totalsurvey.TotalVisited;
                result.ElectricityAvailibity = totalsurvey.ElectricityAvailibity;
                result.HandPumpAvailibity = totalsurvey.HandPumpAvailibity;
                result.WhetherTapWater = totalsurvey.WhetherTapWater;
                result.WhetherRedMark = totalsurvey.WhetherRedMark;
                result.Feasibility = totalsurvey.Feasibility;
                result.NonFeasibiliy = totalsurvey.NonFeasibiliy;
                result.WorkHandover = totalsurvey.TotalWorkHandover;


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }

            return result;
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
        public class DashBoardCountWholeDetails
        {
            public string SchoolTotalVisited { get; set; }
            public string SchoolElectricityAvailibity { get; set; }
            public string SchoolHandPumpAvailibity { get; set; }
            public string SchoolWhetherTapWater { get; set; }
            public string SchoolWhetherRedMark { get; set; }
            public string SchoolFeasibility { get; set; }
            public string SchoolNonFeasibiliy { get; set; }
            public string SchoolWorkHandover { get; set; }



            public string AnganwadiTotalVisited { get; set; }
            public string AnganwadiElectricityAvailibity { get; set; }
            public string AnganwadiHandPumpAvailibity { get; set; }
            public string AnganwadiWhetherTapWater { get; set; }
            public string AnganwadiWhetherRedMark { get; set; }
            public string AnganwadiFeasibility { get; set; }
            public string AnganwadiNonFeasibiliy { get; set; }
            public string AnganwadiWorkHandover { get; set; }


            public string AashramTotalVisited { get; set; }
            public string AashramElectricityAvailibity { get; set; }
            public string AashramHandPumpAvailibity { get; set; }
            public string AashramWhetherTapWater { get; set; }
            public string AashramWhetherRedMark { get; set; }
            public string AashramFeasibility { get; set; }
            public string AashramNonFeasibiliy { get; set; }
            public string AashramWorkHandover { get; set; }


            public string TotalVisited { get; set; }
            public string ElectricityAvailibity { get; set; }
            public string HandPumpAvailibity { get; set; }
            public string WhetherTapWater { get; set; }
            public string WhetherRedMark { get; set; }
            public string Feasibility { get; set; }
            public string NonFeasibiliy { get; set; }
            public string WorkHandover { get; set; }


        }
    }
}
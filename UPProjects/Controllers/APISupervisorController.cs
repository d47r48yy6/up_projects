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
using System.Data;

namespace UPProjects.Controllers
{



    //[Authorize(AuthenticationSchemes = AuthSchemes)]

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class APISupervisorController : ControllerBase
    {


        private const string AuthSchemes =
        CookieAuthenticationDefaults.AuthenticationScheme + "," +
        JwtBearerDefaults.AuthenticationScheme;
        private readonly DAL dAL;
        private readonly AppCommonMethod acm;
        private readonly IWebHostEnvironment _env;
        public APISupervisorController(DAL _dAL, AppCommonMethod _acm, IWebHostEnvironment env)
        {
            dAL = _dAL;
            acm = _acm;
            _env = env;

        }
        //////////////////////////////////////////Supervisor Login Check/////////////////////////////////////
        [AllowAnonymous]
        [HttpPost]
        public async Task<object> App_CheckSupervisor(ExpandoObject expando)
        {
            var Result = (dynamic)null;


            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    MobileNumber = expandoDict["MobileNumber"]
                };
                Result = dAL.Query("App_CheckSupervisorLogin", CommandType.StoredProcedure, param);
                Result = Result[0];

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Result;
        }

        //////////////////////////////////////////Supervisor Districts/////////////////////////////////////

        [HttpPost]
        [ActionName("App_GetSupervisorDistricts")]
        [AllowAnonymous]
        public async Task<object> App_GetSupervisorDistricts(ExpandoObject expando)
        {
            var Result = (dynamic)null;

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    SupervisorId = expandoDict["SupervisorId"],
                };
                Result = dAL.Query("App_GetSupervisorDistricts", CommandType.StoredProcedure, param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Result;
        }


        [HttpPost]
        [ActionName("App_GetSupervisorSites")]
        [AllowAnonymous]
        public async Task<object> App_GetSupervisorSites(ExpandoObject expando)
        {
            var Result = (dynamic)null;

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {

                    SupervisorId = expandoDict["SupervisorId"]
                };
                Result = dAL.Query("App_GetSupervisorSites", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Result;
        }

        [HttpPost]
        [ActionName("App_GetFilteredSites")]
        [AllowAnonymous]
        public async Task<object> App_GetFilteredSites(ExpandoObject expando)
        {
            var Result = (dynamic)null;

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {

                    SupervisorId = expandoDict["SupervisorId"],
                    DistrictId = expandoDict["DistrictId"],
                    TehsilId = expandoDict["TehsilId"],
                    BlockId = expandoDict["BlockId"],
                    GramSabhaId = expandoDict["GramSabhaId"],
                    SiteType = expandoDict["SiteType"],
                    SitetypeStatus = expandoDict["SitetypeStatus"]
                };
                Result = dAL.Query("App_GetSupervisorSites", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Result;
        }
        [HttpPost]
        [ActionName("App_GetSupervisorDashboardCount")]
        [AllowAnonymous]
        public async Task<object> App_GetSupervisorDashboardCount(ExpandoObject expando)
        {
            var Result = (dynamic)null;

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var Fromdate = expandoDict["FromDate"].ToString();
                var Todate = expandoDict["ToDate"].ToString();
                var param = new
                {
                    SupervisorId = expandoDict["SupervisorId"],
                    Fromdate = Fromdate == "" ? null : GetDate(Fromdate),
                    Todate = Todate == "" ? null : GetDate(Todate)
                };
                Result = dAL.Query("App_GetSupervisorDashboardCount", CommandType.StoredProcedure, param);
                //Result = dAL.Query("App_GetSupervisordetailsbyIdContractor", CommandType.StoredProcedure, param);
                Result = Result[0];

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Result;
        }
        public string GetDate(string v)
        {
            var s = "";
            var val = v.Split('/');
            s = val[2] + '-' + val[1] + '-' + val[0];
            return s;
        }



        [AllowAnonymous]
        [HttpPost]
        [ActionName("App_UpdateCurrentWorkStatus")]
        public async Task<object> App_UpdateCurrentWorkStatus(ExpandoObject expando)
        {
            var innerresult = (dynamic)null;
            var expandoDict = expando as IDictionary<string, object>;
            ResponseSet1 result = new ResponseSet1();
            try
            {

                var SupervisorId = expandoDict["SupervisorId"];
                var SiteId = expandoDict["SiteId"];
                var CurrentWorkStatusId = expandoDict["CurrentWorkStatusId"];
                var Latitude = expandoDict["Latitude"];
                var Longitude = expandoDict["Longitude"];
                var LocationName = expandoDict["LocationName"];
                var File1 = Guid.NewGuid();
                var FileName1 = expandoDict["Image"];

                var param = new
                {
                    SupervisorId = SupervisorId,
                    SiteId = SiteId,
                    CurrentWorkStatusId = CurrentWorkStatusId,
                    Latitude = Latitude,
                    Longitude= Longitude,
                    LocationName = LocationName,
                    Image1 = File1 + ".jpg"
                };
                innerresult = dAL.QueryWithExecuteAsync("App_UpdateCurrentWorkStatus", param);
                if (Convert.ToInt32(innerresult.ResultCode) > 0)
                {

                    if (FileName1 != null)
                    {
                        string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/Supervisor");
                        if(CurrentWorkStatusId.ToString() == "1")
                        {
                            var folderPath = Path.Combine(_env.WebRootPath, "Upload/Supervisor/WorkStarted");
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }

                            System.IO.File.WriteAllBytes(Path.Combine(folderPath, File1 + ".jpg"), Convert.FromBase64String(FileName1.ToString()));
                        }
                        if (CurrentWorkStatusId.ToString() == "2")
                        {
                            var folderPath = Path.Combine(_env.WebRootPath, "Upload/Supervisor/WorkInProgress");
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }

                            System.IO.File.WriteAllBytes(Path.Combine(folderPath, File1 + ".jpg"), Convert.FromBase64String(FileName1.ToString()));
                        }
                        if (CurrentWorkStatusId.ToString() == "3")
                        {
                            var folderPath = Path.Combine(_env.WebRootPath, "Upload/Supervisor/WorkCompleted");
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }

                            System.IO.File.WriteAllBytes(Path.Combine(folderPath, File1 + ".jpg"), Convert.FromBase64String(FileName1.ToString()));
                        }
                        if (CurrentWorkStatusId.ToString() == "4")
                        {
                            var folderPath = Path.Combine(_env.WebRootPath, "Upload/Supervisor/WorkHandover");
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }

                            System.IO.File.WriteAllBytes(Path.Combine(folderPath, File1 + ".jpg"), Convert.FromBase64String(FileName1.ToString()));
                        }

                    }
                }

                result.Message = innerresult.ResultMessage;
                result.Status = innerresult.ResultStatus;
                result.WhetherSiteFormFilled = Convert.ToString(innerresult.WhetherSiteFormFilled);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                result.Message = "";
                result.Status = "False";
            }
            return result;
        }

        //[AllowAnonymous]
        //[HttpPost]
        //[ActionName("App_InsertSiteWorkCompleteDetails")]
        //public async Task<object> App_InsertSiteWorkCompleteDetails(ExpandoObject expando)
        //{
        //    var innerresult = (dynamic)null;
        //    var expandoDict = expando as IDictionary<string, object>;
        //    ResponseSet result = new ResponseSet();
        //    try
        //    {

              

        //        var param = new
        //        {
        //                SiteId  =expandoDict["SiteId"]
        //              ,
        //                SupervisorId = expandoDict["SupervisorId"]
        //              ,
        //                VillageName = expandoDict["VillageName"]
        //              ,
        //                SiteName = expandoDict["SiteName"]
        //              ,
        //                DistrictId = expandoDict["DistrictId"]
        //              ,
        //                TehsilId = expandoDict["TehsilId"]
        //              ,
        //                BlockId = expandoDict["BlockId"]
        //              ,
        //                SumersiblePumpSet = expandoDict["SumersiblePumpSet"]
        //              ,
        //                SumersiblePumpSerialNo = expandoDict["SumersiblePumpSerialNo"]
        //              ,
        //                SumersiblePumpHorsePower = expandoDict["SumersiblePumpHorsePower"]
        //              ,
        //                SumersiblePumpSetDischarge = expandoDict["SumersiblePumpSetDischarge"]
        //              ,
        //                SumersiblePumpSetHeadMeter = expandoDict["SumersiblePumpSetHeadMeter"]
        //              ,
        //                StarterMake = expandoDict["StarterMake"]
        //              ,
        //                PVCFlatCable = expandoDict["PVCFlatCable"]
        //              ,
        //                CPVCPipeLowering = expandoDict["CPVCPipeLowering"]
        //              ,
        //                CPVCPipeDelivery = expandoDict["CPVCPipeDelivery"]
        //              ,
        //                CPVCPipeTankTab = expandoDict["CPVCPipeTankTab"]
        //              ,
        //                CPVCBallvalves25 = expandoDict["CPVCBallvalves25"]
        //              ,
        //                CPVCBallvalves20 = expandoDict["CPVCBallvalves20"]
        //              ,
        //                PVCWaterStorageTank = expandoDict["PVCWaterStorageTank"]
        //              ,
        //                CPBrassPushCockTab = expandoDict["CPBrassPushCockTab"]
        //              ,
        //               Remark = expandoDict["Remark"]

        //        };
        //        innerresult = dAL.QueryWithExecuteAsync("App_InsertSiteWorkCompleteDetails", param);

        //        result.Message = innerresult.ResultMessage;
        //        result.Status = innerresult.ResultStatus;

        //    }
        //    catch (Exception ex)
        //    {
        //        await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
        //        result.Message = "";
        //        result.Status = "False";
        //    }
        //    return result;
        //}

        [HttpPost]
        [ActionName("App_GetSitesFromWorkStatus")]
        [AllowAnonymous]
        public async Task<object> App_GetSitesFromWorkStatus(ExpandoObject expando)
        {
            var Result = (dynamic)null;

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    SupervisorId = expandoDict["SupervisorId"],
                    WorkStatus = expandoDict["WorkStatus"],
                    DistrictId = expandoDict["DistrictId"],
                    TehsilId = expandoDict["TehsilId"],
                    BlockId = expandoDict["BlockId"],
                    GramSabhaId = expandoDict["GramSabhaId"],
                    SiteType = expandoDict["SiteType"]
                };
                Result = dAL.Query("App_GetSitesFromWorkStatus", CommandType.StoredProcedure, param);

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Result;
        }

        [HttpPost]
        [ActionName("App_GetSupervisorSiteDetails")]
        [AllowAnonymous]
        public async Task<object> App_GetSupervisorSiteDetails(ExpandoObject expando)
        {
            var Result = (dynamic)null;

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    SupervisorId = expandoDict["SupervisorId"],
                    SiteId = expandoDict["SiteId"],

                };
                Result = dAL.Query("App_GetSupervisorSiteDetails", CommandType.StoredProcedure, param);
                Result = Result[0];
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Result;
        }


        [AllowAnonymous]
        [HttpPost]
        [ActionName("App_SupervisorInsertRemarkIssue")]
        public async Task<object> App_SupervisorInsertRemarkIssue(ExpandoObject expando)
        {
            var innerresult = (dynamic)null;
            var expandoDict = expando as IDictionary<string, object>;
            ResponseSet result = new ResponseSet();
            try
            {

                var SupervisorId = expandoDict["SupervisorId"];
                var SiteId = expandoDict["SiteId"];
                var Remark = expandoDict["Remark"];
                var IpAddress = expandoDict["IpAddress"];

                var param = new
                {
                    SupervisorId = SupervisorId,
                    SiteId = SiteId,
                    Remark= Remark,
                    IpAddress= IpAddress

                };
                innerresult = dAL.QueryWithExecuteAsync("App_SupervisorInsertRemarkIssue", param);
               

                result.Message = innerresult.Message;
                result.Status = innerresult.Status;
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                result.Message = "";
                result.Status = "False";
            }
            return result;
        }


        [AllowAnonymous]
        [HttpPost]
        [ActionName("App_SupervisorConvertIssue")]
        public async Task<object> App_SupervisorConvertIssue(ExpandoObject expando)
        {
            var innerresult = (dynamic)null;
            var expandoDict = expando as IDictionary<string, object>;
            ResponseSet result = new ResponseSet();
            try
            {

                var SupervisorId = expandoDict["SupervisorId"];
                var SiteId = expandoDict["SiteId"];
                var Remark = expandoDict["Remark"];
                var IpAddress = expandoDict["IpAddress"];

                var param = new
                {
                    SupervisorId = SupervisorId,
                    SiteId = SiteId,
                    Remark = Remark,
                    IpAddress = IpAddress

                };
                innerresult = dAL.QueryWithExecuteAsync("App_SupervisorConvertIssue", param);


                result.Message = innerresult.Message;
                result.Status = innerresult.Status;
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                result.Message = "";
                result.Status = "False";
            }
            return result;
        }

        public class ResponseSet
        {

            public string Message { get; set; }
            public string Status { get; set; }

        }
        public class ResponseSet1
        {

            public string Message { get; set; }
            public string Status { get; set; }

            public string WhetherSiteFormFilled { get; set; }
        }

        [HttpGet("test")]
        public IActionResult test()
        {
            return Ok();
        }
    }
}
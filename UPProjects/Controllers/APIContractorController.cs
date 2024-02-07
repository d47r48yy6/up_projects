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
using System.Globalization;

namespace UPProjects.Controllers
{
    [Authorize(AuthenticationSchemes = AuthSchemes)]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class APIContractorController : ControllerBase
    {

        private const string AuthSchemes =
       CookieAuthenticationDefaults.AuthenticationScheme + "," +
       JwtBearerDefaults.AuthenticationScheme;
        private readonly DAL dAL;
        private readonly AppCommonMethod acm;
        private readonly IWebHostEnvironment _env;
        public APIContractorController(DAL _dAL, AppCommonMethod _acm, IWebHostEnvironment env)
        {
            dAL = _dAL;
            acm = _acm;
            _env = env;

        }

        //////////////////////////////////////////Contractor Login Check/////////////////////////////////////
        [AllowAnonymous]
        [HttpPost]
        public async Task<object> App_CheckContractor(ExpandoObject expando)
        {
            ContractorDetails data = new ContractorDetails();

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    MobileNumber = expandoDict["MobileNumber"],
                    Pass = expandoDict["Password"]
                };
                data = dAL.SelectModel<ContractorDetails>("App_CheckContractorLogin", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }

        //////////////////////////////////////////Contractor Dashboard/////////////////////////////////////
        [AllowAnonymous]
        [HttpPost]
        public async Task<object> App_GetDashboardContractor(ExpandoObject expando)
        {
            GetContractorDashboardCount data = new GetContractorDashboardCount();

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var Id = expandoDict["ContractorId"];
                var Fromdate = expandoDict["FromDate"].ToString();
                var Todate = expandoDict["ToDate"].ToString();

                var param = new
                {
                    Id = Id,
                    Fromdate =Fromdate==""?null: GetDate(Fromdate),
                    Todate = Todate == "" ? null : GetDate(Todate),
                };
                data = dAL.SelectModel<GetContractorDashboardCount>("App_GetDashboardCountContractor", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }


        public string GetDate(string v)
        {
            var s = "";
            var val = v.Split('/');
            s = val[2] + '-' + val[1] + '-' + val[0];
            return s;
        }



        //////////////////////////////////////////District List By Contractor Id/////////////////////////////////////
        [AllowAnonymous]
        [HttpPost]
        public async Task<object> App_GetDistrictListByContractorId(ExpandoObject expando)
        {
            List<ResultSet> data = new List<ResultSet>();

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    Id = expandoDict["Id"]
                   
                };
                data = dAL.SelectModelList<ResultSet>("App_GetDistrictListByContractorLogin", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }

        ////////////////////////////////////////////////Insert Supervisor Master Data///////////////////////////////
         [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_InsertSupervisorDetails(SuperVisorDetails expando)
        {
            var innerresult = (dynamic)null;
            ResponseSet result = new ResponseSet();


            try
            {

                var expandoDict = expando as IDictionary<string, object>;
                DataTable dt = new DataTable();
                dt.Columns.Add("DistrictId");
                for (int i = 0; i < expando.SuperVisorDistrict.Count; i++)
                {
                    dt.Rows.Add(expando.SuperVisorDistrict.ElementAt(i).DistrictId);
                }
                var param = new
                {
                   
                    ZoneId = expando.ZoneId,
                    UnitId = expando.UnitId,
                    ContractorId = expando.ContractorId,
                    Supervisorname = expando.SupervisorName,
                    MobileNumber = expando.MobileNumber,
                    UserId = expando.UserId,
                    IpAddress =expando.IPAddress,
                    Reference = dt

                };
                innerresult = dAL.QueryWithExecuteAsync("App_InsertSupervisor", param);


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

        /////////////////////////////////////////////////////////Delete Supervisor By Supervisor Id///////

        [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_DeleteSupervisorMasterById(ExpandoObject expando)
        {
            var innerresult = (dynamic)null;
            ResponseSet result = new ResponseSet();


            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    Id = expandoDict["Id"],
                    UserId = expandoDict["UserId"],
                    IpAddress = expandoDict["IpAddress"],


                };
                innerresult = dAL.QueryWithExecuteAsync("App_DeleteSupervisorMasterById", param);


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



        ///////////////////////////////////////////////////////Get Supervisor List By Contractor Id////////
        [AllowAnonymous]
        [HttpPost]
        public async Task<object> App_GetSupervisorListByContractorId(ExpandoObject expando)
        {
            List<SuperVisorDetails> data = new List<SuperVisorDetails>();

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    Id = expandoDict["Id"]
                };
                data = dAL.SelectModelList<SuperVisorDetails>("App_GetSupervisorListByContractorId", param);
             
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }


        /////////////////////////////////////////////////////////Block/Unblock Supervisor By Supervisor Id///////

        [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_UpdateSupervisorMasterStatus(ExpandoObject expando)
        {
            var innerresult = (dynamic)null;
            ResponseSet result = new ResponseSet();


            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    Id = expandoDict["SupervisorId"],
                    Status= expandoDict["Status"],
                    UserId = expandoDict["UserId"],
                    IpAddress = expandoDict["IpAddress"],


                };
                innerresult = dAL.QueryWithExecuteAsync("[App_UpdateStatusSupervisorMaster]", param);


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


        ///////////////////////////////////////////////////////Get Contractor Dashboard Details By Filters////////
        [AllowAnonymous]
        [HttpPost]
        public async Task<object> App_ContractorDashboardDetailsByFilters(ExpandoObject expando)
        {
            var data = (dynamic)null;

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {

                    DistrictId = expandoDict["DistrictId"].ToString() == "" ? null : expandoDict["DistrictId"].ToString(),
                    TehsilId = expandoDict["TehsilId"].ToString() == "" ? null : expandoDict["TehsilId"].ToString(),
                    Block = expandoDict["Block"].ToString() == "" ? null : expandoDict["Block"].ToString(),
                    GramsabhaId = expandoDict["GramsabhaId"].ToString() == "" ? null : expandoDict["GramsabhaId"].ToString(),
                    Sitetype = expandoDict["Sitetype"].ToString() == "" ? null : expandoDict["Sitetype"].ToString(),
                    SitetypeStatus = expandoDict["SitetypeStatus"].ToString() == "" ? null : expandoDict["SitetypeStatus"].ToString()
                };
                data = dAL.Query("App_SiteListContractorByFilters", CommandType.StoredProcedure, param);

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }

        ///////////////////////////////////////////////////////Get Contractor Dashboard Details By Filters,ProgressType////////
        [AllowAnonymous]
        [HttpPost]
        public async Task<object> App_ContractorDashboardDetailsByFiltersByProgressType(ExpandoObject expando)
        {
            var data = (dynamic)null;

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {

                    DistrictId = expandoDict["DistrictId"].ToString() == "" ? null : expandoDict["DistrictId"].ToString(),
                    TehsilId = expandoDict["TehsilId"].ToString() == "" ? null : expandoDict["TehsilId"].ToString(),
                    Block = expandoDict["Block"].ToString() == "" ? null : expandoDict["Block"].ToString(),
                    GramsabhaId = expandoDict["GramsabhaId"].ToString() == "" ? null : expandoDict["GramsabhaId"].ToString(),
                    Sitetype = expandoDict["Sitetype"].ToString() == "" ? null : expandoDict["Sitetype"].ToString(),
                    ProgressType = expandoDict["ProgressType"].ToString() == "" ? null : expandoDict["ProgressType"].ToString()
                };
               
                data = dAL.Query("App_SiteListContractorByFiltersbyStatus", CommandType.StoredProcedure, param);

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }


        //////////////////////////////////////////Get Supervisor Details by Supervisor Id on Contractor Panel/////////////////////////////////////
        [AllowAnonymous]
        [HttpPost]
        public async Task<object> App_GetSupervisordetailsbyIdContractor(ExpandoObject expando)
        {
            GetContractorDashboardCount data = new GetContractorDashboardCount();

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var Fromdate = expandoDict["FromDate"].ToString();
                var Todate = expandoDict["ToDate"].ToString();
                var param = new
                {
                    SupervisorId = expandoDict["SupervisorId"],
                    Fromdate = Fromdate == "" ? null : GetDate(Fromdate),
                    Todate = Todate == "" ? null : GetDate(Todate),

                };
                data = dAL.SelectModel<GetContractorDashboardCount>("App_GetSupervisordetailsbyIdContractor", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }





        ///////////////////////////////////////////////////////Get Supevisor Dashboard Details By Filters////////
        [AllowAnonymous]
        [HttpPost]
        public async Task<object> App_SupervisorDashboardDetailsByFilters(ExpandoObject expando)
        {
            var data = (dynamic)null;

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    SupervisorId = expandoDict["SupervisorId"].ToString() == "" ? null : expandoDict["SupervisorId"].ToString(),
                    DistrictId = expandoDict["DistrictId"].ToString() == "" ? null : expandoDict["DistrictId"].ToString(),
                    TehsilId = expandoDict["TehsilId"].ToString() == "" ? null : expandoDict["TehsilId"].ToString(),
                    Block = expandoDict["Block"].ToString() == "" ? null : expandoDict["Block"].ToString(),
                    GramsabhaId = expandoDict["GramsabhaId"].ToString() == "" ? null : expandoDict["GramsabhaId"].ToString(),
                    Sitetype = expandoDict["Sitetype"].ToString() == "" ? null : expandoDict["Sitetype"].ToString(),
                    Sitetypestatus = expandoDict["Sitetypestatus"].ToString() == "" ? null : expandoDict["Sitetypestatus"].ToString()
                };
                
                data = dAL.Query("App_SiteListContractorByFiltersBySupervisorId", CommandType.StoredProcedure, param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }

        ///////////////////////////////////////////////////////Get Contractor Dashboard Details By Filters,ProgressType////////
        [AllowAnonymous]
        [HttpPost]
        public async Task<object> App_SupervisorDashboardDetailsByFiltersByProgressType(ExpandoObject expando)
        {
            var data = (dynamic)null;

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    SupervisorId = expandoDict["SupervisorId"].ToString() == "" ? null : expandoDict["SupervisorId"].ToString(),
                    DistrictId = expandoDict["DistrictId"].ToString() == "" ? null : expandoDict["DistrictId"].ToString(),
                    TehsilId = expandoDict["TehsilId"].ToString() == "" ? null : expandoDict["TehsilId"].ToString(),
                    Block = expandoDict["Block"].ToString() == "" ? null : expandoDict["Block"].ToString(),
                    GramsabhaId = expandoDict["GramsabhaId"].ToString() == "" ? null : expandoDict["GramsabhaId"].ToString(),
                    Sitetype = expandoDict["Sitetype"].ToString() == "" ? null : expandoDict["Sitetype"].ToString(),
                    ProgressType = expandoDict["ProgressType"].ToString() == "" ? null : expandoDict["ProgressType"].ToString()
                };
                data = dAL.Query("App_SiteListContractorByFiltersbyStatusBySupervisorId", CommandType.StoredProcedure, param);

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }


        //////////////////////////////////////////District List By Contractor Id/////////////////////////////////////
        [AllowAnonymous]
        [HttpPost]
        public async Task<object> App_GetDistrictListBySupervisorId(ExpandoObject expando)
        {
            List<ResultSet> data = new List<ResultSet>();

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    Id = expandoDict["SupervisorId"]

                };
                data = dAL.SelectModelList<ResultSet>("App_GetDistrictListBySupervisorId", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }


        ///////////////////////////////////Site Details with site Id////////////////////////////////////////////////
        [HttpPost]
        [AllowAnonymous]
        public async Task<object> App_GetSupervisorSiteDetails(ExpandoObject expando)
        {
            var Result = (dynamic)null;

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    SupervisorId = expandoDict["SupervisorId"].ToString()=="0"?null: expandoDict["SupervisorId"].ToString(),
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

        //////////////////////////////////////////Supervisor Districts/////////////////////////////////////

        [HttpPost]
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
        //////////////////////////////////////////Contractor  Districts and Count of Sites/////////////////////////////////////

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> App_GetContractorDistrictsAndCount(ExpandoObject expando)
        {
            var Result = (dynamic)null;

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    ContractorId = expandoDict["ContractorId"],
                };
                Result = dAL.Query("App_GetContractorDistrictsAndCount", CommandType.StoredProcedure, param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Result;
        }

        //////////////////////////////////////////Supervisor DistrictsCountByIdByProgressType/////////////////////////////////////

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> App_GetSupervisorDistrictsByIdAndProgressType(ExpandoObject expando)
        {
            var Result = (dynamic)null;

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    SupervisorId = expandoDict["SupervisorId"],
                    ProgressType = expandoDict["ProgressType"]
                };
                Result = dAL.Query("App_GetSupervisorDistrictsByIdAndProgressType", CommandType.StoredProcedure, param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Result;
        }

        //////////////////////////////////////////Contractor DistrictsCountByIdByProgressType/////////////////////////////////////

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> App_GetContractorDistrictsByIdAndProgressType(ExpandoObject expando)
        {
            var Result = (dynamic)null;

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    ContractorId = expandoDict["ContractorId"],
                    ProgressType = expandoDict["ProgressType"]
                };
                Result = dAL.Query("App_GetContractorDistrictsByIdAndProgressType", CommandType.StoredProcedure, param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Result;
        }

        //////////////////////////////////////////Contractor DistrictsCountByIdByProgressType/////////////////////////////////////

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> App_GetSiteProgressDetailsByIdAndBySuperviaorId(ExpandoObject expando)
        {
            var Result = (dynamic)null;

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    SupervisorId = expandoDict["SupervisorId"],
                    SiteId = expandoDict["SiteId"]
                };
                Result = dAL.Query("App_GetSiteProgressDetailsByIdAndBySuperviaorId", CommandType.StoredProcedure, param);
                Result = Result[0];


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Result;
        }



        //////////////////////////////////////////Get Pending Payment Aprroval List/////////////////////////////////////

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> App_GetPaymentApprovalSiteList(ExpandoObject expando)
        {
            var Result = (dynamic)null;

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    PmId = expandoDict["PmId"],
                    
                };
                Result = dAL.Query("App_GetPendingSitePaymentByPmId", CommandType.StoredProcedure, param);
           


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Result;
        }

        [AllowAnonymous]
        [HttpPost]
    
        public async Task<object> App_ContractorInsertRemarkIssue(ExpandoObject expando)
        {
            var innerresult = (dynamic)null;
            var expandoDict = expando as IDictionary<string, object>;
            ResponseSet result = new ResponseSet();
            try
            {

                var SupervisorId = expandoDict["ContractorId"];
                var SiteId = expandoDict["SiteId"];
                var Remark = expandoDict["Remark"];
                var IpAddress = expandoDict["IpAddress"];

                var param = new
                {
                    ContractorId = SupervisorId,
                    SiteId = SiteId,
                    Remark = Remark,
                    IpAddress = IpAddress

                };
                innerresult = dAL.QueryWithExecuteAsync("App_ContractorInsertRemarkIssue", param);


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


        //////////////////////////////Contractor Convert Issue into Handover
        [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_ContractorConvertIssue(ExpandoObject expando)
        {
            var innerresult = (dynamic)null;
            var expandoDict = expando as IDictionary<string, object>;
            ResponseSet result = new ResponseSet();
            try
            {

                var SupervisorId = expandoDict["ContractorId"];
                var SiteId = expandoDict["SiteId"];
                var Remark = expandoDict["Remark"];
                var IpAddress = expandoDict["IpAddress"];

                var param = new
                {
                    ContractorId = SupervisorId,
                    SiteId = SiteId,
                    Remark = Remark,
                    IpAddress = IpAddress

                };
                innerresult = dAL.QueryWithExecuteAsync("App_ContractorConvertRemarkIssue", param);


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

        public async Task<object> App_PmArrovedPayment(ExpandoObject expando)
        {
            var innerresult = (dynamic)null;
            var expandoDict = expando as IDictionary<string, object>;
            ResponseSet result = new ResponseSet();
            try
            {

                var PmId = expandoDict["PmId"];
                var SiteId = expandoDict["SiteId"];
                var SupervisorId = expandoDict["SupervisorId"];
                var PaymentStatus = expandoDict["PaymentStatus"];
                var IpAddress = expandoDict["IpAddress"];

                var param = new
                {
                    PmId = PmId,
                    SiteId = SiteId,
                    SupervisorId= SupervisorId,
                    PaymentStatus= PaymentStatus,
                    IpAddress = IpAddress

                };
                innerresult = dAL.QueryWithExecuteAsync("App_PmArrovedPayment", param);


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



        ///////////////////////////////Contractor Change Password///////////////////////////////////////////

        [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_ContratorChangePassword(ExpandoObject expando)
        {
            var innerresult = (dynamic)null;
            var expandoDict = expando as IDictionary<string, object>;
            ResponseSet result = new ResponseSet();
            try
            {

                var ContractorId = expandoDict["ContractorId"];
                var OldPassword = expandoDict["OldPassword"];
                var NewPassword = expandoDict["NewPassword"];
                

                var param = new
                {
                    ContractorId = ContractorId,
                    OldPassword = OldPassword,
                    NewPassword = NewPassword

                };
                innerresult = dAL.QueryWithExecuteAsync("App_ContractorChangePassword", param);


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



        //////////////////////////////////Convert non feasible site into feasible site///////////////

        [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_SupervisorConvertnonfeasiblesite(ExpandoObject expando)
        {
            var innerresult = (dynamic)null;
            var expandoDict = expando as IDictionary<string, object>;
            ResponseSet result = new ResponseSet();
            try
            {

                var SupervisorId = expandoDict["SupervisorId"];
                var SiteId = expandoDict["SiteId"];
                var IpAddress = expandoDict["IpAddress"];

                var param = new
                {
                    @SupervisorId = SupervisorId,
                    SiteId = SiteId,
                    IpAddress = IpAddress

                };
                innerresult = dAL.QueryWithExecuteAsync("App_SupervisorConvertNonFeasibleSite", param);


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

        public class ContractorDetails
        {
            public string ContractorId { get; set; }
            public string ApplicantName { get; set; }
            public string FirmName { get; set; }
            public string Email { get; set; }
            public string Mobile { get; set; }
            public string Status { get; set; }
            public string Message { get; set; }

            public string ZoneId { get; set; }
            public string UnitId { get; set; }
        }

        public class SuperVisorDetails
        {
            public string SupervisorId { get; set; }
            public string ZoneId { get; set; }
            public string UnitId { get; set; }
            public string ContractorId { get; set; }
            public string SupervisorName { get; set; }
            public string MobileNumber { get; set; }
            public string Status { get; set; }
            public string UserId { get; set; }
            public string IPAddress { get; set; }
            public string ZoneName { get; set; }
            public string UnitName { get; set; }

            public List<SuperVisorDistrict> SuperVisorDistrict = new List<SuperVisorDistrict>();



        }

        public class SuperVisorDistrict
        {
            public string DistrictId { get; set; }
            public string DistrictName { get; set; }
        }

        public class ResultSet
        {

            public string Id { get; set; }
            public string Value { get; set; }

        }

        public class ResponseSet
        {

            public string Message { get; set; }
            public string Status { get; set; }

        }


        public class GetContractorDashboardCount
        {
            public string TotalDistrict { get; set; }
            public string TotalFeasibleSite { get; set; }

            public string WorkStarted { get; set; }

            public string WorkInProgress { get; set; }

            public string WorkCompleted { get; set; }

            public string WorkHandover { get; set; }
            public string SupervisorName { get; set; }
            public string MobileNumber { get; set; }
            public string SupervisorStatus { get; set; }
            public string SupervisorId { get; set; }
        }

    
        public class ContractorDashboardCompleteDetails
        {
            public string SurveyId { get; set; }
            public string Image1 { get; set; }

            public string Image2 { get; set; }

            public string SiteType { get; set; }

            public string Latitude { get; set; }

            public string Longitude { get; set; }

            public string FullAddress { get; set; }

            public string CurrentStatus { get; set; }
            public string CurrentImage { get; set; }

            public string CurrentLatitude { get; set; }
            public string CurrentLongitude { get; set; }
            public string CurrentLocation { get; set; }
            public string DistrictName { get; set; }
            public string TehsilName { get; set; }
            public string BlockName { get; set; }
            public string GramsabhaName { get; set; }

         

        }

 


    }
}
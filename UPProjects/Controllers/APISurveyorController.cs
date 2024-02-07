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
    public class APISurveyorController : ControllerBase
    {
        private const string AuthSchemes =
       CookieAuthenticationDefaults.AuthenticationScheme + "," +
       JwtBearerDefaults.AuthenticationScheme;
        private readonly DAL dAL;
        private readonly AppCommonMethod acm;
        private readonly IWebHostEnvironment _env;
        public APISurveyorController(DAL _dAL, AppCommonMethod _acm, IWebHostEnvironment env)
        {
            dAL = _dAL;
            acm = _acm;
            _env = env;

        }

        [AllowAnonymous]    
        [HttpGet]

        public async Task<object> App_GetZone()
        {
            List<ResultSet> data = new List<ResultSet>();

            try
            {
                
                
                data = dAL.SelectModelList<ResultSet>("GetZone", null);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }

        [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_GetUnit(ExpandoObject expando)
        {
            List<ResultSet> data = new List<ResultSet>();

            try
            {

                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    ZoneId = expandoDict["Id"]
                };
                data = dAL.SelectModelList<ResultSet>("GetUnit", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }

        /////////////////////////////////////////////////////////Get Urban List By District Id/////////////////////////////////////

        [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_GetUrbanListByDistrictId(ExpandoObject expando)
        {
            List<ResultSet> data = new List<ResultSet>();

            try
            {

                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    DistrictId = expandoDict["DistrictId"]
                };
                data = dAL.SelectModelList<ResultSet>("GetUrbanMasterListByDistrictId", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }


        [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_InsertPMDetail(ExpandoObject expando)
        {
            var innerresult = (dynamic)null;
            ResponseSet result = new ResponseSet();


            try
            {


                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    ZoneId = expandoDict["ZoneId"],
                    UnitIt = expandoDict["UnitId"],
                    PMname = expandoDict["Name"],
                    MobileNumber= expandoDict["MobileNumber"],
                    IPAddress = expandoDict["IPAddress"],
                };

                innerresult = dAL.QueryWithExecuteAsync("App_InsertPMDetails", param);
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

        public async Task<object> App_CheckPM(ExpandoObject expando)
        {
            PMDetail data = new PMDetail();

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    MobileNumber = expandoDict["MobileNumber"]
                };
                data = dAL.SelectModel<PMDetail>("App_CheckPMCredential", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }

        [AllowAnonymous]
        [HttpGet]

        public async Task<object> App_GetDistrictList()
        {
            List<ResultSet> data = new List<ResultSet>();

            try
            {
               
                data = dAL.SelectModelList<ResultSet>("GetDistrictDropDownList", null);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }


        [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_InsertSurveyorDetails(ExpandoObject expando)
        {
            var innerresult = (dynamic)null;
            SurveyorInsert result = new SurveyorInsert();


            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    ZoneId = expandoDict["ZoneId"],
                    UnitId = expandoDict["UnitId"],
                    PmId = expandoDict["PmId"],
                    DistictId= expandoDict["DistictId"],
                    SurveyorName = expandoDict["SurveyorName"],
                    MobileNumber=expandoDict["MobileNumber"],
                    PmName = "",
                    IpAddress = expandoDict["IpAddress"],


                };
                innerresult = dAL.QueryWithExecuteAsync("InsertSurveyorDetail", param);
               

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
        public async Task<object> App_GetSurveyorListByPMId(ExpandoObject expando)
        {
            List<SurveyorListByPMId> data = new List<SurveyorListByPMId>();

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    Id = expandoDict["Id"]
                };
                data = dAL.SelectModelList<SurveyorListByPMId>("App_GetSurveyorListByPmId", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }




        [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_CheckSurveyor(ExpandoObject expando)
        {
            SurveyorDetail data = new SurveyorDetail();
           
            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    MobileNumber = expandoDict["MobileNumber"]
                };
                data = dAL.SelectModel<SurveyorDetail>("App_CheckSuveryorMobileNumber", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }

        [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_GetTehsilList(ExpandoObject expando)
        {
            List<ResultSet> data = new List<ResultSet>();

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    District = expandoDict["DistrictId"]
                };
                data = dAL.SelectModelList<ResultSet>("App_GetTehsilByDistrict", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }

        [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_GetBlockList(ExpandoObject expando)
        {
            List<ResultSet> data = new List<ResultSet>();

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    District = expandoDict["DistrictId"],
                    Tehsil = expandoDict["TehsilId"]
                };
                data = dAL.SelectModelList<ResultSet>("App_GetBlockList", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }

        [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_GetGramSabhaList(ExpandoObject expando)
        {
            List<ResultSet> data = new List<ResultSet>();
            ResultSet d = new ResultSet();
            d.Id = "0";
            d.Value = "Others";

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    Block = expandoDict["Block"],
                    Tehsil = expandoDict["TehsilId"]
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





       [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_InsertSurveyDetail(SurveyDetail expando)
        {
            var innerresult = (dynamic)null;
            ResponseSet result = new ResponseSet();
            try
            {

                var SurveyorId = expando.Id;
                var DistrictId = expando.DistrictId;
                var TehsilId = expando.TehsilId;
                var BlockId = expando.BlockId;
                var GramSabhaId = expando.GramSabhaId;
                var SiteType = expando.SiteType;
                var FileName1 = expando.Image.ElementAt(0).imag;
                var FileName2 = expando.Image.ElementAt(1).imag;
                var Latitude = expando.Latitude;
                var Longitude = expando.Longitude;
                var ElectricityStatus = expando.ElectricityStatus;
                var HandPupWaterStatus = expando.HandPupWaterStatus;

                var FullAddress = expando.FullAddress;
                var IPAddress = expando.IPAddress;
                var Whethertapwater = expando.Whethertapwater;
                var Whetherredmark = expando.Whetherredmark;


                var File1 = Guid.NewGuid();
                var File2 = Guid.NewGuid();

                var param = new
                {
                    SurveyorId = SurveyorId,
                    DistrictId = DistrictId,
                    TehsilId = TehsilId==""?null:TehsilId,
                    BlockId = BlockId == "" ? null : BlockId,
                    GramSabhaId = GramSabhaId==""?null:GramSabhaId,
                    SiteType = SiteType,
                    Image1 = File1 + ".jpg",
                    Image2 = File2 + ".jpg",
                    Latitude = Latitude,
                    Longitude = Longitude,
                    ElectricityStatus = ElectricityStatus,
                    HandPupWaterStatus = HandPupWaterStatus,
                    Whethertapwater= expando.Whethertapwater,
                    Whetherredmark = expando.Whetherredmark,
                    FullAddress = FullAddress,
                    IPAddress = IPAddress,
                   
            };
                innerresult = dAL.QueryWithExecuteAsync("App_InsertSurveyDetail", param);
                if (Convert.ToInt32(innerresult.ResultCode) > 0)
                {

                    if (FileName1 != null)
                    {
                        string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/Surveyor");
                     
                        var Ids = Convert.ToString(innerresult.ResultCode);

                        SaveBase64ImagesMultiple(Ids, File1.ToString(), FileName1.ToString());

                    }
                    if (FileName2 != null)
                    {
                        string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/Surveyor");

                        var Ids = Convert.ToString(innerresult.ResultCode);

                        SaveBase64ImagesMultiple(Ids, File2.ToString(), FileName2.ToString());

                    }

                }

                result.Message = innerresult.Message;
                result.Status = innerresult.Status;

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                result.Message ="";
                result.Status ="False";
            }
            return result;
        }

        public string SaveBase64ImagesMultiple(string id, string rand, string images)
        {
            try
            {
                var folderPath = Path.Combine(_env.WebRootPath, "Upload/Surveyor/" + id);
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



        [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_GetSurveryList(ExpandoObject expando)
        {
            List<SurveryList> data = new List<SurveryList>();

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    Id = expandoDict["Id"],
                   
                };
                data = dAL.SelectModelList<SurveryList>("App_GetSurveryList", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }


        [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_GetSurveryListBySuveryId(ExpandoObject expando)
        {
            List<SurveryList> data = new List<SurveryList>();

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    Id = expandoDict["Id"],

                };
                data = dAL.SelectModelList<SurveryList>("App_GetSurveryListBySuveyorId", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }

        //////////////////////////////////////////////////////Insert New Gram Sabha/////////////////////////////////////////////////
         [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_InsertNewGramSabha(ExpandoObject expando)
        {
            var innerresult = (dynamic)null;
            NewGramSabha result = new NewGramSabha();


            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    DistrictId = expandoDict["DistrictId"],
                    BlockCode = expandoDict["BlockCode"],
                    TehsilId = expandoDict["TehsilId"],
                    GramSabhaName = expandoDict["GramSabhaName"]

                };
                innerresult = dAL.QueryWithExecuteAsync("InsertNewGramSabha", param);


                result.Value = innerresult.Value;
                result.Status = innerresult.Status;


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                result.Value = "";
                result.Status = "False";
            }
            return result;
        }


        //////////////////////////////////////////////////////////HQ Login//////////////////////////////////////////////////////////////
        [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_CheckHQGMLogin(ExpandoObject expando)
        {
            HQLogin data = new HQLogin();

            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    UserId = expandoDict["UserId"],
                    pass = expandoDict["Password"]
                };
                data = dAL.SelectModel<HQLogin>("Get_AppHQGMLogin", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }

        ////////////////////////////////////////////////////////////Survey Details by Filters for App////////////////////////////////

        //[AllowAnonymous]
        //[HttpPost]

        //public async Task<object> App_GetSurveryDetailsById(ExpandoObject expando)
        //{
        //    List<ResultSet> data = new List<ResultSet>();

        //    try
        //    {

        //        var expandoDict = expando as IDictionary<string, object>;
        //        var param = new
        //        {
        //            ZoneId = expandoDict["Id"]
        //        };
        //        data = dAL.SelectModelList<ResultSet>("GetUnit", param);


        //    }
        //    catch (Exception ex)
        //    {
        //        await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
        //    }
        //    return data;
        //}




        ////////////////////////////////////////////////////////////Delete Surveyor Master By Id///////////////////////////////////////////

        [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_DeleteSurveyorMasterById(ExpandoObject expando)
        {
            var innerresult = (dynamic)null;
            ResponseSet result = new ResponseSet();


            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                      Id = expandoDict["Id"],
                      UserId= expandoDict["UserId"],
                    IpAddress = expandoDict["IpAddress"],


                };
                innerresult = dAL.QueryWithExecuteAsync("App_DeleteSurveyorMasterById", param);


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


        ////////////////////////////////////////////////////////////Delete SurveyDetails By Id///////////////////////////////////////////

        [AllowAnonymous]
        [HttpPost]
        /////////////////////////////////////////////////////////////////////////////////////////////
        
        public async Task<object> App_DeleteSurveyDetailsById(ExpandoObject expando)
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
                innerresult = dAL.QueryWithExecuteAsync("App_DeleteSurveyDetailsById", param);


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


        ////////////////////////////////////////////////////////////Get Surveyor Dropdown List by Id///////////////////////////////////
         [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_GetSurveyorDropDownListByPmId(ExpandoObject expando)
        {
            List<ResultSet> data = new List<ResultSet>();

            try
            {

                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    Id = expandoDict["Id"]
                };
                data = dAL.SelectModelList<ResultSet>("GetSurveyorDropDownListByPmId", param);


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return data;
        }


        ////////////////////////////////////////////////////////////////////Transfer Suveyor District/////////////////
        [AllowAnonymous]
        [HttpPost]

        public async Task<object> App_TransferSurveyorDistrict(ExpandoObject expando)
        {
           
            ResponseSet result = new ResponseSet();


            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    DistrictId = expandoDict["DistrictId"],
                    Id = expandoDict["Id"],
                   

                };
                result = dAL.SelectModel<ResponseSet>("App_TransferSurveyorDistrict", param);


          

            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                result.Message = "";
                result.Status = "False";
            }
            return result;
        }


        /////////////////////////////////////////////////////////Get PM DashBoard//////////////////////////////////////////
        [AllowAnonymous]
        [HttpPost]
        public async Task<object> App_GetDashBoardPM(ExpandoObject expando)
        {
            PMDashBoardCountWholeDetails result = new PMDashBoardCountWholeDetails();
            DashBoardCount school = new DashBoardCount();
            DashBoardCount anganwadi = new DashBoardCount();
            DashBoardCount aashram = new DashBoardCount();
            DashBoardCount totalsurvey = new DashBoardCount();
            var t = "";
            var expandoDict = expando as IDictionary<string, object>;
            try
            {
                ///////////////////////////////////////For School Counting//////////////////////////////
                var schoolparam = new
                {
                    
                    Id = expandoDict["Id"],
                    Type="School"
                };
                school = dAL.SelectModel<DashBoardCount>("App_GetDashboardCount", schoolparam);


                ///////////////////////////////////////For Anganwadi Counting//////////////////////////////
                var anganwadiparam = new
                {

                    Id = expandoDict["Id"],
                    Type = "Anganwadi"
                };
                anganwadi = dAL.SelectModel<DashBoardCount>("App_GetDashboardCount", anganwadiparam);


                ///////////////////////////////////////For Aashram Counting//////////////////////////////
                var aashramparam = new
                {

                    Id = expandoDict["Id"],
                    Type = "Aashram"
                };
                aashram = dAL.SelectModel<DashBoardCount>("App_GetDashboardCount", aashramparam);


                ///////////////////////////////////////For Total Counting//////////////////////////////
                var totalparam = new
                {

                    Id = expandoDict["Id"],
                    Type = t == "" ? null : t
                };
                totalsurvey = dAL.SelectModel<DashBoardCount>("App_GetDashboardCount", totalparam);

                //////////////////////////////////////Group all/////////////////////////////////////////

                result.SchoolTotalVisited = school.TotalVisited;
                result.SchoolElectricityAvailibity = school.ElectricityAvailibity;
                result.SchoolHandPumpAvailibity = school.HandPumpAvailibity;
                result.SchoolWhetherTapWater = school.WhetherTapWater;
                result.SchoolWhetherRedMark = school.WhetherRedMark;
                result.SchoolFeasibility = school.Feasibility;
                result.SchoolNonFeasibiliy = school.NonFeasibiliy;



                result.AnganwadiTotalVisited = anganwadi.TotalVisited;
                result.AnganwadiElectricityAvailibity = anganwadi.ElectricityAvailibity;
                result.AnganwadiHandPumpAvailibity = anganwadi.HandPumpAvailibity;
                result.AnganwadiWhetherTapWater = anganwadi.WhetherTapWater;
                result.AnganwadiWhetherRedMark = anganwadi.WhetherRedMark;
                result.AnganwadiFeasibility = anganwadi.Feasibility;
                result.AnganwadiNonFeasibiliy = anganwadi.NonFeasibiliy;



                result.AashramTotalVisited = aashram.TotalVisited;
                result.AashramElectricityAvailibity = aashram.ElectricityAvailibity;
                result.AashramHandPumpAvailibity = aashram.HandPumpAvailibity;
                result.AashramWhetherTapWater = aashram.WhetherTapWater;
                result.AashramWhetherRedMark = aashram.WhetherRedMark;
                result.AashramFeasibility = aashram.Feasibility;
                result.AashramNonFeasibiliy = aashram.NonFeasibiliy;



                result.TotalVisited = totalsurvey.TotalVisited;
                result.ElectricityAvailibity = totalsurvey.ElectricityAvailibity;
                result.HandPumpAvailibity = totalsurvey.HandPumpAvailibity;
                result.WhetherTapWater = totalsurvey.WhetherTapWater;
                result.WhetherRedMark = totalsurvey.WhetherRedMark;
                result.Feasibility = totalsurvey.Feasibility;
                result.NonFeasibiliy = totalsurvey.NonFeasibiliy;


            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }

            return result;
        }



        [AllowAnonymous]
        [HttpPost]
        //////////////////////////////////////////////////////////////////////Survey Update By Id//////////////////////////

        public async Task<object> App_UpdateSurveyById(ExpandoObject expando)
        {
            var innerresult = (dynamic)null;
            ResponseSet result = new ResponseSet();


            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    Id = expandoDict["Id"],
                    SiteType = expandoDict["SiteType"],
                    ElectricityStatus = expandoDict["ElectricityStatus"],
                    HandPupWaterStatus = expandoDict["HandPupWaterStatus"],
                    Whethertapwater = expandoDict["Whethertapwater"],
                    Whetherredmark = expandoDict["Whetherredmark"],
                    UserId = expandoDict["UserId"],
                    IpAddress = expandoDict["IpAddress"],


                };
                innerresult = dAL.QueryWithExecuteAsync("App_UpdateSurveyDetailsById", param);


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
        /////////////////////////////////////////Update Surveyor Status By SurveyorId //////////////////////////

        public async Task<object> App_UpdateStatusSurveyMaster(ExpandoObject expando)
        {
            var innerresult = (dynamic)null;
            ResponseSet result = new ResponseSet();


            try
            {
                var expandoDict = expando as IDictionary<string, object>;
                var param = new
                {
                    Id = expandoDict["Id"],
                    Status = expandoDict["Status"],
                    UserId = expandoDict["UserId"],
                    IpAddress = expandoDict["IpAddress"],


                };
                innerresult = dAL.QueryWithExecuteAsync("App_UpdateStatusSurveyMaster", param);


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


        public class SurveyorListByPMId
        {
            public string ZoneName { get; set; }
            public string UnitName { get; set; }
            public string Districtname { get; set; }
            public string SurveyorName { get; set; }
            public string MobileNumber { get; set; }
            public string dataCount { get; set; }
            public string SurveyorId { get; set; }

            public string Status { get; set; }
        }
        public class PMDetail
        {

            public string ZoneId { get; set; }
            public string UnitId { get; set; }
            public string Status { get; set; }
            public string Name { get; set; }
            public string ZoneName { get; set; }
            public string UnitName { get; set; }
            public string PMId { get; set; }


        }

        public class SurveyorInsert
        {
            public string Status { get; set; }
            public string Message { get; set; }
            public string SurveyorId { get; set; }
        }

        public class SurveyorDetail
        {
          
            public string DistictId { get; set; }
            public string DistrictName { get; set; }
            public string Status { get; set; }
            public string Id { get; set; }

            public string Name { get; set;}

            public string EmailId { get; set; }
            public string Message { get; set; }
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

        public class SurveryList
        {
            public string SurveyorId { get; set; }
            public string SurveyDate { get; set; }
            public string SurveyorName { get; set; }
            public string SiteType { get; set; }
            public string Electrity { get; set; }
            public string Water { get; set; }
            public string Districtname { get; set; }
            public string TehsilName { get; set; }
            public string BlockName { get; set; }
            public string Gramsabha { get; set; }
            public string Longitude { get; set; }
            public string Latitude { get; set; }
            public string Image1 { get; set; }
            public string Image2 { get; set; }
            public string Whethertapwater { get; set; }

            public string Whetherredmark { get; set; }
            public string FullAddress { get; set; }

            public string CreatedOn { get; set; }
            public string SurveyStatus { get; set; }

        }

        public class NewGramSabha
        {

            public string Status { get; set; }
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

        }
        public class PMDashBoardCountWholeDetails
        {
            public string SchoolTotalVisited { get; set; }
            public string SchoolElectricityAvailibity { get; set; }
            public string SchoolHandPumpAvailibity { get; set; }
            public string SchoolWhetherTapWater { get; set; }
            public string SchoolWhetherRedMark { get; set; }
            public string SchoolFeasibility { get; set; }
            public string SchoolNonFeasibiliy { get; set; }



            public string AnganwadiTotalVisited { get; set; }
            public string AnganwadiElectricityAvailibity { get; set; }
            public string AnganwadiHandPumpAvailibity { get; set; }
            public string AnganwadiWhetherTapWater { get; set; }
            public string AnganwadiWhetherRedMark { get; set; }
            public string AnganwadiFeasibility { get; set; }
            public string AnganwadiNonFeasibiliy { get; set; }


            public string AashramTotalVisited { get; set; }
            public string AashramElectricityAvailibity { get; set; }
            public string AashramHandPumpAvailibity { get; set; }
            public string AashramWhetherTapWater { get; set; }
            public string AashramWhetherRedMark { get; set; }
            public string AashramFeasibility { get; set; }
            public string AashramNonFeasibiliy { get; set; }


            public string TotalVisited { get; set; }
            public string ElectricityAvailibity { get; set; }
            public string HandPumpAvailibity { get; set; }
            public string WhetherTapWater { get; set; }
            public string WhetherRedMark { get; set; }
            public string Feasibility { get; set; }
            public string NonFeasibiliy { get; set; }


        }
    }
}
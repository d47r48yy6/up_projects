using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UPProjects.Models;

namespace UPProjects.Controllers
{
    public class HNHomeController : Controller
    {

        private readonly DAL dAL;
        private readonly AppCommonMethod acm;
        private readonly IWebHostEnvironment env;
        public HNHomeController(DAL _dal, AppCommonMethod _acm, IWebHostEnvironment _env)
        {
            dAL = _dal;
            acm = _acm;
            env = _env;
        }
        public async Task<IActionResult> Index()
        {
            List<NewsMaster> data = new List<NewsMaster>();
            List<ServiceMaster> ourservices = new List<ServiceMaster>();
            List<SliderMaster> slider = new List<SliderMaster>();
            List<OurProjectMaster> ourproject = new List<OurProjectMaster>();
            List<MinisterMaster> minister = new List<MinisterMaster>();
            Projects_Count project = new Projects_Count();
            List<Projects_Count> project_count = new List<Projects_Count>();
            try
            {
                var param = new { Id = 0 };
                data = await dAL.SelectProcedureExecute<NewsMaster>("GetNewsDetails", param);
                ourservices = await dAL.SelectProcedureExecute<ServiceMaster>("GetServicesDetails", param);
                slider = await dAL.SelectProcedureExecute<SliderMaster>("GetSliderDetails", param);
                ourproject = await dAL.SelectProcedureExecute<OurProjectMaster>("GetOurProjectDetails", param);
                minister = await dAL.SelectProcedureExecute<MinisterMaster>("GetMinisterDetails", param);
                project_count = DB_Conn.SelectProcedureExecute<Projects_Count>("[GetProjectCount]", null);
                if (project_count != null)
                {
                    project = project_count.FirstOrDefault();
                    ViewBag.CompletedProjects = project.CompletedProjects;
                    ViewBag.TotalOngoingProjects = project.TotalOngoingProjects;
                    ViewBag.TotalProjects = project.TotalProjects;
                    ViewBag.NonHandoverProjects = project.NonHandoverProjects;
                    ViewBag.HandoverProjects = project.HandoverProjects;
                }
                ViewBag.services = ourservices;
                ViewBag.slider = slider;
                ViewBag.ourproject = ourproject;
                ViewBag.Minister = minister;
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(data);
        }

        public IActionResult Proforma()
        {
            List<Proforma> result = new List<Proforma>();
            try
            {
                var param = new { Id = 0 };
                result = DB_Conn.SelectProcedureExecute<Proforma>("GetProformaDetails", param);

            }

            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return View(result);
        }

       

        //public IActionResult organizationstructure()
        //{
        //    return View();
        //}
        public IActionResult managingdirectors()
        {
            return View();
        }
        public IActionResult chiefgenmanagers()
        {
            return View();
        }

        public IActionResult vision2020()
        {
            return View();
        }
        public IActionResult ProfileofCorporations()
        {
            return View();
        }
        public IActionResult ISOcertificate()
        {
            return View();
        }
        public IActionResult achievements()
        {
            return View();
        }
        public IActionResult chairman()
        {
            return View();
        }

        public IActionResult underconstruction()
        {
            return View();
        }

        public IActionResult mdmessage()
        {
            return View();
        }
       
        public IActionResult vacancy()
        {
            return View();
        }
        public IActionResult frmContractor()
        {

            if (TempData["Category"] == null)
            {
                return Redirect("/HNhome/index");
            }
            else if (TempData["SubCategory"] == null)
            {
                return Redirect("/HNhome/index");
            }
            else
            {
                ViewBag.Mobile = TempData["Mobile"].ToString();
                var Subcategory = TempData["SubCategory"];


                ViewBag.SubCategory = TempData["SubCategory"].ToString();
                ViewBag.Category = TempData["Category"].ToString();
                var param = new
                {
                    Subcategory = Subcategory,
                };

                var result = dAL.SelectModel<SubcategoryDetails>("GetSubcategoryDetails", param);
                if (result != null)
                {
                    ViewBag.RegFee = result.RegistrationFee;
                    ViewBag.fdr = result.SecurityFee;
                }
                else
                {
                    return Redirect("/HNhome/index");
                }

                ViewBag.Mobile = TempData["Mobile"].ToString();
                TempData.Keep();
                //if (TempData["Category"].ToString() == "A")
                //{
                //    ViewBag.RegFee = "11800";
                //    ViewBag.fdr = "500000";
                //}
                //if (TempData["Category"].ToString() == "B")
                //{
                //    ViewBag.RegFee = "9440";
                //    ViewBag.fdr = "200000";
                //}
                //if (TempData["Category"].ToString() == "C")
                //{
                //    ViewBag.RegFee = "8260";
                //    ViewBag.fdr = "100000";
                //}
                //if (TempData["Category"].ToString() == "D")
                //{
                //    ViewBag.RegFee = "5900";
                //    ViewBag.fdr = "50000";
                //}
            }
            return View();
        }

        [HttpPost]
        public IActionResult frmContractorNewSubmit(ContractorNewData contractor)
        {
            var Result = (dynamic)null;

            try
            {
                if (ModelState.IsValid)
                {
                    // string strFileExtension = Path.GetFileName(contractor.ToolsPlantsFile.FileName);
                    string ToolsPlantsFile = "", FourYearMainWorksFile = "", ListofStaffFile = "", BlackListFile = "", BalanceSheetFile = "", IncomeTaxReturnFile = "",
                       PANFile = "", GSTFile = "", HasiyatCertificate = "", BloodRelation = "", ApplicantPhoto = "", CharacterFile = "", PrivateExperience = "";
                    //if (contractor.ToolsPlantsFile != null)
                    //    ToolsPlantsFile = contractor.ToolsPlantsFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.ToolsPlantsFile.FileName.Split('.')[1].ToString();
                    //if (contractor.SevenYearMainWorksFile != null)
                    //    FourYearMainWorksFile = contractor.SevenYearMainWorksFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.SevenYearMainWorksFile.FileName.Split('.')[1].ToString();
                    //if (contractor.ListofStaffFile != null)
                    //    ListofStaffFile = contractor.ListofStaffFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.ListofStaffFile.FileName.Split('.')[1].ToString();
                    //if (contractor.BlackListFile != null)
                    //    BlackListFile = contractor.BlackListFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.BlackListFile.FileName.Split('.')[1].ToString();
                    //if (contractor.BalanceSheetFile != null)
                    //    BalanceSheetFile = contractor.BalanceSheetFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.BalanceSheetFile.FileName.Split('.')[1].ToString();
                    //if (contractor.IncomeTaxReturnFile != null)
                    //    IncomeTaxReturnFile = contractor.IncomeTaxReturnFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.IncomeTaxReturnFile.FileName.Split('.')[1].ToString();
                    //if (contractor.PANFile != null)
                    //    PANFile = contractor.PANFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.PANFile.FileName.Split('.')[1].ToString();
                    //if (contractor.GSTFile != null)
                    //    GSTFile = contractor.GSTFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.GSTFile.FileName.Split('.')[1].ToString();
                    //if (contractor.HasiyatCertificate != null)
                    //    HasiyatCertificate = contractor.HasiyatCertificate.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.HasiyatCertificate.FileName.Split('.')[1].ToString();
                    //if (contractor.BloodRelation != null)
                    //    BloodRelation = contractor.BloodRelation.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.BloodRelation.FileName.Split('.')[1].ToString();
                    //if (contractor.ApplicantPhoto != null)
                    //    ApplicantPhoto = contractor.ApplicantPhoto.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.ApplicantPhoto.FileName.Split('.')[1].ToString();

                    //if (contractor.CharacterFile != null)
                    //    CharacterFile = contractor.CharacterFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.CharacterFile.FileName.Split('.')[1].ToString();
                    //if (contractor.PrivateExperience != null)
                    //    PrivateExperience = contractor.PrivateExperience.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.PrivateExperience.FileName.Split('.')[1].ToString();














                    if (contractor.ToolsPlantsFile != null)
                        ToolsPlantsFile = Path.GetFileName(contractor.ToolsPlantsFile.FileName).Remove(contractor.ToolsPlantsFile.FileName.LastIndexOf(".")) + +DateTime.Now.Ticks + Path.GetExtension(contractor.ToolsPlantsFile.FileName);// contractor.ToolsPlantsFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.ToolsPlantsFile.FileName.Split('.')[1].ToString();
                    if (contractor.SevenYearMainWorksFile != null)
                        FourYearMainWorksFile = Path.GetFileName(contractor.SevenYearMainWorksFile.FileName).Remove(contractor.SevenYearMainWorksFile.FileName.LastIndexOf(".")) + +DateTime.Now.Ticks + Path.GetExtension(contractor.SevenYearMainWorksFile.FileName);
                    if (contractor.ListofStaffFile != null)
                        ListofStaffFile = Path.GetFileName(contractor.ListofStaffFile.FileName).Remove(contractor.ListofStaffFile.FileName.LastIndexOf(".")) + +DateTime.Now.Ticks + Path.GetExtension(contractor.ListofStaffFile.FileName);
                    if (contractor.BlackListFile != null)
                        BlackListFile = Path.GetFileName(contractor.BlackListFile.FileName).Remove(contractor.BlackListFile.FileName.LastIndexOf(".")) + +DateTime.Now.Ticks + Path.GetExtension(contractor.BlackListFile.FileName);
                    if (contractor.BalanceSheetFile != null)
                        BalanceSheetFile = Path.GetFileName(contractor.BalanceSheetFile.FileName).Remove(contractor.BalanceSheetFile.FileName.LastIndexOf(".")) + +DateTime.Now.Ticks + Path.GetExtension(contractor.BalanceSheetFile.FileName);
                    if (contractor.IncomeTaxReturnFile != null)
                        IncomeTaxReturnFile = Path.GetFileName(contractor.IncomeTaxReturnFile.FileName).Remove(contractor.IncomeTaxReturnFile.FileName.LastIndexOf(".")) + +DateTime.Now.Ticks + Path.GetExtension(contractor.IncomeTaxReturnFile.FileName);
                    if (contractor.PANFile != null)
                        PANFile = Path.GetFileName(contractor.PANFile.FileName).Remove(contractor.PANFile.FileName.LastIndexOf(".")) + +DateTime.Now.Ticks + Path.GetExtension(contractor.PANFile.FileName);
                    if (contractor.GSTFile != null)
                        GSTFile = Path.GetFileName(contractor.GSTFile.FileName).Remove(contractor.GSTFile.FileName.LastIndexOf(".")) + +DateTime.Now.Ticks + Path.GetExtension(contractor.GSTFile.FileName);
                    if (contractor.HasiyatCertificate != null)
                        HasiyatCertificate = Path.GetFileName(contractor.HasiyatCertificate.FileName).Remove(contractor.HasiyatCertificate.FileName.LastIndexOf(".")) + +DateTime.Now.Ticks + Path.GetExtension(contractor.HasiyatCertificate.FileName);
                    if (contractor.BloodRelation != null)
                        BloodRelation = Path.GetFileName(contractor.BloodRelation.FileName).Remove(contractor.BloodRelation.FileName.LastIndexOf(".")) + +DateTime.Now.Ticks + Path.GetExtension(contractor.BloodRelation.FileName);
                    if (contractor.ApplicantPhoto != null)
                        ApplicantPhoto = Path.GetFileName(contractor.ApplicantPhoto.FileName).Remove(contractor.ApplicantPhoto.FileName.LastIndexOf(".")) + +DateTime.Now.Ticks + Path.GetExtension(contractor.ApplicantPhoto.FileName);

                    if (contractor.CharacterFile != null)
                        CharacterFile = Path.GetFileName(contractor.CharacterFile.FileName).Remove(contractor.CharacterFile.FileName.LastIndexOf(".")) + +DateTime.Now.Ticks + Path.GetExtension(contractor.CharacterFile.FileName);
                    if (contractor.PrivateExperience != null)
                        PrivateExperience = Path.GetFileName(contractor.PrivateExperience.FileName).Remove(contractor.PrivateExperience.FileName.LastIndexOf(".")) + DateTime.Now.Ticks + Path.GetExtension(contractor.PrivateExperience.FileName);

                    string Category = "";


                    var param = new
                    {
                        FirmName = contractor.FirmName,
                        ApplicantName = contractor.ApplicantName,
                        ApplicantPhoto = ApplicantPhoto,
                        OfficeAddress = contractor.OfficeAddress,
                        ResidenceAddress = contractor.ResidenceAddress,
                        OfficePhone = contractor.OfficePhone,
                        ResidencePhone = contractor.ResidencePhone,
                        Mobile = contractor.Mobile,
                        Fax = contractor.Fax,
                        eMail = contractor.eMail,
                        RegFeeBank = contractor.RegFeeBank,
                        RegFee = contractor.RegFee,
                        RegFeeDemandNumber = contractor.RegFeeDemandNumber,
                        RegFeeDemandDate = contractor.RegFeeDemandDate,
                        SecurityBank = contractor.SecurityBank,
                        SecurityAmount = contractor.SecurityAmount,
                        SecurityFDRNuber = contractor.SecurityFDRNuber,
                        SecurityFDRDate = contractor.SecurityFDRDate,
                        SecurityMatureDate = contractor.SecurityMatureDate,
                        FirmDetail = contractor.FirmDetail,
                        ToolsPlantsFile = ToolsPlantsFile,
                        SevenYearMainWorksFile = FourYearMainWorksFile,
                        ListofStaffFile = ListofStaffFile,
                        BlackListFile = BlackListFile,
                        BalanceSheetFile = BalanceSheetFile,
                        IncomeTaxReturnFile = IncomeTaxReturnFile,
                        PAN = contractor.PAN,
                        PANFile = PANFile,
                        GST = contractor.GST,
                        GSTFile = GSTFile,
                        HasiyatCertificate = HasiyatCertificate,
                        BloodRelation = BloodRelation,
                        CharacterFile = CharacterFile,

                        PrivateExperience = PrivateExperience,
                        IPAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
                        Category = Category,
                        SubCategoryId = contractor.SubCategoryId,
                        CategoryId = contractor.CategoryId,
                        WhatsAppNo = contractor.WhatsAppNo
                    };
                    Result = dAL.QueryWithExecuteAsync("InsertContractorDetailsNew", param);
                    if (Convert.ToInt32(Result.Statuskey) > 0)
                    {
                        string folderPath = Path.Combine(env.WebRootPath, "Upload/ContractorNew/" + Convert.ToString(Result.Statuskey));
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        if (contractor.ToolsPlantsFile != null)
                        {
                            string filepath = Path.Combine(folderPath, ToolsPlantsFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.ToolsPlantsFile.CopyTo(stream);
                            }
                        }
                        if (contractor.SevenYearMainWorksFile != null)
                        {
                            string filepath = Path.Combine(folderPath, FourYearMainWorksFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.SevenYearMainWorksFile.CopyTo(stream);
                            }
                        }
                        if (contractor.ListofStaffFile != null)
                        {
                            string filepath = Path.Combine(folderPath, ListofStaffFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.ListofStaffFile.CopyTo(stream);
                            }
                        }
                        if (contractor.BlackListFile != null)
                        {
                            string filepath = Path.Combine(folderPath, BlackListFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.BlackListFile.CopyTo(stream);
                            }
                        }
                        if (contractor.BalanceSheetFile != null)
                        {
                            string filepath = Path.Combine(folderPath, BalanceSheetFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.BalanceSheetFile.CopyTo(stream);
                            }
                        }
                        if (contractor.IncomeTaxReturnFile != null)
                        {
                            string filepath = Path.Combine(folderPath, IncomeTaxReturnFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.IncomeTaxReturnFile.CopyTo(stream);
                            }
                        }
                        if (contractor.PANFile != null)
                        {
                            string filepath = Path.Combine(folderPath, PANFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.PANFile.CopyTo(stream);
                            }
                        }
                        if (contractor.GSTFile != null)
                        {
                            string filepath = Path.Combine(folderPath, GSTFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.GSTFile.CopyTo(stream);
                            }
                        }
                        if (contractor.HasiyatCertificate != null)
                        {
                            string filepath = Path.Combine(folderPath, HasiyatCertificate);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.HasiyatCertificate.CopyTo(stream);
                            }
                        }
                        if (contractor.BloodRelation != null)
                        {
                            string filepath = Path.Combine(folderPath, BloodRelation);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.BloodRelation.CopyTo(stream);
                            }
                        }

                        if (contractor.CharacterFile != null)
                        {
                            string filepath = Path.Combine(folderPath, CharacterFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.CharacterFile.CopyTo(stream);
                            }
                        }
                        if (contractor.PrivateExperience != null)
                        {
                            string filepath = Path.Combine(folderPath, PrivateExperience);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.PrivateExperience.CopyTo(stream);
                            }
                        }

                        if (contractor.ApplicantPhoto != null)
                        {
                            string filepath = Path.Combine(folderPath, ApplicantPhoto);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.ApplicantPhoto.CopyTo(stream);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                Result = new { Statuskey = 0, msg = ex.Message };

            }
            if (Result == null)
            {
                Result = new { Statuskey = 0, msg = "Some validations are missing , please upload all valid documents !!" };
            }
            return Json(Result);
        }

        public IActionResult architectReg()
        {
            string cyear = DateTime.Now.Year.ToString();
            string year1 = (Convert.ToInt32(cyear) - 1).ToString();
            string year2 = (Convert.ToInt32(cyear) - 2).ToString();
            ViewBag.cyear = cyear;
            ViewBag.year1 = year1;
            ViewBag.year2 = year2;
            //TempData["Category"] = "C";
            //TempData["Mobile"] = "9918578212";           
            if (TempData["Category"] == null)
            {
                return Redirect("/HNhome/index");
            }
            else
            {
                ViewBag.Mobile = TempData["Mobile"].ToString();
                ViewBag.Category = TempData["Category"].ToString();
            }
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> CheckContractorMobile(string Category, string Mobile)
        {
            var res = (dynamic)null;
            try
            {
                TempData["Mobile"] = Mobile;
                TempData["Category"] = Category;
                AppCommonMethod objapp = new AppCommonMethod();
                string OTP = "123";// objapp.GenerateOTP(6);
                HttpContext.Session.SetString("ConOTP", OTP);
                var param = new { Mobile = Mobile };
                var result = await dAL.QueryAsync("CheckContractorMobile", param);
                res = new
                {
                    InnerResult = result,
                    Status = 1
                };
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                res = new
                {
                    InnerResult = "",
                    Status = 0
                };
            }
            return Json(res);
        }

        [HttpGet]
        public async Task<JsonResult> CheckArcMobile(string Category, string Mobile)
        {
            var res = (dynamic)null;
            try
            {
                TempData["Mobile"] = Mobile;
                TempData["Category"] = Category;
                AppCommonMethod objapp = new AppCommonMethod();
                string OTP = "123";// objapp.GenerateOTP(6);
                HttpContext.Session.SetString("ConOTP", OTP);
                var param = new { Mobile = Mobile };
                var result = await dAL.QueryAsync("CheckArchitectureMobile", param);
                res = new
                {
                    InnerResult = result,
                    Status = 1
                };
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                res = new
                {
                    InnerResult = "",
                    Status = 0
                };
            }
            return Json(res);
        }
        public JsonResult frmContractorSubmit(Contractor contractor)
        {
            var Result = (dynamic)null;
            try
            {
                if (ModelState.IsValid)
                {
                    string ToolsPlantsFile = "", FourYearMainWorksFile = "", ListofStaffFile = "", BlackListFile = "", BalanceSheetFile = "", IncomeTaxReturnFile = "",
                       PANFile = "", GSTFile = "", HasiyatCertificate = "", BloodRelation = "", ApplicantPhoto = "";
                    if (contractor.ToolsPlantsFile != null)
                        ToolsPlantsFile = contractor.ToolsPlantsFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.ToolsPlantsFile.FileName.Split('.')[1].ToString();
                    if (contractor.FourYearMainWorksFile != null)
                        FourYearMainWorksFile = contractor.FourYearMainWorksFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.FourYearMainWorksFile.FileName.Split('.')[1].ToString();
                    if (contractor.ListofStaffFile != null)
                        ListofStaffFile = contractor.ListofStaffFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.ListofStaffFile.FileName.Split('.')[1].ToString();
                    if (contractor.BlackListFile != null)
                        BlackListFile = contractor.BlackListFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.BlackListFile.FileName.Split('.')[1].ToString();
                    if (contractor.BalanceSheetFile != null)
                        BalanceSheetFile = contractor.BalanceSheetFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.BalanceSheetFile.FileName.Split('.')[1].ToString();
                    if (contractor.IncomeTaxReturnFile != null)
                        IncomeTaxReturnFile = contractor.IncomeTaxReturnFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.IncomeTaxReturnFile.FileName.Split('.')[1].ToString();
                    if (contractor.PANFile != null)
                        PANFile = contractor.PANFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.PANFile.FileName.Split('.')[1].ToString();
                    if (contractor.GSTFile != null)
                        GSTFile = contractor.GSTFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.GSTFile.FileName.Split('.')[1].ToString();
                    if (contractor.HasiyatCertificate != null)
                        HasiyatCertificate = contractor.HasiyatCertificate.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.HasiyatCertificate.FileName.Split('.')[1].ToString();
                    if (contractor.BloodRelation != null)
                        BloodRelation = contractor.BloodRelation.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.BloodRelation.FileName.Split('.')[1].ToString();
                    if (contractor.ApplicantPhoto != null)
                        ApplicantPhoto = contractor.ApplicantPhoto.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + contractor.ApplicantPhoto.FileName.Split('.')[1].ToString();

                    string Category = "";
                    if (contractor.SecurityAmount == "500000")
                    {
                        Category = "A";
                    }
                    if (contractor.SecurityAmount == "200000")
                    {
                        Category = "B";
                    }
                    if (contractor.SecurityAmount == "100000")
                    {
                        Category = "C";
                    }
                    if (contractor.SecurityAmount == "50000")
                    {
                        Category = "D";
                    }

                    var param = new
                    {
                        FirmName = contractor.FirmName,
                        ApplicantName = contractor.ApplicantName,
                        ApplicantPhoto = ApplicantPhoto,
                        OfficeAddress = contractor.OfficeAddress,
                        ResidenceAddress = contractor.ResidenceAddress,
                        OfficePhone = contractor.OfficePhone,
                        ResidencePhone = contractor.ResidencePhone,
                        Mobile = contractor.Mobile,
                        Fax = contractor.Fax,
                        eMail = contractor.eMail,
                        RegFeeBank = contractor.RegFeeBank,
                        RegFee = contractor.RegFee,
                        RegFeeDemandNumber = contractor.RegFeeDemandNumber,
                        RegFeeDemandDate = contractor.RegFeeDemandDate,
                        SecurityBank = contractor.SecurityBank,
                        SecurityAmount = contractor.SecurityAmount,
                        SecurityFDRNuber = contractor.SecurityFDRNuber,
                        SecurityFDRDate = contractor.SecurityFDRDate,
                        SecurityMatureDate = contractor.SecurityMatureDate,
                        FirmDetail = contractor.FirmDetail,
                        ToolsPlantsFile = ToolsPlantsFile,
                        FourYearMainWorksFile = FourYearMainWorksFile,
                        ListofStaffFile = ListofStaffFile,
                        BlackListFile = BlackListFile,
                        BalanceSheetFile = BalanceSheetFile,
                        IncomeTaxReturnFile = IncomeTaxReturnFile,
                        PAN = contractor.PAN,
                        PANFile = PANFile,
                        GST = contractor.GST,
                        GSTFile = GSTFile,
                        HasiyatCertificate = HasiyatCertificate,
                        BloodRelation = BloodRelation,
                        IPAddress = HttpContext.Connection.RemoteIpAddress.ToString(),

                        Category = Category
                    };
                    Result = dAL.QueryWithExecuteAsync("InsertContractorDetails", param);
                    if (Convert.ToInt32(Result.Statuskey) > 0)
                    {
                        string folderPath = Path.Combine(env.WebRootPath, "Upload/Contractor/" + Convert.ToString(Result.Statuskey));
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        if (contractor.ToolsPlantsFile != null)
                        {
                            string filepath = Path.Combine(folderPath, ToolsPlantsFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.ToolsPlantsFile.CopyTo(stream);
                            }
                        }
                        if (contractor.FourYearMainWorksFile != null)
                        {
                            string filepath = Path.Combine(folderPath, FourYearMainWorksFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.FourYearMainWorksFile.CopyTo(stream);
                            }
                        }
                        if (contractor.ListofStaffFile != null)
                        {
                            string filepath = Path.Combine(folderPath, ListofStaffFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.ListofStaffFile.CopyTo(stream);
                            }
                        }
                        if (contractor.BlackListFile != null)
                        {
                            string filepath = Path.Combine(folderPath, BlackListFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.BlackListFile.CopyTo(stream);
                            }
                        }
                        if (contractor.BalanceSheetFile != null)
                        {
                            string filepath = Path.Combine(folderPath, BalanceSheetFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.BalanceSheetFile.CopyTo(stream);
                            }
                        }
                        if (contractor.IncomeTaxReturnFile != null)
                        {
                            string filepath = Path.Combine(folderPath, IncomeTaxReturnFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.IncomeTaxReturnFile.CopyTo(stream);
                            }
                        }
                        if (contractor.PANFile != null)
                        {
                            string filepath = Path.Combine(folderPath, PANFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.PANFile.CopyTo(stream);
                            }
                        }
                        if (contractor.GSTFile != null)
                        {
                            string filepath = Path.Combine(folderPath, GSTFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.GSTFile.CopyTo(stream);
                            }
                        }
                        if (contractor.HasiyatCertificate != null)
                        {
                            string filepath = Path.Combine(folderPath, HasiyatCertificate);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.HasiyatCertificate.CopyTo(stream);
                            }
                        }
                        if (contractor.BloodRelation != null)
                        {
                            string filepath = Path.Combine(folderPath, BloodRelation);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.BloodRelation.CopyTo(stream);
                            }
                        }
                        if (contractor.ApplicantPhoto != null)
                        {
                            string filepath = Path.Combine(folderPath, ApplicantPhoto);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                contractor.ApplicantPhoto.CopyTo(stream);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                Result = new { Statuskey = 0 };
            }
            return Json(Result);
        }

        [HttpGet]
        public async Task<JsonResult> ContractorVerifyOTP(string OTP)
        {
            var res = (dynamic)null;
            try
            {
                var s = HttpContext.Session.GetString("ConOTP");
                if (HttpContext.Session.GetString("ConOTP") == OTP)
                    res = "Success";
                else
                    res = "Wrong OTP";
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");

            }
            return Json(res);
        }



        public JsonResult SubmitArc(Architecture architecture)
        {
            var Result = (dynamic)null;
            try
            {
                if (ModelState.IsValid)
                {
                    string Photo = "", CouncilPhotoCoopy = "", AnnexureAThirdYearFile = "", AnnexureASecondYearFile = "", AnnexureAFirstYearFile = ""
                        , AnnexureAThirdYearFileSingle = "", AnnexureASecondYearFileSingle = "",
           AnnexureAFirstYearFileSingle = "", AnnexureBThirdYearFile = "", AnnexureBSecondYearFile = "",
           AnnexureBFirstYearFile = "", PanFile = "", ServiceTaxFile = "", OtherempanelledFile = "", AnnexureCFile = "";

                    if (architecture.Photo != null)
                        Photo = architecture.Photo.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + architecture.Photo.FileName.Split('.')[1].ToString();
                    if (architecture.CouncilPhotoCoopy != null)
                        CouncilPhotoCoopy = architecture.CouncilPhotoCoopy.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + architecture.CouncilPhotoCoopy.FileName.Split('.')[1].ToString();
                    if (architecture.AnnexureAThirdYearFile != null)
                        AnnexureAThirdYearFile = architecture.AnnexureAThirdYearFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + architecture.AnnexureAThirdYearFile.FileName.Split('.')[1].ToString();
                    if (architecture.AnnexureASecondYearFile != null)
                        AnnexureASecondYearFile = architecture.AnnexureASecondYearFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + architecture.AnnexureASecondYearFile.FileName.Split('.')[1].ToString();
                    if (architecture.AnnexureAFirstYearFile != null)
                        AnnexureAFirstYearFile = architecture.AnnexureAFirstYearFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + architecture.AnnexureAFirstYearFile.FileName.Split('.')[1].ToString();
                    if (architecture.AnnexureAThirdYearFileSingle != null)
                        AnnexureAThirdYearFileSingle = architecture.AnnexureAThirdYearFileSingle.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + architecture.AnnexureAThirdYearFileSingle.FileName.Split('.')[1].ToString();
                    if (architecture.AnnexureASecondYearFileSingle != null)
                        AnnexureASecondYearFileSingle = architecture.AnnexureASecondYearFileSingle.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + architecture.AnnexureASecondYearFileSingle.FileName.Split('.')[1].ToString();
                    if (architecture.AnnexureAFirstYearFileSingle != null)
                        AnnexureAFirstYearFileSingle = architecture.AnnexureAFirstYearFileSingle.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + architecture.AnnexureAFirstYearFileSingle.FileName.Split('.')[1].ToString();
                    if (architecture.AnnexureBThirdYearFile != null)
                        AnnexureBThirdYearFile = architecture.AnnexureBThirdYearFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + architecture.AnnexureBThirdYearFile.FileName.Split('.')[1].ToString();
                    if (architecture.AnnexureBSecondYearFile != null)
                        AnnexureBSecondYearFile = architecture.AnnexureBSecondYearFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + architecture.AnnexureBSecondYearFile.FileName.Split('.')[1].ToString();
                    if (architecture.AnnexureBFirstYearFile != null)
                        AnnexureBFirstYearFile = architecture.AnnexureBFirstYearFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + architecture.AnnexureBFirstYearFile.FileName.Split('.')[1].ToString();
                    if (architecture.PanFile != null)
                        PanFile = architecture.PanFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + architecture.PanFile.FileName.Split('.')[1].ToString();
                    if (architecture.ServiceTaxFile != null)
                        ServiceTaxFile = architecture.ServiceTaxFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + architecture.ServiceTaxFile.FileName.Split('.')[1].ToString();
                    if (architecture.OtherempanelledFile != null)
                        OtherempanelledFile = architecture.OtherempanelledFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + architecture.OtherempanelledFile.FileName.Split('.')[1].ToString();
                    if (architecture.AnnexureCFile != null)
                        AnnexureCFile = architecture.AnnexureCFile.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + architecture.AnnexureCFile.FileName.Split('.')[1].ToString();

                    string year1 = DateTime.Now.Year.ToString();
                    string year2 = (Convert.ToInt32(year1) - 1).ToString();
                    string year3 = (Convert.ToInt32(year1) - 2).ToString();


                    var param = new
                    {
                        Category = architecture.Category
,
                        NameofConsultant = architecture.NameofConsultant
,
                        Address = architecture.Address
,
                        Name = architecture.Name
,
                        Photo = Photo
,
                        PhoneNumber = architecture.PhoneNumber
,
                        Mobile = architecture.Mobile
,
                        Fax = architecture.Fax
,
                        eMail = architecture.eMail
,
                        Website = architecture.Website
,
                        DataProcessingDD = architecture.DataProcessingDD
,
                        DataProcessingDDDate = architecture.DataProcessingDDDate
,
                        DataProcessingDDBank = architecture.DataProcessingDDBank
,
                        SecurityDDNumber = architecture.SecurityDDNumber
,
                        SecurityDDDate = architecture.SecurityDDDate
,
                        SecurityBank = architecture.SecurityBank
,
                        SecurityAmount = architecture.SecurityAmount
,
                        CouncilRegistrationDate = architecture.CouncilRegistrationDate
,
                        CouncilDateofValidity = architecture.CouncilDateofValidity
,
                        CouncilPhotoCoopy = CouncilPhotoCoopy
,
                        AnnexureAThirdYear = architecture.AnnexureAThirdYear
,
                        AnnexureAThirdYearFile = AnnexureAThirdYearFile
,
                        AnnexureASecondYear = architecture.AnnexureASecondYear
,
                        AnnexureASecondYearFile = AnnexureASecondYearFile,
                        AnnexureAFirstYear = architecture.AnnexureAFirstYear,
                        AnnexureAFirstYearFile = AnnexureAFirstYearFile,
                        AnnexureAThirdYearSingle = architecture.AnnexureAThirdYearSingle,
                        AnnexureAThirdYearFileSingle = AnnexureAThirdYearFileSingle,
                        AnnexureASecondYearSingle = architecture.AnnexureASecondYearSingle,
                        AnnexureASecondYearFileSingle = AnnexureASecondYearFileSingle,
                        AnnexureAFirstYearSingle = architecture.AnnexureAFirstYearSingle,
                        AnnexureAFirstYearFileSingle = AnnexureAFirstYearFileSingle,
                        AnnexureBThirdYear = architecture.AnnexureBThirdYear,
                        AnnexureBThirdYearFile = AnnexureBThirdYearFile,
                        AnnexureBSecondYear = architecture.AnnexureBSecondYear,
                        AnnexureBSecondYearFile = AnnexureBSecondYearFile,
                        AnnexureBFirstYear = architecture.AnnexureBFirstYear,
                        AnnexureBFirstYearFile = AnnexureBFirstYearFile,
                        Pan = architecture.Pan,
                        PanFile = PanFile,
                        ServiceTax = architecture.ServiceTax,
                        ServiceTaxFile = ServiceTaxFile,
                        OtherempanelledFile = OtherempanelledFile,
                        AnnexureCFile = AnnexureCFile
,
                        IPAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
                        year1 = year1,
                        year2 = year2,
                        year3 = year3,
                    };
                    Result = dAL.QueryWithExecuteAsync("InsertArchitectureDetails", param);
                    if (Convert.ToInt32(Result.StatusKey) > 0)
                    {
                        string folderPath = Path.Combine(env.WebRootPath, "Upload/Arc/" + Convert.ToString(Result.StatusKey));
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        if (architecture.Photo != null)
                        {
                            string filepath = Path.Combine(folderPath, Photo);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                architecture.Photo.CopyTo(stream);
                            }
                        }
                        if (architecture.CouncilPhotoCoopy != null)
                        {
                            string filepath = Path.Combine(folderPath, CouncilPhotoCoopy);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                architecture.CouncilPhotoCoopy.CopyTo(stream);
                            }
                        }
                        if (architecture.AnnexureAThirdYearFile != null)
                        {
                            string filepath = Path.Combine(folderPath, AnnexureAThirdYearFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                architecture.AnnexureAThirdYearFile.CopyTo(stream);
                            }
                        }
                        if (architecture.AnnexureASecondYearFile != null)
                        {
                            string filepath = Path.Combine(folderPath, AnnexureASecondYearFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                architecture.AnnexureASecondYearFile.CopyTo(stream);
                            }
                        }
                        if (architecture.AnnexureAFirstYearFile != null)
                        {
                            string filepath = Path.Combine(folderPath, AnnexureAFirstYearFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                architecture.AnnexureAFirstYearFile.CopyTo(stream);
                            }
                        }
                        if (architecture.AnnexureAThirdYearFileSingle != null)
                        {
                            string filepath = Path.Combine(folderPath, AnnexureAThirdYearFileSingle);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                architecture.AnnexureAThirdYearFileSingle.CopyTo(stream);
                            }
                        }
                        if (architecture.AnnexureASecondYearFileSingle != null)
                        {
                            string filepath = Path.Combine(folderPath, AnnexureASecondYearFileSingle);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                architecture.AnnexureASecondYearFileSingle.CopyTo(stream);
                            }
                        }
                        if (architecture.AnnexureAFirstYearFileSingle != null)
                        {
                            string filepath = Path.Combine(folderPath, AnnexureAFirstYearFileSingle);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                architecture.AnnexureAFirstYearFileSingle.CopyTo(stream);
                            }
                        }
                        if (architecture.AnnexureBThirdYearFile != null)
                        {
                            string filepath = Path.Combine(folderPath, AnnexureBThirdYearFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                architecture.AnnexureBThirdYearFile.CopyTo(stream);
                            }
                        }
                        if (architecture.AnnexureBSecondYearFile != null)
                        {
                            string filepath = Path.Combine(folderPath, AnnexureBSecondYearFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                architecture.AnnexureBSecondYearFile.CopyTo(stream);
                            }
                        }
                        if (architecture.AnnexureBFirstYearFile != null)
                        {
                            string filepath = Path.Combine(folderPath, AnnexureBFirstYearFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                architecture.AnnexureBFirstYearFile.CopyTo(stream);
                            }
                        }
                        if (architecture.PanFile != null)
                        {
                            string filepath = Path.Combine(folderPath, PanFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                architecture.PanFile.CopyTo(stream);
                            }
                        }
                        if (architecture.ServiceTaxFile != null)
                        {
                            string filepath = Path.Combine(folderPath, ServiceTaxFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                architecture.ServiceTaxFile.CopyTo(stream);
                            }
                        }
                        if (architecture.OtherempanelledFile != null)
                        {
                            string filepath = Path.Combine(folderPath, OtherempanelledFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                architecture.OtherempanelledFile.CopyTo(stream);
                            }
                        }
                        if (architecture.AnnexureCFile != null)
                        {
                            string filepath = Path.Combine(folderPath, AnnexureCFile);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                architecture.AnnexureCFile.CopyTo(stream);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                Result = new { Statuskey = 0 };
            }
            return Json(Result);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////CM Announcements
        [HttpGet]
        public async Task<JsonResult> GetLastestCMAnnouncement()
        {
            var Result = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                Result = await dAL.QueryAsync("GetCMAnnouncementDetails", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                Result = new { Statuskey = 0 };
            }
            return Json(Result);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////CM Announcements
        [HttpGet]
        public async Task<JsonResult> GetLastestMonthlyCompileReport()
        {
            var Result = (dynamic)null;
            try
            {
                var param = new { Id = 0 };
                Result = await dAL.QueryAsync("GetMonthlyCompiledDetails", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                Result = new { Statuskey = 0 };
            }
            return Json(Result);
        }

        public IActionResult MonthlyProgressReport()
        {
            return View();
        }
        public IActionResult Financial()
        {
            List<FinancialMaster> result = new List<FinancialMaster>();
            try
            {

                var param = new { Id = 0 };
                result = DB_Conn.SelectProcedureExecute<FinancialMaster>("GetFinancialDetails", param);


            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(result);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Photo Gallery
        public IActionResult Photo_Album()
        {
            List<PhotoGallery> result = new List<PhotoGallery>();
            try
            {

                result = DB_Conn.SelectProcedureExecute<PhotoGallery>("[GetPhotoAlbumGallary]", null);
            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(result);
        }
        public IActionResult Photo_Gallery(int id)
        {
            List<PhotoGallery> result = new List<PhotoGallery>();
            try
            {

                var param = new { AlbumId = id };
                result = DB_Conn.SelectProcedureExecute<PhotoGallery>("GetPhotoGalleryByAlbum", param);

            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(result);
        }

        public IActionResult VideoGallery()
        {
            List<VideoMaster> video = new List<VideoMaster>();
            try
            {
                var param = new { Id = 0 };
                video = DB_Conn.SelectProcedureExecute<VideoMaster>("GetVideoDetails", param);

            }

            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return View(video);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Media Gallery
        public IActionResult MediaGallery()
        {
            List<MediaGallery> result = new List<MediaGallery>();
            try
            {

                var param = new { Id = 0 };
                result = DB_Conn.SelectProcedureExecute<MediaGallery>("GetMediaGalleryDetails", param);

            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(result);
        }
        ///////////////////////////////////////////////////////////////////OnGoing Projects////////////////
        public IActionResult OnGoingProjects_uppcl()
        {
            return View();
        }
        public IActionResult OnGoingProjects_units()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetUnitsByZone(string zone)
        {
            var Result = (dynamic)null;
            try
            {
                var param = new { ZoneId = zone };
                Result = await dAL.QueryAsync("GetUnitsByZone", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                Result = new { Statuskey = 0 };
            }
            return Json(Result);
        }
        public IActionResult Projects()
        {
            return View();
        }
        public IActionResult Project_Photos()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetOnGoingProjects(string unit)
        {
            var Result = (dynamic)null;
            try
            {
                var param = new { unit = unit };
                Result = await dAL.QueryAsync("GetOnGoingProjects", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                Result = new { Statuskey = 0 };
            }
            return Json(Result);
        }
        [HttpGet]
        public async Task<JsonResult> GetOnGoingProjectPhotos(string unit, string ProjectId)
        {
            var Result = (dynamic)null;
            try
            {
                var param = new { unit = unit, ProjectId = ProjectId };
                Result = await dAL.QueryAsync("GetOnGoingProjectPhotos", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                Result = new { Statuskey = 0 };
            }
            return Json(Result);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////Completd Project
        public IActionResult CompletedProjects()
        {
            return View();
        }

        public IActionResult CompletdProjects_units()
        {
            return View();
        }

        public IActionResult CompleteProjects()
        {
            return View();
        }
        public IActionResult CompleteProjects_Photos()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetCompltedProjects(string unit)
        {
            var Result = (dynamic)null;
            try
            {
                var param = new { unit = unit };
                Result = await dAL.QueryAsync("GetCompletedProjects", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                Result = new { Statuskey = 0 };
            }
            return Json(Result);
        }

        [HttpGet]
        public async Task<JsonResult> GetCompleteProjectPhotos(string unit, string ProjectId)
        {
            var Result = (dynamic)null;
            try
            {
                var param = new { unit = unit, ProjectId = ProjectId };
                Result = await dAL.QueryAsync("GetCompletedProjectPhotos", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                Result = new { Statuskey = 0 };
            }
            return Json(Result);
        }

        public async Task<IActionResult> Tenders()
        {

            try
            {
                await GetZoneHN();

            }
            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }

            return View();
        }
        public async Task GetZone()
        {
            List<CommonSelect> obj = new List<CommonSelect>();
            obj = await dAL.BindSelect("GetZone", null);
            obj.Insert(0, new CommonSelect { Id = "0", Value = "Select Zone" });
            ViewBag.Zone = new SelectList(obj, "Id", "Value");
        }
        public async Task GetZoneHN()
        {
            List<CommonSelect> obj = new List<CommonSelect>();
            obj = await dAL.BindSelect("GetZoneHN", null);
            obj.Insert(0, new CommonSelect { Id = "0", Value = "ज़ोन का चयन करें" });
            ViewBag.Zone = new SelectList(obj, "Id", "Value");
        }
        [HttpGet]
        public async Task<JsonResult> SearchTenders(string ZoneId, string UnitId)
        {
            List<Tender> objtender = new List<Tender>();
            try
            {
                await GetZone();
                var param = new { UnitId = UnitId == "0" ? null : UnitId, ZoneId = ZoneId == "0" ? null : ZoneId };
                objtender = dAL.GetTenderList("GetTenderList", param);
            }
            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Json(objtender);
        }
        public async Task<JsonResult> BindUnit(string Zoneid)
        {
            List<CommonSelect> obj = new List<CommonSelect>();
            try
            {

                var param = new { ZoneId = Zoneid };
                obj = await dAL.BindSelect("GetUnit", param);
                obj.Insert(0, new CommonSelect { Id = "0", Value = "Select Unit" });
                // ViewBag.Unit = new SelectList(obj, "Id", "Value");
            }
            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Json(obj);
        }
        public async Task<JsonResult> BindUnitHN(string Zoneid)
        {
            List<CommonSelect> obj = new List<CommonSelect>();
            try
            {

                var param = new { ZoneId = Zoneid };
                obj = await dAL.BindSelect("GetUnitHN", param);
                obj.Insert(0, new CommonSelect { Id = "0", Value = "इकाई का चयन करें" });
                // ViewBag.Unit = new SelectList(obj, "Id", "Value");
            }
            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return Json(obj);
        }

        public IActionResult EOI()
        {
            List<EOI> result = new List<EOI>();
            try
            {
                var param = new { Id = 0 };
                result = DB_Conn.SelectProcedureExecute<EOI>("GetEOIDetails", param);

            }

            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return View(result);
        }
        public IActionResult GOS()
        {
            List<GOS> result = new List<GOS>();
            try
            {
                var param = new { Id = 0 };
                result = DB_Conn.SelectProcedureExecute<GOS>("GetGOSDetails", param);

            }

            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return View(result);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Registered Contractor List
        public IActionResult ListOfContractors()
        {
            List<Contractor> result = new List<Contractor>();
            try
            {

                result = DB_Conn.SelectProcedureExecute<Contractor>("[GetContractorListHome]", null);
            }
            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(result);
        }
        public IActionResult ListofStateofGSTNumber()
        {
            return View();
        }
        public IActionResult WebDirectory()
        {
            List<WebDirectoryMaster> result = new List<WebDirectoryMaster>();
            var param = new { Id = 0 };
            try
            {
                result = DB_Conn.SelectProcedureExecute<WebDirectoryMaster>("[SelectWebDirectory]", param);
                result = result.OrderBy(x => x.CategoryId).ToList();
            }

            catch (Exception ex)
            {
                acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
            }
            return View(result);
        }
        public IActionResult ReachUs()
        {
            return View();
        }
        public IActionResult Feedback()
        {
            FeedbackMaster fd = new FeedbackMaster();
            return View(fd);
        }
        [HttpPost]
        public async Task<JsonResult> Feedback(FeedbackMaster obj)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            try
            {
                var param = new { Name = obj.Name, EmailId = obj.EmailId, MobileNo = obj.MobileNo, Address = obj.Address, FeedbackQuery = obj.FeedbackQuery, UserIP = HttpContext.Connection.RemoteIpAddress.ToString() };
                innerresult = await dAL.QueryAsync("InsertFeedback", param);
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
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "");
                Result = new { Statuskey = 0 };
            }
            return Json(Result);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Insert Visitor
        public IActionResult insertVisitor()
        {

            string result = "";
            try
            {

                var param = new { UserIP = HttpContext.Connection.RemoteIpAddress.ToString() };

                result = DB_Conn.CUDProcedureExecute("InsertVisitor", param);

            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(result);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Get Visitor count
        public JsonResult GetVisitor()
        {

            List<Visitors> result = new List<Visitors>();
            try
            {

                result = DB_Conn.SelectProcedureExecute<Visitors>("GetVisitor", null);

            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(result);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Get All Pages Last Updated Record
        public JsonResult GetLastUpdatedRecord(string ActionName)
        {

            List<LastUpdated> result = new List<LastUpdated>();
            try
            {
                var param = new { ActionName = ActionName };
                result = DB_Conn.SelectProcedureExecute<LastUpdated>("GetUpdatedRecord", param);

            }
            catch (Exception ex)
            {
                 acm.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(result);
        }

        public IActionResult WebManager()
        {
            return View();
        }
        public ActionResult SiteMap()
        {
            return View();
        }
        public ActionResult HNSecurityPolicy()
        {
            return View();
         
        }
        public async Task<IActionResult> OrganizationStructure()
        {
            Projects_Count project = new Projects_Count();
            List<Projects_Count> project_count = new List<Projects_Count>();
            try
            {
                project_count = DB_Conn.SelectProcedureExecute<Projects_Count>("[GetProjectCount]", null);
                if (project_count != null)
                {
                    project = project_count.FirstOrDefault();
                    ViewBag.Image = project.OrganizationImg;
                }
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View();
        }
        public async Task<IActionResult> boardofdirectors()
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
            }
            return View(data);
        }
        public async Task<IActionResult> RegContractor()
        {
            Projects_Count project = new Projects_Count();
            List<Projects_Count> project_count = new List<Projects_Count>();
            try
            {
                project_count = DB_Conn.SelectProcedureExecute<Projects_Count>("[GetProjectCount]", null);
                if (project_count != null)
                {
                    project = project_count.FirstOrDefault();
                    ViewBag.RegForm = project.RegistrationForm;
                }
            }
            catch (Exception ex)
            {
            }
            return View();
        }
        public async Task<IActionResult> architectTC()
        {
            Projects_Count project = new Projects_Count();
            List<Projects_Count> project_count = new List<Projects_Count>();
            try
            {
                project_count = DB_Conn.SelectProcedureExecute<Projects_Count>("[GetProjectCount]", null);
                if (project_count != null)
                {
                    project = project_count.FirstOrDefault();
                    ViewBag.AnnexureForm = project.AnnexureForm;
                }
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View();
        }
        public ActionResult LabPhotogallery()
        {
            return View();
        }
        public class SubcategoryDetails
        {
            public decimal RegistrationFee { get; set; }
            public decimal RenewalFee { get; set; }
            public decimal SecurityFee { get; set; }
            public string TenderingLimit { get; set; }
        }
    }
}
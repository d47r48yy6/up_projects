using System;
using System.Collections.Generic;
using System.IO;
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
    [Authorize(Roles ="admin,unit,gm,srv")]
    public class TenderController : Controller
    {

        private readonly DAL dAL;
        private readonly AppCommonMethod acm;        
        private readonly IWebHostEnvironment _env;

        public TenderController(DAL _dAL,AppCommonMethod _acm, IWebHostEnvironment env)
        {
            dAL = _dAL;
            acm = _acm;        
            _env = env;
        }
        public async Task CategoryList()
        {
            List<CommonSelect> obj = new List<CommonSelect>();
            obj = await dAL.BindSelect("GetCategory", null);
            obj.Insert(0, new CommonSelect { Id = "0", Value = "Select Category" });
            ViewBag.Category = new SelectList(obj, "Id", "Value");
        }
        public async Task<IActionResult> Tender(int id)
        {
            Tender objtender = new Tender();
            var UserRole = this.User.Claims.First(c => c.Type == "Role").Value.ToString();
            var ZoneIds = this.User.Claims.First(c => c.Type == "ZoneId").Value.ToString();
            var UnitIds = this.User.Claims.First(c => c.Type == "UnitId").Value.ToString();
            ViewBag.UserRole = UserRole;
            ViewBag.ZoneId1 = ZoneIds;
            ViewBag.UnitId1 = UnitIds;
            try
            {
                //await CategoryList();
                if (id > 0)
                {
                    var param = new { TenderId = id, UserId = this.User.Claims.First(c => c.Type == "UserId").Value.ToString() };
                    objtender = dAL.GetTenderDetails("GetTenderDetails", param);
                    ViewBag.ButtonText = "Update";
                }
                else
                {
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
        public  async Task<IActionResult> Tender(Tender tender)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var UserRole = this.User.Claims.First(c => c.Type == "Role").Value.ToString();
                    var ZoneIds = "";
                    var UnitIds = "";


                    if (UserRole=="admin")
                    {
                        ZoneIds = tender.ZoneId;
                        UnitIds = tender.UnitId;

                    }
                    else
                    {
                         ZoneIds = this.User.Claims.First(c => c.Type == "ZoneId").Value.ToString();
                         UnitIds = this.User.Claims.First(c => c.Type == "UnitId").Value.ToString();

                    }
                    if (tender.file !=null)
                        FileName = tender.file.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + tender.file.FileName.Split('.')[1].ToString();
                        var param = new
                        {
                            Id = tender.Id,
                            CategoryId = tender.CategoryId,
                            TenderNumber = tender.TenderNumber,
                            TenderTitle = tender.TenderTitle,
                            HNTenderTitle=tender.HNTenderTitle,
                            DateofTender = tender.DateofTender,
                            LastDateofSale = tender.LastDateofSale,
                            OpeningDate = tender.OpeningDate,
                            FileName = FileName,
                            UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                            IPAddress = HttpContext.Connection.RemoteIpAddress.ToString(),// AppCommonMethod.GetIP(),
                            UnitId = UnitIds,
                            ZoneId = ZoneIds
                        };
                        innerresult = dAL.QueryWithExecuteAsync("InsertTenderDetails", param);

                        if (Convert.ToInt32(innerresult.Status) > 0)
                        {
                            if (tender.file != null)
                            {
                                string RootFolderPath = Path.Combine(_env.WebRootPath, "Upload/Tender/" + innerresult.Status);
                                if (!Directory.Exists(RootFolderPath))
                                {
                                    Directory.CreateDirectory(RootFolderPath);
                                }
                                string FilePath = Path.Combine(RootFolderPath, FileName);
                                using (var stream = new FileStream(FilePath, FileMode.Create))
                                {
                                    tender.file.CopyTo(stream);
                                }
                            }
                        }
                    
                    var DynamicResult = new
                        {
                            innerresult = innerresult,
                            status = true,
                            eventKey = "New Tender"
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

        public IActionResult TenderList()
        {            
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> TendersList()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { UserId = this.User.Claims.First(c=> c.Type== "UserId").Value.ToString() };
                data= await dAL.QueryAsync("GetTenderListbyLogin", param);
            }
            catch (Exception ex)
            {
              await   acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteTender(int id)
        {
            var data = (dynamic)null;
            try
            {
                var param = new { TenderId =id, UserId= this.User.Claims.First(c => c.Type == "UserId").Value , IPAddress = HttpContext.Connection.RemoteIpAddress.ToString()};
                data = await dAL.QueryAsync("DeleteTender", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        
        public IActionResult ListofArchiveTender()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ListofArchiveTenders()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { UserId = this.User.Claims.First(c => c.Type == "UserId").Value.ToString() };
                data = await dAL.QueryAsync("GetListofArchiveTender", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using UPProjects.Models;

namespace UPProjects.Controllers
{
    public class NamamiGangeController : Controller
    {
        private readonly DAL dAL;
        private readonly AppCommonMethod acm;
        private readonly IWebHostEnvironment env;
        public NamamiGangeController(DAL _dal, AppCommonMethod _acm, IWebHostEnvironment _env)
        {
            dAL = _dal;
            acm = _acm;
            env = _env;
        }
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> NamamiGangePhoto(int id)
        {
            NamamiGange objtender = new NamamiGange();
            ViewBag.Id = id;
            try
            {
                if (id > 0)
                {
                    var param = new
                    {
                        Id = id,
                        UserId = this.User.Claims.First(c => c.Type == "UserId").Value.ToString(),
                        Role = this.User.Claims.First(c => c.Type == "Role").Value.ToString()
                    };
                    objtender = dAL.GetNamamiGangePhotoEdit("GetNamamiGangePhoto", param);
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
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> NamamiGangePhoto(NamamiGange photoup)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (photoup.File != null)
                        FileName = photoup.File.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + photoup.File.FileName.Split('.')[1].ToString();
                    var param = new
                    {
                        Id = photoup.Id,
                        Description = photoup.Description,
                        FileName = FileName,
                        UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        IPAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
                        Role = ((ClaimsIdentity)this.User.Identity).FindFirst("Role").Value
                    };
                    innerresult = dAL.QueryWithExecuteAsync("InsUpdNamamiGangePhoto", param);
                    if (Convert.ToInt32(innerresult.Status) > 0)
                    {
                        if (photoup.File != null)
                        {
                            string RootFolderPath = Path.Combine(env.WebRootPath, "Upload/NamamiGange/" + innerresult.Status);
                            if (!Directory.Exists(RootFolderPath))
                            {
                                Directory.CreateDirectory(RootFolderPath);
                            }
                            string FilePath = Path.Combine(RootFolderPath, FileName);
                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                photoup.File.CopyTo(stream);
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
                    await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }
    }
}
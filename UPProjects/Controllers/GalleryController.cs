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
   
    public class GalleryController : Controller
    {
        private readonly DAL dAL;
        private readonly AppCommonMethod acm;
        private readonly IWebHostEnvironment env;
        public GalleryController(DAL _dal, AppCommonMethod _acm, IWebHostEnvironment _env) {
            dAL = _dal;
            acm = _acm;
            env = _env;
        }
        
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> PhotoGallery(int id)
        {
            PhotoGallery objtender = new PhotoGallery();
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
                    objtender = dAL.GetPhotoGalleryEdit("GetPhotoGallery", param);
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
        public async Task<IActionResult> PhotoGallery(PhotoGallery photoup)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            string FileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (photoup.file != null)
                    {
                        string extension = System.IO.Path.GetExtension(photoup.file.FileName);
                        Guid guid = Guid.NewGuid();
                        //FileName = photoup.file.FileName.Split('.')[0] + DateTime.Now.Ticks + "." + photoup.file.FileName.Split('.')[1].ToString();
                        FileName = guid.ToString().Substring(0, 8) + DateTime.Now.Ticks + extension;
                        
                    }
                    
                    var param = new
                    {
                        Id = photoup.Id,                    
                        AlbumId = photoup.AlbumId,
                        Description = photoup.Description,
                        FileName = FileName,
                        UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value,
                        IPAddress = HttpContext.Connection.RemoteIpAddress.ToString(),                       
                        Role = ((ClaimsIdentity)this.User.Identity).FindFirst("Role").Value
                    };                    
                    innerresult = dAL.QueryWithExecuteAsync("InsertPhotoGallery", param);
                    if (Convert.ToInt32(innerresult.Status) > 0)
                    {
                        if (photoup.file != null)
                        {
                            string RootFolderPath = Path.Combine(env.WebRootPath, "Upload/PhotoGallery/" + innerresult.Status);
                            if (!Directory.Exists(RootFolderPath))
                            {
                                Directory.CreateDirectory(RootFolderPath);
                            }
                            string FilePath = Path.Combine(RootFolderPath, FileName);
                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                photoup.file.CopyTo(stream);
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

        [Authorize(Roles = "admin,srv")]
        public IActionResult PhotoGalleryList()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> PhotoGalleryUploadedList()
        {
            var data = (dynamic)null;
            try
            {
                var param = new { UserId = this.User.Claims.First(c => c.Type == "UserId").Value.ToString(), Role = this.User.Claims.First(c => c.Type == "Role").Value.ToString() };
                data = await dAL.QueryAsync("GetPhotoGalleryUploadedList", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }
        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> PhotoGalleryUploadedListByAlbum(int Albumid)
        {
            var data = (dynamic)null;
            try
            {
                var param = new { AlbumId= Albumid, UserId = this.User.Claims.First(c => c.Type == "UserId").Value.ToString(), Role = this.User.Claims.First(c => c.Type == "Role").Value.ToString() };
                data = await dAL.QueryAsync("GetPhotoGalleryUploadedListByAlbum", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        [HttpGet]
        [Authorize(Roles = "admin,srv")]
        public async Task<IActionResult> DeletePhotos(int id)
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = id, UserId = this.User.Claims.First(c => c.Type == "UserId").Value, Role = this.User.Claims.First(c => c.Type == "Role").Value, IPAddress = HttpContext.Connection.RemoteIpAddress.ToString() };
                data = await dAL.QueryAsync("DeletePhotoGallery", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }
        public JsonResult BindAlbum()
        {
            PhotoGallery mt = new PhotoGallery();
            var data = mt.GetAllAlbum();
            return Json(data);
        }
        public JsonResult DelPhotos(int id)
        {
            var data = (dynamic)null;
            string msg = "NA";
            try
            {
                var param = new { Id = id, UserId = this.User.Claims.First(c => c.Type == "UserId").Value, Role = this.User.Claims.First(c => c.Type == "Role").Value, IPAddress = HttpContext.Connection.RemoteIpAddress.ToString() };
                data = dAL.QueryAsync("DeletePhotoGallery", param);
                msg = "Album photos has been deleted.";
            }
            catch
            {
                msg = "Album photos not deleted.";
            }
           
            return Json(msg);

        }
    }
}
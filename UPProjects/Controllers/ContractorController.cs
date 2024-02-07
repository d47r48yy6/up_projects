using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UPProjects.Models;
using System.IO.Compression;
using Microsoft.AspNetCore.Hosting;

namespace UPProjects.Controllers
{
    public class ContractorController : Controller
    {
        private readonly DAL dAL;
        private readonly AppCommonMethod acm;
        private readonly IWebHostEnvironment env;
        public ContractorController(DAL _dal,AppCommonMethod _acm, IWebHostEnvironment _env)
        {
            dAL = _dal;
            acm = _acm;
            env = _env;
        }

        public IActionResult ListOfContractors()
        {
            return View();
        }

        public IActionResult ListOfContractorsNew()
        {
            return View();
        }

        public IActionResult ListContractorsRenewal()
        {
            return View();
        }
        public IActionResult DownloadDocument()
        {
           
            return View();
        }

        [Authorize(Roles = "admin,gm")]
        [HttpGet]
        public async Task<IActionResult> GetListOfRenewalContractors()
        {
            var data = (dynamic)null;
            try
            {
                data = await dAL.QueryAsync("GetListOfRenewalContractors", null);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        [Authorize (Roles ="admin,gm") ]
        [HttpGet]
        public async Task<IActionResult> GetListOfContractors()
        {
            var data = (dynamic)null;
            try
            {                
                data = await dAL.QueryAsync("GetContractorList1", null) ;
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }


        [Authorize(Roles = "admin,gm")]
        [HttpGet]
        public async Task<IActionResult> GetListOfContractorsNew()
        {
            var data = (dynamic)null;
            try
            {
                data = await dAL.QueryAsync("GetContractorListNew", null);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        [Authorize(Roles = "admin,gm")]
        [HttpGet]
        public async Task<IActionResult> GetDetailsOfContractors(string ID)
        {
            var data = (dynamic)null;
            try
            {
                var param = new {Id=ID };
                data = await dAL.QueryAsync("GetContractorDetails", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        [Authorize(Roles = "admin,gm")]
        [HttpGet]
        public async Task<IActionResult> GetDetailsOfContractorsNew(string ID)
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = ID };
                data = await dAL.QueryAsync("GetContractorDetailsNew", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }
        [Authorize(Roles = "admin,gm")]
        [HttpGet]
        public async Task<IActionResult> RemoveContractorRenewal(string ID)
        {
            dynamic msg = (null);
            try
            {
                var param = new { Id = ID };
                msg = DB_Conn.CUDProcedureExecute("RemoveContractorRenewal", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(msg);
        }
        [Authorize(Roles = "admin,gm")]
        [HttpGet]
        public async Task<IActionResult> RemoveContractor(string ID)
        {
            dynamic msg = (null);
            try
            {
                var param = new { Id = ID };
                msg = DB_Conn.CUDProcedureExecute("RemoveContractor", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(msg);
        }
        [Authorize(Roles = "admin,gm")]
        [HttpGet]
        public async Task<IActionResult> RemoveContractorNew(string ID)
        {
            dynamic msg = (null);
            try
            {
                var param = new { Id = ID };
                msg = DB_Conn.CUDProcedureExecute("RemoveContractorNew", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(msg);
        }

        public ActionResult Download(string Mobile)
        {
            FileDownloads obj = new FileDownloads();
            string fileSavePath = Path.Combine(env.WebRootPath, "Upload/ContractorRenewal/" + Mobile);
            var filesCol = obj.GetFile(fileSavePath).ToList();
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    for (int i = 0; i < filesCol.Count; i++)
                    {
                        ziparchive.CreateEntryFromFile(filesCol[i].FilePath, filesCol[i].FileName);
                    }
                }
                return File(memoryStream.ToArray(), "application/zip", Mobile+".zip");
            }
        }
        public class FileInfo
        {
            public int FileId
            {
                get;
                set;
            }
            public string FileName
            {
                get;
                set;
            }
            public string FilePath
            {
                get;
                set;
            }
        }
        public class FileDownloads
        {
            public List<FileInfo> GetFile(string fileSavePath)
            {
                List<FileInfo> listFiles = new List<FileInfo>();
                //Path For download From Network Path.  
                //string fileSavePath = Path.Combine(env.WebRootPath, "Upload/ContractorRenewal/" + "9389742234");
                DirectoryInfo dirInfo = new DirectoryInfo(fileSavePath);
                int i = 0;
                foreach (var item in dirInfo.GetFiles())
                {
                    listFiles.Add(new FileInfo()
                    {
                        FileId = i + 1,
                        FileName = item.Name,
                        FilePath = dirInfo.FullName + @"\" + item.Name  
                    });
                    i = i + 1;
                }
                return listFiles;
            }
        }
    }
}
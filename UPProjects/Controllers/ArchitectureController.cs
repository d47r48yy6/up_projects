using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UPProjects.Models;

namespace UPProjects.Controllers
{
    public class ArchitectureController : Controller
    {
        private readonly DAL dAL;
        private readonly AppCommonMethod acm;
        public ArchitectureController(DAL _dal , AppCommonMethod _acm) {
            dAL = _dal;
            acm = _acm;
        }
        public IActionResult ListOfArchitectures()
        {
            return View();
        }
        [Authorize(Roles = "admin,gm")]
        [HttpGet]
        public async Task<IActionResult> GetListOfArchitectures()
        {
            var data = (dynamic)null;
            try
            {
                data = await dAL.QueryAsync("GetArchitectureDetails", null);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }
        [Authorize(Roles = "admin,gm")]
        [HttpGet]
        public async Task<IActionResult> GetDetailsOfArchitecture(string ID)
        {
            var data = (dynamic)null;
            try
            {
                var param = new { Id = ID };
                data = await dAL.QueryAsync("GetArchitectureDetailsByID", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(data);
        }

        [Authorize(Roles = "admin,gm")]
        [HttpGet]
        public async Task<IActionResult> RemoveArchitecture(string ID)
        {
            dynamic msg = (null);
            try
            {
                var param = new { Id = ID };
                msg = DB_Conn.CUDProcedureExecute("RemoveArchitecture", param);
            }
            catch (Exception ex)
            {
                await acm.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return Json(msg);
        }
    }
}
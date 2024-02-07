using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.HttpOverrides;
using Org.BouncyCastle.Asn1.Ocsp;

namespace UPProjects.Models
{
    public class ProjectMaster
    {
        DB_Conn c1 = new DB_Conn();
        public int Id { get; set; }
        [DisplayName("Project Title (in English)")]
        [StringLength(100, ErrorMessage = "Please enter Project Title.", MinimumLength = 1)]
        public string ProjectTitle { get; set; }
        [DisplayName("Project Description (in English)")]
        [StringLength(500, ErrorMessage = "Please enter Project Description.", MinimumLength = 1)]
        public string ProjectDescription { get; set; }

        [DisplayName("Project Title (in Hindi)")]
        [StringLength(100, ErrorMessage = "Please enter Project Title.", MinimumLength = 1)]
        public string HNProjectTitle { get; set; }
        [DisplayName("Project Description (in HIndi)")]
        [StringLength(500, ErrorMessage = "Please enter Project Description.", MinimumLength = 1)]
        public string HNProjectDescription { get; set; }
        [DisplayName("Start Year")]
        public string StartYear { get; set; }
        [DisplayName("Start Month")]
        public string StartMonth { get; set; }
        [DisplayName("Project Title")]
        public string UserId { get; set; }
        public string UserIP { get; set; }
        [DisplayName("Department")]
        public int DepartmentId { get; set; }
        [DisplayName("Unit")]
        public int UnitId { get; set; }
        public string DepartmentName { get; set; }
        public string UnitName { get; set; }

        //////////////////////////////////////////////////////////////////////////////////////Insert Project 
        public dynamic InsertProjectMaster(ProjectMaster pt,string userid)
        {
            string msg = "NA";
            var result = (dynamic)null;
            var mon = "";
            var yea = "";
            if(pt.StartMonth=="0")
            {
                mon = null;
            }
            else
            {
                mon = pt.StartMonth;
            }
            if (pt.StartYear == "0")
            {
                yea = null;
            }
            else
            {
                yea = pt.StartYear;
            }
            var param = new { Id = pt.Id, ProjectTitle = pt.ProjectTitle, ProjectDescription = pt.ProjectDescription,
                HNProjectTitle=pt.HNProjectTitle,
                HNProjectDescription=pt.HNProjectDescription,

                StartYear =yea,
                StartMonth =mon, UserId = userid, UserIP = AppCommonMethod.GetIP(),
                DepartmentId = pt.DepartmentId, UnitId = pt.UnitId };
            try
            {
                using (var conn = new SqlConnection(c1.GetConnection()))
                {
                    result = conn.QueryFirstOrDefault("[InsertProject]", param, commandType: CommandType.StoredProcedure);
                    msg = result.Message;


                }
            }
            catch (Exception ex)
            {
                if (pt.Id == 0)
                    msg = "Project not submitted.";
                else
                    msg = "Project not updated.";


            }
            finally
            {

            }
            return msg;
        }

        //////////////////////////////////////////////////////////////////////////////////////Get All Project List 
        public List<ProjectMaster> GetAllProjectList(Object unitparam)
        {
            List<ProjectMaster> result = new List<ProjectMaster>();
            using (var conn = new SqlConnection(c1.GetConnection()))
            {
                result = conn.Query<ProjectMaster>("[GetAllProject]", unitparam, commandType: CommandType.StoredProcedure).ToList();
            }
            return result;
        }
        //////////////////////////////////////////////////////////////////////////////////////Get  Project By Id 
        public ProjectMaster GetProjectById(int Id)
        {
            List<ProjectMaster> result = new List<ProjectMaster>();
            ProjectMaster result1 = new ProjectMaster();
            var param = new { Id = Id };
            using (var conn = new SqlConnection(c1.GetConnection()))
            {
                result = conn.Query<ProjectMaster>("[GetProjectById]", param, commandType: CommandType.StoredProcedure).ToList();
                result1 = result.FirstOrDefault();
            }
            return result1;
        }
        /////////////////////////////////////////////////////////////////////////////////////////Delete Project
        public string DeleteProjects(int Id,string Userid)
        {
            string result = "NA";
            var param = new { Id = Id, UserId=Userid, UserIP=AppCommonMethod.GetIP() };
            using (var conn = new SqlConnection(c1.GetConnection()))
            {
                try
                {
                    conn.Execute("[DeleteProject]", param, commandType: CommandType.StoredProcedure);
                    result = "Project has been deleted.";
                }
                catch
                {
                    result = "Project not deleted.";
                }


            }
            return result;
        }

        public List<ProgressMonth> GetProgressMonth(string y)
        {
            List<ProgressMonth> result = new List<ProgressMonth>();
            var param = new { Year =y};
            using (var conn = new SqlConnection(c1.GetConnection()))
            {
                result = conn.Query<ProgressMonth>("[GetProgressMonth]", param, commandType: CommandType.StoredProcedure).ToList();
            }
            return result;
        }
        public List<ProgressMonth> GetProgressMonth()
        {
            List<ProgressMonth> result = new List<ProgressMonth>();
            var param = new { Year =DateTime.Now.Year };
            using (var conn = new SqlConnection(c1.GetConnection()))
            {
                result = conn.Query<ProgressMonth>("[GetProgressMonth]", param, commandType: CommandType.StoredProcedure).ToList();
            }
            return result;
        }
        public List<ProgressYear> GetProgressYear()
        {
            List<ProgressYear> result = new List<ProgressYear>();
            using (var conn = new SqlConnection(c1.GetConnection()))
            {
                result = conn.Query<ProgressYear>("[GetProgressYearNew]", null, commandType: CommandType.StoredProcedure).ToList();
            }
            return result;
        }
        public List<ProgressYear> GetProgressYearNPR()
        {
            List<ProgressYear> result = new List<ProgressYear>();
            using (var conn = new SqlConnection(c1.GetConnection()))
            {
                result = conn.Query<ProgressYear>("[GetProgressYearNewNPR]", null, commandType: CommandType.StoredProcedure).ToList();
            }
            return result;
        }



    }
       public class ProgressMonth
        {
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class ProgressYear
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }




}

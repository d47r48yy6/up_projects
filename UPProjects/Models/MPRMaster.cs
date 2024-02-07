using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class MPRMaster
    {
        DB_Conn c1 = new DB_Conn();
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string WorkName { get; set; }
        public string AcceptableMoney { get; set; }
        public decimal? AvmuktMoney { get; set; }
        public decimal? VithyaPragati { get; set; }
        public decimal? BhotikPragati { get; set; }
        public string Remark { get; set; }
        public string UserId { get; set; }
        public string UserIP { get; set; }
        public string DepartmentName { get; set; }
        public string StartMonth { get; set; }
        public string StartYear { get; set; }

        //////////////////////////////////////////////////////////////////////////////////////Insert MPR Master 
        public dynamic InsertMPRMaster(MPRMaster pt,string userId,string unitid)
        {
            string msg = "NA";
            var result = (dynamic)null;
            var param = new
            {
                Id = pt.Id,
                StartMonth=pt.StartMonth,
                StartYear=pt.StartYear,
                DepartmentId =pt.DepartmentId,
                WorkName=pt.WorkName,
                AcceptableMoney=pt.AcceptableMoney,
                AvmuktMoney=pt.AvmuktMoney,
                VithyaPragati=pt.VithyaPragati,
                BhotikPragati=pt.BhotikPragati,
                Remark=pt.Remark,
                UserId= userId,
                UserIP= AppCommonMethod.GetIP(),
                UnitId=unitid


            };
            try
            {
                using (var conn = new SqlConnection(c1.GetConnection()))
                {
                    result = conn.QueryFirstOrDefault("[InsertMPRMaster]", param, commandType: CommandType.StoredProcedure);
                    msg = result.Msg;


                }
            }
            catch (Exception ex)
            {
                if (pt.Id == 0)
                {
                    msg = "MPR not Created.";
                }
                else
                {
                    msg = "MPR not Updated.";
                }


            }
            finally
            {

            }
            return msg;
        }
        //////////////////////////////////////////////////////////////////////////////////////Get All MPR Master List 
        public List<MPRMaster> GetAllMPRList()
        {
            List<MPRMaster> result = new List<MPRMaster>();
            using (var conn = new SqlConnection(c1.GetConnection()))
            {
                result = conn.Query<MPRMaster>("[GetAllMPRMaster]", null, commandType: CommandType.StoredProcedure).ToList();
            }
            return result;
        }
        //////////////////////////////////////////////////////////////////////////////////////Get  MPR By Id 
        public MPRMaster GetMPRMaterById(int Id)
        {
            List<MPRMaster> result = new List<MPRMaster>();
            MPRMaster result1 = new MPRMaster();
            var param = new { Id = Id };
            using (var conn = new SqlConnection(c1.GetConnection()))
            {
                result = conn.Query<MPRMaster>("[GetMPRMasterById]", param, commandType: CommandType.StoredProcedure).ToList();
                result1 = result.FirstOrDefault();
            }
            return result1;
        }

        /////////////////////////////////////////////////////////////////////////////////////////Delete MPR
        public string DeleteMPRMaster(int Id, string userId)
        {
            string result = "NA";
            var param = new { Id = Id, UserId=userId, UserIP=AppCommonMethod.GetIP()};
            using (var conn = new SqlConnection(c1.GetConnection()))
            {
                try
                {
                    conn.Execute("[DeleteMPRMaster]", param, commandType: CommandType.StoredProcedure);
                    result = "MPR Details has been deleted.";
                }
                catch
                {
                    result = "MPR Details not deleted.";
                }


            }
            return result;
        }

        /////////////////////////////////////////////////////////////////////////////////////////Get Zone
        public List<Zone> GetAllZone()
        {
            List<Zone> result = new List<Zone>();
            using (var conn = new SqlConnection(c1.GetConnection()))
            {
                result = conn.Query<Zone>("[GetZone]", null, commandType: CommandType.StoredProcedure).ToList();
            }
            return result;
        }

        /////////////////////////////////////////////////////////////////////////////////////////Get Zone
        public List<UnitByZone> GetUnitsByZone(int Zone)
        {
            List<UnitByZone> result = new List<UnitByZone>();
            var param =new { ZoneId = Zone };
            using (var conn = new SqlConnection(c1.GetConnection()))
            {
                result = conn.Query<UnitByZone>("[GetUnitsByZone]", param, commandType: CommandType.StoredProcedure).ToList();
            }
            return result;
        }

        /////////////////////////////////////////////////////////////////////////////////////////Get Zone
        public List<MPRMaster> GetAllReportByUnit(string UnitId)
        {
            List<MPRMaster> result = new List<MPRMaster>();
            var param = new { UnitId = UnitId };
            using (var conn = new SqlConnection(c1.GetConnection()))
            {
                result = conn.Query<MPRMaster>("[GetAllMPRReportByUnit]", param, commandType: CommandType.StoredProcedure).ToList();
            }
            return result;
        }
    }
    public class Zone
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class UnitByZone
    {
        public int Id { get; set; }
        public string UnitName { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class AppCommonMethod
    {
        private readonly DAL dAL;
        private readonly IActionContextAccessor _accessor;
        public AppCommonMethod()
        {
            
        }
        public AppCommonMethod(DAL _dal, IActionContextAccessor accessor)
        {
            dAL = _dal;
            _accessor = accessor;
        }
        public  string GenerateOTP(int NoOfDigit)
        {
            try
            {
                var chars = "0123456789";
                var stringChars = new char[NoOfDigit];
                var random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var finalString = new String(stringChars);
                return finalString.ToLower();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task InsertException(string ExceptionMsg, string ControllerName, string ActionName, string OtherRemark, string CreatedBy)
        {
         
            var param = new { ExceptionMsg = ExceptionMsg, ControllerName = ControllerName, ActionName = ActionName, Other = OtherRemark, CreatedBy = CreatedBy };
            await dAL.ExecuteAsync("CreateExceptionLog", param);
        }
        public void InsertExceptionsync(string ExceptionMsg, string ControllerName, string ActionName, string OtherRemark, string CreatedBy)
        {
            
            var param = new { ExceptionMsg = ExceptionMsg, ControllerName = ControllerName, ActionName = ActionName, Other = OtherRemark, CreatedBy = CreatedBy };
             dAL.Executesync("CreateExceptionLog", param);
        }

        public static string GetIP()
        {
            string ipaddress = "";
            try
            {               

                //System.Text.ASCIIEncoding ASCII = new System.Text.ASCIIEncoding();
                //string server = null;
                //server = Dns.GetHostName();

                //IPHostEntry heserver = Dns.GetHostEntry(server);
                //ipaddress = heserver.AddressList[1].ToString();


            }
            catch (Exception e)
            {
                Console.WriteLine("[DoResolve] Exception: " + e.ToString());
            }
            return ipaddress;
        }
        public string EncryptMD5(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));
            int Len = bytes.Length;
            for (int i = 0; i < Len; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public string CheckZero(string val)
        { 
            return val=="0"? null :  val;
        }

       
    }
}

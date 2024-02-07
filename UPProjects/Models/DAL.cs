using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using UPProjects.Entities;

namespace UPProjects.Models
{

    public class DAL
    {
        private readonly string _connection;

        //public DAL()
        //{
        //}

        public  DAL(IConfiguration configuration) {
            _connection = configuration.GetConnectionString("DefaultConnection");
        }
        // Common Method to Bind Dropdown/Select
        public async Task<List<CommonSelect>> BindSelect(string StoredProc,object parameter)
        {
            var Res = (dynamic)null;
            using (var con = new SqlConnection(_connection))
            {
                await con.OpenAsync();
                Res =  await con.QueryAsync<CommonSelect>(StoredProc, parameter, null, null, commandType: CommandType.StoredProcedure);
                await con.CloseAsync();
            }
            return Res;
        }
        // Common method to execute Proc
        public async Task<string> ExecuteAsync(string StoredProc, object parameter)
        {
                int Res = 0;
            try
            {

                using (var con = new SqlConnection(_connection))
                {
                    await con.OpenAsync();
                    Res = await con.ExecuteAsync(StoredProc, parameter, null, null, commandType: CommandType.StoredProcedure);
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (Res > 0)
                return "Success";
            else
            return "Failed";
        }
        // Common method to execute Proc sync
        public string Executesync(string StoredProc, object parameter)
        {
            int Res = 0;
            try
            {

                using (var con = new SqlConnection(_connection))
                {
                     con.Open();
                     Res =  con.Execute(StoredProc, parameter, null, null, commandType: CommandType.StoredProcedure);
                     con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (Res > 0)
                return "Success";
            else
                return "Failed";
        }

        // Common method to query from Proc
        public async Task<dynamic> QueryAsync(string StoredProc, object parameter)
        {
            var Result = (dynamic)null;
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    await con.OpenAsync();
                    Result = await con.QueryAsync(StoredProc, parameter, null, commandTimeout: 10, commandType: CommandType.StoredProcedure);
                    await con.CloseAsync();
                }
                return Result;
            }
            catch( Exception ex)
            {
                throw ex;
            }
        }
       // common method to query with execute 
        public object QueryWithExecuteAsync(string StoredProc, object parameter)
        {
            var Result = (dynamic)null;
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    con.Open();
                    Result =  con.Query(StoredProc, parameter, null, commandType: CommandType.StoredProcedure).FirstOrDefault();
                     con.Close();
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public async Task<List<T>> SelectProcedureExecute<T>(string proc, object param)
        {
            var result = (dynamic)null;         
            try
            {
                using (var conn = new SqlConnection(_connection))
                {
                    result = await conn.QueryAsync<T>(proc, param, commandType: System.Data.CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
              
            }
            return result;
        }
        public Tender GetTenderDetails(string StoredProc, object parameter)
        {
            Tender objtender = new Tender();
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    con.OpenAsync();
                    objtender = con.Query<Tender>(StoredProc, parameter, null, commandTimeout: 10, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    con.CloseAsync();
                }
            }
            catch (Exception ex)
            { throw ex; }
            return objtender;
        }
        public List<Tender> GetTenderList(string StoredProc, object parameter)
        {
            List<Tender> objtender = new List<Tender>();
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    con.Open();
                    objtender = con.Query<Tender>(StoredProc, parameter, null, commandTimeout: 10, commandType: CommandType.StoredProcedure).ToList();
                    con.Close();
                }
            }
            catch (Exception ex)
            { throw ex; }
            return objtender;
        }


        public ProgressPhotoUpload GetProgressPhotoDetails(string StoredProc, object parameter)
        {
            ProgressPhotoUpload objtender = new ProgressPhotoUpload();
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    con.OpenAsync();
                    objtender = con.Query<ProgressPhotoUpload>(StoredProc, parameter, null, commandTimeout: 10, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    con.CloseAsync();
                }
            }
            catch (Exception ex)
            { throw ex; }
            return objtender;
        }



        ////// Photo Gallery//////////////////////////////////////////////////////////////
        public PhotoGallery GetPhotoGalleryEdit(string StoredProc, object parameter)
        {
            PhotoGallery objtender = new PhotoGallery();
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    con.OpenAsync();
                    objtender = con.Query<PhotoGallery>(StoredProc, parameter, null, commandTimeout: 10, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    con.CloseAsync();
                }
            }
            catch (Exception ex)
            { throw ex; }
            return objtender;
        }

        public NamamiGange GetNamamiGangePhotoEdit(string StoredProc, object parameter)
        {
            NamamiGange objtender = new NamamiGange();
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    con.OpenAsync();
                    objtender = con.Query<NamamiGange>(StoredProc, parameter, null, commandTimeout: 10, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    con.CloseAsync();
                }
            }
            catch (Exception ex)
            { throw ex; }
            return objtender;
        }

        public List<ProjectDetails> GetProjectList (string StoredProc, object parameter)
        {
            List<ProjectDetails> objtender = new List<ProjectDetails>();
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    con.Open();
                    objtender = con.Query<ProjectDetails>(StoredProc, parameter, null, commandTimeout: 10, commandType: CommandType.StoredProcedure).ToList();
                    con.Close();
                }
            }
            catch (Exception ex)
            { throw ex; }
            return objtender;
        }

        public T SelectModel<T>(string proc, object param)
        {
            var result = (dynamic)null;
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    con.Open();
                    result = con.Query<T>(proc, param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return result;
        }
        public List<T> SelectModelList<T>(string proc, object param)
        {
            var result = (dynamic)null;
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    con.Open();
                    result = con.Query<T>(proc, param, commandType: System.Data.CommandType.StoredProcedure);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return result;
        }

        public object Query(string commandString, CommandType commandType, object parameters)
        {
            var result = (dynamic)null;
            try
            {
                using (IDbConnection db = new SqlConnection(_connection))
                {
                    db.Open();
                    result = db.Query(commandString, parameters, commandType: commandType, commandTimeout: 120).ToList();
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
     
    }
   
}

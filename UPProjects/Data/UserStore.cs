using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UPProjects.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace UPProjects.Data
{
    public class UserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserRoleStore<ApplicationUser>,
        IUserClaimStore<ApplicationUser>, IUserEmailStore<ApplicationUser>
    {
        private readonly string _connectionString;
        public UserStore(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public string  UpdateLoginInfo(string UserId)
        {
            var res = (dynamic)null;
            var param = new
            {
                UserId = UserId,
                IpAddress = AppCommonMethod.GetIP()
            };
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                res = con.Query("UpdateLoginInfo", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
            }
            return res.Role;
        }

        public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return  null;
        }
        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return null;
        }

        public void Dispose()
        {
       
        }

        public Task<ApplicationUser> FindByIdAsync(string  userId, CancellationToken cancellationToken)
        {
            return null;
        }
        public async Task<ApplicationUser> FindByNameAsync(string Id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ApplicationUser>("GetLoginDetailByUserId", new { UserId = Id }, commandType: CommandType.StoredProcedure);
            }
        }
        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserId.ToString());
        }
        
        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserId.ToString());
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user,string normalizedName, CancellationToken cancellationToken)
        {
            user.UserId = normalizedName;
            return Task.FromResult(0);
        }
        public Task SetUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.UserId = normalizedName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();


            return IdentityResult.Success;
        }
        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.Password = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Password != null);
        }

        public Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(roleName);
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            IList<string> roles = new List<string>();
            roles.Add(user.Role);
            return Task.FromResult(roles);
        }

        public Task<IList<Claim>> GetClaimsAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            IList<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Id", user.Id));
            claims.Add(new Claim("UserId", user.UserId));
            claims.Add(new Claim("EmployeeId", user.EmployeeId));
            claims.Add(new Claim("MobileNo", user.MobileNo));
            claims.Add(new Claim("UserName", user.UserName));
            claims.Add(new Claim("OfficeId", user.OfficeId));
            claims.Add(new Claim("Role", user.Role));
            claims.Add(new Claim("LastLoginDateTime", user.LastLoginDateTime));
            claims.Add(new Claim("UnitId", user.UnitId));
            claims.Add(new Claim("ZoneId", user.ZoneId));
            return Task.FromResult(claims);
        }
        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

      

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            IList<ApplicationUser> users = new List<ApplicationUser>();
            return Task.FromResult(users);
        }

        

        public Task AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return Task.FromResult(claims);
        }

        public Task ReplaceClaimAsync(ApplicationUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public Task RemoveClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public Task<IList<ApplicationUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public object ChangePassword(ChangePasswordViewModel ChangePassword, string User)
        {
            var res = (dynamic)null;
            var param = new
            {
                CurrentPassword = MD5Encryption.getMd5Hash(ChangePassword.CurrentPassword),
                NewPassword = MD5Encryption.getMd5Hash(ChangePassword.NewPassword),
                UserId = User,
                IPAddress = AppCommonMethod.GetIP()
            };
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                res = con.Query("ChangePassword", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
            }
            return res;
        }

        public object UpdateUserProfile(UserProfileEdit usprof)
        {
            var res = (dynamic)null;
            var param = new
            {
                UserId = usprof.UserId,
                Name = usprof.Name,
                Mobile = usprof.Mobile,
                Email = usprof.Email,
                IPAddress = AppCommonMethod.GetIP()
            };
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                res = con.Query("UpdateProfile", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
            }
            return res;
        }
        public UserProfileEdit GetUserProfileEdit(string User)
        {
            UserProfileEdit us = new UserProfileEdit();
            var param = new
            {
                UserId = User
            };
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                us = con.Query<UserProfileEdit>("GetProfileInfo", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
            }
            return us;
        }
    }
    public class RoleStore : IRoleStore<ApplicationRole>
    {
        private readonly string _connectionString;

        public RoleStore(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }



        public async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    await connection.OpenAsync(cancellationToken);
            //    role.Id = await connection.QuerySingleAsync<int>($@"INSERT INTO [ApplicationRole] ([Name], [NormalizedName])
            //    VALUES (@{nameof(ApplicationRole.Name)}, @{nameof(ApplicationRole.NormalizedName)});
            //    SELECT CAST(SCOPE_IDENTITY() as int)", role);
            //}

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    await connection.OpenAsync(cancellationToken);
            //    await connection.ExecuteAsync($@"UPDATE [ApplicationRole] SET
            //    [Name] = @{nameof(ApplicationRole.Name)},
            //    [NormalizedName] = @{nameof(ApplicationRole.NormalizedName)}
            //    WHERE [Id] = @{nameof(ApplicationRole.Id)}", role);
            //}

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    await connection.OpenAsync(cancellationToken);
            //    await connection.ExecuteAsync($"DELETE FROM [ApplicationRole] WHERE [Id] = @{nameof(ApplicationRole.Id)}", role);
            //}

            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        public async Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ApplicationRole>($@"SELECT * FROM [ApplicationRole]
                WHERE [Id] = @{nameof(roleId)}", new { roleId });
            }
        }

        public async Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ApplicationRole>($@"SELECT * FROM [ApplicationRole]
                WHERE [NormalizedName] = @{nameof(normalizedRoleName)}", new { normalizedRoleName });
            }
        }


        public void Dispose()
        {
            // Nothing to dispose.
        }


       
    }
    //public class RoleStore : IRoleStore<ApplicationRole>
    //{
    //    private readonly string _connectionString;

    //    public RoleStore(IConfiguration configuration)
    //    {
    //        _connectionString = configuration.GetConnectionString("DefaultConnection");
    //    }



    //    public async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
    //    {
    //        return null;
    //    }

    //    public async Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
    //    {
    //        return null;
    //    }

    //    public async Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
    //    {
    //        return null;
    //    }

    //    public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
    //    {
    //        return Task.FromResult(role.Id.ToString());
    //    }

    //    public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
    //    {
    //        return Task.FromResult(role.Name);
    //    }

    //    public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
    //    {
    //        role.Name = roleName;
    //        return Task.FromResult(0);
    //    }

    //    public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
    //    {
    //        return Task.FromResult(role.NormalizedName);
    //    }

    //    public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
    //    {
    //        role.NormalizedName = normalizedName;
    //        return Task.FromResult(0);
    //    }

    //    public async Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    //    {
    //        return null;
    //    }

    //    public async Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    //    {
    //        return null;
    //    }


    //    public void Dispose()
    //    {
    //        // Nothing to dispose.
    //    }



    //}

  
}

using System;

using System.Text;

using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

using Microsoft.IdentityModel.Tokens;
using PTPL_eMandi.Models;
using UPProjects.Data;

using UPProjects.Models;
using UPProjects.Services;

namespace UPProjects
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static string ConnectionString { get; private set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();
            services.AddMvc().AddNewtonsoftJson();
            //services.AddNewtonsoftJson();
            //.ConfigureApiBehaviorOptions(options =>
            //{
            //    options.SuppressModelStateInvalidFilter = true;
            //}); 
            services.AddRazorPages();
            services.AddScoped<UserStore>();
            //////// DAL Services
            services.AddSingleton<DAL>();
            services.AddSingleton<DB_Conn>();
            services.AddSingleton<MPRDAL>();
            services.AddSingleton<AppCommonMethod>();
            services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, RoleStore>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddIdentity<ApplicationUser, ApplicationRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
                services.AddMvc(options =>
                {
                    options.Filters.Add<OperationCancelledExceptionFilter>();
                }).AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.PropertyNamingPolicy = null;
                    o.JsonSerializerOptions.DictionaryKeyPolicy = null;
                });
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.Name = "id4578widiwjfffffmpu";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Account/index";
                options.LogoutPath = "/Account/LogOut";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(600);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddDNTCaptcha(options =>
      options.UseCookieStorageProvider()
          .ShowThousandsSeparators(false)
  );
            services.AddRazorPages().AddRazorRuntimeCompilation();
            ConnectionString = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddCors();
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                                .AddCookie(x =>
                                {
                                    x.LoginPath = "/";
                                    x.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                                })
                                .AddJwtBearer(x =>
                                {
                                    x.RequireHttpsMetadata = false;
                                    x.SaveToken = true;
                                    x.TokenValidationParameters = new TokenValidationParameters
                                    {
                                        ValidateIssuerSigningKey = true,
                                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConfigurationManager.AppSetting["AppSettings:Secret"])),
                                        ValidateIssuer = false,
                                        ValidateAudience = false
                                    };
                                });


            services.AddScoped<IUserServices, UserServices>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStatusCodePagesWithRedirects("/Home/Error");
            app.UseHttpsRedirection();
           // app.UseStaticFiles();
            FileExtensionContentTypeProvider contentTypes = new FileExtensionContentTypeProvider();
            contentTypes.Mappings[".apk"] = "application/vnd.android.package-archive";
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = contentTypes
            });

            app.UseRouting();

            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "areaRoute",
                                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"); endpoints.MapRazorPages();
            });
        }
    }
}

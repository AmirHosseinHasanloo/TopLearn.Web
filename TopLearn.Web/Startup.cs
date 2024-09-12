using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TopLearn.Core.Services;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.Core.Convertors;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using Elmah.Io.AspNetCore;

namespace TopLearn.Web
{
    public class Startup
    {
        public IConfiguration _configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddHttpContextAccessor();


            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 524288000;
            });

            #region Context

            services.AddDbContext<TopLearnContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("TopLearnConnection"));
            });


            #endregion

            #region IoC

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPermissionServices, PermissionServices>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IUserPanelService, UserPanelService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IViewRenderService, RenderViewToString>();

            #endregion

            #region Authentication

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
            });

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region Controll 404 page

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404)
                {
                    context.Response.Redirect("/Home/Error404");
                }
            });

            #endregion

            #region Check User Can Download Course Or not

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.ToString().ToLower().StartsWith("/courseimages"))
                {

                    var callingUrl = context.Request.Headers["referer"].ToString();

                    if (callingUrl != "" && (callingUrl.StartsWith("https://localhost:4001") || callingUrl.StartsWith("http://localhost:4001")))
                    {
                        await next.Invoke();
                    }
                    else
                    {
                        context.Response.Redirect("/Login");
                    }
                }
                else
                {
                    await next.Invoke();
                }
            });

            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                      name: "areas",
                      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );
                });


                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Helpers;
using Application.Services;
using Application.Services.Interface;
using Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.DependencyInjection;
using Tool.Configurations;
using Tool.Utilities;

namespace Application
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        private readonly IHostingEnvironment Environment;

        public Startup(IConfiguration configuration,
                       IHostingEnvironment environment)
        {
            Environment = environment;
            Configuration = Util.GetConfiguration("Application", "appconfig.json");
        }

        public void ConfigureServices(IServiceCollection services)
        {

#if !DEBUG

            var keysFolder = Path.Combine(Environment.ContentRootPath, "Keys");

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
                .SetApplicationName("MyWebsite")
                .SetDefaultKeyLifetime(TimeSpan.FromDays(90));

#endif

            services.AddCors();

            services.AddScoped<IViewRenderService, ViewRenderService>();

            #region database

            services.ApplicationServicesIoC();
            services.InfrastructureORM<InfrastructureOrm>();

            #endregion

            #region route

            services.AddRouting(options => options.LowercaseUrls = true);

            #endregion

            #region context

            //services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            #endregion

            #region culture

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("pt-BR"),
                };

                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.DefaultRequestCulture = new RequestCulture(supportedCultures[0]);
            });

            #endregion

            #region session

            services.Configure<CookiePolicyOptions>(options =>
            {
                //options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                //options.Cookie.IsEssential = true;
                //options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.Name = AppConfig.Session.User;
            });

            #endregion

            #region recaptcha

            //services.AddRecaptcha(_configuration.GetSection("service:recaptcha"));

            #endregion

            //#region authentication

            //services.AddIdentity<UserSession, IdentityRole>()
            //        .AddEntityFrameworkStores<DBContext>()
            //        .AddDefaultTokenProviders();

            //#endregion

            #region mvc

            services.AddMvc();
            //.AddRazorRuntimeCompilation();
            //.AddDataAnnotationsLocalization()
            //.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
            //.AddSessionStateTempDataProvider()

            #endregion

            services.AddAntiforgery();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            AppContextHelper.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseHsts();
            }

            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            //});

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseCors();

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseCookiePolicy();
            app.UseSession();

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute("default", "{controller=Login}/{action=Index}/{id?}");
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}

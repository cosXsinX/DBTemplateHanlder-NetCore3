using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DBTemplateHandler.Studio.Data;
using DBTemplateHandler.Studio.Deployment;
using System.IO;
using DBTemplateHandler.Studio.Controllers;

namespace DBTemplateHandler.Studio
{
    public class Startup
    {
        private static string ApplicationRomingFolderName = string.Join(".", nameof(DBTemplateHandler), nameof(DBTemplateHandler.Studio));

        Deployer deployer = new Deployer();
        private readonly string DeploymentHistoryFolderPath = 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                ApplicationRomingFolderName,
               "deploymentInfos");
        private Persistance.PersistenceFacadeConfiguration persistenceConfig =
                new Persistance.PersistenceFacadeConfiguration()
                {
                    TemplatesFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        ApplicationRomingFolderName,
                        "templates"),
                    DatabaseModelsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        ApplicationRomingFolderName,
                        "databaseModels"),
                };
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            deployer.Deploy(new DeploymentConfiguration()
            {
                ForceReDeploy = true,
                DeploymentHistoryFolderPath = DeploymentHistoryFolderPath,
                DeploymentTemplateFolderPath = persistenceConfig.TemplatesFolderPath
            });
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var exportService = new ExportService(new ExportService.Config
            {
                ReadyDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        ApplicationRomingFolderName,
                        "exportReady"),
                WorkingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        ApplicationRomingFolderName,
                        "exportPrepare"),
            });

            var dbTemplateService = new DBTemplateService(new DBTemplateService.Config()
            {
                PersistenceConfig = persistenceConfig
            });

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
            services.AddSingleton<DBTemplateService>(dbTemplateService);
            services.AddSingleton<ExportService>(exportService);
            services.AddSingleton<ExportController>(new ExportController(dbTemplateService, exportService));
            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Export}/{action=Export}/{id?}");
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}

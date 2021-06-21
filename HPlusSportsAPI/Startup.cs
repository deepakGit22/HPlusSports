using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace HPlusSportsAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            IConfiguration dbconfig = Configuration.GetSection(Constants.KEY_DB_CONFIG);
            services.Configure<Services.CosmosDBServiceOptions>(dbconfig);

            var docClient = new DocumentClient(
                new Uri(Configuration[Constants.KEY_COSMOS_URI]), Configuration[Constants.KEY_COSMOS_KEY]);
            services.AddSingleton<DocumentClient>(docClient);

            services.AddScoped<Services.IQueueService, Services.AzureQueueService>();
            services.AddScoped<Services.ITableService, Services.AzureTableService>();
            services.AddScoped<Services.IDocumentDBService, Services.CosmosDBService>();
            services.AddScoped<Services.IBlobService, Services.AzureBlobService>();            

            services.AddSwaggerGen( s => 
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Hplus Sports API",
                    Version = "1.0"
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(s => {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Hplus Sports");
            });

            //app.UseHttpsRedirection();
            app.UseMvcWithDefaultRoute();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SinSense.Infastructure;
using SinSense.Infastructure.Services;
using SinSense.Infastructure.Services.External;
using SinSense.Infastructure.Services.NLP;
using SinSense.Infastructure.Services.NLP.Sinhala;
using SinSense.Infastructure.Services.Sinhala;

namespace SinSense.Web
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
            services.AddRazorPages();
            services.AddDbContext<AppDbContext>(options => Configuration.ConfigureDbContext(options, "DefaultDb"));
            services.AddScoped<GoogleTranslateService>();
            services.AddScoped<SinhalaDictionaryService>();
            services.AddScoped<EnglishMorphologyService>();
            services.AddScoped<SinhalaMorphologyService>();
            services.AddScoped<BabelNetService>();
            services.AddScoped<SinhalaDisambiguatorService>();
            services.AddScoped<MaduraDictionaryService>();
            services.AddScoped<SimpleTokenizer>();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}

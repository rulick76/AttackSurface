using AttackSurface.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttackSurface
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

            services.AddControllers();
            services.AddSingleton<IMemoryCache, MemoryCache>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMemoryCache cache)
        {
            LoadCache(cache);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void LoadCache(IMemoryCache cache)
        {
            string json = System.IO.File.ReadAllText(@"Samples\\input-2.json");
            var CloudEnv = Newtonsoft.Json.JsonConvert.DeserializeObject<CloudEnvironment>(json);
            List<string> sources = new List<string>();
            List<string> dest = new List<string>();
            foreach (var fwr in CloudEnv.fw_rules)
            {
                sources.AddRange(CloudEnv.vms.Where(vm => vm.tags.Contains(fwr.source_tag)).Select(src=>src.vm_id).Distinct().ToList());
                dest.AddRange(CloudEnv.vms.Where(vm => vm.tags.Contains(fwr.dest_tag)).Select(src => src.vm_id).ToList());

            }
        }
    }
}

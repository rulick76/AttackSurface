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
            services.AddSingleton<IMemoryCache,MemoryCache>();
            services.AddResponseCaching();
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
            app.UseResponseCaching();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void LoadCache(IMemoryCache cache)
        {
            
            string json = System.IO.File.ReadAllText("Samples/input-2.json");
            var CloudEnv = Newtonsoft.Json.JsonConvert.DeserializeObject<CloudEnvironment>(json);
            Statistics.Instance.Vm_count = CloudEnv.vms.Count();
            List<string> sources = new List<string>();
            List<string> dests = new List<string>();
            foreach (var fwr in CloudEnv.fw_rules)
            {
                sources.AddRange(CloudEnv.vms.Where(vm => vm.tags.Contains(fwr.source_tag)).Select(src=>src.vm_id).ToList());
                dests.AddRange(CloudEnv.vms.Where(vm => vm.tags.Contains(fwr.dest_tag)).Select(src => src.vm_id).ToList());

            }
            sources = sources.Distinct().ToList();
            dests = dests.Distinct().ToList();
            List<string> cacheEntryValues;
            foreach (var dest in dests)
            {
                if (!cache.TryGetValue(dest, out cacheEntryValues))
                {
                    var actualSources = sources.Where(src=>src!=dest);
                    if(actualSources != null && actualSources.Count()>0)
                        cache.Set(dest, actualSources.ToList());
                }
                else
                {
                    cache.TryGetValue(dest, out cacheEntryValues);
                    foreach (var source in sources)
                    {
                        if(!cacheEntryValues.Contains(source))
                        {
                            if(!dest.Equals(source))
                                cacheEntryValues.Add(source);
                        }
                    }
                }
            }
        }
    }
}

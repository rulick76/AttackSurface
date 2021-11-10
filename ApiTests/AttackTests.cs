using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests
{
   
    public class AttackTests
    {
        private readonly HttpClient httpclient;
        public static string BaseAddress = "baseAddress";
        private static IConfigurationRoot Configuration;
        public AttackTests()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback += (a, b, c, d) => true;
            //handler.ServerCertificateCustomValidationCallback += HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            httpclient = new HttpClient(handler);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            httpclient.BaseAddress = new Uri("https://" + Configuration["HostAddress"] + "/api/v1/");
        }

        [Fact]
        public async Task Http_get_attackers()
        {
            var response = await httpclient.GetAsync("attack?vm_id=" + "vm-ab51cba10");
            Assert.True(response.IsSuccessStatusCode, response.StatusCode.ToString());
            string result = await response.Content.ReadAsStringAsync();
            //"userName": "Administrator"
            JObject o = JObject.Parse(result);
            Assert.True(o["userName"].ToString() == "");
        }
    }
}

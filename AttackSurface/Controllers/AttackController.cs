using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using AttackSurface.Models;
using System.Threading.Tasks;
using System.Text.Json;

namespace AttackSurface.Controllers
{
    [Route("api/v1/")]
    public class AttackController : Controller
    {
        private readonly ILogger<AttackController> _logger;
        private readonly IMemoryCache _cache;
        private static readonly object objLock = new object();
       
        
        public AttackController(ILogger<AttackController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        public void Index()
        {
            //
        }

        [ResponseCache(Duration = 0)]
        [HttpGet("attack")]
        public  IActionResult Attack(string vm_id)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            lock (objLock)
            {
                try
                {
                    List<string> attackers;
                    _cache.TryGetValue(vm_id, out attackers);
                    if (attackers != null)
                    {
                        return Ok(Json(attackers).Value);
                    }
                    else
                    {
                        //status code
                        return NotFound();
                    }

                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex.Message);
                    return BadRequest();
                }
                finally
                {

                    stopWatch.Stop();
                    Statistics.Instance.TotalRequestsTime += stopWatch.Elapsed.TotalMilliseconds;
                    ++Statistics.Instance.request_count;
                    Statistics.Instance.average_request_time = (Statistics.Instance.TotalRequestsTime / Statistics.Instance.request_count);
                    stopWatch = null;
                }
            }
        }

        [HttpGet("stats")]
        public IActionResult Stats()
        {
            try
            {
                return Ok(Json(Statistics.Instance).Value);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return BadRequest();
            }
        }
    }
}

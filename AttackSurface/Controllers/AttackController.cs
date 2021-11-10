using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using AttackSurface.Models;
using System.Net.Http;
using System.Threading.Tasks;

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

        [HttpGet("attack")]
        public IActionResult Attack(string vm_id)
        {
           Stopwatch stopWatch = new Stopwatch();
           stopWatch.Start();
            try
            {
                
                lock (objLock)
                {
                    List<string> attackers;
                    _cache.TryGetValue(vm_id, out attackers);
                    if (attackers!=null)
                    {
                       return Ok(attackers);
                    }
                    else
                    {
                        //status code
                        return NotFound();
                    }
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
                ++Statistics.Instance.request_count;
                Statistics.Instance.TotalRequestsTime += stopWatch.Elapsed.TotalMilliseconds;
                Statistics.Instance.average_request_time = (Statistics.Instance.TotalRequestsTime / Statistics.Instance.request_count);
                stopWatch = null;
            }
        }

        [HttpGet("stats")]
        public IActionResult Stats()
        {
            try
            {
                return Ok(Statistics.Instance);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return BadRequest();
            }
        }
    }
}

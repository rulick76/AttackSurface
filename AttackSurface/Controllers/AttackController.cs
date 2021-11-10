using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttackSurface.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class AttackController : ControllerBase
    {
        private readonly ILogger<AttackController> _logger;
        private readonly IMemoryCache _cache;

        public AttackController(ILogger<AttackController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        public void Index()
        {
          //Return 
        }

        [HttpGet("attack")]
        public void Attack(int vm_id)
        {
            //Return 
        }

        [HttpGet("stats")]
        public void Stats()
        {
            //Return 
        }
        //attack?vm_id
    }
}

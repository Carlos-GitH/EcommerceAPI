using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("/teste")]
    public class TesteController : ControllerBase
    {
        [HttpGet("publica")]
        public ActionResult<string> Get()
        {
            return Ok("publica");
        }

        [HttpGet("privada")]
        public ActionResult<string> GetPrivate()
        {
            return Ok("privada");
        }
    }
}
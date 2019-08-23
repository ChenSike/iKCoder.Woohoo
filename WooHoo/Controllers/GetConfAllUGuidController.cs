using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using WooHoo.Base;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetConfAllUGuidController : WHControllerBase
    {
        /*[HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string key)
        {

        }
        */
    }
}
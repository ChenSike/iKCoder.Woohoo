using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WooHoo.Configs;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetSysStatusController : WooHoo.Base.WHControllerBase
    {
        [HttpGet]
        [Filter.Filter_ConnectDB]
        [Filter.Filter_SysAuthor]
        public ActionResult Action()
        {
            Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
            conf_ResponseMessageObj.code = "201";
            conf_ResponseMessageObj.status = "OK";
            conf_ResponseMessageObj.message = "In Service";
            HttpContext.Response.StatusCode = 201;
            return Json(conf_ResponseMessageObj);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WooHoo.Base;
using WooHoo.Filter;
using WooHoo.Orm;
using WooHoo.Configs;
using Dapper;


namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetSysExecuteController : WHControllerBase
    {
        /// <summary>
        /// 系统API：执行所有DB脚本
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        [HttpPost]
        [Filter_SysAuthor]
        [Filter_ConnectDB]
        public ActionResult Action(string script)
        {
            Conf_ResponseMessage conf_ResponseMessageObj;
            try
            {
                dbConnection.Execute(script);
                conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "200";
                conf_ResponseMessageObj.status = "ok";
                conf_ResponseMessageObj.message = "Executed";
                HttpContext.Response.StatusCode = 200;
                return Json(conf_ResponseMessageObj);
            }
            catch
            {
                conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "451";
                conf_ResponseMessageObj.status = "Error";
                conf_ResponseMessageObj.message = "Not Executed.";
                HttpContext.Response.StatusCode = 451;
                return Json(conf_ResponseMessageObj);
            }
        }
    }
}
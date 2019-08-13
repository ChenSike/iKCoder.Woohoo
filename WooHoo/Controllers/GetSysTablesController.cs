using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using WooHoo.Base;
using WooHoo.Configs;
using WooHoo.Orm;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetSysTablesController : WHControllerBase
    {
        [HttpGet]
        [Filter.Filter_SysAuthor]
        [Filter.Filter_ConnectDB]
        public ActionResult Action()
        {
            Orm_sys_table lstAccessObj = new Orm_sys_table();
            string strQuery = "SELECT TABLE_NAME,CREATE_TIME,UPDATE_TIME FROM information_schema.TABLES WHERE table_schema = 'woohoo'";
            List<Orm_sys_table> exsitedAccessObj = dbConnection.Query<Orm_sys_table>(strQuery, lstAccessObj).ToList();
            if (exsitedAccessObj != null)
            {
                HttpContext.Response.StatusCode = 200;
                return Json(exsitedAccessObj);
            }
            else
            {
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "421";
                conf_ResponseMessageObj.status = "ERROR";
                conf_ResponseMessageObj.message = "Empty result or error";
                HttpContext.Response.StatusCode = 421;
                return Json(conf_ResponseMessageObj);
            }
        }
    }
}
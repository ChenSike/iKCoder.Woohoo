using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using WooHoo.Base;
using WooHoo.Orm;
using WooHoo.Filter;
using WooHoo.Configs;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetSysTableDataController : WHControllerBase
    {
        /// <summary>
        /// 获取数据表的数据
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        [HttpGet]
        [Filter.Filter_SysAuthor]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string tablename)
        {
            try
            {
                if (!string.IsNullOrEmpty(tablename))
                {
                    string query = "select * from " + tablename;
                    var result = dbConnection.Query(query).AsList();
                    return Json(result);
                }
                else
                {
                    Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                    conf_ResponseMessageObj.code = "424";
                    conf_ResponseMessageObj.status = "Error";
                    conf_ResponseMessageObj.message = "Tablename is empty";
                    HttpContext.Response.StatusCode = 424;
                    return Json(conf_ResponseMessageObj);
                }
            }
            catch
            {
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "424";
                conf_ResponseMessageObj.status = "Error";
                conf_ResponseMessageObj.message = "Faild to execute";
                HttpContext.Response.StatusCode = 424;
                return Json(conf_ResponseMessageObj);
            }
                       
        }

    }
}
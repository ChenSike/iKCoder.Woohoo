using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using WooHoo.Base;
using WooHoo.Configs;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetConfAllAddressDefaultController : WHControllerBase
    {
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(int id,string guid)
        {
            try
            {
                Orm.Orm_conf_all_address orm_Conf_All_Address = new Orm.Orm_conf_all_address();
                orm_Conf_All_Address.id = id;
                orm_Conf_All_Address.guid = guid;
                string query = "update conf_all_address set def='0' where guid=@guid";
                dbConnection.Execute(query, orm_Conf_All_Address);
                query = "update conf_all_address set def='1' where id=@id";
                dbConnection.Execute(query, orm_Conf_All_Address);
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "200";
                conf_ResponseMessageObj.status = "ok";
                conf_ResponseMessageObj.message = "Executed";
                HttpContext.Response.StatusCode = 200;
                return Json(conf_ResponseMessageObj);
            }
            catch
            {
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "500";
                conf_ResponseMessageObj.status = "error";
                conf_ResponseMessageObj.message = "faild to execute";
                HttpContext.Response.StatusCode = 500;
                return Json(conf_ResponseMessageObj);
            }
        }

    }
}
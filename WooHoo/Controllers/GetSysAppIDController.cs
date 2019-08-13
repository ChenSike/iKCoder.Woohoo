using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WooHoo.Base;
using WooHoo.Orm;
using WooHoo.Configs;
using System.Data;
using Dapper;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetSysAppIDController : WHControllerBase
    {
        [HttpGet]
        [Filter.Filter_SysAuthor]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string uname , string password)
        {
            Orm_lst_access_ids lstAccessObj = new Orm_lst_access_ids();
            lstAccessObj.username = uname;
            string strQuery = "select * from lst_access_ids where username=@username and password=@password";
            Orm_lst_access_ids exsitedAccessObj = (Orm_lst_access_ids)dbConnection.Query<Orm_lst_access_ids>(strQuery, lstAccessObj).SingleOrDefault();
            if (exsitedAccessObj != null)
            {
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "200";
                conf_ResponseMessageObj.status = "OK";
                conf_ResponseMessageObj.message = exsitedAccessObj.appid;
                HttpContext.Response.StatusCode = 200;
                return Json(conf_ResponseMessageObj);
            }
            else
            {
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "424";
                conf_ResponseMessageObj.status = "Error";
                conf_ResponseMessageObj.message = "User Not Existed Or Invalidated Password";
                HttpContext.Response.StatusCode = 424;
                return Json(conf_ResponseMessageObj);
            }
        }
    }
}
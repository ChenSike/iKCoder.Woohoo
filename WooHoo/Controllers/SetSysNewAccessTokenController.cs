using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WooHoo.Filter;
using WooHoo.Orm;
using WooHoo.Configs;
using Dapper;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetSysNewAccessTokenController : WooHoo.Base.WHControllerBase
    {
        [HttpGet]
        [Filter_SysAuthor]
        [Filter_ConnectDB]
        public ActionResult action(string uname,string password)
        {
            Orm_lst_access_ids lstAccessObj = new Orm_lst_access_ids();
            lstAccessObj.username = uname;
            string strQuery = "select * from lst_access_ids where username=@username";
            Orm_lst_access_ids exsitedAccessObj = (Orm_lst_access_ids)dbConnection.Query< Orm_lst_access_ids>(strQuery, lstAccessObj).SingleOrDefault();
            if(exsitedAccessObj!=null)
            {
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "424";
                conf_ResponseMessageObj.status = "Error";
                conf_ResponseMessageObj.message = "User Existed";
                HttpContext.Response.StatusCode = 424;
                return Json(conf_ResponseMessageObj);
            }
            else
            {
                lstAccessObj.appid = Guid.NewGuid().ToString();
                lstAccessObj.password = password;
                strQuery = "insert into lst_access_ids(appid,username,password) values(@appid,@username,@password)";
                dbConnection.Execute(strQuery,lstAccessObj);
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "200";
                conf_ResponseMessageObj.status = "OK";
                conf_ResponseMessageObj.message = "OK";
                HttpContext.Response.StatusCode = 200;
                return Json(lstAccessObj);
            }
        }
    }
}
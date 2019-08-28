using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using WooHoo.Base;
using WooHoo.Configs;
using WooHoo.Filter;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetSysRegCalledController : WHControllerBase
    {
        /// <summary>
        /// 系统API：注册访问TOKEN，每次调用以后会获得一个TOKEN，在后续访问任意访问API中必须带上，可以是用QUERYSTRING或者COOKIE的方式，COOKIENAME:token,Queryname:token、appid
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Token</returns>
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string username,string password)
        {
            Conf_ResponseMessage conf_ResponseMessageObj;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "424";
                conf_ResponseMessageObj.status = "Error";
                conf_ResponseMessageObj.message = "User Existed";
                HttpContext.Response.StatusCode = 424;
                return Json(conf_ResponseMessageObj);
            }
            else
            {
                Orm.Orm_lst_access_ids orm_Lst_Access_Ids = new Orm.Orm_lst_access_ids();
                orm_Lst_Access_Ids.username = username;
                orm_Lst_Access_Ids.password = password;
                string query = "select * from lst_access_ids where username=@usernamw and password=@password";
                Orm.Orm_lst_access_ids orm_Lst_Access_Ids_Selected = dbConnection.Query<Orm.Orm_lst_access_ids>(query, orm_Lst_Access_Ids).SingleOrDefault();
                if (orm_Lst_Access_Ids_Selected != null)
                {
                    string WebToken = Guid.NewGuid().ToString();
                    HttpContext.Session.SetString("from", username);
                    HttpContext.Session.SetString("token", WebToken);
                    HttpContext.Session.SetString("ts", DateTime.Now.ToString());
                    HttpContext.Response.Cookies.Append("token", WebToken);
                    conf_ResponseMessageObj = new Conf_ResponseMessage();
                    conf_ResponseMessageObj.code = "201";
                    conf_ResponseMessageObj.status = "OK";
                    conf_ResponseMessageObj.message = "Web Token : " + WebToken;
                    HttpContext.Response.StatusCode = 201;
                    return Json(conf_ResponseMessageObj);
                }
                else
                {
                    conf_ResponseMessageObj = new Conf_ResponseMessage();
                    conf_ResponseMessageObj.code = "424";
                    conf_ResponseMessageObj.status = "Error";
                    conf_ResponseMessageObj.message = "Invalidated Account";
                    HttpContext.Response.StatusCode = 424;
                    return Json(conf_ResponseMessageObj);
                }
            }
        }
    }
}
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
    public class SetConfAllResetUserController : WHControllerBase
    {
        /// <summary>
        /// 更新用户的基础数据，GUID / OPENID 需要提供一个
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="openid"></param>
        /// <param name="uid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Action(string guid,string openid,string uid,string password)
        {
            Orm.Orm_conf_all_users orm_Conf_All_Users = new Orm.Orm_conf_all_users();
            orm_Conf_All_Users.guid = guid;
            orm_Conf_All_Users.openid = openid;
            orm_Conf_All_Users.uid = uid;
            orm_Conf_All_Users.password = password;
            string query = "select * from conf_all_users where openid=@openid or guid=@guid";
            Orm.Orm_conf_all_users orm_Conf_All_Users_Selected = dbConnection.Query<Orm.Orm_conf_all_users>(query).SingleOrDefault();
            Conf_ResponseMessage conf_ResponseMessageObj;
            if (orm_Conf_All_Users_Selected!=null)
            {
                query = "update conf_all_users set uid=@uid,password=@password where openid=@openid or guid=@guid";
                dbConnection.Execute(query);
                conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "200";
                conf_ResponseMessageObj.status = "ok";
                conf_ResponseMessageObj.message = "Executed";
                HttpContext.Response.StatusCode = 200;
                return Json(conf_ResponseMessageObj);
            }
            else
            {
                conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "500";
                conf_ResponseMessageObj.status = "error";
                conf_ResponseMessageObj.message = "faild to reset user";
                HttpContext.Response.StatusCode = 500;
                return Json(conf_ResponseMessageObj);
            }
        }
    }
}
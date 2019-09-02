using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WooHoo.Base;
using WooHoo.Configs;
using WooHoo.Orm;
using Newtonsoft.Json;
using Dapper;



namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetConfAllNewUserController : WHControllerBase
    {
        /// <summary>
        /// 建立一个新用户，默认为空UID, 空PASSWORD，需要重置
        /// </summary>
        /// <param name="code"></param>
        /// <returns>GUID</returns>
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string code)
        {
            Global.GlobalTestingLog globalTestingLog = new Global.GlobalTestingLog("SetConfAllNewUser");
            Conf_ResponseMessage conf_ResponseMessageObj;
            string openid = Global.GlobalFunctions.GetOpenIDFromWX(code);
            globalTestingLog.AddRecord("openid", openid);
            try
            {
                if (openid == "")
                {
                    conf_ResponseMessageObj = new Conf_ResponseMessage();
                    conf_ResponseMessageObj.code = "500";
                    conf_ResponseMessageObj.status = "error";
                    conf_ResponseMessageObj.message = "User existed.";
                    HttpContext.Response.StatusCode = 500;
                    return Json(conf_ResponseMessageObj);
                }
                Orm_conf_all_users orm_Conf_All_Users;
                string query = "select * from conf_all_users where openid=" + openid;
                globalTestingLog.AddRecord("query", query);
                orm_Conf_All_Users = (Orm_conf_all_users)dbConnection.Query<Orm_conf_all_users>(query).Single();
                globalTestingLog.AddRecord("step", "orm_Conf_All_Users created");
                if (orm_Conf_All_Users == null)
                {
                    orm_Conf_All_Users = new Orm_conf_all_users();
                    orm_Conf_All_Users.guid = Guid.NewGuid().ToString();
                    orm_Conf_All_Users.openid = openid;
                    query = "insert into conf_all_users(openid,guid) values(@openid,@guid)";
                    globalTestingLog.AddRecord("insert query", query);
                    dbConnection.Execute(query, orm_Conf_All_Users);
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
                    conf_ResponseMessageObj.message = "User existed.";
                    HttpContext.Response.StatusCode = 500;
                    return Json(conf_ResponseMessageObj);
                }
            }
            catch(Exception err)
            {
                globalTestingLog.AddRecord("stace", err.StackTrace);
                globalTestingLog.AddRecord("msg", err.Message);
                conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "500";
                conf_ResponseMessageObj.status = "error";
                conf_ResponseMessageObj.message = "User existed.";
                HttpContext.Response.StatusCode = 500;
                return Json(conf_ResponseMessageObj);
            }
        }
    }
}
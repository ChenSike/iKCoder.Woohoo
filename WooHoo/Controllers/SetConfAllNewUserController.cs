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
            Global.GlobalTestingLog globalTestingLog = new Global.GlobalTestingLog("NewUser");
            Conf_ResponseMessage conf_ResponseMessageObj;
            globalTestingLog.AddRecord("code", code);
            string openid = Global.GlobalFunctions.GetOpenIDFromWX(code);
            globalTestingLog.AddRecord("openid", openid);
            try
            {
                if (openid == "" || openid == "Null" || openid == "null" || openid == "NULL")
                {
                    conf_ResponseMessageObj = new Conf_ResponseMessage();
                    conf_ResponseMessageObj.code = "500";
                    conf_ResponseMessageObj.status = "Invalidated OpenID";
                    conf_ResponseMessageObj.message = "Open ID is NULL";
                    HttpContext.Response.StatusCode = 500;
                    return Json(conf_ResponseMessageObj);
                }
                Orm_conf_all_users orm_Conf_All_Users = new Orm_conf_all_users();
                string query = "select * from conf_all_users where openid=@openid";
                orm_Conf_All_Users.openid = openid;
                globalTestingLog.AddRecord("query", query);
                Orm_conf_all_users orm_Conf_All_Users_selected = dbConnection.Query<Orm_conf_all_users>(query, orm_Conf_All_Users).SingleOrDefault();
                globalTestingLog.AddRecord("step", "orm_Conf_All_Users created");
                if (orm_Conf_All_Users_selected == null)
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
                    conf_ResponseMessageObj.message = orm_Conf_All_Users.guid;
                    HttpContext.Response.StatusCode = 200;
                    return Json(conf_ResponseMessageObj);
                }
                else
                {
                    conf_ResponseMessageObj = new Conf_ResponseMessage();
                    conf_ResponseMessageObj.code = "501";
                    conf_ResponseMessageObj.status = "user existed";
                    conf_ResponseMessageObj.message = orm_Conf_All_Users_selected.guid;
                    HttpContext.Response.StatusCode = 501;
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
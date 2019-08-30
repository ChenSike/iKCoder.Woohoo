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
    public class SetConfAllULoginController : WHControllerBase
    {
        /// <summary>
        /// CLIENT微信登录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string openid)
        {
            if(string.IsNullOrEmpty(openid))
            {
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "424";
                conf_ResponseMessageObj.status = "Error";
                conf_ResponseMessageObj.message = "Empty UnionID";
                HttpContext.Response.StatusCode = 424;
                return Json(conf_ResponseMessageObj);
            }
            else
            {
                Orm.Orm_conf_all_users orm_Conf_All_Users = new Orm.Orm_conf_all_users();
                orm_Conf_All_Users.openid = openid;
                string query = "select * from conf_all_users where openid=@openid";
                Orm.Orm_conf_all_users orm_Conf_All_Users_Selected = dbConnection.Query<Orm.Orm_conf_all_users>(query, orm_Conf_All_Users).SingleOrDefault();
                if(orm_Conf_All_Users_Selected==null)
                {
                    Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                    conf_ResponseMessageObj.code = "424";
                    conf_ResponseMessageObj.status = "Error";
                    conf_ResponseMessageObj.message = "No reg unionid";
                    HttpContext.Response.StatusCode = 424;
                    return Json(conf_ResponseMessageObj);
                }
                else
                {
                    string sessionID = HttpContext.Session.Id.ToString();
                    HttpContext.Session.SetString("guid", orm_Conf_All_Users_Selected.guid);
                    HttpContext.Session.SetString("regdt", DateTime.Now.ToString());
                    HttpContext.Session.SetString("openid", orm_Conf_All_Users_Selected.openid);
                    orm_Conf_All_Users_Selected.sessionID = sessionID;
                    return Json(orm_Conf_All_Users_Selected);
                }
            }
        }
    }
}
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
    public class SetConfAllULoginByPageControllerController : WHControllerBase
    {
        
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string uid, string password)
        {
            if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(password))
            {
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "424";
                conf_ResponseMessageObj.status = "Error";
                conf_ResponseMessageObj.message = "Empty uid or password";
                HttpContext.Response.StatusCode = 424;
                return Json(conf_ResponseMessageObj);
            }
            else
            {
                Orm.Orm_conf_all_users orm_Conf_All_Users = new Orm.Orm_conf_all_users();
                orm_Conf_All_Users.uid = uid;
                orm_Conf_All_Users.password = password;
                string query = "select * from conf_all_users where uid=@uid and password = @password";
                Orm.Orm_conf_all_users orm_Conf_All_Users_Selected = dbConnection.Query<Orm.Orm_conf_all_users>(query, orm_Conf_All_Users).SingleOrDefault();
                if (orm_Conf_All_Users_Selected == null)
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
                    HttpContext.Session.SetString("unionid", orm_Conf_All_Users_Selected.unionid);
                    HttpContext.Response.Cookies.Append("guid", orm_Conf_All_Users_Selected.guid);
                    return Json(orm_Conf_All_Users_Selected);
                }
            }
        }
    }
}
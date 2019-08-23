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
        
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string unionID)
        {
            if(string.IsNullOrEmpty(unionID))
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
                orm_Conf_All_Users.unionid = unionID;
                string query = "select * from conf_all_users where unionid=@unionid";
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
                    HttpContext.Session.SetString("unionid", orm_Conf_All_Users_Selected.unionid);
                    orm_Conf_All_Users_Selected.sessionID = sessionID;
                    return Json(orm_Conf_All_Users_Selected);
                }
            }
        }
    }
}
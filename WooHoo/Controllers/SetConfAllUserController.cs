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
    public class SetConfAllUserController : WHControllerBase
    {
        /// <summary>
        /// 解密微信USERDATA并且建立GUID，GUID为CLIENT的唯一标识，与UNIONID对应
        /// </summary>
        /// <param name="userdata"></param>
        /// <param name="sesionKey"></param>
        /// <returns>GUID</returns>
        [HttpPost]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string userdata,string sesionKey)
        {
            AES aesObj = new AES();
            aesObj.optionalKey = sesionKey;
            string deAESData = aesObj.AesDecrypt(userdata);
            Orm_conf_wcuserinfo JsonUserObj = (Orm_conf_wcuserinfo)JsonConvert.DeserializeObject(deAESData);
            string unionID = JsonUserObj.uniodId;
            Orm_conf_all_users orm_Conf_All_Users = new Orm_conf_all_users();
            orm_Conf_All_Users.unionid = unionID;
            string query = "select * from conf_all_users where unionid=@unionid";
            orm_Conf_All_Users = (Orm_conf_all_users)dbConnection.Query<Orm_conf_all_users>(query).SingleOrDefault();
            Conf_ResponseMessage conf_ResponseMessageObj;
            if (orm_Conf_All_Users==null)
            {
                orm_Conf_All_Users.guid = Guid.NewGuid().ToString();
                query = "insert into conf_all_users(unionid,guid) values(@unionid,@guid)";
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
                conf_ResponseMessageObj.code = "200";
                conf_ResponseMessageObj.status = "ok";
                conf_ResponseMessageObj.message = "Executed";
                HttpContext.Response.StatusCode = 200;
                return Json(conf_ResponseMessageObj);
            }
        }
    }
}
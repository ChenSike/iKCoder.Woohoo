using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WooHoo.Base;
using WooHoo.Filter;
using WooHoo.Orm;
using WooHoo.Configs;
using Dapper;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetSysRestryDomainController : WHControllerBase
    {
        /// <summary>
        /// 系统API：用于REMOTE CALLED的DOMAIN注册，如果没有注册，在访问的时候会被拒绝。
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        [Filter_SysAuthor]
        [Filter_ConnectDB]
        public ActionResult action(string username,string password)
        {
            Orm_lst_access_ids lstAccessObj = new Orm_lst_access_ids();
            lstAccessObj.username = username;
            string strQuery = "select * from lst_access_ids where username=@username";
            Orm_lst_access_ids exsitedAccessObj = (Orm_lst_access_ids)dbConnection.Query(strQuery, lstAccessObj).Single();
            Conf_ResponseMessage conf_ResponseMessageObj;
            if (exsitedAccessObj == null)
            {
                conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "451";
                conf_ResponseMessageObj.status = "Error";
                conf_ResponseMessageObj.message = "User not Existed";
                HttpContext.Response.StatusCode = 451;
                return Json(conf_ResponseMessageObj);
            }
            else
            {
                if (exsitedAccessObj.password == password)
                {
                    string strHost = HttpContext.Request.Host.Host;
                    string appid = exsitedAccessObj.appid;
                    Orm_lst_access_domains orm_Lst_Access_Domains = new Orm_lst_access_domains();
                    orm_Lst_Access_Domains.appid = appid;
                    strQuery = "select * from lst_access_domains where appid=@appid";
                    Orm_lst_access_domains existedLstAccessDoaminsObj = (Orm_lst_access_domains)dbConnection.Query(strQuery, orm_Lst_Access_Domains).Single();
                    if (existedLstAccessDoaminsObj != null)
                    {
                        if (orm_Lst_Access_Domains.changedtimes <= 10)
                        {
                            orm_Lst_Access_Domains.changedtimes++;
                            orm_Lst_Access_Domains.domains = strHost;
                            strQuery = "update lst_access_domains set domains=@domains where appid=@appid";
                            dbConnection.Execute(strQuery);
                            conf_ResponseMessageObj = new Conf_ResponseMessage();
                            conf_ResponseMessageObj.code = "200";
                            conf_ResponseMessageObj.status = "OK";
                            conf_ResponseMessageObj.message = "updated";
                            HttpContext.Response.StatusCode = 200;
                            return Json(conf_ResponseMessageObj);
                        }
                        else
                        {
                            conf_ResponseMessageObj = new Conf_ResponseMessage();
                            conf_ResponseMessageObj.code = "451";
                            conf_ResponseMessageObj.status = "Error";
                            conf_ResponseMessageObj.message = "Changed Over Limited";
                            HttpContext.Response.StatusCode = 451;
                            return Json(conf_ResponseMessageObj);
                        }
                    }
                    else
                    {
                        orm_Lst_Access_Domains.changedtimes = 1;
                        orm_Lst_Access_Domains.domains = strHost;
                        strQuery = "insert into lst_access_domains(appid,domains,changedtimes) value(@appid,@domains,@changedtimes)";
                        dbConnection.Execute(strQuery, orm_Lst_Access_Domains);
                        conf_ResponseMessageObj = new Conf_ResponseMessage();
                        conf_ResponseMessageObj.code = "200";
                        conf_ResponseMessageObj.status = "OK";
                        conf_ResponseMessageObj.message = "updated";
                        HttpContext.Response.StatusCode = 200;
                        return Json(conf_ResponseMessageObj);
                    }
                }
                else
                {
                    conf_ResponseMessageObj = new Conf_ResponseMessage();
                    conf_ResponseMessageObj.code = "451";
                    conf_ResponseMessageObj.status = "Error";
                    conf_ResponseMessageObj.message = "Invalidated Password";
                    HttpContext.Response.StatusCode = 451;
                    return Json(conf_ResponseMessageObj);
                }
            }
            
        }
    }
}
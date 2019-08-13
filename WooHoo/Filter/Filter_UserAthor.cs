using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using WooHoo.Base;
using Dapper;


namespace WooHoo.Filter
{
    public class Filter_UserAthor : Attribute, IActionFilter
    {
        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {

        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            string host = context.HttpContext.Request.Host.Host;
            string appid = context.HttpContext.Request.Query["appid"].ToString();
            WHControllerBase ControllerBaseObj = (WHControllerBase)context.Controller;
            if (ControllerBaseObj != null)
            {
                if (ControllerBaseObj.dbConnection.State == System.Data.ConnectionState.Open)
                {
                    Orm.Orm_lst_access_domains orm_Lst_Access_Domains = new Orm.Orm_lst_access_domains();
                    orm_Lst_Access_Domains.appid = appid;
                    orm_Lst_Access_Domains.domains = host;
                    string query = "select * from lst_access_domains where appid=@appid and domains=@domains";
                    Orm.Orm_lst_access_domains returnObj = (Orm.Orm_lst_access_domains)ControllerBaseObj.dbConnection.Query<Orm.Orm_lst_access_domains>(query, orm_Lst_Access_Domains).SingleOrDefault();
                    if(returnObj!=null)
                    {
                        return;
                    }
                    else
                    {
                        context.HttpContext.Response.StatusCode = 401;
                        Configs.Conf_ResponseMessage responseMessageObj = new Configs.Conf_ResponseMessage();
                        responseMessageObj.code = "401";
                        responseMessageObj.status = "Error";
                        responseMessageObj.message = "No Access";
                        JsonResult jsonResult = new JsonResult(responseMessageObj);
                        context.Result = jsonResult;
                    }
                }
                
            }
            else
            {
                context.HttpContext.Response.StatusCode = 401;
                Configs.Conf_ResponseMessage responseMessageObj = new Configs.Conf_ResponseMessage();
                responseMessageObj.code = "401";
                responseMessageObj.status = "Error";
                responseMessageObj.message = "No Access";
                JsonResult jsonResult = new JsonResult(responseMessageObj);
                context.Result = jsonResult;
            }
        }
    }
}

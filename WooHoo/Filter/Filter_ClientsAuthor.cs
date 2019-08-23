using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using WooHoo.Base;
using Dapper;
using System.Text;

namespace WooHoo.Filter
{
    public class Filter_ClientsAuthor : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.HttpContext.Session.Keys.Contains("guid") || context.HttpContext.Session.Keys.Contains("unionid"))
            {
                string guid = context.HttpContext.Request.Query["guid"].ToString();
                string unionid = context.HttpContext.Request.Query["guid"].ToString();
                string val = string.Empty;
                byte[] byVal;
                if (!string.IsNullOrEmpty(guid))
                {                    
                    context.HttpContext.Session.TryGetValue("guid", out byVal);
                    val = Encoding.Default.GetString(byVal);
                    if(guid==val)
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
                else if(!string.IsNullOrEmpty(unionid))
                {
                    context.HttpContext.Session.TryGetValue("unionid", out byVal);
                    val = Encoding.Default.GetString(byVal);
                    if (unionid == val)
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

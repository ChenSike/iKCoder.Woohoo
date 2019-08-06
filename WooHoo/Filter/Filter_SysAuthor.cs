using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace WooHoo.Filter
{
    public class Filter_SysAuthor : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string operationCode = context.HttpContext.Request.Query["code"];
            if (operationCode == "sys_operation")
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
}

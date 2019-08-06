using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using MySql.Data;
using Dapper;

namespace WooHoo.Filter
{
    public class Filter_ConnectDB : Attribute,IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            WooHoo.Base.WHControllerBase Base_Controller = context.Controller as WooHoo.Base.WHControllerBase;
            Base_Controller.dbConnection.Close();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            WooHoo.Base.WHControllerBase Base_Controller = context.Controller as WooHoo.Base.WHControllerBase;
            WooHoo.Global.DBServices dBServices = new Global.DBServices();
            Base_Controller.dbConnection = dBServices.CreateMysqlConnection(Base_Controller.confServices.GetConf_DB());
        }
    }
}

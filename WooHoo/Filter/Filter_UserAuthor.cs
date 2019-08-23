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
    public class Filter_UserAuthor : Attribute, IActionFilter
    {
        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {

        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            string host = context.HttpContext.Request.Host.Host;
            string appid = context.HttpContext.Request.Query["appid"].ToString();
            WHControllerBase ControllerBaseObj = (WHControllerBase)context.Controller;
            if (!string.IsNullOrEmpty(appid))
            {
                if (ControllerBaseObj != null)
                {
                    if (ControllerBaseObj.dbConnection.State == System.Data.ConnectionState.Open)
                    {
                        Orm.Orm_lst_access_ids orm_Lst_Access_Ids = new Orm.Orm_lst_access_ids();
                        orm_Lst_Access_Ids.appid = appid;
                        string query = "select * from lst_access_ids where appid=@appid";
                        Orm.Orm_lst_access_ids orm_Lst_Access_Ids_Selected = ControllerBaseObj.dbConnection.Query<Orm.Orm_lst_access_ids>(query, orm_Lst_Access_Ids).SingleOrDefault();
                        if (orm_Lst_Access_Ids_Selected != null)
                        {
                            context.HttpContext.Response.StatusCode = 201;
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
            else
            {
                string webtoken = string.Empty;
                if (context.HttpContext.Request.Cookies.ContainsKey("token"))
                    webtoken = context.HttpContext.Request.Cookies["token"].ToString();
                else if (context.HttpContext.Request.Query.ContainsKey("token"))
                    webtoken = context.HttpContext.Request.Query["token"].ToString();
                if(string.IsNullOrEmpty(webtoken))
                {
                    context.HttpContext.Response.StatusCode = 401;
                    Configs.Conf_ResponseMessage responseMessageObj = new Configs.Conf_ResponseMessage();
                    responseMessageObj.code = "401";
                    responseMessageObj.status = "Error";
                    responseMessageObj.message = "No Access";
                    JsonResult jsonResult = new JsonResult(responseMessageObj);
                    context.Result = jsonResult;
                }
                else
                {
                    string valToken = string.Empty;
                    byte[] byVal;
                    context.HttpContext.Session.TryGetValue("token", out byVal);
                    valToken = Encoding.Default.GetString(byVal);
                    if(string.IsNullOrEmpty( valToken))
                    {
                        if(valToken==webtoken)
                        {
                            context.HttpContext.Session.TryGetValue("token", out byVal);
                            string strTS= Encoding.Default.GetString(byVal);
                            DateTime dtTS = DateTime.Parse(strTS);
                            if ((DateTime.Now - dtTS).Hours >= 1)
                            {
                                context.HttpContext.Session.Clear();
                                context.HttpContext.Response.Cookies.Delete("token");
                                context.HttpContext.Response.StatusCode = 401;
                                Configs.Conf_ResponseMessage responseMessageObj = new Configs.Conf_ResponseMessage();
                                responseMessageObj.code = "401";
                                responseMessageObj.status = "Error";
                                responseMessageObj.message = "No Access";
                                JsonResult jsonResult = new JsonResult(responseMessageObj);
                                context.Result = jsonResult;
                            }
                            else
                            {
                                return;
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
    }
}

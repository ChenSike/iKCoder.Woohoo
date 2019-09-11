using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using WooHoo.Base;
using WooHoo.Configs;
using Newtonsoft.Json;


namespace WooHoo.Controllers
{

    public class JC_CheckStock_Input
    {
        public List<JC_CheckStock_Output> Lst
        {
            set;
            get;
        }
    }

    public class JC_CheckStock_Output
    {
        public int id
        {
            set;
            get;
        }

        public int count
        {
            set;
            get;
        }

        public string outofstock
        {
            set;
            get;
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class GetConfAllCheckStockController : WHControllerBase
    {
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "500";
                conf_ResponseMessageObj.status = "error";
                conf_ResponseMessageObj.message = "faild to execute";
                HttpContext.Response.StatusCode = 500;
                return Json(conf_ResponseMessageObj);
            }
            else
            {
                JC_CheckStock_Input jC_CheckStock_Input = (JC_CheckStock_Input)JsonConvert.DeserializeObject(data, typeof(JC_CheckStock_Input));
                foreach(JC_CheckStock_Output jC_CheckStock_Output in jC_CheckStock_Input.Lst)
                {
                    string query = "select * from conf_all_proitems_store where proid=" + jC_CheckStock_Output.id;
                    Orm.Orm_conf_all_proitems_store orm_Conf_All_Proitems_Store = dbConnection.Query<Orm.Orm_conf_all_proitems_store>(query).FirstOrDefault();
                    if(orm_Conf_All_Proitems_Store.stock>=jC_CheckStock_Output.count)
                    {
                        jC_CheckStock_Output.outofstock = "0";
                    }
                    else
                    {
                        jC_CheckStock_Output.outofstock = "1";
                    }
                }
                return Json(jC_CheckStock_Input);
            }
        }
    }
}
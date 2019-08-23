using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WooHoo.Base;
using Dapper;
using WooHoo.Configs;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetConfAllUpdateAddressController : WHControllerBase
    {
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string guid, string name, string country, string state, string city, string district, string address)
        {
            try
            {
                Orm.Orm_conf_all_address orm_Conf_All_Address = new Orm.Orm_conf_all_address();
                orm_Conf_All_Address.guid = guid;
                orm_Conf_All_Address.name = name;
                orm_Conf_All_Address.country = country;
                orm_Conf_All_Address.state = state;
                orm_Conf_All_Address.district = district;
                orm_Conf_All_Address.address = address;
                string query = "update conf_all_address set name=@name,country=@country,state=@state,city=@city,address=@address where guid=@guid";
                dbConnection.Execute(query, orm_Conf_All_Address);
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "200";
                conf_ResponseMessageObj.status = "ok";
                conf_ResponseMessageObj.message = "Executed";
                HttpContext.Response.StatusCode = 200;
                return Json(conf_ResponseMessageObj);
            }
            catch
            {
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "500";
                conf_ResponseMessageObj.status = "error";
                conf_ResponseMessageObj.message = "faild to update";
                HttpContext.Response.StatusCode = 500;
                return Json(conf_ResponseMessageObj);
            }
        }
    }
}
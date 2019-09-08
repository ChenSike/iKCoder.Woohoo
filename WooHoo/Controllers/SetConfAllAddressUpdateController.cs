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
    public class SetConfAllAddressUpdateController : WHControllerBase
    {
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(int id, string name, string country, string state, string city, string district, string address, string phone)
        {
            Orm.Orm_conf_all_address orm_Conf_All_Address = new Orm.Orm_conf_all_address();
            orm_Conf_All_Address.id = id;
            orm_Conf_All_Address.name = name;
            orm_Conf_All_Address.country = country;
            orm_Conf_All_Address.state = state;
            orm_Conf_All_Address.district = district;
            orm_Conf_All_Address.address = address;
            orm_Conf_All_Address.phone = phone;
            string query = "update conf_all_address set name=@name,country=@country,state=@state,city=@city,address=@address,phone=@phone where id=@id";
            dbConnection.Execute(query, orm_Conf_All_Address);
            Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
            conf_ResponseMessageObj.code = "200";
            conf_ResponseMessageObj.status = "ok";
            conf_ResponseMessageObj.message = "Executed";
            HttpContext.Response.StatusCode = 200;
            return Json(conf_ResponseMessageObj);
        }
    }
}
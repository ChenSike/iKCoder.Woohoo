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
    public class SetConfAllNewAddressController : WHControllerBase
    {
        /// <summary>
        /// 建立一个新的收货地址
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="name"></param>
        /// <param name="country"></param>
        /// <param name="state"></param>
        /// <param name="city"></param>
        /// <param name="district"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string guid,string name,string country,string state,string city,string district,string address)
        {
            Orm.Orm_conf_all_address orm_Conf_All_Address = new Orm.Orm_conf_all_address();
            orm_Conf_All_Address.guid = guid;
            orm_Conf_All_Address.name = name;
            orm_Conf_All_Address.country = country;
            orm_Conf_All_Address.state = state;
            orm_Conf_All_Address.district = district;
            orm_Conf_All_Address.address = address;
            string query = "insert into conf_all_address(guid,name,country,state,city,address) values(@guid,@name,@country,@state,@city,@address)";
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
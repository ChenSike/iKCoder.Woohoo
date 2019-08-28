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
    public class GetConfAllAddressController : WHControllerBase
    {
        /// <summary>
        /// 获取所有用户收货地址列表
        /// </summary>
        /// <param name="guid">用户GUID</param>
        /// <returns></returns>
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string guid)
        {
            string query = "select * from conf_all_address where guid=@guid";
            Orm.Orm_conf_all_address orm_Conf_All_Address = new Orm.Orm_conf_all_address();
            orm_Conf_All_Address.guid = guid;
            List<Orm.Orm_conf_all_address> orm_Conf_All_Addresses = dbConnection.Query<Orm.Orm_conf_all_address>(query, orm_Conf_All_Address).ToList<Orm.Orm_conf_all_address>();
            return Json(orm_Conf_All_Addresses);
        }
    }
}
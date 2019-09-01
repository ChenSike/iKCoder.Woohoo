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
    public class GetConfTypesSearchingController : WHControllerBase
    {
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string keyvalue)
        {
            string query = "select * from conf_all_proitems where id in ( select proid from conf_main_topics_products where topicsid in( select id from conf_main_topics where title like '%" + keyvalue + "%')) or title like '%" + keyvalue + "%' or des like '%" + keyvalue + "%'";
            List<Orm.Orm_conf_all_proitems> lst_result = dbConnection.Query<Orm.Orm_conf_all_proitems>(query).ToList();
            return Json(lst_result);
        }
    }
}
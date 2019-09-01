using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using WooHoo.Base;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetConfCurrentOrdersController : WHControllerBase
    {
        /*

        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string guid)
        {
            string query = "select * from conf_all_orders where guid=" + guid;
            Orm.Orm_conf_all_orders orm_Conf_All_Orders_Obj = new Orm.Orm_conf_all_orders();

        }
        */

    }
}
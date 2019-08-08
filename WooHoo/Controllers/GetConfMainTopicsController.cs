using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WooHoo.Base;
using Dapper;
using WooHoo.Filter;
using WooHoo.Orm;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetConfMainTopicsController : WHControllerBase
    {
        [HttpGet]
        [Filter_ConnectDB]
        public ActionResult Action()
        {
            Orm_conf_topics orm_Conf_TopicsObj = (Orm_conf_topics)dbConnection.Query<Orm_conf_topics>("select * from conf_main_topics").Single();
            return Json(orm_Conf_TopicsObj);
        }
    }
}
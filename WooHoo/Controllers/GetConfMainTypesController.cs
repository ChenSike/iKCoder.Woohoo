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
    public class GetConfMainTypesController : WHControllerBase
    {
        [HttpGet]
        [Filter_ConnectDB]
        public ActionResult action()
        {
            List<Orm_conf_main_types> lstResult = dbConnection.Query<Orm_conf_main_types>("select * from conf_main_types").ToList();
            return Json(lstResult);
        }
    }
}
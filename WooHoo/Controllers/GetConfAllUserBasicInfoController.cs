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
    public class GetConfAllUserBasicInfoController : WHControllerBase
    {
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string guid)
        {
            Orm.Orm_conf_all_users orm_Conf_All_Users = new Orm.Orm_conf_all_users();
            orm_Conf_All_Users.guid = guid;
            string query = "select * from conf_all_users where guid='"+guid+"'";
            orm_Conf_All_Users = dbConnection.Query<Orm.Orm_conf_all_users>(query).SingleOrDefault();
            return Json(orm_Conf_All_Users);
        }
        
    }
}
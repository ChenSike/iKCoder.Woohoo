using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using System.Data;
using MySql.Data;
using Microsoft.AspNetCore.Cors;

namespace WooHoo.Base
{
    [EnableCors("AllowSameDomain")]
    public class WHControllerBase:Controller
    {

        public WHControllerBase()
        {
            confServices = new Global.ConfServices();
        }

        public IDbConnection dbConnection
        {
            set;
            get;
        }

        public Global.ConfServices confServices
        {
            get;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.Configuration;
using WooHoo.Configs;

namespace WooHoo.Global
{
    public class ConfServices
    {
        public string mode = string.Empty;
        IConfigurationRoot configurationObject;
        public ConfServices()
        {
            var binder = new ConfigurationBuilder();
            binder.SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json");
            configurationObject = binder.Build();
            mode = configurationObject["CurrentEnv"];
        }

        public Conf_DB GetConf_DB()
        {
            try
            {
                string getString = "DBConnection" + ":" + mode + ":";
                Conf_DB configObj = new Conf_DB();
                configObj.server = configurationObject[getString + "server"];
                configObj.uid = configurationObject[getString + "uid"];
                configObj.password = configurationObject[getString + "password"];
                configObj.database = configurationObject[getString + "database"];
                return configObj;
            }
            catch
            {
                return null;
            }
        }

    }
}

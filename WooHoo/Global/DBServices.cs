using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using MySql.Data;
using System.Text;

namespace WooHoo.Global
{
    public class DBServices
    {
        public IDbConnection CreateMysqlConnection(Configs.Conf_DB dbConfigobj)
        {
            if (dbConfigobj != null)
            {
                StringBuilder connectionString = new StringBuilder();
                connectionString.Append("server=");
                connectionString.Append(dbConfigobj.server);
                connectionString.Append(";");
                connectionString.Append("uid=");
                connectionString.Append(dbConfigobj.uid);
                connectionString.Append(";");
                connectionString.Append("password=");
                connectionString.Append(dbConfigobj.password);
                connectionString.Append(";");
                connectionString.Append("database=");
                connectionString.Append(dbConfigobj.database);
                connectionString.Append(";");
                return (new MySql.Data.MySqlClient.MySqlConnection(connectionString.ToString()));
            }
            else
                return null;                
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WooHoo.Orm
{

    public class watermarktype
    {
        public string appid
        {
            set;
            get;
        }

        public string timestamp
        {
            set;
            get;
        }
    }


    public class Orm_conf_wcuserinfo
    {

        public string openid
        {
            set;
            get;
        }

        public string nickname
        {
            set;
            get;
        }

        public string gender
        {
            set;
            get;
        }

        public string city
        {
            set;
            get;
        }

        public string province
        {
            set;
            get;
        }

        public string avatarUrl
        {
            set;
            get;
        }

        public string uniodId
        {
            set;
            get;
        }

        public watermarktype watermark
        {
            set;
            get;
        }

    }
           
}

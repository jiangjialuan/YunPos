using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Tb
{
    public class Tb_User_Openid_Query : Tb_User
    {
        public string openid { get; set; }
        public long id_gys_gzh { get; set; }//微信公众号供应商id
    }
}

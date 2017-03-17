using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Ts
{
    [Serializable]
    public class Ts_Sale_Order_Log_Query : Ts_Sale_Order_Log
    {
        // 用户名
        public string user_name { get; set; }

        // 用户公司
        public string user_companyname { get; set; }

        // 操作日志类型名称
        public string flag_mc { get; set; }
    }
}

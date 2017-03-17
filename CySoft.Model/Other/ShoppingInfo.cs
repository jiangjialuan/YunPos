using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Other
{
    [Serializable]
    public class ShoppingInfo
    {
        /// <summary>
        /// 物流公司
        /// </summary>
        public string company_logistics { get; set; }
        /// <summary>
        /// 物流
        /// </summary>
        public string no_logistics { get; set; }
        /// <summary>
        /// 发货日期
        /// </summary>
        public string rq_fh { get; set; }
    }
}

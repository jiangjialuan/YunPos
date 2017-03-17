using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Other
{
    public class HomePagePay
    {
        /// <summary>
        /// 支付类型ID
        /// </summary>
        public string id_pay { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal je { get; set; }
        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string mc { get; set; } 
    }
}

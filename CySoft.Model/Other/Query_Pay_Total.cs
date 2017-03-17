using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Other
{
    [Serializable]
    public class Query_Pay_Total
    {
        public decimal sum_je_pay { get; set; }
        public long count_je_pay { get; set; }
        public decimal je_order_today { get; set; }
        public long count_order_today { get; set; }
        public decimal je_order_yesterday { get; set; }
        public long count_order_yesterday { get; set; }
        public decimal je_order_month { get; set; }
        public long count_order_month { get; set; }
    }
}

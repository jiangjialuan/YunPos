using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Model.Td;

namespace CySoft.Model.Other
{
    [Serializable]
    public class Query_Pay
    {
        public decimal je_pay { get; set; }
        public decimal je_payed { get; set; }
        public decimal je_nopay { get; set; }
        public List<Td_sale_pay_Query> listPay { get; set; }
    }
}

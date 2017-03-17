using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Other
{
    public class HomePageCxspModel
    {
        public string id_kcsp { get; set; }
        public int sl { get; set; }
        public string mc { get; set; }
        public decimal je { get; set; }
    }

    public class HomePageZxspModel
    {
        public string id { get; set; }
        public string mc { get; set; }
    }

    public class HomePageSumJe
    {
        public object value { get; set; }
    }
}

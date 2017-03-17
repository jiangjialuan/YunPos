using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Other
{
    public class Tb_Spfl_Import
    {
        public string mc { get; set; }
        public string bm { get; set; }
        public string father { get; set; }

        public string bz { get; set; }
    }

    public class Tb_Spfl_Import_All
    {
        public List<Tb_Spfl_Import> SuccessList { set; get; }
        public List<Tb_Spfl_Import> FailList { set; get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Tb
{
    [Serializable]
    public class Tb_Sp_Cgs_Query : Tb_Sp_Cgs
    {
        public string name_cgs { get; set; }
        public string name_cgs_level { get; set; }
    }
}

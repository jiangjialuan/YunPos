using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CySoft.Model.Tb
{
    [Serializable]
    [DebuggerDisplay("id_sku = {id_sku},name_cgs_level = {name_cgs_level}, agio = {agio}")]
    public class Tb_Sp_Dj_Query 
    {
        public long id_sku { get; set; }
        public decimal dj_dh { get; set; }
        public decimal sl_dh_min { get; set; }
        public string name_cgs_level { get; set; }
        public long id_cgs_level { get; set; }
        public decimal agio { get; set; }
    }
}

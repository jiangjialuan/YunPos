using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using CySoft.Model.Flags;

namespace CySoft.Model.Tb
{
    [Serializable]
    public class Tb_Cgs_Attention : Tb_Cgs_Edit
    {
        public string remark { get; set; }
        public string refuse { get; set; }
    }
}

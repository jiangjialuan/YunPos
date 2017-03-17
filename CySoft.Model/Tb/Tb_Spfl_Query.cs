#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    [Serializable]
    public class Tb_Spfl_Query : Tb_Spfl
    {
        public string name_hy { get; set; }
        public string name_father { get; set; }

        public string name_sp_expand_template { get; set; }//商品属性名
    }
}
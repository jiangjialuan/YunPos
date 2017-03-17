#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using CySoft.Model.Flags;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    /// <summary>
    /// 供应商商品
    /// </summary>
    [Serializable]
    [Table("Tb_gys_sp", "Tb_Gys_Sp")]
    [DebuggerDisplay("id_gys = {id_gys},id_sku = {id_sku}")]
    public class Tb_Gys_Sp_Query : Tb_Gys_Sp
    {
         /// <summary>
        /// 引用商品模板
        /// </summary>
        public Tb_Gys_Sp_Query() { }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string mc { get; set; }
        /// <summary>
        /// 商品名称—组合规格名称
        /// </summary>
        public string mc_zh { get; set; }
    }
}
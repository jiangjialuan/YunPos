using System;
using System.Diagnostics;

namespace CySoft.Model.Tb
{
    [Serializable]
    [DebuggerDisplay("id_sku = {id_sku}, mc = {mc}, val = {val}")]
    public class Tb_Sp_Expand_Query : Tb_Sp_Expand
    {
        private string _mc = String.Empty;
        /// <summary>
        /// 名称
        /// </summary>
        public string mc
        {
            get
            {
                return _mc;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _mc = value;
                }
                else
                {
                    _mc = String.Empty;
                }
            }
        }
    }
}

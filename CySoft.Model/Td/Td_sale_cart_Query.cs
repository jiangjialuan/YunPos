using System;
using System.Diagnostics;
using CySoft.Model.Flags;

namespace CySoft.Model.Td
{
    [Serializable]
    [DebuggerDisplay("id_gys = {id_gys},id_cgs = {id_cgs},bm_Interface = {bm_Interface}, mc = {mc}, dj = {dj}, sl_dh_min = {sl_dh_min}")]
    public class Td_Sale_Cart_Query : Td_Sale_Cart
    {
        public YesNoFlag flag_up { get; set; }
        public string bm_Interface { get; set; }
        public string mc { get; set; }
        public string gg { get; set; }
        public string unit { get; set; }
        public decimal dj { get; set; }
        public decimal sl_dh_min { get; set; }
        public decimal dj_base { get; set; }
        public decimal dj_old { get; set; }
        // 小计  数量sl*单价dj 
        public decimal xj { get; set; }
        public string photo { get; set; }
    }
}

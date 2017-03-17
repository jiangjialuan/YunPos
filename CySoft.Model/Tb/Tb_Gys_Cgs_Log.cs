#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using CySoft.Model.Flags;
#endregion

#region 采购关系日志
#endregion

namespace CySoft.Model.Tb
{
    /// <summary>
    /// 采购关系日志
    /// </summary>
    [Serializable]
    [Table("Tb_Gys_Cgs_Log", "Tb_Gys_Cgs_Log")]
    [DebuggerDisplay("id_gys = {id_gys},mc_gys={mc_gys},id_cgs = {id_cgs},mc_cgs={mc_cgs}")]
    public class Tb_Gys_Cgs_Log
    {
        private DateTime _rq_create = DateTime.Now;
        private string _flag_state = Gys_Cgs_Status.Apply;
        private string _flag_from = "pc";

        [Column(Insert = false, Update = false)]
        public long id { set; get; }
        public long id_gys { set; get; }
        public long id_cgs { set; get; }
        [Column(Insert = false, Update = false)]
        public string mc_gys { set; get; }
        [Column(Insert = false, Update = false)]
        public string mc_cgs { set; get; }
        public string contents { set; get; }
        public string flag_state { set { if (!string.IsNullOrWhiteSpace(value))_flag_state = value; } get { return _flag_state; } }
        public string flag_form { set { _flag_from = value; } get { return _flag_from; } }//来源pc,android,ios
        [Column(Insert = false, Update = false)]
        public Nullable<DateTime> rq_create
        {
            set
            {
                if (value.HasValue)
                {
                    _rq_create = value.Value;
                }
                else
                {
                    _rq_create = DateTime.Now;
                }
            }
            get { return _rq_create; }
        }
        public long id_user { set; get; }
        public string mc_user { set; get; }
    }
}

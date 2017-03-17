#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    /// <summary>
    /// 供应商采购商关注审核
    /// </summary>
    [Serializable]
    [Table("Tb_gys_cgs_check", "Tb_Gys_Cgs_Check")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Gys_Cgs_Check
    {
        #region public method

        public Tb_Gys_Cgs_Check Clone()
        {
            return (Tb_Gys_Cgs_Check)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id = 0;
        private long _id_gys = 0;
        private long _id_cgs = 0;
        private string _flag_form = "pc";
        private DateTime _rq_sq = DateTime.Now;
        private DateTime _rq_check = new DateTime(1900, 1, 1);
        private string _flag_state = Gys_Cgs_Status.Apply;
        private string _remark = String.Empty;
        private string _refuse = String.Empty;

        #endregion

        #region public property

        public long id_user { set; get; }

        /// <summary>
        /// 流水号
        /// </summary>
        [Column(Insert = false, Update = false)]
        public Nullable<long> id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value.HasValue)
                {
                    _id = value.Value;
                }
                else
                {
                    _id = 0;
                }
            }
        }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public Nullable<long> id_gys
        {
            get
            {
                return _id_gys;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_gys = value.Value;
                }
                else
                {
                    _id_gys = 0;
                }
            }
        }

        /// <summary>
        /// 采购商Id
        /// </summary>
        public Nullable<long> id_cgs
        {
            get
            {
                return _id_cgs;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_cgs = value.Value;
                }
                else
                {
                    _id_cgs = 0;
                }
            }
        }
        [Column(Insert = false, Update = false)]
        public string mc_gys { set; get; }
        [Column(Insert = false, Update = false)]
        public string mc_cgs { set; get; }

        /// <summary>
        /// 来源     supplier:供应商   buyer采购商
        /// </summary>
        public string flag_form
        {
            get
            {
                return _flag_form;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _flag_form = value;
                }
                else
                {
                    _flag_form = "pc";
                }
            }
        }

        /// <summary>
        /// 申请日期
        /// </summary>
        [Column(Update = false)]
        public Nullable<DateTime> rq_sq
        {
            get
            {
                return _rq_sq;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_sq = value.Value;
                }
                else
                {
                    _rq_sq = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 审批日期
        /// </summary>
        public Nullable<DateTime> rq_check
        {
            get
            {
                return _rq_check;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_check = value.Value;
                }
                else
                {
                    _rq_check = new DateTime(1900, 1, 1);
                }
            }
        }

        /// <summary>
        /// 状态  bs_lx='approvaFlag'   0未审核    1已通过     2已拒绝
        /// </summary>
        public string flag_state
        {
            get
            {
                return _flag_state;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _flag_state = value;
                }
                else
                {
                    _flag_state = Gys_Cgs_Status.Apply;
                }
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark
        {
            get
            {
                return _remark;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _remark = value;
                }
                else
                {
                    _remark = String.Empty;
                }
            }
        }

        /// <summary>
        /// 拒绝理由
        /// </summary>
        public string refuse
        {
            get
            {
                return _refuse;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _refuse = value;
                }
                else
                {
                    _refuse = String.Empty;
                }
            }
        }

        #endregion

    }
}
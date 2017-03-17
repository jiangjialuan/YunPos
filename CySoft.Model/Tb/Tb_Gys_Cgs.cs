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
    /// 采购关系
    /// </summary>
    [Serializable]
    [Table("Tb_gys_cgs", "Tb_Gys_Cgs")]
    [DebuggerDisplay("id_user_master_gys = {id_user_master_gys},id_user_master_cgs = {id_user_master_cgs}")]
    public class Tb_Gys_Cgs
    {
        #region public method

        public Tb_Gys_Cgs Clone()
        {
            return (Tb_Gys_Cgs)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id_user_master_gys = 0;
        private long _id_user_master_cgs = 0;
        private long _id_gys = 0;
        private long _id_cgs = 0;
        private YesNoFlag _flag_stop = 0;
        private string _alias_gys = String.Empty;
        private string _alias_cgs = String.Empty;
        private long _id_cgs_level = 0;
        private int _flag_pay = 0;
        private DateTime _rq_treaty_start = new DateTime(1900, 1, 1);
        private DateTime _rq_treaty_end = new DateTime(1900, 1, 1);
        private string _bm_gys_Interface = String.Empty;
        private string _bm_cgs_Interface = String.Empty;
        private long _id_user_gys = 0;
        private long _id_user_cgs = 0;
        private string _flag_from = String.Empty;
        private long _id_create = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private long _id_edit = 0;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);

        #endregion

        #region public property

        /// <summary>
        /// 供应商主用户Id
        /// </summary>
        [Column(Update = false)]
        public Nullable<long> id_user_master_gys
        {
            get
            {
                return _id_user_master_gys;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_user_master_gys = value.Value;
                }
                else
                {
                    _id_user_master_gys = 0;
                }
            }
        }

        /// <summary>
        /// 采购商主用户Id
        /// </summary>
        [Column(Update = false)]
        public Nullable<long> id_user_master_cgs
        {
            get
            {
                return _id_user_master_cgs;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_user_master_cgs = value.Value;
                }
                else
                {
                    _id_user_master_cgs = 0;
                }
            }
        }

        /// <summary>
        /// 供应商Id
        /// </summary>
        [Column(Update = false)]
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
        [Column(Update = false)]
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
        /// 状态   0可以   1不可用
        /// </summary>
        [Column(Update = false)]
        public YesNoFlag flag_stop
        {
            get
            {
                return _flag_stop;
            }
            set
            {
                _flag_stop = value;
            }
        }

        /// <summary>
        /// 供应商别名
        /// </summary>
        public string alias_gys
        {
            get
            {
                return _alias_gys;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _alias_gys = value;
                }
                else
                {
                    _alias_gys = String.Empty;
                }
            }
        }

        /// <summary>
        /// 采购商别名
        /// </summary>
        public string alias_cgs
        {
            get
            {
                return _alias_cgs;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _alias_cgs = value;
                }
                else
                {
                    _alias_cgs = String.Empty;
                }
            }
        }

        /// <summary>
        /// 采购级别
        /// </summary>
        public Nullable<long> id_cgs_level
        {
            get
            {
                return _id_cgs_level;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_cgs_level = value.Value;
                }
                else
                {
                    _id_cgs_level = 0;
                }
            }
        }

        /// <summary>
        /// 默认付款方式
        /// </summary>
        public Nullable<int> flag_pay
        {
            get
            {
                return _flag_pay;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_pay = value.Value;
                }
                else
                {
                    _flag_pay = 0;
                }
            }
        }

        /// <summary>
        /// 合约有效期起
        /// </summary>
        public Nullable<DateTime> rq_treaty_start
        {
            get
            {
                return _rq_treaty_start;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_treaty_start = value.Value;
                }
                else
                {
                    _rq_treaty_start = new DateTime(1900, 1, 1);
                }
            }
        }

        /// <summary>
        /// 合约有效期起
        /// </summary>
        public Nullable<DateTime> rq_treaty_end
        {
            get
            {
                return _rq_treaty_end;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_treaty_end = value.Value;
                }
                else
                {
                    _rq_treaty_end = new DateTime(1900, 1, 1);
                }
            }
        }

        /// <summary>
        /// 供应商设置的采购商接口编码
        /// </summary>
        public string bm_gys_Interface
        {
            get
            {
                return _bm_gys_Interface;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _bm_gys_Interface = value;
                }
                else
                {
                    _bm_gys_Interface = String.Empty;
                }
            }
        }

        /// <summary>
        /// 采购商设置的供应商接口编码
        /// </summary>
        public string bm_cgs_Interface
        {
            get
            {
                return _bm_cgs_Interface;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _bm_cgs_Interface = value;
                }
                else
                {
                    _bm_cgs_Interface = String.Empty;
                }
            }
        }

        /// <summary>
        /// 供应商联系用户
        /// </summary>
        public Nullable<long> id_user_gys
        {
            get
            {
                return _id_user_gys;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_user_gys = value.Value;
                }
                else
                {
                    _id_user_gys = 0;
                }
            }
        }

        /// <summary>
        /// 采购商联系用户
        /// </summary>
        public Nullable<long> id_user_cgs
        {
            get
            {
                return _id_user_cgs;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_user_cgs = value.Value;
                }
                else
                {
                    _id_user_cgs = 0;
                }
            }
        }

        /// <summary>
        /// 来源   attention关注、add添加 bs_lx:khly
        /// </summary>
        public string flag_from
        {
            get
            {
                return _flag_from;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _flag_from = value;
                }
                else
                {
                    _flag_from = String.Empty;
                }
            }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column(Update = false)]
        public Nullable<long> id_create
        {
            get
            {
                return _id_create;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_create = value.Value;
                }
                else
                {
                    _id_create = 0;
                }
            }
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Column(Update = false, Insert = false)]
        public Nullable<DateTime> rq_create
        {
            get
            {
                return _rq_create;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_create = value.Value;
                }
                else
                {
                    _rq_create = new DateTime(1900, 1, 1);
                }
            }
        }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public Nullable<long> id_edit
        {
            get
            {
                return _id_edit;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_edit = value.Value;
                }
                else
                {
                    _id_edit = 0;
                }
            }
        }

        /// <summary>
        /// 最后修改日期
        /// </summary>
        [Column(Insert = false)]
        public Nullable<DateTime> rq_edit
        {
            get
            {
                return _rq_edit;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_edit = value.Value;
                }
                else
                {
                    _rq_edit = new DateTime(1900, 1, 1);
                }
            }
        }

        #endregion

    }
}
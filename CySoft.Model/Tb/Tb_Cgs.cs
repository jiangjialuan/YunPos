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
    /// 采购商
    /// </summary>
    [Serializable]
    [Table("Tb_cgs", "Tb_Cgs")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Cgs
    {
        #region public method

        public Tb_Cgs Clone()
        {
            return (Tb_Cgs)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id = 0;
        private long _id_user_master = 0;
        private int _id_cgs_ptjb = 0;
        private int _flag_pay = 0;
        private string _name_bank = String.Empty;
        private string _account_bank = String.Empty;
        private string _khr = String.Empty;
        private string _no_tax = String.Empty;
        private string _title_invoice = String.Empty;
        private long _id_create = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private long _id_edit = 0;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private long _id_cgs_level = 0;

        #endregion

        #region public property

        /// <summary>
        /// 主键
        /// </summary>
        [Column(Update = false)]
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
        [Column(Update = false,Insert=false)]
        public string companyname { set; get; }
        /// <summary>
        /// 所属主用户
        /// </summary>
        [Column(Update = false)]
        public Nullable<long> id_user_master
        {
            get
            {
                return _id_user_master;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_user_master = value.Value;
                }
                else
                {
                    _id_user_master = 0;
                }
            }
        }

        /// <summary>
        /// 平台采购商级别
        /// </summary>
        public Nullable<int> id_cgs_ptjb
        {
            get
            {
                return _id_cgs_ptjb;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_cgs_ptjb = value.Value;
                }
                else
                {
                    _id_cgs_ptjb = 0;
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
        /// 开户银行
        /// </summary>
        public string name_bank
        {
            get
            {
                return _name_bank;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name_bank = value;
                }
                else
                {
                    _name_bank = String.Empty;
                }
            }
        }

        /// <summary>
        /// 银行账号
        /// </summary>
        public string account_bank
        {
            get
            {
                return _account_bank;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _account_bank = value;
                }
                else
                {
                    _account_bank = String.Empty;
                }
            }
        }

        /// <summary>
        /// 开户人
        /// </summary>
        public string khr
        {
            get
            {
                return _khr;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _khr = value;
                }
                else
                {
                    _khr = String.Empty;
                }
            }
        }

        /// <summary>
        /// 纳税号
        /// </summary>
        public string no_tax
        {
            get
            {
                return _no_tax;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _no_tax = value;
                }
                else
                {
                    _no_tax = String.Empty;
                }
            }
        }

        /// <summary>
        /// 发票抬头
        /// </summary>
        public string title_invoice
        {
            get
            {
                return _title_invoice;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _title_invoice = value;
                }
                else
                {
                    _title_invoice = String.Empty;
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
        /// <summary>
        /// 客户级别
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
        
        #endregion

    }
}
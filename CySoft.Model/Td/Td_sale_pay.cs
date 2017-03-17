#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Td
{
    /// <summary>
    /// 收款单
    /// </summary>
    [Serializable]
    [Table("Td_sale_pay", "Td_Sale_Pay")]
    [DebuggerDisplay("dh = {dh}")]
    public class Td_Sale_Pay
    {
        #region public method

        public Td_Sale_Pay Clone()
        {
            return (Td_Sale_Pay)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _dh = String.Empty;
        private string _dh_order = String.Empty;
        private long _id_gys = 0;
        private long _id_cgs = 0;
        private string _name_bank = String.Empty;
        private string _account_bank = String.Empty;
        private string _khr = String.Empty;
        private short _flag_state = 0;
        private decimal _je = 0m;
        private string _filename = String.Empty;
        private string _flag_pay = String.Empty;
        private string _dh_pay = String.Empty;
        private byte _flag_delete = 0;
        private long _id_delete = 0;
        private DateTime _rq_delete = new DateTime(1900, 1, 1);
        private string _remark = String.Empty;
        private long _id_create = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private long _id_edit = 0;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);

        #endregion

        #region public property

        /// <summary>
        /// 单号
        /// </summary>
        public string dh
        {
            get
            {
                return _dh;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _dh = value;
                }
                else
                {
                    _dh = String.Empty;
                }
            }
        }

        /// <summary>
        /// 订单号
        /// </summary>
        public string dh_order
        {
            get
            {
                return _dh_order;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _dh_order = value;
                }
                else
                {
                    _dh_order = String.Empty;
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
        /// 采购商
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
        /// 1=未审核 2=已审核  0=作废 
        /// </summary>
        public Nullable<short> flag_state
        {
            get
            {
                return _flag_state;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_state = value.Value;
                }
                else
                {
                    _flag_state = 0;
                }
            }
        }

        public Nullable<decimal> je
        {
            get
            {
                return _je;
            }
            set
            {
                if (value.HasValue)
                {
                    _je = value.Value;
                }
                else
                {
                    _je = 0m;
                }
            }
        }

        public string filename
        {
            get
            {
                return _filename;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _filename = value;
                }
                else
                {
                    _filename = String.Empty;
                }
            }
        }

        /// <summary>
        /// offlink=线下 onlink=线上 platform=平台
        /// </summary>
        public string flag_pay
        {
            get
            {
                return _flag_pay;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _flag_pay = value;
                }
                else
                {
                    _flag_pay = String.Empty;
                }
            }
        }

        public string dh_pay
        {
            get
            {
                return _dh_pay;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _dh_pay = value;
                }
                else
                {
                    _dh_pay = String.Empty;
                }
            }
        }

        /// <summary>
        /// 是否已删除   1是   0未
        /// </summary>
        public Nullable<byte> flag_delete
        {
            get
            {
                return _flag_delete;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_delete = value.Value;
                }
                else
                {
                    _flag_delete = 0;
                }
            }
        }

        /// <summary>
        /// 删除人
        /// </summary>
        public Nullable<long> id_delete
        {
            get
            {
                return _id_delete;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_delete = value.Value;
                }
                else
                {
                    _id_delete = 0;
                }
            }
        }

        /// <summary>
        /// 删除日期
        /// </summary>
        public Nullable<DateTime> rq_delete
        {
            get
            {
                return _rq_delete;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_delete = value.Value;
                }
                else
                {
                    _rq_delete = new DateTime(1900, 1, 1);
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
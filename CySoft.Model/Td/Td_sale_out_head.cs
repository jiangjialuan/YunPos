#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using CySoft.Model.Flags;
#endregion

#region
#endregion

namespace CySoft.Model.Td
{
    [Serializable]
    [Table("Td_sale_out_head", "Td_Sale_Out_Head")]
    public class Td_Sale_Out_Head
    {
        #region public method

        public Td_Sale_Out_Head Clone()
        {
            return (Td_Sale_Out_Head)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _dh = String.Empty;
        private string _dh_order = String.Empty;
        private long _id_gys = 0;
        private long _id_cgs = 0;
        private OrderFlag _flag_state = 0;
        private byte _flag_delete = 0;
        private decimal _sl_sum = 0m;
        private string _company_logistics = String.Empty;
        private string _no_logistics = String.Empty;
        private DateTime _rq_fh_logistics = new DateTime(1900, 1, 1);
        private DateTime _rq_fh = new DateTime(1900, 1, 1);
        private long _id_fh = 0;
        private string _remark = String.Empty;
        private long _id_create = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private long _id_edit = 0;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private long? _id_cgs_sh = 0;
        /// <summary>
        /// 发货类型
        /// </summary>
        public int fhType { get; set; }

        #endregion

        #region public property
        /// <summary>
        /// 出库单号
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
        /// 供应商编码
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
        /// 采购商编码
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
        /// 收货商
        /// </summary>
        public Nullable<long> id_cgs_sh
        {
            get { return _id_cgs_sh; }
            set
            {
                if (value.HasValue)
                {
                    _id_cgs_sh = value.Value;
                }
                else
                {
                    _id_cgs_sh = 0;
                }
            }
        }

        public OrderFlag flag_state
        {
            get
            {
                return _flag_state;
            }
            set
            {
             
                    _flag_state = value;
            }
        }

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

        public Nullable<decimal> sl_sum
        {
            get
            {
                return _sl_sum;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_sum = value.Value;
                }
                else
                {
                    _sl_sum = 0m;
                }
            }
        }

        public string company_logistics
        {
            get
            {
                return _company_logistics;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _company_logistics = value;
                }
                else
                {
                    _company_logistics = String.Empty;
                }
            }
        }

        public string no_logistics
        {
            get
            {
                return _no_logistics;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _no_logistics = value;
                }
                else
                {
                    _no_logistics = String.Empty;
                }
            }
        }

        public Nullable<DateTime> rq_fh_logistics
        {
            get
            {
                return _rq_fh_logistics;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_fh_logistics = value.Value;
                }
                else
                {
                    _rq_fh_logistics = new DateTime(1900, 1, 1);
                }
            }
        }

        public Nullable<DateTime> rq_fh
        {
            get
            {
                return _rq_fh;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_fh = value.Value;
                }
                else
                {
                    _rq_fh = new DateTime(1900, 1, 1);
                }
            }
        }

        public Nullable<long> id_fh
        {
            get
            {
                return _id_fh;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_fh = value.Value;
                }
                else
                {
                    _id_fh = 0;
                }
            }
        }

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
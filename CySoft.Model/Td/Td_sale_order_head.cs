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
    /// 订单单头
    /// </summary>
    [Serializable]
    [Table("Td_sale_order_head", "Td_Sale_Order_Head")]
    [DebuggerDisplay("dh = {dh}")]
    public class Td_Sale_Order_Head
    {
        #region public method

        public Td_Sale_Order_Head Clone()
        {
            return (Td_Sale_Order_Head)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _dh = String.Empty;
        private long _id_gys = 0;
        private long _id_cgs = 0;
        private short _flag_state = 0;
        private byte _flag_out = 0;
        private byte _flag_fh = 0;
        private byte _flag_delete = 0;
        private long _id_user_bill = 0;
        private DateTime _rq_jh = new DateTime(1900, 1, 1);
        private decimal _je_hs = 0m;
        private decimal _je_pay = 0m;
        private decimal _je_payed = 0m;
        private byte _flag_tj = 0;
        private string _flag_invoice = String.Empty;
        private string _title_invoice = String.Empty;
        private string _content_invoice = String.Empty;
        private string _name_bank = String.Empty;
        private string _account_bank = String.Empty;
        private string _no_tax = String.Empty;
        private string _shr = String.Empty;
        private decimal _slv = 0m;
        private int _id_province = 0;
        private int _id_city = 0;
        private int _id_county = 0;
        private string _zipcode = String.Empty;
        private string _address = String.Empty;
        private string _phone = String.Empty;
        private string _remark = String.Empty;
        private long _id_create = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private long _id_edit = 0;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private long? _id_cgs_sh = 0;
        private short? _flag_fd = 0;
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
        /// 状态   bs_lx='orderstatus'
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

        /// <summary>
        /// 出库状态
        /// </summary>
        public Nullable<byte> flag_out
        {
            get
            {
                return _flag_out;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_out = value.Value;
                }
                else
                {
                    _flag_out = 0;
                }
            }
        }

        /// <summary>
        /// 发货状态
        /// </summary>
        public Nullable<byte> flag_fh
        {
            get
            {
                return _flag_fh;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_fh = value.Value;
                }
                else
                {
                    _flag_fh = 0;
                }
            }
        }

        /// <summary>
        /// 删除状态
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
        /// 业务员
        /// </summary>
        public Nullable<long> id_user_bill
        {
            get
            {
                return _id_user_bill;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_user_bill = value.Value;
                }
                else
                {
                    _id_user_bill = 0;
                }
            }
        }

        /// <summary>
        /// 交货日期
        /// </summary>
        public Nullable<DateTime> rq_jh
        {
            get
            {
                return _rq_jh;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_jh = value.Value;
                }
                else
                {
                    _rq_jh = new DateTime(1900, 1, 1);
                }
            }
        }

        /// <summary>
        /// 含税金额
        /// </summary>
        public Nullable<decimal> je_hs
        {
            get
            {
                return _je_hs;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_hs = value.Value;
                }
                else
                {
                    _je_hs = 0m;
                }
            }
        }

        /// <summary>
        /// 应收金额（含税）
        /// </summary>
        public Nullable<decimal> je_pay
        {
            get
            {
                return _je_pay;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_pay = value.Value;
                }
                else
                {
                    _je_pay = 0m;
                }
            }
        }

        /// <summary>
        /// 已付金额（含税）
        /// </summary>
        public Nullable<decimal> je_payed
        {
            get
            {
                return _je_payed;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_payed = value.Value;
                }
                else
                {
                    _je_payed = 0m;
                }
            }
        }

        /// <summary>
        /// 是否申请特价   1是   0否
        /// </summary>
        public Nullable<byte> flag_tj
        {
            get
            {
                return _flag_tj;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_tj = value.Value;
                }
                else
                {
                    _flag_tj = 0;
                }
            }
        }

        /// <summary>
        /// 发票类型  bs_lx='invoiceflag'
        /// </summary>
        public string flag_invoice
        {
            get
            {
                return _flag_invoice;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _flag_invoice = value;
                }
                else
                {
                    _flag_invoice = String.Empty;
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
        /// 发票内容
        /// </summary>
        public string content_invoice
        {
            get
            {
                return _content_invoice;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _content_invoice = value;
                }
                else
                {
                    _content_invoice = String.Empty;
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
        /// 收货人
        /// </summary>
        public string shr
        {
            get
            {
                return _shr;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _shr = value;
                }
                else
                {
                    _shr = String.Empty;
                }
            }
        }

        /// <summary>
        /// 税率
        /// </summary>
        public Nullable<decimal> slv
        {
            get
            {
                return _slv;
            }
            set
            {
                if (value.HasValue)
                {
                    _slv = value.Value;
                }
                else
                {
                    _slv = 0m;
                }
            }
        }

        /// <summary>
        /// 省(直辖市、特别行政区)
        /// </summary>
        public Nullable<int> id_province
        {
            get
            {
                return _id_province;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_province = value.Value;
                }
                else
                {
                    _id_province = 0;
                }
            }
        }

        /// <summary>
        /// 市
        /// </summary>
        public Nullable<int> id_city
        {
            get
            {
                return _id_city;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_city = value.Value;
                }
                else
                {
                    _id_city = 0;
                }
            }
        }

        /// <summary>
        /// 区（县）
        /// </summary>
        public Nullable<int> id_county
        {
            get
            {
                return _id_county;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_county = value.Value;
                }
                else
                {
                    _id_county = 0;
                }
            }
        }

        /// <summary>
        /// 邮编
        /// </summary>
        public string zipcode
        {
            get
            {
                return _zipcode;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _zipcode = value;
                }
                else
                {
                    _zipcode = String.Empty;
                }
            }
        }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string address
        {
            get
            {
                return _address;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _address = value;
                }
                else
                {
                    _address = String.Empty;
                }
            }
        }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string phone
        {
            get
            {
                return _phone;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _phone = value;
                }
                else
                {
                    _phone = String.Empty;
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


        /// <summary>
        /// 收货商
        /// </summary>
        public Nullable<long> id_cgs_sh
        {
            get
            {
                return _id_cgs_sh;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_cgs_sh = value;
                }
                else
                {
                    _id_cgs_sh = 0;
                }
            }
        }

        /// <summary>
        /// 收货商
        /// </summary>
        public Nullable<short> flag_fd
        {
            get
            {
                return _flag_fd;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_fd = value;
                }
                else
                {
                    _flag_fd = 0;
                }
            }
        }


        #endregion

    }
}
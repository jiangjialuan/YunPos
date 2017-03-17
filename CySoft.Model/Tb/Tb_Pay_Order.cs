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
    /// 支付中心支付单
    /// </summary>
    [Serializable]
    [Table("Tb_pay_order", "Tb_Pay_Order")]
    [DebuggerDisplay("dh = {dh}")]
    public class Tb_Pay_Order
    {
        #region public method

        public Tb_Pay_Order Clone()
        {
            return (Tb_Pay_Order)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _dh = String.Empty;
        private string _order_type = String.Empty;
        private long _id_pay_seller = 0;
        private byte _flag_state = 0;
        private string _trade_no = String.Empty;
        private string _mc_sp = String.Empty;
        private string _bz_sp = String.Empty;
        private decimal _je_pay = 0m;
        private string _buyer_account = String.Empty;
        private long _id_master = 0;
        private long _id_user = 0;
        private string _flag_from = String.Empty;
        private DateTime _rq_finish = new DateTime(1900, 1, 1);
        private DateTime _rq_submit = new DateTime(1900, 1, 1);
        private DateTime _rq_create = new DateTime(1900, 1, 1);

        #endregion

        #region public property

        /// <summary>
        /// 业务单号
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
        /// 单据类型
        /// </summary>
        public string order_type
        {
            get
            {
                return _order_type;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _order_type = value;
                }
                else
                {
                    _order_type = String.Empty;
                }
            }
        }

        /// <summary>
        /// 卖方支付方式id
        /// </summary>
        public Nullable<long> id_pay_seller
        {
            get
            {
                return _id_pay_seller;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_pay_seller = value.Value;
                }
                else
                {
                    _id_pay_seller = 0;
                }
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public Nullable<byte> flag_state
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
        /// 交易流水号
        /// </summary>
        public string trade_no
        {
            get
            {
                return _trade_no;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _trade_no = value;
                }
                else
                {
                    _trade_no = String.Empty;
                }
            }
        }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string mc_sp
        {
            get
            {
                return _mc_sp;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _mc_sp = value;
                }
                else
                {
                    _mc_sp = String.Empty;
                }
            }
        }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string bz_sp
        {
            get
            {
                return _bz_sp;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _bz_sp = value;
                }
                else
                {
                    _bz_sp = String.Empty;
                }
            }
        }

        /// <summary>
        /// 支付金额
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
        /// 买家账号
        /// </summary>
        public string buyer_account
        {
            get
            {
                return _buyer_account;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _buyer_account = value;
                }
                else
                {
                    _buyer_account = String.Empty;
                }
            }
        }

        /// <summary>
        /// 卖方主用户
        /// </summary>
        public Nullable<long> id_master
        {
            get
            {
                return _id_master;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_master = value.Value;
                }
                else
                {
                    _id_master = 0;
                }
            }
        }

        /// <summary>
        /// 买方用户id
        /// </summary>
        public Nullable<long> id_user
        {
            get
            {
                return _id_user;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_user = value.Value;
                }
                else
                {
                    _id_user = 0;
                }
            }
        }

        /// <summary>
        /// 来源
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
        /// 交易完成时间
        /// </summary>
        public Nullable<DateTime> rq_finish
        {
            get
            {
                return _rq_finish;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_finish = value.Value;
                }
                else
                {
                    _rq_finish = new DateTime(1900, 1, 1);
                }
            }
        }

        /// <summary>
        /// 交易提交时间
        /// </summary>
        public Nullable<DateTime> rq_submit
        {
            get
            {
                return _rq_submit;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_submit = value.Value;
                }
                else
                {
                    _rq_submit = new DateTime(1900, 1, 1);
                }
            }
        }

        /// <summary>
        /// 创建时间
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

        #endregion

    }
}
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
    /// 支付日志
    /// </summary>
    [Serializable]
    [Table("Tb_pay_log", "Tb_Pay_Log")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Pay_Log
    {
        #region public method

        public Tb_Pay_Log Clone()
        {
            return (Tb_Pay_Log)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id = 0;
        private long _id_pay_seller = 0;
        private string _order_type = String.Empty;
        private string _dh = String.Empty;
        private int _operate_type = 0;
        private byte _flag_state = 0;
        private string _error_code = String.Empty;
        private string _content = String.Empty;
        private string _mc_pay = String.Empty;
        private decimal _je_pay = 0m;
        private string _buyer_account = String.Empty;
        private string _flag_from = String.Empty;
        private long _id_master = 0;
        private long _id_user = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);

        #endregion

        #region public property

        /// <summary>
        /// 主键
        /// </summary>
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
        /// 支付方式id
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
        /// 单号类型
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
        /// 操作类型
        /// </summary>
        public Nullable<int> operate_type
        {
            get
            {
                return _operate_type;
            }
            set
            {
                if (value.HasValue)
                {
                    _operate_type = value.Value;
                }
                else
                {
                    _operate_type = 0;
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
        /// 错误代码
        /// </summary>
        public string error_code
        {
            get
            {
                return _error_code;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _error_code = value;
                }
                else
                {
                    _error_code = String.Empty;
                }
            }
        }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string content
        {
            get
            {
                return _content;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _content = value;
                }
                else
                {
                    _content = String.Empty;
                }
            }
        }

        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string mc_pay
        {
            get
            {
                return _mc_pay;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _mc_pay = value;
                }
                else
                {
                    _mc_pay = String.Empty;
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
        ///  创建时间
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

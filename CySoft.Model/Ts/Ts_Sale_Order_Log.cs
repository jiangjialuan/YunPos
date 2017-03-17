#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using CySoft.Model.Flags;
#endregion

#region 订单操作日志
#endregion

namespace CySoft.Model.Ts
{
    /// <summary>
    /// 订单操作日志
    /// </summary>
    [Serializable]
    [Table("Ts_sale_order_log", "Ts_Sale_Order_Log")]
    [DebuggerDisplay("id = {id}")]
    public class Ts_Sale_Order_Log
    {
        #region public method

        public Ts_Sale_Order_Log Clone()
        {
            return (Ts_Sale_Order_Log)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id = 0;
        private string _dh = String.Empty;
        private DateTime _rq = DateTime.Now;
        private long _id_user = 0;
        private long _id_user_master = 0;
        private short _flag_type = 0;
        private string _content = String.Empty;

        #endregion

        #region public property

        /// <summary>
        /// 流水号，自正增长
        /// </summary>
        /// 
        [Column(false,false)]
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
        /// 操作日期
        /// </summary>
        public Nullable<DateTime> rq
        {
            get
            {
                return _rq;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq = value.Value;
                }
                else
                {
                    _rq = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 操作人
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
        /// 操作人所属主用户
        /// </summary>
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
        /// 操作类型 bs_lx='orderlogtype'
        /// </summary>
        public Nullable<short> flag_type
        {
            get
            {
                return _flag_type;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_type = value.Value;
                }
                else
                {
                    _flag_type = 0;
                }
            }
        }

        /// <summary>
        /// 内容
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

        #endregion

    }
}
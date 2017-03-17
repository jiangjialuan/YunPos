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
    /// 卖方支付方式
    /// </summary>
    [Serializable]
    [Table("Tb_pay_seller", "Tb_Pay_Seller")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Pay_Seller
    {
        #region public method

        public Tb_Pay_Seller Clone()
        {
            return (Tb_Pay_Seller)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id = 0;
        private int _id_pay = 0;
        private long _id_master = 0;
        private string _seller_bm = String.Empty;
        private string _seller_id = String.Empty;
        private string _seller_key = String.Empty;
        private string _scope = String.Empty;
        private int _sort = 0;
        private byte _flag_stop = 0;
        private long _id_create = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private long _id_edit = 0;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private string _bz = String.Empty;

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
        public Nullable<int> id_pay
        {
            get
            {
                return _id_pay;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_pay = value.Value;
                }
                else
                {
                    _id_pay = 0;
                }
            }
        }

        /// <summary>
        /// 主用户id
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
        /// 卖方编号
        /// </summary>
        public string seller_bm
        {
            get
            {
                return _seller_bm;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _seller_bm = value;
                }
                else
                {
                    _seller_bm = String.Empty;
                }
            }
        }

        /// <summary>
        /// 卖方id
        /// </summary>
        public string seller_id
        {
            get
            {
                return _seller_id;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _seller_id = value;
                }
                else
                {
                    _seller_id = String.Empty;
                }
            }
        }

        /// <summary>
        /// 卖方key
        /// </summary>
        public string seller_key
        {
            get
            {
                return _seller_key;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _seller_key = value;
                }
                else
                {
                    _seller_key = String.Empty;
                }
            }
        }

        /// <summary>
        /// 使用范围
        /// </summary>
        public string scope
        {
            get
            {
                return _scope;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _scope = value;
                }
                else
                {
                    _scope = String.Empty;
                }
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public Nullable<int> sort
        {
            get
            {
                return _sort;
            }
            set
            {
                if (value.HasValue)
                {
                    _sort = value.Value;
                }
                else
                {
                    _sort = 0;
                }
            }
        }

        /// <summary>
        /// 停用状态
        /// </summary>
        [Column(Update = false)]
        public Nullable<byte> flag_stop
        {
            get
            {
                return _flag_stop;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_stop = value.Value;
                }
                else
                {
                    _flag_stop = 0;
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
        /// 修改人
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
        /// 修改日期
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
        /// 备注
        /// </summary>
        public string bz
        {
            get
            {
                return _bz;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _bz = value;
                }
                else
                {
                    _bz = String.Empty;
                }
            }
        }

        #endregion

    }
}
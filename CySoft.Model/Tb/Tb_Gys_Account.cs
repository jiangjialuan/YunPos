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
    /// 供应商银行账户资料
    /// </summary>
    [Serializable]
    [Table("Tb_gys_account", "Tb_Gys_Account")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Gys_Account
    {
        #region public method

        public Tb_Gys_Account Clone()
        {
            return (Tb_Gys_Account)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id = 0;
        private long _id_gys = 0;
        private string _name_bank = String.Empty;
        private string _account_bank = String.Empty;
        private string _khr = String.Empty;
        private byte _flag_stop = 0;
        private byte _flag_default = 0;
        private long _id_create = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private long _id_edit = 0;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);

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
        /// 状态  1不可以  0可用
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
        /// 1=默认 0=不默认
        /// </summary>
        public Nullable<byte> flag_default
        {
            get
            {
                return _flag_default;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_default = value.Value;
                }
                else
                {
                    _flag_default = 0;
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
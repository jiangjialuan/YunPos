#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using CySoft.Model.Flags;
#endregion

#region 采购商收货地址
#endregion

namespace CySoft.Model.Tb
{
    /// <summary>
    /// 采购商收货地址
    /// </summary>
    [Serializable]
    [Table("Tb_cgs_shdz", "Tb_Cgs_Shdz")]
    [DebuggerDisplay("id = {id}, id_cgs = {id_cgs}")]
    public class Tb_Cgs_Shdz
    {
        #region public method

        public Tb_Cgs_Shdz Clone()
        {
            return (Tb_Cgs_Shdz)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id = 0;
        private long _id_cgs = 0;
        private YesNoFlag _flag_default = YesNoFlag.No;
        private string _shr = String.Empty;
        private string _phone = String.Empty;
        private string _tel = String.Empty;
        private int _id_province = 0;
        private int _id_city = 0;
        private int _id_county = 0;
        private string _zipcode = String.Empty;
        private string _address = String.Empty;
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
        /// 所属采购商
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
        /// 是否默认    1是    0否
        /// </summary>
        public YesNoFlag flag_default
        {
            get
            {
                return _flag_default;
            }
            set
            {
                _flag_default = value;
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
        /// 手机号码
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
        /// 电话号码
        /// </summary>
        public string tel
        {
            get
            {
                return _tel;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _tel = value;
                }
                else
                {
                    _tel = String.Empty;
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
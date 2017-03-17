using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using CySoft.Model.Flags;

namespace CySoft.Model.Tb
{
    /// <summary>
    /// 编辑采购商
    /// </summary>
    [Serializable]
    [Table("Tb_cgs", "Tb_Cgs")]
    [DebuggerDisplay("id = {id}, id_user_master = {id_user_master}, flag_active = {flag_active}, username = {username}, password = {password}")]
    public class Tb_Cgs_Edit : Tb_Cgs
    {
        private YesNoFlag _flag_active = YesNoFlag.No;
        private YesNoFlag _flag_activeed = YesNoFlag.No;
        private string _username = String.Empty;
        private string _password = String.Empty;
        private string _name = String.Empty;
        private string _companyname = String.Empty;
        private string _job = String.Empty;
        private string _qq = String.Empty;
        private string _email = String.Empty;
        private string _phone = String.Empty;
        private string _tel = String.Empty;
        private string _fax = String.Empty;
        private int _id_hy = 0;
        private int _id_province = 0;
        private int _id_city = 0;
        private int _id_county = 0;
        private string _zipcode = String.Empty;
        private string _address = String.Empty;
        private DateTime _rq_treaty_start = new DateTime(1900, 1, 1);
        private DateTime _rq_treaty_end = new DateTime(1900, 1, 1);
        private long _id_cgs_shdz = 0;
        private string _bm_gys_Interface = String.Empty;
        private string _name_hy = String.Empty;
        private string _name_cgs_level = String.Empty;
        private string _name_province = String.Empty;
        private string _name_city = String.Empty;
        private string _name_county = String.Empty;
        private long _id_gys = 0;
        private long _id_gys_cgs_check = 0;
        private string _flag_from = "pc";
        private string _location_des = string.Empty;
        private string _location = string.Empty;
        public bool isFindGys { get; set; }
        //收货人
        public string shr { get; set; }

        /// <summary>
        /// 是否已激活用户
        /// </summary>
        public YesNoFlag flag_activeed
        {
            get
            {
                return _flag_activeed;
            }
            set
            {
                _flag_activeed = value;
            }
        }
        /// <summary>
        /// 是否激活用户
        /// </summary>
        public YesNoFlag flag_active
        {
            get
            {
                return _flag_active;
            }
            set
            {
                _flag_active = value;
            }
        }

        [Column(Update = false,Insert=false)]
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
                    _flag_from = "pc";
                }
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string username
        {
            get
            {
                return _username;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _username = value;
                }
                else
                {
                    _username = String.Empty;
                }
            }
        }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string password
        {
            get
            {
                return _password;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _password = value;
                }
                else
                {
                    _password = String.Empty;
                }
            }
        }

        /// <summary>
        /// 姓名
        /// </summary>
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name = value;
                }
                else
                {
                    _name = String.Empty;
                }
            }
        }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string companyname
        {
            get
            {
                return _companyname;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _companyname = value;
                }
                else
                {
                    _companyname = String.Empty;
                }
            }
        }

        /// <summary>
        /// 职务
        /// </summary>
        public string job
        {
            get
            {
                return _job;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _job = value;
                }
                else
                {
                    _job = String.Empty;
                }
            }
        }

        /// <summary>
        /// 腾讯QQ
        /// </summary>
        public string qq
        {
            get
            {
                return _qq;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _qq = value;
                }
                else
                {
                    _qq = String.Empty;
                }
            }
        }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string email
        {
            get
            {
                return _email;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _email = value;
                }
                else
                {
                    _email = String.Empty;
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
        /// 传真
        /// </summary>
        public string fax
        {
            get
            {
                return _fax;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _fax = value;
                }
                else
                {
                    _fax = String.Empty;
                }
            }
        }

        /// <summary>
        /// 行业id
        /// </summary>
        public Nullable<int> id_hy
        {
            get
            {
                return _id_hy;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_hy = value.Value;
                }
                else
                {
                    _id_hy = 0;
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
                    this._id_province = value.Value;
                }
                else
                {
                    this._id_province = 0;
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
        /// 合约有效期起
        /// </summary>
        public Nullable<DateTime> rq_treaty_start
        {
            get
            {
                return _rq_treaty_start;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_treaty_start = value.Value;
                }
                else
                {
                    _rq_treaty_start = new DateTime(1900, 1, 1);
                }
            }
        }

        /// <summary>
        /// 合约有效期止
        /// </summary>
        public Nullable<DateTime> rq_treaty_end
        {
            get
            {
                return _rq_treaty_end;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_treaty_end = value.Value;
                }
                else
                {
                    _rq_treaty_end = new DateTime(1900, 1, 1);
                }
            }
        }

        /// <summary>
        /// 默认收获地址id
        /// </summary>
        public Nullable<long> id_cgs_shdz
        {
            get
            {
                return _id_cgs_shdz;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_cgs_shdz = value.Value;
                }
                else
                {
                    _id_cgs_shdz = 0;
                }
            }
        }

        /// <summary>
        /// 供应商设置的采购商接口编码
        /// </summary>
        public string bm_gys_Interface
        {
            get
            {
                return _bm_gys_Interface;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _bm_gys_Interface = value;
                }
                else
                {
                    _bm_gys_Interface = String.Empty;
                }
            }
        }

        /// <summary>
        /// 所属行业
        /// </summary>
        public string name_hy
        {
            get
            {
                return _name_hy;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name_hy = value;
                }
                else
                {
                    _name_hy = String.Empty;
                }
            }
        }
        /// <summary>
        /// 客户级别
        /// </summary>
        public string name_cgs_level
        {
            get
            {
                return _name_cgs_level;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name_cgs_level = value;
                }
                else
                {
                    _name_cgs_level = String.Empty;
                }
            }
        }
        /// <summary>
        /// 省份
        /// </summary>
        public string name_province
        {
            get
            {
                return _name_province;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name_province = value;
                }
                else
                {
                    _name_province = String.Empty;
                }
            }
        }
        /// <summary>
        /// 城市
        /// </summary>
        public string name_city
        {
            get
            {
                return _name_city;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name_city = value;
                }
                else
                {
                    _name_city = String.Empty;
                }
            }
        }
        /// <summary>
        /// 县(区)
        /// </summary>
        public string name_county
        {
            get
            {
                return _name_county;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name_county = value;
                }
                else
                {
                    _name_county = String.Empty;
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
        /// 采购商供应商关注Id
        /// </summary>
        public Nullable<long> id_gys_cgs_check
        {
            get
            {
                return _id_gys_cgs_check;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_gys_cgs_check = value.Value;
                }
                else
                {
                    _id_gys_cgs_check = 0;
                }
            }
        }

        /// <summary>
        /// 位置经纬度
        /// </summary>        
        public string location
        {
            get
            {
                return _location;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _location = value;
                }
                else
                {
                    _location = String.Empty;
                }
            }
        }
        /// <summary>
        /// 位置经纬度描述
        /// </summary>       
        public string location_des
        {
            get
            {
                return _location_des;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _location_des = value;
                }
                else
                {
                    _location_des = String.Empty;
                }
            }
        }
    }
}

#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("tb_user", "Tb_User")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_User
    {
        #region public method

        public Tb_User Clone()
        {
            return (Tb_User)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_cyuser = String.Empty;
        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _username = String.Empty;
        private string _password = String.Empty;
        private string _name = String.Empty;
        private string _companyname = String.Empty;
        private byte _flag_sex = 0;
        private string _id_father = String.Empty;
        private string _job = String.Empty;
        private string _qq = String.Empty;
        private string _email = String.Empty;
        private string _phone = String.Empty;
        private string _tel = String.Empty;
        private string _fax = String.Empty;
        private DateTime _rq_start = new DateTime(1900, 1, 1);
        private DateTime _rq_stop = new DateTime(1900, 1, 1);
        private byte _flag_state = 0;
        private byte _flag_master = 0;
        private string _bz = String.Empty;
        private int _id_province = 0;
        private int _id_city = 0;
        private int _id_county = 0;
        private string _zipcode = String.Empty;
        private string _address = String.Empty;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _id_edit = String.Empty;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private string _id_shop=String.Empty;

        private byte _flag_delete = 0;
        private byte _flag_industry = 0;
        private string _companyno = String.Empty;
        #endregion

        #region public property
        public string companyno
        {
            get
            {
                return _companyno;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _companyno = value;
                }
                else
                {
                    _companyno = String.Empty;
                }
            }
        }

        public Nullable<byte> flag_industry
        {
            get
            {
                return _flag_industry;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_industry = value.Value;
                }
                else
                {
                    _flag_industry = 0;
                }
            }
        }
        public string id_cyuser
        {
            get
            {
                return _id_cyuser;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_cyuser = value;
                }
                else
                {
                    _id_cyuser = String.Empty;
                }
            }
        }

        public string id_masteruser
        {
            get
            {
                return _id_masteruser;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_masteruser = value;
                }
                else
                {
                    _id_masteruser = String.Empty;
                }
            }
        }

        public string id
        {
            get
            {
                return _id;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id = value;
                }
                else
                {
                    _id = String.Empty;
                }
            }
        }

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


        public string id_shop
        {
            get
            {
                return _id_shop;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_shop = value;
                }
                else
                {
                    _id_shop = String.Empty;
                }
            }
        }
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

        public Nullable<byte> flag_sex
        {
            get
            {
                return _flag_sex;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_sex = value.Value;
                }
                else
                {
                    _flag_sex = 0;
                }
            }
        }

        public string id_father
        {
            get
            {
                return _id_father;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_father = value;
                }
                else
                {
                    _id_father = String.Empty;
                }
            }
        }

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

        public byte version { get; set; }
        public Nullable<DateTime> rq_start
        {
            get
            {
                return _rq_start;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_start = value.Value;
                }
                else
                {
                    _rq_start = new DateTime(1900, 1, 1);
                }
            }
        }

        public Nullable<DateTime> rq_stop
        {
            get
            {
                return _rq_stop;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_stop = value.Value;
                }
                else
                {
                    _rq_stop = new DateTime(1900, 1, 1);
                }
            }
        }

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

        public Nullable<byte> flag_master
        {
            get
            {
                return _flag_master;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_master = value.Value;
                }
                else
                {
                    _flag_master = 0;
                }
            }
        }

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

        [Column(Update = false)]
        public string id_create
        {
            get
            {
                return _id_create;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_create = value;
                }
                else
                {
                    _id_create = String.Empty;
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

        public string id_edit
        {
            get
            {
                return _id_edit;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_edit = value;
                }
                else
                {
                    _id_edit = String.Empty;
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

        

        #endregion

    }
}



//#region Imports
//using System;
//using System.Diagnostics;
//using CySoft.Model.Flags;
//using CySoft.Model.Mapping;
//#endregion

//#region 用户
//#endregion

//namespace CySoft.Model.Tb
//{
//    /// <summary>
//    /// 用户
//    /// </summary>
//    [Serializable]
//    [Table("Tb_user", "Tb_User")]
//    [DebuggerDisplay("id = {id}, username = {username}, password = {password}")]
//    public class Tb_User
//    {
//        #region public method

//        public Tb_User Clone()
//        {
//            return (Tb_User)this.MemberwiseClone();
//        }

//        #endregion

//        #region private field

//        private string _id = String.Empty; 
//        private string _username = String.Empty;
//        private string _password = String.Empty;
//        private string _name = String.Empty;
//        private string _companyname = String.Empty;
//        private byte _flag_sex = 0;
//        private YesNoFlag _flag_master = 0;
//        private YesNoFlag _flag_stop = 0;
//        private string _id_father = String.Empty;
//        private string _job = String.Empty;
//        private string _qq = String.Empty;
//        private string _email = String.Empty;
//        private string _phone = String.Empty;
//        private string _tel = String.Empty;
//        private string _fax = String.Empty;
//        private int _id_hy = 0;
//        private int _id_province = 0;
//        private int _id_city = 0;
//        private int _id_county = 0;
//        private string _zipcode = String.Empty;
//        private string _address = String.Empty;
//        private string _id_create = String.Empty;
//        private DateTime _rq_create = new DateTime(1900, 1, 1);
//        private string _id_edit = String.Empty;
//        private DateTime _rq_edit = new DateTime(1900, 1, 1);
//        private string _pic = string.Empty;
//        private string _location_des = String.Empty;
//        private string _location = String.Empty;
//        private string _pic_erwei = String.Empty;
//        private string _id_cyuser = String.Empty;
//        #endregion

//        #region public property

//        /// <summary>
//        /// 主键
//        /// </summary>
//        public string id
//        {
//            get
//            {
//                return _id;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _id = value;
//                }
//                else
//                {
//                    _id = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 用户名
//        /// </summary>
//        public string username
//        {
//            get
//            {
//                return _username;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _username = value;
//                }
//                else
//                {
//                    _username = String.Empty;
//                }
//            }
//        }
//        public string id_cyuser
//        {
//            get
//            {
//                return _id_cyuser;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _id_cyuser = value;
//                }
//                else
//                {
//                    _id_cyuser = String.Empty;
//                }
//            }
//        }
//        /// <summary>
//        /// 登录密码
//        /// </summary>
//        public string password
//        {
//            get
//            {
//                return _password;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _password = value;
//                }
//                else
//                {
//                    _password = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 姓名
//        /// </summary>
//        public string name
//        {
//            get
//            {
//                return _name;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _name = value;
//                }
//                else
//                {
//                    _name = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 公司名称
//        /// </summary>
//        public string companyname
//        {
//            get
//            {
//                return _companyname;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _companyname = value;
//                }
//                else
//                {
//                    _companyname = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 性别    1男   2女
//        /// </summary>
//        public Nullable<byte> flag_sex
//        {
//            get
//            {
//                return _flag_sex;
//            }
//            set
//            {
//                if (value.HasValue)
//                {
//                    _flag_sex = value.Value;
//                }
//                else
//                {
//                    _flag_sex = 0;
//                }
//            }
//        }

//        /// <summary>
//        /// 是否主用户   1是  0否
//        /// </summary>
//        public YesNoFlag flag_master
//        {
//            get
//            {
//                return _flag_master;
//            }
//            set
//            {
//                _flag_master = value;
//            }
//        }

//        /// <summary>
//        /// 状态  1不可以  0可用
//        /// </summary>
//        [Column(Update = false)]
//        public YesNoFlag flag_stop
//        {
//            get
//            {
//                return _flag_stop;
//            }
//            set
//            {
//                _flag_stop = value;
//            }
//        }

//        /// <summary>
//        /// 父级用户
//        /// </summary>
//        public string id_father
//        {
//            get
//            {
//                return _id_father;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _id_father = value;
//                }
//                else
//                {
//                    _id_father = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 职务
//        /// </summary>
//        public string job
//        {
//            get
//            {
//                return _job;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _job = value;
//                }
//                else
//                {
//                    _job = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 腾讯QQ
//        /// </summary>
//        public string qq
//        {
//            get
//            {
//                return _qq;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _qq = value;
//                }
//                else
//                {
//                    _qq = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 电子邮箱
//        /// </summary>
//        public string email
//        {
//            get
//            {
//                return _email;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _email = value;
//                }
//                else
//                {
//                    _email = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 手机号码
//        /// </summary>
//        public string phone
//        {
//            get
//            {
//                return _phone;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _phone = value;
//                }
//                else
//                {
//                    _phone = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 电话号码
//        /// </summary>
//        public string tel
//        {
//            get
//            {
//                return _tel;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _tel = value;
//                }
//                else
//                {
//                    _tel = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 传真
//        /// </summary>
//        public string fax
//        {
//            get
//            {
//                return _fax;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _fax = value;
//                }
//                else
//                {
//                    _fax = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 行业id
//        /// </summary>
//        public Nullable<int> id_hy
//        {
//            get
//            {
//                return _id_hy;
//            }
//            set
//            {
//                if (value.HasValue)
//                {
//                    _id_hy = value.Value;
//                }
//                else
//                {
//                    _id_hy = 0;
//                }
//            }
//        }

//        /// <summary>
//        /// 省(直辖市、特别行政区)
//        /// </summary>
//        public Nullable<int> id_province
//        {
//            get
//            {
//                return _id_province;
//            }
//            set
//            {
//                if (value.HasValue)
//                {
//                    _id_province = value.Value;
//                }
//                else
//                {
//                    _id_province = 0;
//                }
//            }
//        }

//        /// <summary>
//        /// 市
//        /// </summary>
//        public Nullable<int> id_city
//        {
//            get
//            {
//                return _id_city;
//            }
//            set
//            {
//                if (value.HasValue)
//                {
//                    _id_city = value.Value;
//                }
//                else
//                {
//                    _id_city = 0;
//                }
//            }
//        }

//        /// <summary>
//        /// 区（县）
//        /// </summary>
//        public Nullable<int> id_county
//        {
//            get
//            {
//                return _id_county;
//            }
//            set
//            {
//                if (value.HasValue)
//                {
//                    _id_county = value.Value;
//                }
//                else
//                {
//                    _id_county = 0;
//                }
//            }
//        }

//        /// <summary>
//        /// 邮编
//        /// </summary>
//        public string zipcode
//        {
//            get
//            {
//                return _zipcode;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _zipcode = value;
//                }
//                else
//                {
//                    _zipcode = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 详细地址
//        /// </summary>
//        public string address
//        {
//            get
//            {
//                return _address;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _address = value;
//                }
//                else
//                {
//                    _address = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 创建人
//        /// </summary>
//        [Column(Update = false)]
//        public string id_create
//        {
//            get
//            {
//                return _id_create;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _id_create = value;
//                }
//                else
//                {
//                    _id_create = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 创建日期
//        /// </summary>
//        [Column(Update = false, Insert = false)]
//        public Nullable<DateTime> rq_create
//        {
//            get
//            {
//                return _rq_create;
//            }
//            set
//            {
//                if (value.HasValue)
//                {
//                    _rq_create = value.Value;
//                }
//                else
//                {
//                    _rq_create = new DateTime(1900, 1, 1);
//                }
//            }
//        }

//        /// <summary>
//        /// 最后修改人
//        /// </summary>
//        public string id_edit
//        {
//            get
//            {
//                return _id_edit;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _id_edit = value;
//                }
//                else
//                {
//                    _id_edit = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 最后修改日期
//        /// </summary>
//        [Column(Insert = false)]
//        public Nullable<DateTime> rq_edit
//        {
//            get
//            {
//                return _rq_edit;
//            }
//            set
//            {
//                if (value.HasValue)
//                {
//                    _rq_edit = value.Value;
//                }
//                else
//                {
//                    _rq_edit = new DateTime(1900, 1, 1);
//                }
//            }
//        }
//        /// <summary>
//        /// 图片
//        /// </summary>
//        [Column(Update = false, Insert=false)]
//        public string pic
//        {
//            get
//            {
//                return _pic;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _pic = value;
//                }
//                else
//                {
//                    _pic = String.Empty;
//                }
//            }
//        }
//        /// <summary>
//        /// 位置经纬度
//        /// </summary>        
//        public string location
//        {
//            get
//            {
//                return _location;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _location = value;
//                }
//                else
//                {
//                    _location = String.Empty;
//                }
//            }
//        }
//        /// <summary>
//        /// 位置经纬度描述
//        /// </summary>       
//        public string location_des
//        {
//            get
//            {
//                return _location_des;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _location_des = value;
//                }
//                else
//                {
//                    _location_des = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 二维码图片
//        /// </summary>
//        public string pic_erwei
//        {
//            get
//            {
//                return _pic_erwei;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _pic_erwei = value;
//                }
//                else
//                {
//                    _pic_erwei = String.Empty;
//                }
//            }
//        }
//        public string key_email{get;set;}

//        public Nullable<DateTime> rq_email_key{ get; set; }

//        public string flag_from { get; set; }
//        #endregion

//    }
//}
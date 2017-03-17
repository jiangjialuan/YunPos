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
    [Table("tb_shop", "Tb_Shop")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Shop
    {
        #region public method

        public Tb_Shop Clone()
        {
            return (Tb_Shop)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _bm = String.Empty;
        private string _mc = String.Empty;
        private string _qq = String.Empty;
        private string _email = String.Empty;
        private string _phone = String.Empty;
        private string _tel = String.Empty;
        private string _fax = String.Empty;
        private string _lxr = String.Empty;
        private DateTime _rq_start = new DateTime(1900, 1, 1);
        private string _zipcode = String.Empty;
        private string _address = String.Empty;
        private byte _flag_state = 0;
        private string _bz = String.Empty;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _id_edit = String.Empty;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private byte _flag_delete = 0;
        private byte[] _nlast = null;
        private string _id_cloneshop = String.Empty;
        private string _yzm = String.Empty;
        private string _pic_path = String.Empty;
        private int _discern_no = 0;

        private int _flag_type = 4;
        private string _id_shop_ps = String.Empty;
        private string _id_kh = String.Empty;
        

        #endregion

        #region public property

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

        public string bm
        {
            get
            {
                return _bm;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _bm = value;
                }
                else
                {
                    _bm = String.Empty;
                }
            }
        }

        public string mc
        {
            get
            {
                return _mc;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _mc = value;
                }
                else
                {
                    _mc = String.Empty;
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

        public string lxr
        {
            get
            {
                return _lxr;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _lxr = value;
                }
                else
                {
                    _lxr = String.Empty;
                }
            }
        }

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
        /// 正常1,关闭0
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

        /// <summary>
        /// 创建人
        /// </summary>
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

        public Byte[] nlast { get; set; }




        public string id_cloneshop
        {
            get
            {
                return _id_cloneshop;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_cloneshop = value;
                }
                else
                {
                    _id_cloneshop = String.Empty;
                }
            }
        }

        public string yzm
        {
            get
            {
                return _yzm;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _yzm = value;
                }
                else
                {
                    _yzm = String.Empty;
                }
            }
        }

        public string pic_path
        {
            get
            {
                return _pic_path;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _pic_path = value;
                }
                else
                {
                    _pic_path = String.Empty;
                }
            }
        }
        [Column(Insert = false)]
        public Nullable<int> discern_no
        {
            get
            {
                return _discern_no;
            }
            set
            {
                if (value.HasValue)
                {
                    _discern_no = value.Value;
                }
                else
                {
                    _discern_no = 0;
                }
            }
        }

        /// <summary>
        /// 门店类型： 1总部 2分公司 3配送中心 4直营店 5加盟店
        /// </summary>
        public int? flag_type
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
        /// 配送中心
        /// </summary>
        public string id_shop_ps
        {
            get
            {
                return _id_shop_ps;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_shop_ps = value;
                }
                else
                {
                    _id_shop_ps = String.Empty;
                }
            }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public string id_kh
        {
            get
            {
                return _id_kh;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_kh = value;
                }
                else
                {
                    _id_kh = String.Empty;
                }
            }
        }

        #endregion

    }

    public class Tb_ShopWithFatherId : Tb_Shop
    {
        public string id_shop_father { get; set; }
    }

}

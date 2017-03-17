#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using System.Collections.Generic;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("tb_kh", "Tb_Kh")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Kh
    {
        #region public method

        public Tb_Kh Clone()
        {
            return (Tb_Kh)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _bm = String.Empty;
        private string _mc = String.Empty;
        private string _id_khfl = String.Empty;
        private string _companytel = String.Empty;
        private string _zjm = String.Empty;
        private string _tel = String.Empty;
        private string _lxr = String.Empty;
        private string _email = String.Empty;
        private string _zipcode = String.Empty;
        private string _address = String.Empty;
        private decimal _je_xyed = 0m;
        private decimal _je_xyed_temp = 0m;
        private DateTime _rq_xyed_temp_b = new DateTime(1900, 1, 1);
        private DateTime _rq_xyed_temp_e = new DateTime(1900, 1, 1);
        private string _id_shop_relate = String.Empty;
        private byte _flag_state = 0;
        private string _bz = String.Empty;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _id_edit = String.Empty;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private byte _flag_delete = 0;
        private byte[] _nlast;

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

        public string id_khfl
        {
            get
            {
                return _id_khfl;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_khfl = value;
                }
                else
                {
                    _id_khfl = String.Empty;
                }
            }
        }

        public string companytel
        {
            get
            {
                return _companytel;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _companytel = value;
                }
                else
                {
                    _companytel = String.Empty;
                }
            }
        }

        public string zjm
        {
            get
            {
                return _zjm;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _zjm = value;
                }
                else
                {
                    _zjm = String.Empty;
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

        public Nullable<decimal> je_xyed
        {
            get
            {
                return _je_xyed;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_xyed = value.Value;
                }
                else
                {
                    _je_xyed = 0m;
                }
            }
        }

        public Nullable<decimal> je_xyed_temp
        {
            get
            {
                return _je_xyed_temp;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_xyed_temp = value.Value;
                }
                else
                {
                    _je_xyed_temp = 0m;
                }
            }
        }

        public Nullable<DateTime> rq_xyed_temp_b
        {
            get
            {
                return _rq_xyed_temp_b;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_xyed_temp_b = value.Value;
                }
                else
                {
                    _rq_xyed_temp_b = new DateTime(1900, 1, 1);
                }
            }
        }

        public Nullable<DateTime> rq_xyed_temp_e
        {
            get
            {
                return _rq_xyed_temp_e;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_xyed_temp_e = value.Value;
                }
                else
                {
                    _rq_xyed_temp_e = new DateTime(1900, 1, 1);
                }
            }
        }

        public string id_shop_relate
        {
            get
            {
                return _id_shop_relate;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_shop_relate = value;
                }
                else
                {
                    _id_shop_relate = String.Empty;
                }
            }
        }

        /// <summary>
        /// 正常1,停用0
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

        #endregion

    }

    public class Tb_Kh_Query: Tb_Kh
    {
        
            public string mc_khfl { set; get; }
        public string mc_shop { set; get; }
    }
    public class Tb_Kh_Import
    {
        public string id_khfl { set; get; }
        public string mc { set; get; }
        public string bm { set; get; }
        public string flag_state { set; get; }
        public string lxr { set; get; }
        public string tel { set; get; }
        public string companytel { set; get; }
        public string email { set; get; }
        public string zipcode { set; get; }
        public string address { set; get; }
        public string bz { set; get; }
        public string bz_kh { set; get; }
        public string je_xyed { set; get; }
        public string je_xyed_temp { set; get; }
        public string rq_xyed_temp_b { set; get; }
        public string rq_xyed_temp_e { set; get; }
    }

    public class Tb_Kh_Import_All
    {
        public List<Tb_Kh_Import> SuccessList { set; get; }
        public List<Tb_Kh_Import> FailList { set; get; }

    }



}
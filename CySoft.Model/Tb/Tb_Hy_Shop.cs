using CySoft.Model.Mapping;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("tb_hy_shop", "Tb_Hy_Shop")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Hy_Shop
    {
        #region public method

        public Tb_Hy_Shop Clone()
        {
            return (Tb_Hy_Shop)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_shop = String.Empty;
        private string _id_hy = String.Empty;
        private string _membercard = String.Empty;
        private string _phone = String.Empty;
        private string _id_hyfl = String.Empty;
        private byte _flag_yhlx = 0;
        private decimal _zk = 0m;
        private DateTime _rq_b = new DateTime(1900, 1, 1);
        private DateTime _rq_e = new DateTime(1900, 1, 1);
        private int _flag_stop = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);

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

        public string id_hy
        {
            get
            {
                return _id_hy;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_hy = value;
                }
                else
                {
                    _id_hy = String.Empty;
                }
            }
        }

        public string membercard
        {
            get
            {
                return _membercard;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _membercard = value;
                }
                else
                {
                    _membercard = String.Empty;
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

        public string id_hyfl
        {
            get
            {
                return _id_hyfl;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_hyfl = value;
                }
                else
                {
                    _id_hyfl = String.Empty;
                }
            }
        }

        public Nullable<byte> flag_yhlx
        {
            get
            {
                return _flag_yhlx;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_yhlx = value.Value;
                }
                else
                {
                    _flag_yhlx = 0;
                }
            }
        }

        public Nullable<decimal> zk
        {
            get
            {
                return _zk;
            }
            set
            {
                if (value.HasValue)
                {
                    _zk = value.Value;
                }
                else
                {
                    _zk = 0m;
                }
            }
        }

        public Nullable<DateTime> rq_b
        {
            get
            {
                return _rq_b;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_b = value.Value;
                }
                else
                {
                    _rq_b = new DateTime(1900, 1, 1);
                }
            }
        }

        public Nullable<DateTime> rq_e
        {
            get
            {
                return _rq_e;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_e = value.Value;
                }
                else
                {
                    _rq_e = new DateTime(1900, 1, 1);
                }
            }
        }

        [Column(Update = false)]
        public Nullable<int> flag_stop
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



    public class Tb_Hy_Shop_Query : Tb_Hy_Shop
    {
        public decimal? je_hy { set; get; }
        public decimal? jf_hy { set; get; }
        public string hyfl_mc { set; get; }
        public string hyfl_bm { set; get; }



        public string hy_password { set; get; }
        public string hy_name { set; get; }
        public string hy_qq { set; get; }
        public string hy_email { set; get; }
        public string hy_tel { set; get; }
        public string hy_address { set; get; }
        public string hy_MMno { set; get; }
        public string hy_zipcode { set; get; }
        public DateTime? hy_birthday { set; get; }
        public string hy_hysr { set; get; }
        public string hy_hysr_cn { set; get; }
        public byte? hy_flag_nl { set; get; }
        public string hy_id_shop_create { set; get; }
        public byte? hy_flag_sex { set; get; }

        public  decimal? je_qm { set; get; }
        public decimal? je_qm_zs { set; get; }
        public decimal? jf_qm { set; get; }
        

    }


}

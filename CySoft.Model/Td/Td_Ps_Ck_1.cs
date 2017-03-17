using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Mapping;

namespace CySoft.Model.Td
{
    [Serializable]
    [Table("td_ps_ck_1", "Td_Ps_Ck_1")]
    [DebuggerDisplay("id = {id}")]
    public class Td_Ps_Ck_1
    {
        #region public method

        public Td_Ps_Ck_1 Clone()
        {
            return (Td_Ps_Ck_1)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _dh = String.Empty;
        private DateTime _rq = new DateTime(1900, 1, 1);
        private string _id_shop = String.Empty;
        private string _bm_djlx = String.Empty;
        private string _bm_djlx_origin = String.Empty;
        private string _id_bill_origin = String.Empty;
        private string _dh_origin = String.Empty;
        private string _id_shop_rk = String.Empty;
        private string _id_shop_sk = String.Empty;
        private string _id_jbr = String.Empty;
        private decimal _je_mxtotal = 0m;
        private DateTime _rq_sh = new DateTime(1900, 1, 1);
        private byte _flag_sh = 0;
        private string _id_user_sh = String.Empty;
        private byte _flag_cancel = 0;
        private string _bz = String.Empty;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _id_edit = String.Empty;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private byte _flag_delete = 0;
        //private string _id_shop_ck = String.Empty;
        private decimal _je_cb = 0m;
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

        //public string id_shop_ck
        //{
        //    get
        //    {
        //        return _id_shop_ck;
        //    }
        //    set
        //    {
        //        if (!String.IsNullOrEmpty(value))
        //        {
        //            _id_shop_ck = value;
        //        }
        //        else
        //        {
        //            _id_shop_ck = String.Empty;
        //        }
        //    }
        //}
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
                    _rq = new DateTime(1900, 1, 1);
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

        public string bm_djlx
        {
            get
            {
                return _bm_djlx;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _bm_djlx = value;
                }
                else
                {
                    _bm_djlx = String.Empty;
                }
            }
        }

        public string bm_djlx_origin
        {
            get
            {
                return _bm_djlx_origin;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _bm_djlx_origin = value;
                }
                else
                {
                    _bm_djlx_origin = String.Empty;
                }
            }
        }

        public string id_bill_origin
        {
            get
            {
                return _id_bill_origin;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_bill_origin = value;
                }
                else
                {
                    _id_bill_origin = String.Empty;
                }
            }
        }

        public string dh_origin
        {
            get
            {
                return _dh_origin;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _dh_origin = value;
                }
                else
                {
                    _dh_origin = String.Empty;
                }
            }
        }

        public string id_shop_rk
        {
            get
            {
                return _id_shop_rk;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_shop_rk = value;
                }
                else
                {
                    _id_shop_rk = String.Empty;
                }
            }
        }
        public string id_shop_sk
        {
            get
            {
                return _id_shop_sk;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_shop_sk = value;
                }
                else
                {
                    _id_shop_sk = String.Empty;
                }
            }
        }
        public string id_jbr
        {
            get
            {
                return _id_jbr;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_jbr = value;
                }
                else
                {
                    _id_jbr = String.Empty;
                }
            }
        }

        public Nullable<decimal> je_mxtotal
        {
            get
            {
                return _je_mxtotal;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_mxtotal = value.Value;
                }
                else
                {
                    _je_mxtotal = 0m;
                }
            }
        }

        public Nullable<DateTime> rq_sh
        {
            get
            {
                return _rq_sh;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_sh = value.Value;
                }
                else
                {
                    _rq_sh = new DateTime(1900, 1, 1);
                }
            }
        }

        public Nullable<byte> flag_sh
        {
            get
            {
                return _flag_sh;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_sh = value.Value;
                }
                else
                {
                    _flag_sh = 0;
                }
            }
        }

        public string id_user_sh
        {
            get
            {
                return _id_user_sh;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_user_sh = value;
                }
                else
                {
                    _id_user_sh = String.Empty;
                }
            }
        }

        public Nullable<byte> flag_cancel
        {
            get
            {
                return _flag_cancel;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_cancel = value.Value;
                }
                else
                {
                    _flag_cancel = 0;
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
        public Nullable<decimal> je_cb
        {
            get
            {
                return _je_cb;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_cb = value.Value;
                }
                else
                {
                    _je_cb = 0m;
                }
            }
        }
        #endregion

    }

    [Serializable]
    [Table("td_ps_ck_1", "Td_Ps_Ck_1")]
    [DebuggerDisplay("id = {id}")]
    public class Td_Ps_Ck_1_Query : Td_Ps_Ck_1
    {
        /// <summary>
        /// 制单人
        /// </summary>
        public string zdr { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string shr { get; set; }
        /// <summary>
        /// 开单门店
        /// </summary>
        public string shop_name { get; set; }
        /// <summary>
        /// 经办人名称
        /// </summary>
        public string jbr_name { get; set; }

    }

    

}

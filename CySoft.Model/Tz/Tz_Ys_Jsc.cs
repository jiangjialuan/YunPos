#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Tz
{
    [Serializable]
    [Table("tz_ys_jsc", "Tz_Ys_Jsc")]
    [DebuggerDisplay("id = {id}")]
    public class Tz_Ys_Jsc
    {
        #region public method

        public Tz_Ys_Jsc Clone()
        {
            return (Tz_Ys_Jsc)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_shop = String.Empty;
        private string _id_kh = String.Empty;
        private string _id_bill = String.Empty;
        private string _dh = String.Empty;
        private DateTime _rq = new DateTime(1900, 1, 1);
        private string _bm_djlx = String.Empty;
        private string _bz = String.Empty;
        private decimal _je = 0m;
        private decimal _je_sk = 0m;
        private decimal _je_yh = 0m;
        private decimal _je_ws = 0m;
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

        public string id_bill
        {
            get
            {
                return _id_bill;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_bill = value;
                }
                else
                {
                    _id_bill = String.Empty;
                }
            }
        }

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

        /// <summary>
        /// 除原单备注外，可增加其他单据信息
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

        public Nullable<decimal> je
        {
            get
            {
                return _je;
            }
            set
            {
                if (value.HasValue)
                {
                    _je = value.Value;
                }
                else
                {
                    _je = 0m;
                }
            }
        }

        /// <summary>
        /// 累计的已收金额
        /// </summary>
        public Nullable<decimal> je_sk
        {
            get
            {
                return _je_sk;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_sk = value.Value;
                }
                else
                {
                    _je_sk = 0m;
                }
            }
        }

        /// <summary>
        /// 累计的优惠金额
        /// </summary>
        public Nullable<decimal> je_yh
        {
            get
            {
                return _je_yh;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_yh = value.Value;
                }
                else
                {
                    _je_yh = 0m;
                }
            }
        }

        /// <summary>
        /// je_ws = je - je_sk - je_yh
        /// </summary>
        public Nullable<decimal> je_ws
        {
            get
            {
                return _je_ws;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_ws = value.Value;
                }
                else
                {
                    _je_ws = 0m;
                }
            }
        }

        public byte[] nlast { set; get; }
        

        #endregion

    }

    public class Tz_Ys_Jsc_QueryModel : Tz_Ys_Jsc
    {
        string shop_name { set; get; }
        public string kh_name { set; get; }
        public string jbr_name { set; get; }
        public string bm_djlx_name { set; get; }
    }



}
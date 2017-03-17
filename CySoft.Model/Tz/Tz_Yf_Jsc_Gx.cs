using CySoft.Model.Mapping;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Tz
{
    [Serializable]
    [Table("tz_yf_jsc_gx", "Tz_Yf_Jsc_Gx")]
    [DebuggerDisplay("id = {id}")]
    public class Tz_Yf_Jsc_Gx
    {
        #region public method

        public Tz_Yf_Jsc_Gx Clone()
        {
            return (Tz_Yf_Jsc_Gx)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_shop = String.Empty;
        private string _id_gys = String.Empty;
        private string _id_bill = String.Empty;
        private string _dh = String.Empty;
        private DateTime _rq = new DateTime(1900, 1, 1);
        private string _bm_djlx = String.Empty;
        private string _bz = String.Empty;
        private decimal _je = 0m;
        private decimal _je_fk = 0m;
        private decimal _je_yh = 0m;
        private decimal _je_wf = 0m;

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

        public string id_gys
        {
            get
            {
                return _id_gys;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_gys = value;
                }
                else
                {
                    _id_gys = String.Empty;
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
        /// 累计的已付金额
        /// </summary>
        public Nullable<decimal> je_fk
        {
            get
            {
                return _je_fk;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_fk = value.Value;
                }
                else
                {
                    _je_fk = 0m;
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
        /// je_wf = je - je_fk - je_yh
        /// </summary>
        public Nullable<decimal> je_wf
        {
            get
            {
                return _je_wf;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_wf = value.Value;
                }
                else
                {
                    _je_wf = 0m;
                }
            }
        }

        #endregion

    }


    public class Tz_Yf_Jsc_Gx_QueryModel : Tz_Yf_Jsc_Gx
    {
        string shop_name { set; get; }
        public string gys_name { set; get; }
        public string jbr_name { set; get; }
        public string bm_djlx_name { set; get; }
        string bz_origin { set; get; }
    }

}

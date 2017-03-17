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
    [Table("td_ls_2", "Td_Ls_2")]
    [DebuggerDisplay("id = {id}")]
    public class Td_Ls_2
    {
        #region public method

        public Td_Ls_2 Clone()
        {
            return (Td_Ls_2)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_bill = String.Empty;
        private int _sort_id = 0;
        private string _id_shopsp = String.Empty;
        private string _id_kcsp = String.Empty;
        private decimal _zhl = 0m;
        private decimal _sl = 0m;
        private decimal _sl_total = 0m;
        private string _barcode = String.Empty;
        private string _dw = String.Empty;
        private decimal _dj = 0m;
        private decimal _dj_hy = 0m;
        private decimal _dj_ls = 0m;
        private decimal _je = 0m;
        private decimal _zk = 0m;
        private decimal _je_zk = 0m;
        private decimal _je_yh = 0m;
        private decimal _je_yh_total = 0m;
        private decimal _je_yh_fp = 0m;
        private decimal _je_ys = 0m;
        private decimal _je_cb = 0m;
        private decimal _je_ml = 0m;
        private decimal _sl_cx = 0m;
        private string _id_billcx = String.Empty;
        private string _rulename_cx = String.Empty;
        private decimal _je_yh_cx = 0m;
        private decimal _sl_cx_zsz = 0m;
        private string _id_billcx_zsz = String.Empty;
        private decimal _je_yh_cx_zsz = 0m;
        private string _rulename_cx_zsz = String.Empty;
        private string _bz = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private decimal _jf_sp = 0m;
        private decimal _jf_yd = 0m;

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

        public Nullable<int> sort_id
        {
            get
            {
                return _sort_id;
            }
            set
            {
                if (value.HasValue)
                {
                    _sort_id = value.Value;
                }
                else
                {
                    _sort_id = 0;
                }
            }
        }

        public string id_shopsp
        {
            get
            {
                return _id_shopsp;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_shopsp = value;
                }
                else
                {
                    _id_shopsp = String.Empty;
                }
            }
        }

        public string id_kcsp
        {
            get
            {
                return _id_kcsp;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_kcsp = value;
                }
                else
                {
                    _id_kcsp = String.Empty;
                }
            }
        }

        public Nullable<decimal> zhl
        {
            get
            {
                return _zhl;
            }
            set
            {
                if (value.HasValue)
                {
                    _zhl = value.Value;
                }
                else
                {
                    _zhl = 0m;
                }
            }
        }

        public Nullable<decimal> sl
        {
            get
            {
                return _sl;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl = value.Value;
                }
                else
                {
                    _sl = 0m;
                }
            }
        }

        public Nullable<decimal> sl_total
        {
            get
            {
                return _sl_total;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_total = value.Value;
                }
                else
                {
                    _sl_total = 0m;
                }
            }
        }

        public string barcode
        {
            get
            {
                return _barcode;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _barcode = value;
                }
                else
                {
                    _barcode = String.Empty;
                }
            }
        }

        public string dw
        {
            get
            {
                return _dw;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _dw = value;
                }
                else
                {
                    _dw = String.Empty;
                }
            }
        }

        public Nullable<decimal> dj
        {
            get
            {
                return _dj;
            }
            set
            {
                if (value.HasValue)
                {
                    _dj = value.Value;
                }
                else
                {
                    _dj = 0m;
                }
            }
        }

        public Nullable<decimal> dj_hy
        {
            get
            {
                return _dj_hy;
            }
            set
            {
                if (value.HasValue)
                {
                    _dj_hy = value.Value;
                }
                else
                {
                    _dj_hy = 0m;
                }
            }
        }

        public Nullable<decimal> dj_ls
        {
            get
            {
                return _dj_ls;
            }
            set
            {
                if (value.HasValue)
                {
                    _dj_ls = value.Value;
                }
                else
                {
                    _dj_ls = 0m;
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

        public Nullable<decimal> je_zk
        {
            get
            {
                return _je_zk;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_zk = value.Value;
                }
                else
                {
                    _je_zk = 0m;
                }
            }
        }

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

        public Nullable<decimal> je_yh_total
        {
            get
            {
                return _je_yh_total;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_yh_total = value.Value;
                }
                else
                {
                    _je_yh_total = 0m;
                }
            }
        }

        public Nullable<decimal> je_yh_fp
        {
            get
            {
                return _je_yh_fp;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_yh_fp = value.Value;
                }
                else
                {
                    _je_yh_fp = 0m;
                }
            }
        }

        public Nullable<decimal> je_ys
        {
            get
            {
                return _je_ys;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_ys = value.Value;
                }
                else
                {
                    _je_ys = 0m;
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

        public Nullable<decimal> je_ml
        {
            get
            {
                return _je_ml;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_ml = value.Value;
                }
                else
                {
                    _je_ml = 0m;
                }
            }
        }

        public Nullable<decimal> sl_cx
        {
            get
            {
                return _sl_cx;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_cx = value.Value;
                }
                else
                {
                    _sl_cx = 0m;
                }
            }
        }

        public string id_billcx
        {
            get
            {
                return _id_billcx;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_billcx = value;
                }
                else
                {
                    _id_billcx = String.Empty;
                }
            }
        }

        public string rulename_cx
        {
            get
            {
                return _rulename_cx;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _rulename_cx = value;
                }
                else
                {
                    _rulename_cx = String.Empty;
                }
            }
        }

        public Nullable<decimal> je_yh_cx
        {
            get
            {
                return _je_yh_cx;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_yh_cx = value.Value;
                }
                else
                {
                    _je_yh_cx = 0m;
                }
            }
        }

        public Nullable<decimal> sl_cx_zsz
        {
            get
            {
                return _sl_cx_zsz;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_cx_zsz = value.Value;
                }
                else
                {
                    _sl_cx_zsz = 0m;
                }
            }
        }

        public string id_billcx_zsz
        {
            get
            {
                return _id_billcx_zsz;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_billcx_zsz = value;
                }
                else
                {
                    _id_billcx_zsz = String.Empty;
                }
            }
        }

        public Nullable<decimal> je_yh_cx_zsz
        {
            get
            {
                return _je_yh_cx_zsz;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_yh_cx_zsz = value.Value;
                }
                else
                {
                    _je_yh_cx_zsz = 0m;
                }
            }
        }

        public string rulename_cx_zsz
        {
            get
            {
                return _rulename_cx_zsz;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _rulename_cx_zsz = value;
                }
                else
                {
                    _rulename_cx_zsz = String.Empty;
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

        public Nullable<decimal> jf_sp
        {
            get
            {
                return _jf_sp;
            }
            set
            {
                if (value.HasValue)
                {
                    _jf_sp = value.Value;
                }
                else
                {
                    _jf_sp = 0m;
                }
            }
        }

        public Nullable<decimal> jf_yd
        {
            get
            {
                return _jf_yd;
            }
            set
            {
                if (value.HasValue)
                {
                    _jf_yd = value.Value;
                }
                else
                {
                    _jf_yd = 0m;
                }
            }
        }

        #endregion

    }
}

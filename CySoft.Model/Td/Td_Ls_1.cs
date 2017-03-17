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
    [Table("td_ls_1", "Td_Ls_1")]
    [DebuggerDisplay("id = {id}")]
    public class Td_Ls_1
    {
        #region public method

        public Td_Ls_1 Clone()
        {
            return (Td_Ls_1)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _dh = String.Empty;
        private DateTime _rq = new DateTime(1900, 1, 1);
        private string _id_shop = String.Empty;
        private string _id_bill_origin = String.Empty;
        private string _id_lszd = String.Empty;
        private string _id_lsjb = String.Empty;
        private string _id_jbr = String.Empty;
        private string _bz = String.Empty;
        private decimal _zk = 0m;
        private decimal _je_yhzd = 0m;
        private decimal _je_yh_mxtotal = 0m;
        private decimal _je_yh_total = 0m;
        private decimal _je_mxtotal = 0m;
        private decimal _je_ys_mxtotal = 0m;
        private decimal _je_ys = 0m;
        private decimal _je_ss = 0m;
        private decimal _je_zh = 0m;
        private decimal _jf_sp_mxtotal = 0m;
        private decimal _jf_yd_mxtotal = 0m;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _id_edit = String.Empty;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private string _id_hy = String.Empty;

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

        public string id_lszd
        {
            get
            {
                return _id_lszd;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_lszd = value;
                }
                else
                {
                    _id_lszd = String.Empty;
                }
            }
        }

        public string id_lsjb
        {
            get
            {
                return _id_lsjb;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_lsjb = value;
                }
                else
                {
                    _id_lsjb = String.Empty;
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

        public Nullable<decimal> je_yhzd
        {
            get
            {
                return _je_yhzd;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_yhzd = value.Value;
                }
                else
                {
                    _je_yhzd = 0m;
                }
            }
        }

        public Nullable<decimal> je_yh_mxtotal
        {
            get
            {
                return _je_yh_mxtotal;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_yh_mxtotal = value.Value;
                }
                else
                {
                    _je_yh_mxtotal = 0m;
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

        public Nullable<decimal> je_ys_mxtotal
        {
            get
            {
                return _je_ys_mxtotal;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_ys_mxtotal = value.Value;
                }
                else
                {
                    _je_ys_mxtotal = 0m;
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

        public Nullable<decimal> je_ss
        {
            get
            {
                return _je_ss;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_ss = value.Value;
                }
                else
                {
                    _je_ss = 0m;
                }
            }
        }

        public Nullable<decimal> je_zh
        {
            get
            {
                return _je_zh;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_zh = value.Value;
                }
                else
                {
                    _je_zh = 0m;
                }
            }
        }

        public Nullable<decimal> jf_sp_mxtotal
        {
            get
            {
                return _jf_sp_mxtotal;
            }
            set
            {
                if (value.HasValue)
                {
                    _jf_sp_mxtotal = value.Value;
                }
                else
                {
                    _jf_sp_mxtotal = 0m;
                }
            }
        }

        public Nullable<decimal> jf_yd_mxtotal
        {
            get
            {
                return _jf_yd_mxtotal;
            }
            set
            {
                if (value.HasValue)
                {
                    _jf_yd_mxtotal = value.Value;
                }
                else
                {
                    _jf_yd_mxtotal = 0m;
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

        #endregion

    }
}

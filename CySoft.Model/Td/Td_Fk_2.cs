using CySoft.Model.Mapping;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Td
{

    [Serializable]
    [Table("td_fk_2", "Td_Fk_2")]
    [DebuggerDisplay("id = {id}")]
    public class Td_Fk_2
    {
        #region public method

        public Td_Fk_2 Clone()
        {
            return (Td_Fk_2)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_bill = String.Empty;
        private string _dh = String.Empty;
        private int _xh = 0;
        private int _sort_id = 0;
        private string _id_bill_origin = String.Empty;
        private decimal _je_origin = 0m;
        private decimal _je_yf = 0m;
        private decimal _je_wf = 0m;
        private decimal _je_yh = 0m;
        private decimal _je_fk = 0m;
        private string _bz = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);

        private string _dh_origin = String.Empty;
        private string _bm_djlx_origin = String.Empty;

        
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

        public Nullable<int> xh
        {
            get
            {
                return _xh;
            }
            set
            {
                if (value.HasValue)
                {
                    _xh = value.Value;
                }
                else
                {
                    _xh = 0;
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

        /// <summary>
        /// 原单包括(进货单,退货单,期初单等)
        /// </summary>
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

        /// <summary>
        /// 原单上的实付金额
        /// </summary>
        public Nullable<decimal> je_origin
        {
            get
            {
                return _je_origin;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_origin = value.Value;
                }
                else
                {
                    _je_origin = 0m;
                }
            }
        }

        /// <summary>
        /// 累计的已付金额
        /// </summary>
        public Nullable<decimal> je_yf
        {
            get
            {
                return _je_yf;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_yf = value.Value;
                }
                else
                {
                    _je_yf = 0m;
                }
            }
        }

        /// <summary>
        /// je_wf = je_origin - je_yf
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
      

        #endregion

    }

    public class Td_Fk_2_QueryModel : Td_Fk_2
    {
        public decimal? dj_jh { set; get; }
        public decimal? dj_ls { set; get; }
        public string shopsp_name { set; get; }
        public string bm_djlx_name { set; get; }

    }

   

}

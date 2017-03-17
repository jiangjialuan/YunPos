#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Td
{
    [Serializable]
    [Table("td_sk_2", "Td_Sk_2")]
    [DebuggerDisplay("id = {id}")]
    public class Td_Sk_2
    {
        #region public method

        public Td_Sk_2 Clone()
        {
            return (Td_Sk_2)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_bill = String.Empty;
        private int _sort_id = 0;
        private string _id_bill_origin = String.Empty;
        private string _bm_djlx_origin = String.Empty;
        private string _dh_origin = String.Empty;
        private decimal _je_origin = 0m;
        private decimal _je_ys = 0m;
        private decimal _je_ws = 0m;
        private decimal _je_yh = 0m;
        private decimal _je_sk = 0m;
        private string _bz = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
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

        /// <summary>
        /// 原单包括(配送出库单、返配入库单、批发单等)
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

        /// <summary>
        /// 原单上的实 收金额
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
        /// 累计的（ 收款金额 +优惠金额）
        /// </summary>
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

        /// <summary>
        /// je_ws= je_origin - je_ys
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

        public byte[] nlast { get; set; }

        #endregion

    }

    public class Td_Sk_2_QueryModel : Td_Sk_2
    {


    }



}

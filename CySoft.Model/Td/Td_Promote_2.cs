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
    [Table("td_promote_2", "Td_Promote_2")]
    [DebuggerDisplay("id = {id}")]
    public class Td_Promote_2
    {
        #region public method

        public Td_Promote_2 Clone()
        {
            return (Td_Promote_2)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_bill = String.Empty;
        private int _sort_id = 0;
        private string _id_object = String.Empty;
        private decimal _sl_largess_1 = 0m;
        private decimal _sl_largess_2 = 0m;
        private decimal _sl_largess_3 = 0m;
        private decimal _condition_1 = 0m;
        private decimal _condition_2 = 0m;
        private decimal _condition_3 = 0m;
        private decimal _result_1 = 0m;
        private decimal _result_2 = 0m;
        private decimal _result_3 = 0m;
        private decimal _dj_ls = 0m;
        private string _zh_group = String.Empty;
        private decimal _dj_hy = 0m;
        private string _dw = String.Empty;
        private decimal _dj_jh = 0m;
        private decimal _sl_billxl = 0m;
        private string _bz = String.Empty;
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

        public string id_object
        {
            get
            {
                return _id_object;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_object = value;
                }
                else
                {
                    _id_object = String.Empty;
                }
            }
        }

        

        public Nullable<decimal> sl_largess_1
        {
            get
            {
                return _sl_largess_1;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_largess_1 = value.Value;
                }
                else
                {
                    _sl_largess_1 = 0m;
                }
            }
        }

        public Nullable<decimal> sl_largess_2
        {
            get
            {
                return _sl_largess_2;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_largess_2 = value.Value;
                }
                else
                {
                    _sl_largess_2 = 0m;
                }
            }
        }

        public Nullable<decimal> sl_largess_3
        {
            get
            {
                return _sl_largess_3;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_largess_3 = value.Value;
                }
                else
                {
                    _sl_largess_3 = 0m;
                }
            }
        }

        public Nullable<decimal> condition_1
        {
            get
            {
                return _condition_1;
            }
            set
            {
                if (value.HasValue)
                {
                    _condition_1 = value.Value;
                }
                else
                {
                    _condition_1 = 0m;
                }
            }
        }

        public Nullable<decimal> condition_2
        {
            get
            {
                return _condition_2;
            }
            set
            {
                if (value.HasValue)
                {
                    _condition_2 = value.Value;
                }
                else
                {
                    _condition_2 = 0m;
                }
            }
        }

        public Nullable<decimal> condition_3
        {
            get
            {
                return _condition_3;
            }
            set
            {
                if (value.HasValue)
                {
                    _condition_3 = value.Value;
                }
                else
                {
                    _condition_3 = 0m;
                }
            }
        }

        public Nullable<decimal> result_1
        {
            get
            {
                return _result_1;
            }
            set
            {
                if (value.HasValue)
                {
                    _result_1 = value.Value;
                }
                else
                {
                    _result_1 = 0m;
                }
            }
        }

        public Nullable<decimal> result_2
        {
            get
            {
                return _result_2;
            }
            set
            {
                if (value.HasValue)
                {
                    _result_2 = value.Value;
                }
                else
                {
                    _result_2 = 0m;
                }
            }
        }

        public Nullable<decimal> result_3
        {
            get
            {
                return _result_3;
            }
            set
            {
                if (value.HasValue)
                {
                    _result_3 = value.Value;
                }
                else
                {
                    _result_3 = 0m;
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

        public string zh_group
        {
            get
            {
                return _zh_group;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _zh_group = value;
                }
                else
                {
                    _zh_group = String.Empty;
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

        public Nullable<decimal> dj_jh
        {
            get
            {
                return _dj_jh;
            }
            set
            {
                if (value.HasValue)
                {
                    _dj_jh = value.Value;
                }
                else
                {
                    _dj_jh = 0m;
                }
            }
        }

        public Nullable<decimal> sl_billxl
        {
            get
            {
                return _sl_billxl;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_billxl = value.Value;
                }
                else
                {
                    _sl_billxl = 0m;
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

        #endregion

    }

    public class Td_Promote_2_Query : Td_Promote_2
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string mc { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string barcode { get; set; }
    }
}

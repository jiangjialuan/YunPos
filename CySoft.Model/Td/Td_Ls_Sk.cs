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
    [Table("td_ls_sk", "Td_Ls_Sk")]
    [DebuggerDisplay("id = {id}")]
    public class Td_Ls_Sk
    {
        #region public method

        public Td_Ls_Sk Clone()
        {
            return (Td_Ls_Sk)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_bill = String.Empty;
        private int _sort_id = 0;
        private string _id_pay = String.Empty;
        private decimal _je = 0m;
        private byte _flag_sk = 0;
        private string _dh_pay = String.Empty;
        private string _id_hy = String.Empty;
        private string _numbercard = String.Empty;
        private decimal _je_ye = 0m;
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

        public string id_pay
        {
            get
            {
                return _id_pay;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_pay = value;
                }
                else
                {
                    _id_pay = String.Empty;
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

        public Nullable<byte> flag_sk
        {
            get
            {
                return _flag_sk;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_sk = value.Value;
                }
                else
                {
                    _flag_sk = 0;
                }
            }
        }

        public string dh_pay
        {
            get
            {
                return _dh_pay;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _dh_pay = value;
                }
                else
                {
                    _dh_pay = String.Empty;
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

        public string numbercard
        {
            get
            {
                return _numbercard;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _numbercard = value;
                }
                else
                {
                    _numbercard = String.Empty;
                }
            }
        }

        public Nullable<decimal> je_ye
        {
            get
            {
                return _je_ye;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_ye = value.Value;
                }
                else
                {
                    _je_ye = 0m;
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
}

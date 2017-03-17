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
    [Table("td_hy_cz_2", "Td_Hy_Cz_2")]
    [DebuggerDisplay("id = {id}")]
    public class Td_Hy_Cz_2
    {
        #region public method

        public Td_Hy_Cz_2 Clone()
        {
            return (Td_Hy_Cz_2)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_hy = String.Empty;
        private string _id_bill = String.Empty;
        private int _sort_id = 0;
        private decimal _je = 0m;
        private decimal _je_zs = 0m;
        private string _bz = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);

        private decimal _je_ye = 0m;

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

        public Nullable<decimal> je_zs
        {
            get
            {
                return _je_zs;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_zs = value.Value;
                }
                else
                {
                    _je_zs = 0m;
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

        #endregion

    }
}
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
    [Table("tz_sp_kc", "Tz_Sp_Kc")]
    [DebuggerDisplay("id = {id}")]
    public class Tz_Sp_Kc
    {
        #region public method

        public Tz_Sp_Kc Clone()
        {
            return (Tz_Sp_Kc)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_shop = String.Empty;
        private string _id_kcsp = String.Empty;
        private decimal _sl_qm = 0m;
        private decimal _je_qm = 0m;
        private decimal _dj_cb = 0m;

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

        public Nullable<decimal> sl_qm
        {
            get
            {
                return _sl_qm;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_qm = value.Value;
                }
                else
                {
                    _sl_qm = 0m;
                }
            }
        }

        public Nullable<decimal> je_qm
        {
            get
            {
                return _je_qm;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_qm = value.Value;
                }
                else
                {
                    _je_qm = 0m;
                }
            }
        }

        public Nullable<decimal> dj_cb
        {
            get
            {
                return _dj_cb;
            }
            set
            {
                if (value.HasValue)
                {
                    _dj_cb = value.Value;
                }
                else
                {
                    _dj_cb = 0m;
                }
            }
        }

        #endregion

    }
}

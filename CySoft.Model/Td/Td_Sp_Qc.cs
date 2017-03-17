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
    [Table("td_sp_qc", "Td_Sp_Qc")]
    [DebuggerDisplay("id = {id}")]
    public class Td_Sp_Qc
    {
        #region public method

        public Td_Sp_Qc Clone()
        {
            return (Td_Sp_Qc)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_shop = String.Empty;
        private string _id_shopsp = String.Empty;
        private decimal _sl_qc = 0m;
        private decimal _dj_cb = 0m;
        private decimal _je_qc = 0m;

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

        public Nullable<decimal> sl_qc
        {
            get
            {
                return _sl_qc;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_qc = value.Value;
                }
                else
                {
                    _sl_qc = 0m;
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

        public Nullable<decimal> je_qc
        {
            get
            {
                return _je_qc;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_qc = value.Value;
                }
                else
                {
                    _je_qc = 0m;
                }
            }
        }


        private string _id_kcsp = String.Empty;

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

        #endregion

    }
}
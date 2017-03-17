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
    [Table("tz_ys", "Tz_Ys")]
    [DebuggerDisplay("id = {id}")]
    public class Tz_Ys
    {
        #region public method

        public Tz_Ys Clone()
        {
            return (Tz_Ys)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_shop = String.Empty;
        private string _id_kh = String.Empty;
        private decimal _je_qm = 0m;
        private decimal _je_pre_qm = 0m;
        private byte[] _nlast ;

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

        public string id_kh
        {
            get
            {
                return _id_kh;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_kh = value;
                }
                else
                {
                    _id_kh = String.Empty;
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

        public Nullable<decimal> je_pre_qm
        {
            get
            {
                return _je_pre_qm;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_pre_qm = value.Value;
                }
                else
                {
                    _je_pre_qm = 0m;
                }
            }
        }

        public byte[] nlast { get; set; }
        

        #endregion

    }
}

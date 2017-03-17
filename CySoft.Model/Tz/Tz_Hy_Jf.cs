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
    [Table("tz_hy_jf", "Tz_Hy_Jf")]
    [DebuggerDisplay("id = {id}")]
    public class Tz_Hy_Jf
    {
        #region public method

        public Tz_Hy_Jf Clone()
        {
            return (Tz_Hy_Jf)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_shop = String.Empty;
        private string _id_hy = String.Empty;
        private decimal _jf_qm = 0m;
        private string _recodekey = String.Empty;

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

        public Nullable<decimal> jf_qm
        {
            get
            {
                return _jf_qm;
            }
            set
            {
                if (value.HasValue)
                {
                    _jf_qm = value.Value;
                }
                else
                {
                    _jf_qm = 0m;
                }
            }
        }

        public string recodekey
        {
            get
            {
                return _recodekey;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _recodekey = value;
                }
                else
                {
                    _recodekey = String.Empty;
                }
            }
        }

        #endregion

    }
}

#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("Tb_user_accredit", "Tb_User_Accredit")]
    [DebuggerDisplay("appkey = {appkey}")]
    public class Tb_User_Accredit
    {
        #region public method

        public Tb_User_Accredit Clone()
        {
            return (Tb_User_Accredit)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _appkey = String.Empty;
        private string _secret = String.Empty;
        private long _id_master = 0;
        private string _name = String.Empty;
        private long _id_create = 0;
        private DateTime _rq_create = DateTime.Now;

        #endregion

        #region public property

        public string appkey
        {
            get
            {
                return _appkey;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _appkey = value;
                }
                else
                {
                    _appkey = String.Empty;
                }
            }
        }

        public string secret
        {
            get
            {
                return _secret;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _secret = value;
                }
                else
                {
                    _secret = String.Empty;
                }
            }
        }

        public Nullable<long> id_master
        {
            get
            {
                return _id_master;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_master = value.Value;
                }
                else
                {
                    _id_master = 0;
                }
            }
        }

        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name = value;
                }
                else
                {
                    _name = String.Empty;
                }
            }
        }

        [Column(Update = false)]
        public Nullable<long> id_create
        {
            get
            {
                return _id_create;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_create = value.Value;
                }
                else
                {
                    _id_create = 0;
                }
            }
        }

        [Column(Update = false)]
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
                    _rq_create = DateTime.Now;
                }
            }
        }

        #endregion

    }
}

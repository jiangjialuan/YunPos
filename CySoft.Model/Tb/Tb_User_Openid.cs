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
    [Table("tb_user_openid", "Tb_User_Openid")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_User_Openid
    {
        #region public method

        public Tb_User_Openid Clone()
        {
            return (Tb_User_Openid)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private decimal _id = 0m;
        private string _id_user = String.Empty;
        private string _id_user_master =String.Empty;
        private long _id_gys = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _openid = String.Empty;

        #endregion

        #region public property

        public Nullable<decimal> id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value.HasValue)
                {
                    _id = value.Value;
                }
                else
                {
                    _id = 0m;
                }
            }
        }

        public string id_user
        {
            get
            {
                return _id_user;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _id_user = value;
                }
                else
                {
                    _id_user = String.Empty;
                }
            }
        }

        public string id_user_master
        {
            get
            {
                return _id_user_master;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _id_user_master = value;
                }
                else
                {
                    _id_user_master =String.Empty;
                }
            }
        }

        public Nullable<long> id_gys
        {
            get
            {
                return _id_gys;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_gys = value.Value;
                }
                else
                {
                    _id_gys = 0;
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

        public string openid
        {
            get
            {
                return _openid;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _openid = value;
                }
                else
                {
                    _openid = String.Empty;
                }
            }
        }

        #endregion

    }
}

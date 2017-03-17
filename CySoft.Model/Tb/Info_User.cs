using System;
using System.Diagnostics;
using CySoft.Model.Mapping;

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("info_User", "Info_User")]
    [DebuggerDisplay("id_user = {id_user},id_info = {id_info}")]
    public class Info_User
    {
        #region public method

        public Info_User Clone()
        {
            return (Info_User)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id_user = 0;
        private long _id_info = 0;
        private int _flag_reade = 0;
        private DateTime _rq = new DateTime(1900, 1, 1);
        private string _flag_from = String.Empty;

        #endregion

        #region public property

        public Nullable<long> id_user
        {
            get
            {
                return _id_user;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_user = value.Value;
                }
                else
                {
                    _id_user = 0;
                }
            }
        }

        public Nullable<long> id_info
        {
            get
            {
                return _id_info;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_info = value.Value;
                }
                else
                {
                    _id_info = 0;
                }
            }
        }

        public Nullable<int> flag_reade
        {
            get
            {
                return _flag_reade;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_reade = value.Value;
                }
                else
                {
                    _flag_reade = 0;
                }
            }
        }

        public Nullable<DateTime> rq
        {
            get
            {
                return _rq;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq = value.Value;
                }
                else
                {
                    _rq = new DateTime(1900, 1, 1);
                }
            }
        }

        public string flag_from
        {
            get
            {
                return _flag_from;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _flag_from = value;
                }
                else
                {
                    _flag_from = String.Empty;
                }
            }
        }

        #endregion

    }
}

#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region 
#endregion

namespace CySoft.Model.Tb
{
    /// <summary>
    /// 业务员签到类
    /// </summary>
    [Serializable]
    [Table("Tb_user_checkin", "Tb_User_Checkin")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_User_Checkin
    {
        #region public method

        public Tb_User_Checkin Clone()
        {
            return (Tb_User_Checkin)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id = String.Empty;
        private string _id_user = String.Empty;
        private string _location = String.Empty;
        private string _des = String.Empty;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);

        #endregion

        #region public property

        public string id
        {
            get
            {
                return _id;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _id = value;
                }
                else
                {
                    _id = String.Empty;
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

        public string location
        {
            get
            {
                return _location;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _location = value;
                }
                else
                {
                    _location = String.Empty;
                }
            }
        }

        public string des
        {
            get
            {
                return _des;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _des = value;
                }
                else
                {
                    _des = String.Empty;
                }
            }
        }

        [Column(Update = false)]
        public string id_create
        {
            get
            {
                return _id_create;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _id_create = value;
                }
                else
                {
                    _id_create = String.Empty;
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

        #endregion

    }
}
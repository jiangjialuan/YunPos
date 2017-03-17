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
    /// 用户图册管理类
    /// </summary>
    [Serializable]
    [Table("Tb_user_pic", "Tb_User_Pic")]
    [DebuggerDisplay("xh = {xh},id_master = {id_master}")]
    public class Tb_User_Pic
    {
        #region public method

        public Tb_User_Pic Clone()
        {
            return (Tb_User_Pic)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id = 0;
        private long _id_master = 0;
        private byte _xh = 0;
        private string _photo_min = String.Empty;
        private string _photo = String.Empty;
        private long _id_create = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);

        #endregion

        #region public property

        public Nullable<long> id
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
                    _id = 0;
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

        public Nullable<byte> xh
        {
            get
            {
                return _xh;
            }
            set
            {
                if (value.HasValue)
                {
                    _xh = value.Value;
                }
                else
                {
                    _xh = 0;
                }
            }
        }

        public string photo_min
        {
            get
            {
                return _photo_min;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _photo_min = value;
                }
                else
                {
                    _photo_min = String.Empty;
                }
            }
        }

        public string photo
        {
            get
            {
                return _photo;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _photo = value;
                }
                else
                {
                    _photo = String.Empty;
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
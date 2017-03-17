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
    /// 商品图片
    /// </summary>
    [Serializable]
    [Table("Tb_sp_pic", "Tb_Sp_Pic")]
    [DebuggerDisplay("xh = {xh},id_sp = {id_sp}")]
    public class Tb_Sp_Pic
    {
        #region public method

        public Tb_Sp_Pic Clone()
        {
            return (Tb_Sp_Pic)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id_sp = 0;
        private byte _xh = 0;
        private string _photo_min = String.Empty;
        private string _photo = String.Empty;
        private long _id_create = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);

        #endregion

        #region public property

        public Nullable<long> id { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public Nullable<long> id_sp
        {
            get
            {
                return _id_sp;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_sp = value.Value;
                }
                else
                {
                    _id_sp = 0;
                }
            }
        }

        /// <summary>
        /// 序号
        /// </summary>
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

        /// <summary>
        /// 缩略图
        /// </summary>
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

        /// <summary>
        /// 图片
        /// </summary>
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

        /// <summary>
        /// 创建人
        /// </summary>
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

        /// <summary>
        /// 创建日期
        /// </summary>
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
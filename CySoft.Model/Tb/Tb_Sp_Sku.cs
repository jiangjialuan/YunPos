#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using CySoft.Model.Flags;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    /// <summary>
    /// 商品SKU
    /// </summary>
    [Serializable]
    [Table("Tb_sp_sku", "Tb_Sp_Sku")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Sp_Sku
    {
        #region public method

        public Tb_Sp_Sku Clone()
        {
            return (Tb_Sp_Sku)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id = 0;
        private long _id_sp = 0;
        private string _bm = String.Empty;
        private string _unit = String.Empty;
        private YesNoFlag _flag_stop = 0;
        private string _barcode = String.Empty;
        private string _description = String.Empty;
        private string _photo_min2 = String.Empty;
        private string _photo_min = String.Empty;
        private string _photo = String.Empty;
        private long _id_create = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private long _id_edit = 0;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private string _keywords = String.Empty;

        #endregion

        #region public property

        /// <summary>
        /// SKU码
        /// </summary>
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
        /// 编码
        /// </summary>
        public string bm
        {
            get
            {
                return _bm;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _bm = value;
                }
                else
                {
                    _bm = String.Empty;
                }
            }
        }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string unit
        {
            get
            {
                return _unit;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _unit = value;
                }
                else
                {
                    _unit = String.Empty;
                }
            }
        }

        /// <summary>
        /// 关键字
        /// </summary>
        public string keywords
        {
            get
            {
                return _keywords;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _keywords = value;
                }
                else
                {
                    _keywords = String.Empty;
                }
            }
        }

        /// <summary>
        /// 状态    1停用   0可用
        /// </summary>
        [Column(Update = false)]
        public YesNoFlag flag_stop
        {
            get
            {
                return _flag_stop;
            }
            set
            {
                
             _flag_stop = value;
              
            }
        }

        /// <summary>
        /// 条码
        /// </summary>
        public string barcode
        {
            get
            {
                return _barcode;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _barcode = value;
                }
                else
                {
                    _barcode = String.Empty;
                }
            }
        }

        /// <summary>
        /// 简要描述
        /// </summary>
        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _description = value;
                }
                else
                {
                    _description = String.Empty;
                }
            }
        }

        public string photo_min2
        {
            get
            {
                return _photo_min2;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _photo_min2 = value;
                }
                else
                {
                    _photo_min2 = String.Empty;
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

        /// <summary>
        /// 最后修改人
        /// </summary>
        public Nullable<long> id_edit
        {
            get
            {
                return _id_edit;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_edit = value.Value;
                }
                else
                {
                    _id_edit = 0;
                }
            }
        }

        /// <summary>
        /// 最后修改日期
        /// </summary>
        [Column(Insert = false)]
        public Nullable<DateTime> rq_edit
        {
            get
            {
                return _rq_edit;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_edit = value.Value;
                }
                else
                {
                    _rq_edit = new DateTime(1900, 1, 1);
                }
            }
        }

        #endregion

    }
}
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
    /// 商品扩展属性
    /// </summary>
    [Serializable]
    [Table("Tb_sp_expand", "Tb_Sp_Expand")]
    [DebuggerDisplay("id_sku = {id_sku}, val = {val}")]
    public class Tb_Sp_Expand
    {
        #region public method

        public Tb_Sp_Expand Clone()
        {
            return (Tb_Sp_Expand)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id_sku = 0;
        private long _id_sp_expand_template = 0;
        private long _id_sp = 0;
        private string _val = String.Empty;
        private long _id_create = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private long _id_edit = 0;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);

        #endregion

        #region public property

        /// <summary>
        /// 商品SKU
        /// </summary>
        public Nullable<long> id_sku
        {
            get
            {
                return _id_sku;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_sku = value.Value;
                }
                else
                {
                    _id_sku = 0;
                }
            }
        }

        /// <summary>
        /// 商品扩展属性模版Id
        /// </summary>
        public Nullable<long> id_sp_expand_template
        {
            get
            {
                return _id_sp_expand_template;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_sp_expand_template = value.Value;
                }
                else
                {
                    _id_sp_expand_template = 0;
                }
            }
        }

        /// <summary>
        /// 商品Id
        /// </summary>
        [Column(Update = false)]
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
        /// 属性值
        /// </summary>
        public string val
        {
            get
            {
                return _val;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _val = value;
                }
                else
                {
                    _val = String.Empty;
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
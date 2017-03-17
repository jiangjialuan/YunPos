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
    /// 商品
    /// </summary>
    [Serializable]
    [Table("Tb_sp", "Tb_Sp")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Sp
    {
        #region public method

        public Tb_Sp Clone()
        {
            return (Tb_Sp)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id = 0;
        private byte _flag_stop = 0;
        private string _mc = String.Empty;
        private string _keywords = String.Empty;
        private string _cd = String.Empty;
        private string _brand = String.Empty;
        private long _id_gys_create = 0;
        private long _id_create = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private long _id_edit = 0;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);

        #endregion

        #region public property

        /// <summary>
        /// 流水号
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
        /// 状态    1停用   0可用
        /// </summary>
        [Column(Update = false)]
        public Nullable<byte> flag_stop
        {
            get
            {
                return _flag_stop;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_stop = value.Value;
                }
                else
                {
                    _flag_stop = 0;
                }
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string mc
        {
            get
            {
                return _mc;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _mc = value;
                }
                else
                {
                    _mc = String.Empty;
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
        /// 产地
        /// </summary>
        public string cd
        {
            get
            {
                return _cd;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _cd = value;
                }
                else
                {
                    _cd = String.Empty;
                }
            }
        }

        /// <summary>
        /// 品牌
        /// </summary>
        public string brand
        {
            get
            {
                return _brand;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _brand = value;
                }
                else
                {
                    _brand = String.Empty;
                }
            }
        }

        /// <summary>
        /// 创建供应商
        /// </summary>
        public Nullable<long> id_gys_create
        {
            get
            {
                return _id_gys_create;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_gys_create = value.Value;
                }
                else
                {
                    _id_gys_create = 0;
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
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
    /// 供应商商品
    /// </summary>
    [Serializable]
    [Table("Tb_gys_sp", "Tb_Gys_Sp")]
    [DebuggerDisplay("id_gys = {id_gys},id_sku = {id_sku}")]
    public class Tb_Gys_Sp
    {
        #region public method

        public Tb_Gys_Sp Clone()
        {
            return (Tb_Gys_Sp)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id_gys = 0;
        private long _id_sku = 0;
        private long _id_sp = 0;
        private YesNoFlag _flag_stop = 0;
        private string _bm_Interface = String.Empty;
        private long _id_spfl = 0;
        private int _sort_id = 0;
        private decimal _dj_base = 0m;
        private decimal _zhl = 0m;
        private YesNoFlag _flag_up = 0;
        private decimal _sl_kc = 0m;
        private decimal _sl_kc_bj = 0m;
        private long _id_create = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private long _id_edit = 0;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);

        #endregion

        #region public property

        /// <summary>
        /// 供应商Id
        /// </summary>
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

        /// <summary>
        /// 商品sku
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
        /// 接口编码
        /// </summary>
        public string bm_Interface
        {
            get
            {
                return _bm_Interface;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _bm_Interface = value;
                }
                else
                {
                    _bm_Interface = String.Empty;
                }
            }
        }

        /// <summary>
        /// 商品分类
        /// </summary>
        public Nullable<long> id_spfl
        {
            get
            {
                return _id_spfl;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_spfl = value.Value;
                }
                else
                {
                    _id_spfl = 0;
                }
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public Nullable<int> sort_id
        {
            get
            {
                return _sort_id;
            }
            set
            {
                if (value.HasValue)
                {
                    _sort_id = value.Value;
                }
                else
                {
                    _sort_id = 0;
                }
            }
        }

        /// <summary>
        /// 基准价
        /// </summary>
        public Nullable<decimal> dj_base
        {
            get
            {
                return _dj_base;
            }
            set
            {
                if (value.HasValue)
                {
                    _dj_base = value.Value;
                }
                else
                {
                    _dj_base = 0m;
                }
            }
        }

        /// <summary>
        /// 包装数
        /// </summary>
        public Nullable<decimal> zhl
        {
            get
            {
                return _zhl;
            }
            set
            {
                if (value.HasValue)
                {
                    _zhl = value.Value;
                }
                else
                {
                    _zhl = 0m;
                }
            }
        }

        /// <summary>
        /// 上架状态     1上架   0下架
        /// </summary>
        public YesNoFlag flag_up
        {
            get
            {
                return _flag_up;
            }
            set
            {
               
             _flag_up = value;
                
            }
        }

        /// <summary>
        /// 当前库存
        /// </summary>
        public Nullable<decimal> sl_kc
        {
            get
            {
                return _sl_kc;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_kc = value.Value;
                }
                else
                {
                    _sl_kc = 0m;
                }
            }
        }

        /// <summary>
        /// 预警库存
        /// </summary>
        public Nullable<decimal> sl_kc_bj
        {
            get
            {
                return _sl_kc_bj;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_kc_bj = value.Value;
                }
                else
                {
                    _sl_kc_bj = 0m;
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
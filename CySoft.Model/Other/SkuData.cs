using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Model.Tb;
using CySoft.Model.Flags;

namespace CySoft.Model.Other
{
    [Serializable]
    public class SkuData : Tb_Sp_Sku
    {
        private string _bm_Interface = String.Empty;
        private string _bm = String.Empty;
        private long _id_spfl = 0;
        private long _id_gys_create = 0;
        private int _sort_id = 0;
        private decimal _dj_base = 0m;
        private decimal _zhl = 0m;
        private YesNoFlag _flag_up = 0;
        private decimal _sl_dh_min = 0m;
        private decimal _sl_kc = 0m;
        private decimal _sl_kc_bj = 0m;
        private string _name_fl = String.Empty;
        private string  _name_spec_zh = String.Empty;
        private string _name_sp = String.Empty; //商品名称
        private string _name_spec_id = String.Empty;
        private int _flag_Favorites = 0;
        private int _gwc = 0;
        private int _kc_flag = 0;
        private string _alias_gys = string.Empty;
        private decimal _gwcsl = 0;
        
        public long id_gys_create
        {
            get
            {
                return _id_gys_create;
            }
            set
            {
                if (_id_gys_create == value)
                    return;
                _id_gys_create = value;
            }
        }
        // sku 客户订货价
        public decimal dj { get; set; }

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
        /// 系统编码
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
        /// 最小订货量
        /// </summary>
        public Nullable<decimal> sl_dh_min
        {
            get
            {
                return _sl_dh_min;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_dh_min = value.Value;
                }
                else
                {
                    _sl_dh_min = 0m;
                }
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
        /// 商品 扩展属性(sku 规格值)
        /// </summary>
        public List<Tb_Sp_Expand> sp_expand
        {
            get;
            set;
        }

        /// <summary>
        /// 商品 sku 单价
        /// </summary>
        public List<Tb_Sp_Dj> sp_dj { get; set; }

        /// <summary>
        /// 商品 扩展属性(sku 规格值)
        /// </summary>
        public List<Tb_Sp_Expand_Query> sp_expand_query
        {
            get;
            set;
        }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string name_fl
        {
            get
            {
                return _name_fl;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name_fl = value;
                }
                else
                {
                    _name_fl = String.Empty;
                }
            }
        }

        /// <summary>
        /// 规格组合
        /// </summary>
        public string name_spec_zh
        {
            get
            {
                return _name_spec_zh;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name_spec_zh = value;
                }
                else
                {
                    _name_spec_zh = String.Empty;
                }
            }
        }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string name_sp
        {
            get
            {
                return _name_sp;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name_sp = value;
                }
                else
                {
                    _name_sp = String.Empty;
                }
            }
        }

        public string name_spec_id
        {
            get
            {
                return _name_spec_id;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name_spec_id = value;
                }
                else
                {
                    _name_spec_id = String.Empty;
                }
            }
        }
        /// <summary>
        /// 供应商id
        /// </summary>
        public long? id_gys { get; set; }

        public int kc_flag {
            get { return _kc_flag; }
            set { _kc_flag = value; } 
        }//是否允许看库存

        public string alias_gys {
            get { return _alias_gys; }
            set { _alias_gys = value; }
        }//供应商
        public int gwc { 
            get {return _gwc;}
            set { _gwc = value; }
        }//商品是否已加入购物车
        public decimal gwcsl {
            get { return _gwcsl; }
            set { _gwcsl = value; }
        }//购物车商品数量
        
        //是否收藏，1-是，0-否
        public int flag_Favorites 
        {
            get { return _flag_Favorites; }
            set { _flag_Favorites = value; }
        }
        public string sp_tag { get; set; }
        /// <summary>
        /// 商品标签
        /// </summary>
        public IList<Tb_Gys_Sp_Tag_Query> sp_tag_list { get; set; }
    }
}

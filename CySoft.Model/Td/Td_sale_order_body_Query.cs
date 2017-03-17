#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Td
{
    /// <summary>
    /// 订单单体
    /// </summary>
    [Serializable]
    [Table("Td_sale_order_body", "Td_Sale_Order_Body")]
    [DebuggerDisplay("dh = {dh},xh = {xh}")]
    public class Td_Sale_Order_Body_Query : Td_Sale_Order_Body
    {
        #region public method

        public Td_Sale_Order_Body Clone()
        {
            return (Td_Sale_Order_Body)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _goodsbm = String.Empty;
        private string _goodsname = String.Empty;
        private decimal _agio = 0m;
        private string _formatname = String.Empty;
        private string _unit = String.Empty;
        private string _bm = String.Empty;
        private short _flag_state = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _mc_flag_state = String.Empty;

        public decimal dj_base { get; set; }
        public decimal sl_dh_min { get; set; }
        public decimal dj { get; set; }

        private string _alias_cgs = String.Empty;
        private string _alias_gys = String.Empty;
        // 审批价
        public decimal spj { get; set; }

        public string photo { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string barcode { get; set; }

        #endregion

        #region public property

        /// <summary>
        /// 折扣
        /// </summary>
        public Nullable<decimal> agio
        {
            get
            {
                return _agio;
            }
            set
            {
                if (value.HasValue)
                {
                    _agio = value.Value;
                }
                else
                {
                    _agio = 0m;
                }
            }
        }

        /// <summary>
        /// 单号
        /// </summary>
        /// <summary>
        /// 商品编码
        /// </summary>
        public string GoodsBm
        {
            get
            {
                return _goodsbm;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _goodsbm = value;
                }
                else
                {
                    _goodsbm = String.Empty;
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
        ///  订单状态名称(已提交、待审核等)
        /// </summary>
        public string mc_flag_state
        {
            get
            {
                if (_mc_flag_state == null)
                {
                    _mc_flag_state = String.Empty;
                }
                return _mc_flag_state;
            }
            set
            {

                _mc_flag_state = value;
            }
        }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName
        {
            get
            {
                return _goodsname;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _goodsname = value;
                }
                else
                {
                    _goodsname = String.Empty;
                }
            }
        }

        /// <summary>
        /// 商品规格
        /// </summary>
        public string formatname
        {
            get
            {
                return _formatname;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _formatname = value;
                }
                else
                {
                    _formatname = String.Empty;
                }
            }
        }

        /// <summary>
        /// 商品单位
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
        /// 开单业务员 客户别名
        /// </summary>
        public string alias_cgs
        {
            get
            {
                return _alias_cgs;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _alias_cgs = value;
                }
                else
                {
                    _alias_cgs = String.Empty;
                }
            }
        }

        /// <summary>
        ///  关注商别名
        /// </summary>
        public string alias_gys
        {
            get
            {
                return _alias_gys;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _alias_gys = value;
                }
                else
                {
                    _alias_gys = String.Empty;
                }
            }
        }

        /// <summary>
        /// 商品编码
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
        /// 状态   bs_lx='orderstatus'
        /// </summary>
        public Nullable<short> flag_state
        {
            get
            {
                return _flag_state;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_state = value.Value;
                }
                else
                {
                    _flag_state = 0;
                }
            }
        }
        #endregion

    }
}
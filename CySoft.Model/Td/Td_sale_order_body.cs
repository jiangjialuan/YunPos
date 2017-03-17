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
    public class Td_Sale_Order_Body
    {
        #region public method

        public Td_Sale_Order_Body Clone()
        {
            return (Td_Sale_Order_Body)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _dh = String.Empty;
        private short _xh = 0;
        private long _id_sku = 0;
        private long _id_sp = 0;
        private string _unit = String.Empty;
        private decimal _zhl = 0m;
        private decimal _sl = 0m;
        private decimal _sl_ck = 0m;
        private decimal _sl_fh = 0m;
        private decimal _slv = 0m;
        private decimal _dj_bhs = 0m;
        private decimal _dj_hs = 0m;
        private decimal _je_hs = 0m;
        private decimal _je_pay = 0m;
        private string _remark = String.Empty;

        #endregion

        #region public property

        /// <summary>
        /// 单号
        /// </summary>
        public string dh
        {
            get
            {
                return _dh;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _dh = value;
                }
                else
                {
                    _dh = String.Empty;
                }
            }
        }

        /// <summary>
        /// 序号
        /// </summary>
        public Nullable<short> xh
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
        /// 单位
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
        /// 数量
        /// </summary>
        public Nullable<decimal> sl
        {
            get
            {
                return _sl;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl = value.Value;
                }
                else
                {
                    _sl = 0m;
                }
            }
        }

        /// <summary>
        /// 已出库数量
        /// </summary>
        public Nullable<decimal> sl_ck
        {
            get
            {
                return _sl_ck;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_ck = value.Value;
                }
                else
                {
                    _sl_ck = 0m;
                }
            }
        }

        /// <summary>
        /// 已发货数量
        /// </summary>
        public Nullable<decimal> sl_fh
        {
            get
            {
                return _sl_fh;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_fh = value.Value;
                }
                else
                {
                    _sl_fh = 0m;
                }
            }
        }

        /// <summary>
        /// 税率
        /// </summary>
        public Nullable<decimal> slv
        {
            get
            {
                return _slv;
            }
            set
            {
                if (value.HasValue)
                {
                    _slv = value.Value;
                }
                else
                {
                    _slv = 0m;
                }
            }
        }

        /// <summary>
        /// 不含税单价
        /// </summary>
        public Nullable<decimal> dj_bhs
        {
            get
            {
                return _dj_bhs;
            }
            set
            {
                if (value.HasValue)
                {
                    _dj_bhs = value.Value;
                }
                else
                {
                    _dj_bhs = 0m;
                }
            }
        }

        /// <summary>
        /// 含税单价
        /// </summary>
        public Nullable<decimal> dj_hs
        {
            get
            {
                return _dj_hs;
            }
            set
            {
                if (value.HasValue)
                {
                    _dj_hs = value.Value;
                }
                else
                {
                    _dj_hs = 0m;
                }
            }
        }

        /// <summary>
        /// 含税金额
        /// </summary>
        public Nullable<decimal> je_hs
        {
            get
            {
                return _je_hs;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_hs = value.Value;
                }
                else
                {
                    _je_hs = 0m;
                }
            }
        }

        /// <summary>
        /// 应收金额（含税）
        /// </summary>
        public Nullable<decimal> je_pay
        {
            get
            {
                return _je_pay;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_pay = value.Value;
                }
                else
                {
                    _je_pay = 0m;
                }
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark
        {
            get
            {
                return _remark;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _remark = value;
                }
                else
                {
                    _remark = String.Empty;
                }
            }
        }

        #endregion

    }
}
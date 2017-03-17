#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using System.Collections.Generic;
#endregion

#region
#endregion

namespace CySoft.Model.Td
{
    /// <summary>
    /// 销售出库单单头
    /// </summary>
    [Serializable]
    [Table("Td_sale_out_head", "Td_Sale_Out_Head")]
    [DebuggerDisplay("dh = {dh}")]
    public class Td_Sale_Out_Head_Query:Td_Sale_Out_Head
    {
        #region public method

        public Td_Sale_Out_Head Clone()
        {
            return (Td_Sale_Out_Head)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _dh = String.Empty;
        private string _dh_order = String.Empty;
        private string _customename = String.Empty;
        private DateTime _libraryhours = new DateTime(1900, 1, 1);
        private DateTime _deliverytime = new DateTime(1900, 1, 1);
        private string _goodsbm = String.Empty;
        private string _goodsname = String.Empty;
        private decimal _sl_ck = 0m;
        private decimal _sl_fh = 0m;
        private string _bm = String.Empty;
        List<Td_Sale_Out_Body> _out_body = new List<Td_Sale_Out_Body>();
        
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
        /// 订单号
        /// </summary>
        public string dh_order
        {
            get
            {
                return _dh_order;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _dh_order = value;
                }
                else
                {
                    _dh_order = String.Empty;
                }
            }
        }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomeName
        {
            get
            {
                return _customename;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _customename = value;
                }
                else
                {
                    _customename = String.Empty;
                }
            }
        }

        /// <summary>
        /// 出库时间
        /// </summary>
        public Nullable<DateTime> LibraryHours
        {
            get
            {
                return _libraryhours;
            }
            set
            {
                if (value.HasValue)
                {
                    _libraryhours = value.Value;
                }
                else
                {
                    _libraryhours = new DateTime(1900, 1, 1);
                }
            }
        }


        /// <summary>
        /// 发货时间
        /// </summary>
        public Nullable<DateTime> DeliveryTime
        {
            get
            {
                return _deliverytime;
            }
            set
            {
                if (value.HasValue)
                {
                    _deliverytime = value.Value;
                }
                else
                {
                    _deliverytime = new DateTime(1900, 1, 1);
                }
            }
        }

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
        /// 已出库数量
        /// </summary>
        public Nullable<decimal> Sl_ck
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
        /// 出库单单体
        /// </summary>
        public List<Td_Sale_Out_Body> out_body
        {
            get {
                return _out_body;
            }
            set {
                _out_body = value;
            }
        }

        /// <summary>
        /// 已发货数量
        /// </summary>
        public Nullable<decimal> Sl_fh
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

        #endregion

    }
}
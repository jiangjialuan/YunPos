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
    /// 销售出库单单体
    /// </summary>
    [Serializable]
    [Table("Td_sale_out_body", "Td_Sale_Out_Body")]
    [DebuggerDisplay("dh = {dh},xh = {xh}, unit = {unit}")]
    public class Td_Sale_Out_Body_Query : Td_Sale_Out_Body
    {
        #region public method

        public Td_Sale_Out_Body Clone()
        {
            return (Td_Sale_Out_Body)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _goodsbm = String.Empty;
        private string _goodsname = String.Empty;
        private string _formatname = String.Empty;
        private decimal _dgsl = 0m;
        private decimal _fhsl = 0m;

        #endregion

        #region public property

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
        /// 订购数
        /// </summary>
        public Nullable<decimal> fhsl
        {
            get
            {
                return _fhsl;
            }
            set
            {
                if (value.HasValue)
                {
                    _fhsl = value.Value;
                }
                else
                {
                    _fhsl = 0m;
                }
            }
        }

        /// <summary>
        /// 订购数
        /// </summary>
        public Nullable<decimal> dgsl
        {
            get
            {
                return _dgsl;
            }
            set
            {
                if (value.HasValue)
                {
                    _dgsl = value.Value;
                }
                else
                {
                    _dgsl = 0m;
                }
            }
        }
        #endregion

    }
}
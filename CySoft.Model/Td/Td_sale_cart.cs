#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region 购物车
#endregion

namespace CySoft.Model.Td
{
    /// <summary>
    /// 购物车
    /// </summary>
    [Serializable]
    [Table("Td_sale_cart", "Td_Sale_Cart")]
    [DebuggerDisplay("id_user = {id_user},id_gys = {id_gys},id_sku = {id_sku}, id_sp = {id_sp}, sl = {sl}")]
    public class Td_Sale_Cart
    {
        #region public method

        public Td_Sale_Cart Clone()
        {
            return (Td_Sale_Cart)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id_user = 0;
        private long _id_gys = 0;
        private long _id_sku = 0;
        private long _id_cgs = 0;
        private long _id_sp = 0;
        private decimal _sl = 0m;

        #endregion

        #region public property

        /// <summary>
        /// 所属用户
        /// </summary>
        public Nullable<long> id_user
        {
            get
            {
                return _id_user;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_user = value.Value;
                }
                else
                {
                    _id_user = 0;
                }
            }
        }

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
        /// 采购商Id
        /// </summary>
        public Nullable<long> id_cgs
        {
            get
            {
                return _id_cgs;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_cgs = value.Value;
                }
                else
                {
                    _id_cgs = 0;
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

        #endregion

    }
}

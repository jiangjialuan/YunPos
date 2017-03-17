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
    /// 收款单
    /// </summary>
    [Serializable]
    [Table("Td_sale_pay", "Td_Sale_Pay")]
    [DebuggerDisplay("dh = {dh}")]
    public class Td_sale_pay_Query:Td_Sale_Pay
    {
        #region public method

        public Td_sale_pay_Query Clone()
        {
            return (Td_sale_pay_Query)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _cgs_name = String.Empty;
        private string _gys_name = string.Empty;

        #endregion

        #region public property

        /// <summary>
        /// 采购商名称
        /// </summary>
        public string cgs_name
        {
            get
            {
                return _cgs_name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _cgs_name = value;
                }
                else
                {
                    _cgs_name = String.Empty;
                }
            }
        }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string gys_name
        {
            get
            {
                return _gys_name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _gys_name = value;
                }
                else
                {
                    _gys_name = String.Empty;
                }
            }
        }

        /// <summary>
        /// 供应商主用户
        /// </summary>
        public long id_user_master_gys { get; set; }
        /// <summary>
        /// 采购商主用户
        /// </summary>
        public long id_user_master_cgs { get; set; }
        #endregion

    }
}
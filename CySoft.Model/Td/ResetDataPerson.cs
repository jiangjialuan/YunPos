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
    public class ResetDataPerson
    {
        #region public method

        public ResetDataPerson Clone()
        {
            return (ResetDataPerson)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_gys = String.Empty;
        private string _del_lx = String.Empty;


        #endregion

        #region public property

        /// <summary>
        /// 单号
        /// </summary>
        public string id_gys
        {
            get
            {
                return _id_gys;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_gys = value;
                }
                else
                {
                    _id_gys = String.Empty;
                }
            }
        }

        /// <summary>
        /// 订单号
        /// </summary>
        public string del_lx
        {
            get
            {
                return _del_lx;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _del_lx = value;
                }
                else
                {
                    _del_lx = String.Empty;
                }
            }
        }

      
        #endregion

    }
}
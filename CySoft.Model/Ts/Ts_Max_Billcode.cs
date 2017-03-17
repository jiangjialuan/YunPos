#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Ts
{
    /// <summary>
    /// 最大单号
    /// </summary>
    [Serializable]
    [Table("Ts_max_billcode", "Ts_Max_Billcode")]
    [DebuggerDisplay("billcode = {billcode},ymd = {ymd}")]
    public class Ts_Max_Billcode
    {
        #region public method

        public Ts_Max_Billcode Clone()
        {
            return (Ts_Max_Billcode)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _billcode = String.Empty;
        private string _ymd = String.Empty;
        private long _max_dh = 0;
        private long _id_master = 0;

        #endregion

        #region public property

        /// <summary>
        /// 单据编码
        /// </summary>
        public string billcode
        {
            get
            {
                return _billcode;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _billcode = value;
                }
                else
                {
                    _billcode = String.Empty;
                }
            }
        }

        /// <summary>
        /// 年月日
        /// </summary>
        public string ymd
        {
            get
            {
                return _ymd;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _ymd = value;
                }
                else
                {
                    _ymd = String.Empty;
                }
            }
        }

        public Nullable<long> max_dh
        {
            get
            {
                return _max_dh;
            }
            set
            {
                if (value.HasValue)
                {
                    _max_dh = value.Value;
                }
                else
                {
                    _max_dh = 0;
                }
            }
        }
        public Nullable<long> id_master
        {
            get
            {
                return _id_master;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_master = value.Value;
                }
                else
                {
                    _id_master = 0;
                }
            }
        }
        #endregion

    }
}
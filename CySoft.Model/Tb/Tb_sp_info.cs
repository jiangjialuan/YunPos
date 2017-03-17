#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    /// <summary>
    /// 商品资料
    /// </summary>
    [Serializable]
    [Table("Tb_sp_info", "Tb_Sp_Info")]
    [DebuggerDisplay("id_sp = {id_sp}")]
    public class Tb_Sp_Info
    {
        #region public method

        public Tb_Sp_Info Clone()
        {
            return (Tb_Sp_Info)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id_sp = 0;
        private long _id_sku = 0;
        private string _description = String.Empty;
        private string _bm = String.Empty;
        private string _name_sp = String.Empty;
        private string _name_spec_zh = String.Empty;
        #endregion

        #region public property

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
        /// 简要描述
        /// </summary>
        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _description = value;
                }
                else
                {
                    _description = String.Empty;
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
        /// 商品名
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
        /// <summary>
        /// 商品规格
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
        #endregion

    }
}
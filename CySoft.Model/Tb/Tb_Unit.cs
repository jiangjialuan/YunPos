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
    /// 计量单位
    /// </summary>
    [Serializable]
    [Table("Tb_unit", "Tb_Unit")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Unit
    {
        #region public method

        public Tb_Unit Clone()
        {
            return (Tb_Unit)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id = 0;
        private long _id_gys = 0;
        private string _name = String.Empty;

        #endregion

        #region public property

        /// <summary>
        /// 主键
        /// </summary>
        [Column(false,false)]
        public Nullable<long> id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value.HasValue)
                {
                    _id = value.Value;
                }
                else
                {
                    _id = 0;
                }
            }
        }

        /// <summary>
        /// 所属供应商
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
        /// 名称
        /// </summary>
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name = value;
                }
                else
                {
                    _name = String.Empty;
                }
            }
        }

        #endregion

    }
}
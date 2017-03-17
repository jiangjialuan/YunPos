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
    /// 系统参数
    /// </summary>
    [Serializable]
    [Table("Ts_param", "Ts_Param")]
    [DebuggerDisplay("bm = {bm}, mc = {mc}, val = {val}, flag = {flag}")]
    public class Ts_Param
    {
        #region public method

        public Ts_Param Clone()
        {
            return (Ts_Param)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _bm = String.Empty;
        private string _mc = String.Empty;
        private string _val = String.Empty;
        private string _flag = String.Empty;
        private int _sort_id = 0;
        private string _description = String.Empty;

        #endregion

        #region public property

        /// <summary>
        /// 键
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
        /// 名称
        /// </summary>
        public string mc
        {
            get
            {
                return _mc;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _mc = value;
                }
                else
                {
                    _mc = String.Empty;
                }
            }
        }

        /// <summary>
        /// 值
        /// </summary>
        public string val
        {
            get
            {
                return _val;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _val = value;
                }
                else
                {
                    _val = String.Empty;
                }
            }
        }

        /// <summary>
        /// 分组
        /// </summary>
        public string flag
        {
            get
            {
                return _flag;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _flag = value;
                }
                else
                {
                    _flag = String.Empty;
                }
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public Nullable<int> sort_id
        {
            get
            {
                return _sort_id;
            }
            set
            {
                if (value.HasValue)
                {
                    _sort_id = value.Value;
                }
                else
                {
                    _sort_id = 0;
                }
            }
        }

        /// <summary>
        /// 简要说明
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

        #endregion

    }
}
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
    /// 编码规则表
    /// </summary>
    [Serializable]
    [Table("Ts_codingrule", "Ts_Codingrule")]
    [DebuggerDisplay("id = {id}, coding = {coding}, name = {name}, flag_type = {flag_type}")]
    public class Ts_Codingrule
    {
        #region public method

        public Ts_Codingrule Clone()
        {
            return (Ts_Codingrule)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private int _id = 0;
        private string _coding = String.Empty;
        private string _name = String.Empty;
        private string _flag_type = String.Empty;
        private string _lx = String.Empty;
        private string _lx_bm = String.Empty;
        private string _prefix = String.Empty;
        private int _length = 0;
        private int _step = 0;
        private int _level = 0;
        private string _flag_bs = String.Empty;

        #endregion

        #region public property

        /// <summary>
        /// 流水
        /// </summary>
        public Nullable<int> id
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
        /// 编码
        /// </summary>
        public string coding
        {
            get
            {
                return _coding;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _coding = value;
                }
                else
                {
                    _coding = String.Empty;
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

        /// <summary>
        /// 自动编码类型
        /// </summary>
        public string flag_type
        {
            get
            {
                return _flag_type;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _flag_type = value;
                }
                else
                {
                    _flag_type = String.Empty;
                }
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string lx
        {
            get
            {
                return _lx;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _lx = value;
                }
                else
                {
                    _lx = String.Empty;
                }
            }
        }

        /// <summary>
        /// 编码类别
        /// </summary>
        public string lx_bm
        {
            get
            {
                return _lx_bm;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _lx_bm = value;
                }
                else
                {
                    _lx_bm = String.Empty;
                }
            }
        }

        /// <summary>
        /// 前缀
        /// </summary>
        public string prefix
        {
            get
            {
                return _prefix;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _prefix = value;
                }
                else
                {
                    _prefix = String.Empty;
                }
            }
        }

        /// <summary>
        /// 长度
        /// </summary>
        public Nullable<int> length
        {
            get
            {
                return _length;
            }
            set
            {
                if (value.HasValue)
                {
                    _length = value.Value;
                }
                else
                {
                    _length = 0;
                }
            }
        }

        /// <summary>
        /// 逐步
        /// </summary>
        public Nullable<int> step
        {
            get
            {
                return _step;
            }
            set
            {
                if (value.HasValue)
                {
                    _step = value.Value;
                }
                else
                {
                    _step = 0;
                }
            }
        }

        /// <summary>
        /// 级别
        /// </summary>
        public Nullable<int> level
        {
            get
            {
                return _level;
            }
            set
            {
                if (value.HasValue)
                {
                    _level = value.Value;
                }
                else
                {
                    _level = 0;
                }
            }
        }

        /// <summary>
        /// 0 全部可以修改，1不可修改前缀，2不可以修改长度，3 不可修改层级，4不可修改步长，5不可修改自动编码类型
        /// </summary>
        public string flag_bs
        {
            get
            {
                return _flag_bs;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _flag_bs = value;
                }
                else
                {
                    _flag_bs = String.Empty;
                }
            }
        }

        #endregion

    }
}
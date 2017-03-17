#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using CySoft.Model.Flags;
#endregion

#region 采购级别(客户级别)
#endregion

namespace CySoft.Model.Tb
{
    /// <summary>
    /// 采购级别(客户级别)
    /// </summary>
    [Serializable]
    [Table("Tb_cgs_level", "Tb_Cgs_Level")]
    [DebuggerDisplay("id = {id}, id_gys = {id_gys}, name = {name}, agio = {agio}")]
    public class Tb_Cgs_Level
    {
        #region public method

        public Tb_Cgs_Level Clone()
        {
            return (Tb_Cgs_Level)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id = 0;
        private long _id_gys = 0;
        private string _name = String.Empty;
        private decimal _agio = 0m;
        private string _remark = String.Empty;
        private long _id_create = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private long _id_edit = 0;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private YesNoFlag _flag_sys = YesNoFlag.No;

        #endregion

        #region public property

        /// <summary>
        /// 主键
        /// </summary>
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
        [Column(Update = false)]
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
        /// 是否系统默认   1是  0否
        /// </summary>
        [Column(Update = false)]
        public YesNoFlag flag_sys
        {
            get { return this._flag_sys; }
            set { this._flag_sys = value; }
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
        /// 折扣
        /// </summary>
        public Nullable<decimal> agio
        {
            get
            {
                return _agio;
            }
            set
            {
                if (value.HasValue)
                {
                    _agio = value.Value;
                }
                else
                {
                    _agio = 0m;
                }
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark
        {
            get
            {
                return _remark;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _remark = value;
                }
                else
                {
                    _remark = String.Empty;
                }
            }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column(Update = false)]
        public Nullable<long> id_create
        {
            get
            {
                return _id_create;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_create = value.Value;
                }
                else
                {
                    _id_create = 0;
                }
            }
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Column(Update = false, Insert = false)]
        public Nullable<DateTime> rq_create
        {
            get
            {
                return _rq_create;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_create = value.Value;
                }
                else
                {
                    _rq_create = new DateTime(1900, 1, 1);
                }
            }
        }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public Nullable<long> id_edit
        {
            get
            {
                return _id_edit;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_edit = value.Value;
                }
                else
                {
                    _id_edit = 0;
                }
            }
        }

        /// <summary>
        /// 最后修改日期
        /// </summary>
        [Column(Insert = false)]
        public Nullable<DateTime> rq_edit
        {
            get
            {
                return _rq_edit;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_edit = value.Value;
                }
                else
                {
                    _rq_edit = new DateTime(1900, 1, 1);
                }
            }
        }

        #endregion

    }
}
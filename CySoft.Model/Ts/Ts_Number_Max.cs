#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region 最大编号
#endregion

namespace CySoft.Model.Ts
{
    /// <summary>
    /// 最大编号
    /// </summary>
    [Serializable]
    [Table("Ts_number_max", "Ts_Number_Max")]
    [DebuggerDisplay("table_name = {table_name}, id_max = {id_max}, xh_max = {xh_max}")]
    public class Ts_Number_Max
    {
        #region public method

        public Ts_Number_Max Clone()
        {
            return (Ts_Number_Max)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _table_name = String.Empty;
        private long _id_max = 0;
        private long _xh_max = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private DateTime _rq_edit = new DateTime(1900, 1, 1);

        #endregion

        #region public property

        /// <summary>
        /// 表名
        /// </summary>
        public string table_name
        {
            get
            {
                return _table_name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _table_name = value;
                }
                else
                {
                    _table_name = String.Empty;
                }
            }
        }

        /// <summary>
        /// 最大主键
        /// </summary>
        public Nullable<long> id_max
        {
            get
            {
                return _id_max;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_max = value.Value;
                }
                else
                {
                    _id_max = 0;
                }
            }
        }

        /// <summary>
        /// 最大序号
        /// </summary>
        public Nullable<long> xh_max
        {
            get
            {
                return _xh_max;
            }
            set
            {
                if (value.HasValue)
                {
                    _xh_max = value.Value;
                }
                else
                {
                    _xh_max = 0;
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
#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Ts
{
    [Serializable]
    [Table("Ts_param_business", "Ts_Param_Business")]
    [DebuggerDisplay("id_user_master = {id_user_master},bm = {bm}")]
    public class Ts_Param_Business
    {
        #region public method

        public Ts_Param_Business Clone()
        {
            return (Ts_Param_Business)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id_user_master = 0;
        private string _bm = String.Empty;
        private string _mc = String.Empty;
        private string _val = String.Empty;
        private int _sort_id = 0;
        private string _description = String.Empty;
        private long _id_create = 0;
        private DateTime _rq_create = DateTime.Now;

        #endregion

        #region public property

        public Nullable<long> id_user_master
        {
            get
            {
                return _id_user_master;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_user_master = value.Value;
                }
                else
                {
                    _id_user_master = 0;
                }
            }
        }

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
        [Column(Update = false)]
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
                    _rq_create = DateTime.Now;
                }
            }
        }
        #endregion

    }
}
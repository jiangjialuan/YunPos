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
    [Table("ts_notice", "Ts_Notice")]
    [DebuggerDisplay("id = {id}")]
    public class Ts_Notice
    {
        #region public method

        public Ts_Notice Clone()
        {
            return (Ts_Notice)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_shop = String.Empty;
        private byte _flag_type = 0;
        private string _id_shop_target = String.Empty;
        private string _id_user_target = String.Empty;
        private string _title = String.Empty;
        private string _content = String.Empty;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _id_edit = String.Empty;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private byte _flag_delete = 0;

        #endregion

        #region public property

        /// <summary>
        /// '0'为系统默认参数
        /// </summary>
        public string id_masteruser
        {
            get
            {
                return _id_masteruser;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_masteruser = value;
                }
                else
                {
                    _id_masteruser = String.Empty;
                }
            }
        }

        public string id
        {
            get
            {
                return _id;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id = value;
                }
                else
                {
                    _id = String.Empty;
                }
            }
        }

        public string id_shop
        {
            get
            {
                return _id_shop;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_shop = value;
                }
                else
                {
                    _id_shop = String.Empty;
                }
            }
        }

        /// <summary>
        /// 1：系统公告 2：门店公告 3：个人公告'
        /// </summary>
        public Nullable<byte> flag_type
        {
            get
            {
                return _flag_type;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_type = value.Value;
                }
                else
                {
                    _flag_type = 0;
                }
            }
        }

        public string id_shop_target
        {
            get
            {
                return _id_shop_target;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_shop_target = value;
                }
                else
                {
                    _id_shop_target = String.Empty;
                }
            }
        }

        public string id_user_target
        {
            get
            {
                return _id_user_target;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_user_target = value;
                }
                else
                {
                    _id_user_target = String.Empty;
                }
            }
        }

        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _title = value;
                }
                else
                {
                    _title = String.Empty;
                }
            }
        }

        public string content
        {
            get
            {
                return _content;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _content = value;
                }
                else
                {
                    _content = String.Empty;
                }
            }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column(Update = false)]
        public string id_create
        {
            get
            {
                return _id_create;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_create = value;
                }
                else
                {
                    _id_create = String.Empty;
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
        public string id_edit
        {
            get
            {
                return _id_edit;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_edit = value;
                }
                else
                {
                    _id_edit = String.Empty;
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

        public Nullable<byte> flag_delete
        {
            get
            {
                return _flag_delete;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_delete = value.Value;
                }
                else
                {
                    _flag_delete = 0;
                }
            }
        }

        public byte[] nlast { set; get; }
        

        #endregion

    }
    public class Ts_Notice_View : Ts_Notice
    {

    }


}

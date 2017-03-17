
#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("tb_role", "Tb_Role")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Role
    {
        #region public method

        public Tb_Role Clone()
        {
            return (Tb_Role)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id = String.Empty;
        private byte _flag_type = 0;
        private string _id_masteruser = String.Empty;
        private byte _flag_update = 0;
        private string _name = String.Empty;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _id_edit = String.Empty;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private string _role_describe = String.Empty;
        #endregion

        #region public property

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

        public Nullable<byte> flag_update
        {
            get
            {
                return _flag_update;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_update = value.Value;
                }
                else
                {
                    _flag_update = 0;
                }
            }
        }

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
        public string role_describe
        {
            get
            {
                return _role_describe;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _role_describe = value;
                }
                else
                {
                    _role_describe = String.Empty;
                }
            }
        }
        #endregion

    }
}



//#region Imports
//using System;
//using System.Diagnostics;
//using CySoft.Model.Mapping;
//using CySoft.Model.Flags;
//#endregion

//#region
//#endregion

//namespace CySoft.Model.Tb
//{
//    /// <summary>
//    /// 角色
//    /// </summary>
//    [Serializable]
//    [Table("Tb_role", "Tb_Role")]
//    [DebuggerDisplay("id = {id}, flag_type = {flag_type}, id_master = {id_master}, flag_update = {flag_update}")]
//    public class Tb_Role
//    {
//        #region public method

//        public Tb_Role Clone()
//        {
//            return (Tb_Role)this.MemberwiseClone();
//        }

//        #endregion

//        #region private field

//        private long _id = 0;
//        private byte _flag_type = 0;
//        private string _id_master =String.Empty;
//        private YesNoFlag _flag_update = 0;
//        private string _name = String.Empty;
//        private string _id_create = String.Empty;
//        private DateTime _rq_create = new DateTime(1900, 1, 1);
//        private string _id_edit = String.Empty;
//        private DateTime _rq_edit = new DateTime(1900, 1, 1);

//        #endregion

//        #region public property

//        /// <summary>
//        /// 主键
//        /// </summary>
//        public Nullable<long> id
//        {
//            get
//            {
//                return _id;
//            }
//            set
//            {
//                if (value.HasValue)
//                {
//                    _id = value.Value;
//                }
//                else
//                {
//                    _id = 0;
//                }
//            }
//        }

//        /// <summary>
//        /// 角色类型    1平台角色  2系统角色  3角色模版 4所属主用户角色
//        /// </summary>
//        public Nullable<byte> flag_type
//        {
//            get
//            {
//                return _flag_type;
//            }
//            set
//            {
//                if (value.HasValue)
//                {
//                    _flag_type = value.Value;
//                }
//                else
//                {
//                    _flag_type = 0;
//                }
//            }
//        }

//        /// <summary>
//        /// 所属主用户
//        /// </summary>
//        public string id_master
//        {
//            get
//            {
//                return _id_master;
//            }
//            set
//            {
//                if (!string.IsNullOrEmpty(value))
//                {
//                    _id_master = value;
//                }
//                else
//                {
//                    _id_master =String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 是否允许修改    1是    0否
//        /// </summary>
//        public YesNoFlag flag_update
//        {
//            get
//            {
//                return _flag_update;
//            }
//            set
//            {
//                _flag_update = value;
//            }
//        }

//        /// <summary>
//        /// 名称
//        /// </summary>
//        public string name
//        {
//            get
//            {
//                return _name;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _name = value;
//                }
//                else
//                {
//                    _name = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 创建人
//        /// </summary>
//        [Column(Update = false)]
//        public string id_create
//        {
//            get
//            {
//                return _id_create;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _id_create = value;
//                }
//                else
//                {
//                    _id_create = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 创建日期
//        /// </summary>
//        [Column(Update = false, Insert = false)]
//        public Nullable<DateTime> rq_create
//        {
//            get
//            {
//                return _rq_create;
//            }
//            set
//            {
//                if (value.HasValue)
//                {
//                    _rq_create = value.Value;
//                }
//                else
//                {
//                    _rq_create = new DateTime(1900, 1, 1);
//                }
//            }
//        }

//        /// <summary>
//        /// 最后修改人
//        /// </summary>
//        public string id_edit
//        {
//            get
//            {
//                return _id_edit;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _id_edit = value;
//                }
//                else
//                {
//                    _id_edit = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 最后修改日期
//        /// </summary>
//        [Column(Insert = false)]
//        public Nullable<DateTime> rq_edit
//        {
//            get
//            {
//                return _rq_edit;
//            }
//            set
//            {
//                if (value.HasValue)
//                {
//                    _rq_edit = value.Value;
//                }
//                else
//                {
//                    _rq_edit = new DateTime(1900, 1, 1);
//                }
//            }
//        }

//        #endregion

//    }
//}
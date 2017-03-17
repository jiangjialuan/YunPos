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
    /// 用户角色
    /// </summary>
    [Serializable]
    [Table("tb_user_role", "Tb_User_Role")]
    [DebuggerDisplay("id_user = {id_user},id_role = {id_role}")]
    public class Tb_User_Role
    {
        #region public method

        public Tb_User_Role Clone()
        {
            return (Tb_User_Role)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_user = String.Empty;
        private string _id_role = String.Empty;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _id_masteruser = String.Empty;
        private byte[] _nlast = null;
        private byte _flag_delete = 0;

        #endregion

        #region public property

        /// <summary>
        /// 用户
        /// </summary>
        public string id_user
        {
            get
            {
                return _id_user;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_user = value;
                }
                else
                {
                    _id_user = String.Empty;
                }
            }
        }

        /// <summary>
        /// 角色
        /// </summary>
        public string id_role
        {
            get
            {
                return _id_role;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_role = value;
                }
                else
                {
                    _id_role = String.Empty;
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
        public Byte[] nlast
        {
            get
            {
                return _nlast;
            }
            set { _nlast = value; }
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

        #endregion

    }
}








//#region Imports
//using System;
//using System.Diagnostics;
//using CySoft.Model.Mapping;
//#endregion

//#region 用户角色
//#endregion

//namespace CySoft.Model.Tb
//{
//    /// <summary>
//    /// 用户角色
//    /// </summary>
//    [Serializable]
//    [Table("Tb_user_role", "Tb_User_Role")]
//    [DebuggerDisplay("id_user = {id_user},id_role = {id_role}")]
//    public class Tb_User_Role
//    {
//        #region public method

//        public Tb_User_Role Clone()
//        {
//            return (Tb_User_Role)this.MemberwiseClone();
//        }

//        #endregion

//        #region private field

//        private string _id_user = String.Empty;
//        private long _id_role = 0;
//        private string _id_create =String.Empty;
//        private DateTime _rq_create = new DateTime(1900, 1, 1);

//        #endregion

//        #region public property

//        /// <summary>
//        /// 用户
//        /// </summary>
//        public string id_user
//        {
//            get
//            {
//                return _id_user;
//            }
//            set
//            {
//                if (!string.IsNullOrEmpty(value))
//                {
//                    _id_user = value ;
//                }
//                else
//                {
//                    _id_user = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 角色
//        /// </summary>
//        public Nullable<long> id_role
//        {
//            get
//            {
//                return _id_role;
//            }
//            set
//            {
//                if (value.HasValue)
//                {
//                    _id_role = value.Value;
//                }
//                else
//                {
//                    _id_role = 0;
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
//                if (!string.IsNullOrEmpty(value))
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

//        #endregion

//    }
//}
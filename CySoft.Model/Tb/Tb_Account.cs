
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
    [Table("tb_account", "Tb_Account")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Account
    {
        #region public method

        public Tb_Account Clone()
        {
            return (Tb_Account)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id = String.Empty;
        private byte _flag_lx = 0;
        private string _username = String.Empty;
        private string _id_user = String.Empty;
        private string _id_edit = String.Empty;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private string _id_masteruser=String.Empty;
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

        public Nullable<byte> flag_lx
        {
            get
            {
                return _flag_lx;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_lx = value.Value;
                }
                else
                {
                    _flag_lx = 0;
                }
            }
        }

        public string username
        {
            get
            {
                return _username;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _username = value;
                }
                else
                {
                    _username = String.Empty;
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

        #endregion

    }
}









//#region Imports
//using System;
//using System.Diagnostics;
//using CySoft.Model.Flags;
//using CySoft.Model.Mapping;
//#endregion

//#region 登录账户
//#endregion

//namespace CySoft.Model.Tb
//{
//    /// <summary>
//    /// 登录账户
//    /// </summary>
//    [Serializable]
//    [Table("Tb_account", "Tb_Account")]
//    [DebuggerDisplay("id = {id}, flag_lx = {flag_lx}, username = {username}, id_user = {id_user}")]
//    public class Tb_Account
//    {
//        #region public method

//        public Tb_Account Clone()
//        {
//            return (Tb_Account)this.MemberwiseClone();
//        }

//        #endregion

//        #region private field

//        private long _id = 0;
//        private AccountFlag _flag_lx = 0;
//        private string _username = String.Empty;
//        private string _id_user = String.Empty;
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
//        /// 账户类型
//        /// </summary>
//        public AccountFlag flag_lx
//        {
//            get
//            {
//                return _flag_lx;
//            }
//            set
//            {
//                _flag_lx = value;
//            }
//        }

//        /// <summary>
//        /// 账户名
//        /// </summary>
//        public string username
//        {
//            get
//            {
//                return _username;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _username = value;
//                }
//                else
//                {
//                    _username = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 用户Id
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
//                if (string.IsNullOrEmpty(value))
//                {
//                    _id_edit = value;
//                }
//                else
//                {
//                    _id_edit =String.Empty;
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
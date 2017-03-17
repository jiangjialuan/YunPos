#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using System.Web;
#endregion

#region
#endregion

namespace CySoft.Model.Ts
{
    [Serializable]
    [Table("ts_log", "Ts_Log")]
    [DebuggerDisplay("id = {id}")]
    public class Ts_Log
    {
        #region public method

        public Ts_Log Clone()
        {
            return (Ts_Log)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id = String.Empty;
        private string _id_user = String.Empty;
        private DateTime _rq = new DateTime(1900, 1, 1);
        private string _content = String.Empty;
        private string _IP = String.Empty;
        private string _flag_from = String.Empty;
        private string _flag_lx = String.Empty;

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

        public Nullable<DateTime> rq
        {
            get
            {
                return _rq;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq = value.Value;
                }
                else
                {
                    _rq = new DateTime(1900, 1, 1);
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

        public string IP
        {
            get
            {
                return _IP;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _IP = value;
                }
                else
                {
                    _IP = String.Empty;
                }
            }
        }

        public string flag_from
        {
            get
            {
                return _flag_from;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _flag_from = value;
                }
                else
                {
                    _flag_from = String.Empty;
                }
            }
        }

        public string flag_lx
        {
            get
            {
                return _flag_lx;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _flag_lx = value;
                }
                else
                {
                    _flag_lx = String.Empty;
                }
            }
        }

        #endregion

    }



    public class Ts_Log_Thread
    {
        public Ts_Log Log{set;get;}
        public HttpContext Context { get; set; }
    }
}










//#region Imports
//using System;
//using System.Diagnostics;
//using CySoft.Model.Flags;
//using CySoft.Model.Mapping;
//#endregion

//#region 日志
//#endregion

//namespace CySoft.Model.Ts
//{
//    /// <summary>
//    /// 日志
//    /// </summary>
//    [Serializable]
//    [Table("Ts_log", "Ts_Log")]
//    [DebuggerDisplay("id = {id}, id_user = {id_user}, flag_lx = {flag_lx}, rq = {rq:yyyy-MM-dd HH:mm:ss}, flag_from = {flag_from}, content = {content}")]
//    public class Ts_Log
//    {
//        #region public method

//        public Ts_Log Clone()
//        {
//            return (Ts_Log)this.MemberwiseClone();
//        }

//        #endregion

//        #region private field

//        private long _id = 0;
//        private string _id_user = String.Empty;
//        private string _flag_lx = String.Empty;
//        private DateTime _rq = new DateTime(1900, 1, 1);
//        private string _ip = String.Empty;
//        private string _flag_from = String.Empty;
//        private string _client = String.Empty;
//        private string _version_client = String.Empty;
//        private string _content = String.Empty;

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
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _id_user = value;
//                }
//                else
//                {
//                    _id_user = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 类型
//        /// </summary>
//        public string flag_lx
//        {
//            get
//            {
//                return _flag_lx;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _flag_lx = value;
//                }
//                else
//                {
//                    _flag_lx = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 时间
//        /// </summary>
//        public Nullable<DateTime> rq
//        {
//            get
//            {
//                return _rq;
//            }
//            set
//            {
//                if (value.HasValue)
//                {
//                    _rq = value.Value;
//                }
//                else
//                {
//                    _rq = new DateTime(1900, 1, 1);
//                }
//            }
//        }

//        /// <summary>
//        /// IP地址
//        /// </summary>
//        public string ip
//        {
//            get
//            {
//                return _ip;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _ip = value;
//                }
//                else
//                {
//                    _ip = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 来源
//        /// </summary>
//        public string flag_from
//        {
//            get
//            {
//                return _flag_from;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _flag_from = value;
//                }
//                else
//                {
//                    _flag_from = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 客户端
//        /// </summary>
//        public string client
//        {
//            get
//            {
//                return _client;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _client = value;
//                }
//                else
//                {
//                    _client = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 客户端版本
//        /// </summary>
//        public string version_client
//        {
//            get
//            {
//                return _version_client;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _version_client = value;
//                }
//                else
//                {
//                    _version_client = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 内容
//        /// </summary>
//        public string content
//        {
//            get
//            {
//                return _content;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _content = value;
//                }
//                else
//                {
//                    _content = String.Empty;
//                }
//            }
//        }

//        #endregion
		

//    }
//}
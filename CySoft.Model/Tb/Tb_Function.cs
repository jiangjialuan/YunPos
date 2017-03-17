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
    [Table("tb_function", "Tb_Function")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Function
    {
        #region public method

        public Tb_Function Clone()
        {
            return (Tb_Function)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id = String.Empty;
        private string _name = String.Empty;
        private string _version = String.Empty;
        private string _flag_type = String.Empty;
        private byte _flag_stop = 0;
        private string _id_father = String.Empty;
        private string _path = String.Empty;
        private int _sort_id = 0;
        private string _controller_name = String.Empty;
        private string _action_name = String.Empty;
        private string _icon = String.Empty;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _id_edit = String.Empty;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private string _tag_name = String.Empty;
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

        public string version
        {
            get
            {
                return _version;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _version = value;
                }
                else
                {
                    _version = String.Empty;
                }
            }
        }

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

        [Column(Update = false)]
        public Nullable<byte> flag_stop
        {
            get
            {
                return _flag_stop;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_stop = value.Value;
                }
                else
                {
                    _flag_stop = 0;
                }
            }
        }

        public string id_father
        {
            get
            {
                return _id_father;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_father = value;
                }
                else
                {
                    _id_father = String.Empty;
                }
            }
        }

        public string path
        {
            get
            {
                return _path;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _path = value;
                }
                else
                {
                    _path = String.Empty;
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

        public string controller_name
        {
            get
            {
                return _controller_name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _controller_name = value;
                }
                else
                {
                    _controller_name = String.Empty;
                }
            }
        }

        public string action_name
        {
            get
            {
                return _action_name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _action_name = value;
                }
                else
                {
                    _action_name = String.Empty;
                }
            }
        }

        public string icon
        {
            get
            {
                return _icon;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _icon = value;
                }
                else
                {
                    _icon = String.Empty;
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
        public string tag_name
        {
            get
            {
                return _tag_name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _tag_name = value;
                }
                else
                {
                    _tag_name = String.Empty;
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

//#region 系统功能模块
//#endregion

//namespace CySoft.Model.Tb
//{
//    /// <summary>
//    /// 系统功能模块
//    /// </summary>
//    [Serializable]
//    [Table("Tb_function", "Tb_Function")]
//    [DebuggerDisplay("id = {id}, fatherId = {fatherId},controller_name={controller_name},action_name={action_name},icon={icon}, name = {name}, flag_type = {flag_type}, flag_stop = {flag_stop}, sort_id = {sort_id}")]
//    public class Tb_Function
//    {
//        #region public method

//        public Tb_Function Clone()
//        {
//            return (Tb_Function)this.MemberwiseClone();
//        }

//        #endregion

//        #region private field

//        private int _id = 0;
//        private string _name = String.Empty;
//        private string _version = String.Empty;
//        private string _flag_type = String.Empty;
//        private byte _flag_stop = 0;
//        private long _fatherId = 0;
//        private string _path = String.Empty;
//        private int _sort_id = 0;
//        private string _id_create = String.Empty;
//        private DateTime _rq_create = new DateTime(1900, 1, 1);
//        private string _id_edit = String.Empty;
//        private DateTime _rq_edit = new DateTime(1900, 1, 1);
//        private string _controller_name = String.Empty;
//        private string _action_name = String.Empty;
//        private string _icon = String.Empty;
//        #endregion

//        #region public property

//        /// <summary>
//        /// 主键
//        /// </summary>
//        public Nullable<int> id
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
//        /// 模块名称
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
//        /// 版本信息
//        /// </summary>
//        public string version
//        {
//            get
//            {
//                return _version;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _version = value;
//                }
//                else
//                {
//                    _version = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 类型    bs_lx='moduleFlag'    module模块   controller控制层    action方法
//        /// </summary>
//        public string flag_type
//        {
//            get
//            {
//                return _flag_type;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _flag_type = value;
//                }
//                else
//                {
//                    _flag_type = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 状态    1停用   0可用
//        /// </summary>
//        [Column(Update = false)]
//        public Nullable<byte> flag_stop
//        {
//            get
//            {
//                return _flag_stop;
//            }
//            set
//            {
//                if (value.HasValue)
//                {
//                    _flag_stop = value.Value;
//                }
//                else
//                {
//                    _flag_stop = 0;
//                }
//            }
//        }

//        /// <summary>
//        /// 上级id
//        /// </summary>
//        public Nullable<long> fatherId
//        {
//            get
//            {
//                return _fatherId;
//            }
//            set
//            {
//                if (value.HasValue)
//                {
//                    _fatherId = value.Value;
//                }
//                else
//                {
//                    _fatherId = 0;
//                }
//            }
//        }

//        /// <summary>
//        /// 上级关系路径
//        /// </summary>
//        public string path
//        {
//            get
//            {
//                return _path;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _path = value;
//                }
//                else
//                {
//                    _path = String.Empty;
//                }
//            }
//        }

//        /// <summary>
//        /// 排序
//        /// </summary>
//        public Nullable<int> sort_id
//        {
//            get
//            {
//                return _sort_id;
//            }
//            set
//            {
//                if (value.HasValue)
//                {
//                    _sort_id = value.Value;
//                }
//                else
//                {
//                    _sort_id = 0;
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
//                if (!string.IsNullOrEmpty(value))
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
//        public string controller_name
//        {
//            get
//            {
//                return _controller_name;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _controller_name = value;
//                }
//                else
//                {
//                    _controller_name = String.Empty;
//                }
//            }
//        }

//        public string action_name
//        {
//            get
//            {
//                return _action_name;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _action_name = value;
//                }
//                else
//                {
//                    _action_name = String.Empty;
//                }
//            }
//        }

//        public string icon
//        {
//            get
//            {
//                return _icon;
//            }
//            set
//            {
//                if (!String.IsNullOrEmpty(value))
//                {
//                    _icon = value;
//                }
//                else
//                {
//                    _icon = String.Empty;
//                }
//            }
//        }
//        #endregion

//    }
//}
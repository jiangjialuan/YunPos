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
    [Table("tb_role_function", "Tb_Role_Function")]
    [DebuggerDisplay("id_role = {id_role},id_function = {id_function}")]
    public class Tb_Role_Function
    {
        #region public method

        public Tb_Role_Function Clone()
        {
            return (Tb_Role_Function)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_role = String.Empty;
        private string _id_function = String.Empty;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);

        #endregion

        #region public property

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

        public string id_function
        {
            get
            {
                return _id_function;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_function = value;
                }
                else
                {
                    _id_function = String.Empty;
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

        #endregion

    }
}






//#region Imports
//using System;
//using System.Diagnostics;
//using CySoft.Model.Mapping;
//#endregion

//#region
//#endregion

//namespace CySoft.Model.Tb
//{
//    /// <summary>
//    /// 角色功能
//    /// </summary>
//    [Serializable]
//    [Table("Tb_role_function", "Tb_Role_Function")]
//    [DebuggerDisplay("id_role = {id_role},id_function = {id_function}")]
//    public class Tb_Role_Function
//    {
//        #region public method

//        public Tb_Role_Function Clone()
//        {
//            return (Tb_Role_Function)this.MemberwiseClone();
//        }

//        #endregion

//        #region private field

//        private long _id_role = 0;
//        private int _id_function = 0;
//        private string _id_create = String.Empty;
//        private DateTime _rq_create = new DateTime(1900, 1, 1);

//        #endregion

//        #region public property

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
//        /// 系统模块功能
//        /// </summary>
//        public Nullable<int> id_function
//        {
//            get
//            {
//                return _id_function;
//            }
//            set
//            {
//                if (value.HasValue)
//                {
//                    _id_function = value.Value;
//                }
//                else
//                {
//                    _id_function = 0;
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
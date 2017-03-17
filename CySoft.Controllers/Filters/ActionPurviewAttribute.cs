#region Imports
using System;
using System.Runtime.InteropServices;
#endregion

namespace CySoft.Controllers.Filters
{
    /// <summary>
    /// 权限校验设置
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class ActionPurviewAttribute : Attribute, _Attribute
    {
        private bool _Check = true;
        private string _ActionName;

        /// <summary>
        /// 实例化权限校验设置
        /// </summary>
        public ActionPurviewAttribute() { }
        /// <summary>
        /// 实例化权限校验设置
        /// </summary>
        /// <param name="check">是否校验</param>
        public ActionPurviewAttribute(bool check)
        {
            this._Check = check;
        }
        /// <summary>
        /// 实例化权限校验设置
        /// </summary>
        /// <param name="actionName">Action别名</param>
        public ActionPurviewAttribute(string actionName)
        {
            this._ActionName = actionName;
        }
        /// <summary>
        /// 实例化权限校验设置
        /// </summary>
        /// <param name="check">是否校验</param>
        /// <param name="actionName">Action别名</param>
        public ActionPurviewAttribute(bool check, string actionName)
        {
            this._Check = check;
            this._ActionName = actionName;
        }
        /// <summary>
        /// 是否校验
        /// </summary>
        public bool Check
        {
            get
            {
                return _Check;
            }
        }
        /// <summary>
        /// Action别名
        /// </summary>
        public string ActionName
        {
            get
            {
                return _ActionName;
            }
            set
            {
                _ActionName = value;
            }
        }
    }
}

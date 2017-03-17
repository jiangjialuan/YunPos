using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Controllers.Filters
{
    /// <summary>
    /// 权限验证别名标签
    /// </summary>
    [AttributeUsage(AttributeTargets.All,AllowMultiple = true)]
    public class ActionAliasAttribute:Attribute
    {
        private string controller;
        private string action;
        public ActionAliasAttribute(string controllerName, string actionName)
        {
            this.controller = controllerName;
            this.action = actionName;
        }
        public string Controller
        {
            get
            {
                if (!string.IsNullOrEmpty(controller))
                {
                    return controller.ToLower();
                }
                return String.Empty;
            }
        }
        public string Action
        {
            get
            {
                if (!string.IsNullOrEmpty(action))
                {
                    return action.ToLower();
                }
                return String.Empty;
            }
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CySoft.Controllers.Base;
using CySoft.Frame.Core;

namespace CySoft.Controllers.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class W_LoginActionFilter : ActionFilterAttribute
    {
        private static IDictionary<string, ActionPurviewAttribute> attrList = new Dictionary<string, ActionPurviewAttribute>();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            BaseResult br = new BaseResult();
            HttpContextBase context = filterContext.HttpContext;
            if (context.Session["LoginInfo"] == null)
            {
                context.Response.Clear();
                if (context.Request.IsAjaxRequest())
                {
                    br.Success = false;
                    br.Level = ErrorLevel.NotLogin;
                    br.Message.Add("<h5>您未登录或登录已超时！</h5>");
                    br.Message.Add("");
                    br.Message.Add("请重新登录");

                    filterContext.Result = new JsonResult() { Data = br };
                }
                else
                {
                    RouteValueDictionary routeValues = new RouteValueDictionary();
                    routeValues.Add("redirect", context.Request.Url.PathAndQuery);
                    routeValues.Add("action", "Login");
                    routeValues.Add("controller", "W_Account");
                    filterContext.Result = new RedirectToRouteResult("WeiWeb_default", routeValues);
                }
            }
            else
            {
                br = CheckPurview(filterContext);
                if (!br.Success)
                {
                    context.Response.Clear();
                    if (context.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new JsonResult() { Data = br };
                    }
                    else
                    {
                        RouteValueDictionary routeValues = new RouteValueDictionary();
                        routeValues.Add("action", "Index");
                        routeValues.Add("controller", "Error");
                        routeValues.Add("id", "noPurview");
                        filterContext.Result = new RedirectToRouteResult("Default", routeValues);
                    }
                }
            }
        }

        /// <summary>
        /// 校验权限
        /// </summary>
        private BaseResult CheckPurview(ActionExecutingContext filterContext)
        {
            BaseResult br = new BaseResult();
            //获取请求的controller和action
            Type controllerType = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType;
            string actionName = filterContext.ActionDescriptor.ActionName.Trim();
            //获取Action的ActionPurviewAttribute
            ActionPurviewAttribute apAttr = GetActionPurviewAttr(controllerType, actionName);
            if (!apAttr.Check)
            {
                br.Success = true;
                return br;
            }
            Hashtable userinfo = (Hashtable)filterContext.HttpContext.Session["LoginInfo"];
            long id_user = Convert.ToInt64(userinfo["id_user"]);


            br.Message.Add("您无权操作此功能，请联系管理员！");
            br.Level = ErrorLevel.Error;
            br.Success = false;
            return br;
        }

        /// <summary>
        /// 获取校验设置
        /// </summary>
        private ActionPurviewAttribute GetActionPurviewAttr(Type controllerType, string actionName)
        {
            string key = controllerType.FullName + "." + actionName.ToLower();
            if (attrList.ContainsKey(key))
            {
                return attrList[key];
            }
            //处理请求的Action名大小写问题
            MethodInfo[] methodinfos = controllerType.GetMethods();
            foreach (MethodInfo methodinfo in methodinfos)
            {
                if (methodinfo.Name.ToUpper() == actionName.ToUpper())
                {
                    actionName = methodinfo.Name;
                    break;
                }
            }
            //获取Action的ActionPurviewAttribute
            ActionPurviewAttribute apaAttr = null;
            object[] attrs = controllerType.GetMethod(actionName).GetCustomAttributes(true);
            foreach (object attr in attrs)
            {
                if (attr is ActionPurviewAttribute)
                {
                    apaAttr = (ActionPurviewAttribute)attr;
                    break;
                }
            }
            if (apaAttr == null)
            {
                apaAttr = new ActionPurviewAttribute(actionName);
            }
            if (String.IsNullOrEmpty(apaAttr.ActionName))
            {
                apaAttr.ActionName = actionName;
            }
            attrList[key] = apaAttr;
            return apaAttr;
        }
    }
}
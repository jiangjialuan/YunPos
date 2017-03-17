using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CySoft.Controllers.Base;
using CySoft.Frame.Core;
using CySoft.Utility;
using System.Text;

namespace CySoft.Controllers.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class LoginActionFilter : ActionFilterAttribute
    {
        protected static IDictionary<string, ActionPurviewAttribute> attrList = new Dictionary<string, ActionPurviewAttribute>();
        protected static readonly Type actionPurviewAttributeType = typeof(ActionPurviewAttribute);

        #region 执行Action前执行 过滤
        /// <summary>
        /// 执行Action前执行
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            BaseResult br = new BaseResult();
            HttpContextBase context = filterContext.HttpContext;

            #region 验登录 Session
            if (context.Session["LoginInfo"] == null)
            {
                //var savePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Log\\" + "FilterLog\\FilterLog.txt";
                //new CySoft.Frame.Common.LogText().Log2File(savePath, "Session Error:    " + JSON.Serialize(br));

                context.Response.Clear();
                if (context.Request.IsAjaxRequest())
                {
                    br.Success = false;
                    br.Level = ErrorLevel.NotLogin;
                    br.Message.Add("<h5>您未登录或登录已超时！</h5>");
                    filterContext.Result = new JsonResult() { Data = br, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    return;
                }
                else
                {
                    RouteValueDictionary routeValues = new RouteValueDictionary();
                    routeValues.Add("action", "Login");
                    routeValues.Add("controller", "Account");
                    filterContext.Result = new RedirectToRouteResult(routeValues);
                    return;
                }
            }
            #endregion

            #region 验权限
            br = CheckPurview(filterContext);
            if (!br.Success)
            {
                //var savePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Log\\" + "FilterLog\\FilterLog.txt";
                //new CySoft.Frame.Common.LogText().Log2File(savePath, "权限 Error:    " + JSON.Serialize(br));

                context.Response.Clear();
                if (context.Request.IsAjaxRequest())
                {
                    br.Level = ErrorLevel.Warning;
                    filterContext.Result = new JsonResult() { Data = br, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    return;
                }
                else
                {
                    RouteValueDictionary routeValues = new RouteValueDictionary();
                    routeValues.Add("id", "noPurview");
                    filterContext.Result = new RedirectToRouteResult("Error", routeValues);
                    return;
                }
            }
            #endregion

            #region 验服务

            if (PublicSign.flagCheckService == "1")
            {
                #region 验服务
                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                string actionName = filterContext.ActionDescriptor.ActionName;
                if (
                    (controllerName.ToLower() == "shopsp"&& actionName.ToLower()=="add")
                    || (controllerName.ToLower() == "shopsp" && actionName.ToLower() == "edit")
                    || (controllerName.ToLower() == "shopsp" && actionName.ToLower() == "batchedit")
                    || (controllerName.ToLower() == "shopsp" && actionName.ToLower() == "delete")
                    || (controllerName.ToLower() == "shopsp" && actionName.ToLower() == "importIn")
                    || (controllerName.ToLower() == "account" && actionName.ToLower() == "add")
                    )
                {
                    br = CheckService(filterContext);
                    if (!br.Success)
                    {
                        var savePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Log\\" + "FilterLog\\FilterLog.txt";
                        new CySoft.Frame.Common.LogText().Log2File(savePath, "服务 Error:    " + JSON.Serialize(br));

                        #region 获取服务异常
                        if (br.Level == ErrorLevel.Error)
                        {
                            //获取服务异常
                            context.Session["LoginInfo"] = null;
                            context.Response.Clear();
                            if (context.Request.IsAjaxRequest())
                            {
                                br.Success = false;
                                br.Level = ErrorLevel.NotLogin;
                                br.Message.Add("<h5>服务信息丢失 需重新登录！</h5>");
                                filterContext.Result = new JsonResult() { Data = br, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                                return;
                            }
                            else
                            {
                                RouteValueDictionary routeValues = new RouteValueDictionary();
                                routeValues.Add("action", "Login");
                                routeValues.Add("controller", "Account");
                                filterContext.Result = new RedirectToRouteResult(routeValues);
                                return;
                            }
                        }
                        #endregion

                        #region 门店超过购买服务 需要跳转至门店处理页面
                        if (br.Level == ErrorLevel.Question)
                        {
                            //如果是 门店超过购买服务 需要跳转至门店处理页面
                            context.Response.Clear();
                            if (context.Request.IsAjaxRequest())
                            {
                                filterContext.Result = new JsonResult() { Data = br, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                                return;
                            }
                            else
                            {
                                RouteValueDictionary routeValues = new RouteValueDictionary();
                                routeValues.Add("action", "Set");
                                routeValues.Add("controller", "ShopManager");
                                filterContext.Result = new RedirectToRouteResult(routeValues);
                                return;
                            }
                        }
                        #endregion

                        #region 跳转至 购买服务 页面
                        if (br.Level == ErrorLevel.Drump)
                        {
                            //如果是 需要购买服务 需要跳转至购买服务页面
                            context.Response.Clear();
                            RouteValueDictionary routeValues = new RouteValueDictionary();
                            routeValues.Add("action", "Index");
                            routeValues.Add("controller", "Iframe");
                            routeValues.Add("url", br.Data);
                            filterContext.Result = new RedirectToRouteResult(routeValues);
                            return;
                        }
                        #endregion

                    }
                }
                #endregion
            }



            #region 注释

            //if (PublicSign.flagCheckService == "1")
            //{
            //    #region 验服务
            //    string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //    string actionName = filterContext.ActionDescriptor.ActionName;

            //    if (controllerName.ToLower() != "manager" && actionName.ToLower() != "home")
            //    {
            //        br = CheckService(filterContext);
            //        if (!br.Success)
            //        {
            //            var savePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Log\\" + "FilterLog\\FilterLog.txt";
            //            new CySoft.Frame.Common.LogText().Log2File(savePath, "服务 Error:    " + JSON.Serialize(br));

            //            #region 获取服务异常
            //            if (br.Level == ErrorLevel.Error)
            //            {
            //                //获取服务异常
            //                context.Session["LoginInfo"] = null;
            //                context.Response.Clear();
            //                if (context.Request.IsAjaxRequest())
            //                {
            //                    br.Success = false;
            //                    br.Level = ErrorLevel.NotLogin;
            //                    br.Message.Add("<h5>服务信息丢失 需重新登录！</h5>");
            //                    filterContext.Result = new JsonResult() { Data = br, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //                    return;
            //                }
            //                else
            //                {
            //                    RouteValueDictionary routeValues = new RouteValueDictionary();
            //                    routeValues.Add("action", "Login");
            //                    routeValues.Add("controller", "Account");
            //                    filterContext.Result = new RedirectToRouteResult(routeValues);
            //                    return;
            //                }
            //            }
            //            #endregion

            //            #region 门店超过购买服务 需要跳转至门店处理页面
            //            if (br.Level == ErrorLevel.Question)
            //            {
            //                //如果是 门店超过购买服务 需要跳转至门店处理页面
            //                context.Response.Clear();
            //                if (context.Request.IsAjaxRequest())
            //                {
            //                    filterContext.Result = new JsonResult() { Data = br, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //                    return;
            //                }
            //                else
            //                {
            //                    RouteValueDictionary routeValues = new RouteValueDictionary();
            //                    routeValues.Add("action", "Set");
            //                    routeValues.Add("controller", "ShopManager");
            //                    filterContext.Result = new RedirectToRouteResult(routeValues);
            //                    return;
            //                }
            //            }
            //            #endregion

            //            #region 跳转至 购买服务 页面
            //            if (br.Level == ErrorLevel.Drump)
            //            {
            //                //如果是 需要购买服务 需要跳转至购买服务页面
            //                context.Response.Clear();
            //                RouteValueDictionary routeValues = new RouteValueDictionary();
            //                routeValues.Add("action", "Index");
            //                routeValues.Add("controller", "Iframe");
            //                routeValues.Add("url", br.Data);
            //                filterContext.Result = new RedirectToRouteResult(routeValues);
            //                return;
            //            }
            //            #endregion

            //        }
            //    }
            //    #endregion
            //}

            #endregion

            #endregion

        }
        #endregion

        #region 校验权限
        /// <summary>
        /// 校验权限
        /// </summary>
        private BaseResult CheckPurview(ActionExecutingContext filterContext)
        {
            BaseResult br = new BaseResult();

            //TODO：YZQ 屏蔽校验权限
            //获取请求的controller和action
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            //获取Action的ActionPurviewAttribute
            ActionPurviewAttribute apAttr = GetActionPurviewAttr(filterContext.ActionDescriptor, actionName);
            if (!apAttr.Check)
            {
                br.Success = true;
                return br;
            }
            Hashtable userinfo = (Hashtable)filterContext.HttpContext.Session["LoginInfo"];
            var attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(ActionAliasAttribute), false);
            ActionAliasAttribute actionAliasAttribute = null;
            List<ActionAliasAttribute> actionAliasAttributes = new List<ActionAliasAttribute>();
            if (attrs != null && attrs.Length > 0)
            {
                var objs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(ActionAliasAttribute), false);
                foreach (var obj in objs)
                {
                    actionAliasAttributes.Add(obj as ActionAliasAttribute);
                }
                actionAliasAttribute = filterContext.ActionDescriptor.GetCustomAttributes(typeof(ActionAliasAttribute), false)[0] as ActionAliasAttribute;
            }
            Hashtable param = new Hashtable();
            if (actionAliasAttributes.Any())
            {
                var strController = string.Join(",", from a in actionAliasAttributes select a.Controller);
                var strAction = string.Join(",", from a in actionAliasAttributes select a.Action);
                param.Add("id_user", userinfo["id_user"]);
                param.Add("controllerName", strController);
                param.Add("actionName", strAction);
                br = BusinessFactory.AccountFunction.Check(param);
            }
            else if (actionAliasAttribute != null
                && !string.IsNullOrEmpty(actionAliasAttribute.Controller)
                && !string.IsNullOrEmpty(actionAliasAttribute.Action))
            {
                param.Add("id_user", userinfo["id_user"]);
                param.Add("controllerName", actionAliasAttribute.Controller);
                param.Add("actionName", actionAliasAttribute.Action);
                br = BusinessFactory.AccountFunction.Check(param);
            }
            else
            {
                param.Add("id_user", userinfo["id_user"]);
                param.Add("controllerName", controllerName);
                param.Add("actionName", apAttr.ActionName);
                br = BusinessFactory.AccountFunction.Check(param);
            }
            return br;
        }

        #endregion

        #region 获取校验设置
        /// <summary>
        /// 获取校验设置
        /// </summary>
        private ActionPurviewAttribute GetActionPurviewAttr(ActionDescriptor actionDescriptor, string actionName)
        {
            if (attrList.ContainsKey(actionDescriptor.UniqueId))
            {
                return attrList[actionDescriptor.UniqueId];
            }
            ActionPurviewAttribute apaAttr = null;
            object[] attrs = actionDescriptor.GetCustomAttributes(actionPurviewAttributeType, true);
            if (attrs.Length > 0)
            {
                apaAttr = (ActionPurviewAttribute)attrs[0];
            }
            if (apaAttr == null)
            {
                apaAttr = new ActionPurviewAttribute(actionName);
            }
            if (String.IsNullOrEmpty(apaAttr.ActionName))
            {
                apaAttr.ActionName = actionName;
            }
            attrList[actionDescriptor.UniqueId] = apaAttr;
            return apaAttr;
        }

        #endregion

        #region 检查购买的服务
        /// <summary>
        /// 检查购买的服务
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private BaseResult CheckService(ActionExecutingContext filterContext)
        {
            BaseResult br = new BaseResult() { Success = true };
            Hashtable userinfo = (Hashtable)filterContext.HttpContext.Session["LoginInfo"];
            Hashtable cyServiceHas = new Hashtable();

            if (DataCache.Get(userinfo["id_user_master"].ToString() + "_GetCYService") != null && ((Hashtable)DataCache.Get(userinfo["id_user_master"].ToString() + "_GetCYService")).Keys.Count > 0)
                cyServiceHas = (Hashtable)DataCache.Get(userinfo["id_user_master"].ToString() + "_GetCYService");

            if (cyServiceHas == null || !cyServiceHas.ContainsKey("cyServiceList") || !cyServiceHas.ContainsKey("isExpire") || !cyServiceHas.ContainsKey("isStop") || !cyServiceHas.ContainsKey("isOutLimit"))
            {
                BusinessFactory.Account.CheckServiceForLogin(userinfo);
                if (DataCache.Get(userinfo["id_user_master"].ToString() + "_GetCYService") != null && ((Hashtable)DataCache.Get(userinfo["id_user_master"].ToString() + "_GetCYService")).Keys.Count > 0)
                    cyServiceHas = (Hashtable)DataCache.Get(userinfo["id_user_master"].ToString() + "_GetCYService");
            }

            if (cyServiceHas == null || !cyServiceHas.ContainsKey("cyServiceList") || !cyServiceHas.ContainsKey("isExpire") || !cyServiceHas.ContainsKey("isStop") || !cyServiceHas.ContainsKey("isOutLimit"))
            {
                br.Message.Clear();
                br.Success = false;
                br.Message.Add("获取购买服务信息失败 请重新登录!");
                br.Level = ErrorLevel.Error;
                return br;
            }



            if (cyServiceHas["isStop"].ToString() == "1")
            {
                br.Message.Clear();
                br.Success = false;
                br.Message.Add("您购买的服务信息已停用 不允许操作!");
                br.Level = ErrorLevel.Drump;
                br.Data = this.GetUrl(userinfo);
                return br;
            }

            if (cyServiceHas["isOutLimit"].ToString() == "1")
            {
                br.Message.Clear();
                br.Success = false;
                br.Message.Add("您的门店设置已经超过购买的服务 不允许操作!");
                br.Level = ErrorLevel.Question;
                br.Data = this.GetUrl(userinfo);
                return br;
            }

            if (cyServiceHas["isExpire"].ToString() == "1")
            {
                br.Message.Clear();
                br.Success = false;
                br.Message.Add("您还未购买服务信息或已超过有效期 不允许操作!");
                br.Level = ErrorLevel.Drump;
                br.Data = this.GetUrl(userinfo);
                return br;
            }

            return br;

        } 
        #endregion

        #region 获取服务购买地址
        /// <summary>
        /// 获取服务购买地址
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public string GetUrl(Hashtable loginInfo)
        {
            string bm = BusinessFactory.Account.GetServiceBM(loginInfo["version"].ToString());
            if (string.IsNullOrEmpty(bm))
            {
                return "";
            }
            Hashtable ht = new Hashtable();
            ht.Clear();
            ht.Add("id_cyuser", loginInfo["id_cyuser"].ToString());
            ht.Add("id", bm);
            ht.Add("phone", loginInfo["phone_master"].ToString());
            ht.Add("service", "Detail");
            ht.Add("id_masteruser", loginInfo["id_user_master"].ToString());
            string buyUrl = BusinessFactory.Account.GetBuyServiceUrl(ht);
            if (string.IsNullOrEmpty(buyUrl))
                buyUrl = PublicSign.cyBuyServiceUrl;

            var jumpUrl = buyUrl = HttpUtility.UrlEncode(buyUrl, Encoding.UTF8);
            return jumpUrl;
        } 
        #endregion

    }
}
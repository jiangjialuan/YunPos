using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using CySoft.Controllers.Base;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Utility;
using CySoft.Frame.Common;
using System.Web.Security;
using CySoft.Utility.WeChat;
using System.IO;
using System.Text;
using System.Web.Routing;
using CySoft.Model.Config;

namespace CySoft.Controllers.Mobile.Base
{
    public class MobileBaseController : Controller
    {

        //获取access_token
        protected string GetAccessToken(string appid, string appsecret)
        {
            if (string.IsNullOrEmpty(appid) || string.IsNullOrEmpty(appsecret))
            {
                return null;
            }

            string key = string.Format("WeChat.Access_Token_{0}_{1}", appid, appsecret);

            string access_token = DataCache.Get<string>(key);

            if (string.IsNullOrEmpty(access_token))
            {
                var dict = WeChatHelper.GetAccessToken(appid, appsecret);

                if (dict != null && dict["access_token"] != null)
                {
                    access_token = dict["access_token"].ToString();
                    DataCache.Remove(key);
                    DataCache.Add(key, access_token, new TimeSpan(0, 0, int.Parse(dict["expires_in"].ToString()) - 200));
                }
            }

            return access_token;
        }

        /// <summary>
        /// 获得登录信息
        /// 用户名:username
        /// 公司名：companyname
        /// 所属主用户：id_user_master
        /// 是否供应商：flag_supplier
        /// 供应商Id：id_supplier
        /// 供应商是否停用：flag_stop_supplier
        /// 是否采购商：flag_buyer
        /// 采购商Id：id_buyer
        /// </summary>
        /// <typeparam name="T">指定返回类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        protected T GetLoginInfo<T>(string key)
        {
            T result = default(T);
            if (!IsLogOn)
            {
                BaseResult br = new BaseResult();
                br.Success = false;
                br.Message.Add("您未登录或登录已超时，请重新登录！");
                br.Level = ErrorLevel.Drump;
                throw new CySoftException(br);
            }
            try
            {
                Hashtable loginInfo = (Hashtable)Session["MobileLoginInfo"];
                if (loginInfo.ContainsKey(key))
                {
                    object value = loginInfo[key];
                    if (value != null && !Convert.IsDBNull(value))
                    {
                        Type type = typeof(T);
                        if (Type.GetTypeCode(type) != TypeCode.Object && Type.GetTypeCode(type) != TypeCode.Empty)
                        {
                            return (T)Convert.ChangeType(value, type);
                        }
                        return (T)value;
                    }
                }
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                throw new CySoftException("登陆信息获取失败,请联系管理员！", ErrorLevel.Error);
            }
            return result;
        }

        /// <summary>
        /// 设置登录信息
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        protected void SetLoginInfo(string key, object value)
        {
            if (!IsLogOn)
            {
                BaseResult br = new BaseResult();
                br.Success = false;
                br.Message.Add("您未登录或登录已超时，请重新登录！");
                br.Level = ErrorLevel.Drump;
                throw new CySoftException(br);
            }
            try
            {
                Hashtable loginInfo = (Hashtable)Session["MobileLoginInfo"];
                loginInfo[key] = value;
                Session["MobileLoginInfo"] = loginInfo;
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                throw new CySoftException("登陆信息设置失败,请联系管理员！", ErrorLevel.Error);
            }
        }

        /// <summary>
        /// 是否已登录
        /// </summary>
        protected bool IsLogOn
        {
            get
            {
                object obj;
                if ((obj = Session["MobileLoginInfo"]) == null || !(obj is Hashtable))
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 获得所有请求参数
        /// </summary>
        protected virtual Hashtable GetParameters()
        {
            Hashtable param = new Hashtable();
            foreach (string item in Request.QueryString.AllKeys)
            {
                param[item] = Request.QueryString[item].Trim();
            }
            foreach (string item in Request.Form.AllKeys)
            {
                param[item] = Request.Form[item].Trim();
            }
            return param;
        }

        /// <summary>
        /// 获得指定请求参数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">当值为null时使用的默认值。</param>
        /// <returns>值</returns>
        protected string GetParameter(string key, string defaultValue = null)
        {
            string value = Request.Form[key] ?? Request.QueryString[key];
            if (value == null)
            {
                return defaultValue;
            }
            return value.Trim();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!IsLogOn && filterContext.ActionDescriptor.ActionName.ToLower() != "login" && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower() != "mobileaccount")
            {
                if (!(filterContext.ActionDescriptor.ActionName.ToLower() == "index" && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower() == "mobilehome"))
                {
                    filterContext.Result = new RedirectToRouteResult("Default",
                                   new System.Web.Routing.RouteValueDictionary{
                   {"controller", "MobileAccount"},
                   {"action", "Login"},
                   {"returnUrl", filterContext.HttpContext.Request.RawUrl}
                   });
                }
                return;
            }
            else if (IsLogOn && ((filterContext.ActionDescriptor.ActionName.ToLower() == "login" && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower() == "mobileaccount") ||
                Request.Path == "/"))
            {
                Response.Redirect("/MobileOrderGoods/List");
                Response.End();
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 页面信息
        /// </summary>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!(filterContext.Result is JsonResult) //Ajax请求
                && !(filterContext.Result is ContentResult) //字符串内容返回
                && !(filterContext.Result is RedirectToRouteResult) //重定向
                )
            {
                Hashtable loginInfo = Session["MobileLoginInfo"] != null ? (Hashtable)Session["MobileLoginInfo"] : new Hashtable();
                ViewData["loginInfo.isLogOn"] = loginInfo == null ? false : true;
                ViewData["loginInfo.name"] = loginInfo["name"] ?? "";
                ViewData["loginInfo.username"] = loginInfo["username"] ?? "";
                ViewData["loginInfo.companyname"] = loginInfo["companyname"] ?? "";
                ViewData["loginInfo.id_user"] = loginInfo["id_user"] ?? "";
                ViewData["loginInfo.id_user_master"] = loginInfo["id_user_master"] ?? "";
                ViewData["loginInfo.flag_buyer"] = loginInfo["flag_buyer"] ?? YesNoFlag.No;
                ViewData["loginInfo.id_buyer"] = loginInfo["id_buyer"] ?? (long)0;
                ViewData["loginInfo.flag_supplier"] = loginInfo["flag_supplier"] ?? YesNoFlag.No;
                ViewData["loginInfo.id_supplier"] = loginInfo["id_supplier"] ?? (long)0;
                ViewData["loginInfo.flag_agent"] = loginInfo["flag_agent"] ?? YesNoFlag.Yes;
                ViewData["loginInfo.flag_platform"] = loginInfo["flag_platform"] ?? YesNoFlag.Yes;
                ViewData["loginInfo.flag_master"] = loginInfo["flag_master"] ?? YesNoFlag.No;
                if (IsLogOn)
                {
                    if (!Request.IsAjaxRequest())
                    {
                        Hashtable param = new Hashtable();
                        param.Add("id_cgs", loginInfo["id_buyer"]);
                        param.Add("id_user", loginInfo["id_user"]);
                        BaseResult br1 = BusinessFactory.GoodsCart.GetCount(param);
                        ViewData["context.cartCount"] = br1.Data ?? 0;
                    }
                }
            }
            base.OnActionExecuted(filterContext);
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="flag_lx">日志类型</param>
        /// <param name="content">内容</param>
        protected void WriteDBLog(LogFlag flag_lx, string content)
        {
            Hashtable loginInfo = (Hashtable)(Session["MobileLoginInfo"] ?? new Hashtable());
            BusinessFactory.Log.Add(loginInfo, flag_lx, content);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="flag_lx">日志类型</param>
        /// <param name="messageList">消息</param>
        protected void WriteDBLog(LogFlag flag_lx, IList<string> messageList)
        {
            Hashtable loginInfo = (Hashtable)(Session["MobileLoginInfo"] ?? new Hashtable());
            BusinessFactory.Log.Add(loginInfo, flag_lx, messageList);
        }

        /// <summary>
        /// 判断请求来源是否微信
        /// </summary>
        /// <returns></returns>
        protected bool IsRequestFromWeChat()
        {
            return Request.UserAgent.ToLower().Contains("micromessenger");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Model.Other;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Utility;
using System.Collections;

namespace Web.Areas.WeiWeb.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None)]
    public class W_ProductController : BaseController
    {
        /// <summary>
        /// 登录页
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 用户登录
        /// lxt
        /// 2014-02-11
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult LogOn(UserLogin model)
        {
            //post获取用户名和密码2015年2月5日16:30:45 create by cxb
            Hashtable param = GetParameters();
            BaseResult br = new BaseResult();
            try
            {
                //if (String.IsNullOrEmpty(param["username"].ToString().Trim()))
                //{
                //    br.Message.Add("请输入用户名");
                //}
                //if (String.IsNullOrEmpty(param["password"].ToString().Trim()))
                //{
                //    br.Message.Add("请输入密码");
                //}
                //string allowloginvalid = (AppConfig.GetValue("allowloginvalid") ?? "yes").ToLower();
                //if (allowloginvalid != "no")
                //{
                //    if (model.vaildcode == null || String.IsNullOrEmpty(model.vaildcode = model.vaildcode.Trim()))
                //    {
                //        br.Message.Add("请输入验证码");
                //    }
                //}
                if (br.Message.Count > 0)
                {
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                //if (allowloginvalid != "no")
                //{
                //    object validCode = Session["SystemValidCode"];
                //    if (validCode == null)
                //    {
                //        br.Success = false;
                //        br.Message.Add("验证码已过期");
                //        br.Level = ErrorLevel.Warning;
                //        return Json(br);
                //    }
                //    Session.Remove("SystemValidCode");
                //    if (model.vaildcode.ToLower() != validCode.ToString().ToLower())
                //    {
                //        br.Success = false;
                //        br.Message.Add("验证码错误");
                //        br.Level = ErrorLevel.Warning;
                //        return Json(br);
                //    }
                //    if (br.Message.Count > 0)
                //    {
                //        br.Success = false;
                //        br.Level = ErrorLevel.Warning;
                //        return Json(br);
                //    }
                //}
            //    model.flag_lx = AccountFlag.standard;
            //    UserLogin model_test = new UserLogin();
            //    br = BusinessFactory.Account.LogOn(model);
            //    if (br.Success)
            //    {
            //        Hashtable loginInfo = (Hashtable)br.Data;
            //        var browserInfo = BrowserHelper.GetBrowserInfo(Request.UserAgent);
            //        loginInfo.Add("flag_from", FromFlag.browser);
            //        loginInfo.Add("client", browserInfo.browser);
            //        loginInfo.Add("version_client", browserInfo.version);
            //        Session["LoginInfo"] = loginInfo;

            //        if (loginInfo["flag_platform"].Equals(YesNoFlag.Yes))
            //        {
            //            br.Data = Url.RouteUrl("Admin", new { controller = "Manager", action = "Index" });
            //        }
            //        if (loginInfo["flag_agent"].Equals(YesNoFlag.Yes))
            //        {
            //            br.Data = Url.RouteUrl("Default", new { controller = "Agent", action = "Index" });
            //        }
            //        if (loginInfo["flag_supplier"].Equals(YesNoFlag.Yes))
            //        {
            //            br.Data = Url.RouteUrl("Default", new { controller = "Supplier", action = "Index" });
            //        }
            //        br.Data = Url.RouteUrl("Default", new { controller = "Buyer", action = "Index" });

            //        WriteDBLog(LogFlag.LogOn, br.Message);
            //        return Json(br);
            //    }
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(br);

        }

        /// <summary>
        /// 推出登录
        /// lxt
        /// 2014-02-12
        /// </summary>
        #region public ActionResult LogOff()
        [ValidateInput(false)]
        public ActionResult LogOff()
        {
            WriteDBLog(LogFlag.LogOff, "退出/注销成功");
            Session.Clear();
            Session.Abandon();
            return RedirectToRoute("Default", new { controller = "Account", action = "Login" });
        }
        #endregion
    }
}

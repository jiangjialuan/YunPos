using CySoft.Controllers.Base;
using CySoft.Controllers.Mobile.Base;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CySoft.Controllers.Mobile
{
    public class MobileAccountController : MobileBaseController
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 用户登录
        /// lxt
        /// 2015-02-05
        /// </summary>
        [HttpPost]
        public ActionResult Login(UserLogin model)
        {
            BaseResult br = new BaseResult();

            //string redirect = GetParameter("redirect");

            try
            {
                if (model.username == null || String.IsNullOrEmpty(model.username = model.username.Trim()))
                {
                    br.Message.Add("请输入用户名");
                }
                if (model.password == null || String.IsNullOrEmpty(model.password = model.password.Trim()))
                {
                    br.Message.Add("请输入密码");
                }

                if (br.Message.Count > 0)
                {
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }

                model.flag_lx = AccountFlag.standard;
                br = BusinessFactory.Account.MobileLogin(model);
                if (br.Success)
                {
                    Tb_User userInfo = (Tb_User)br.Data;
                    var loginInfo = new Hashtable();

                    Session["MobileLoginInfo"] = loginInfo;

                    //if (!String.IsNullOrEmpty(redirect))
                    //{
                    //    br.Data = redirect;
                    //}
                    //else
                    //{
                    //    br.Data = Url.RouteUrl("Default", new { controller = "Main", action = "Index" });
                    //}
                    WriteDBLog(LogFlag.LogOn, br.Message);
                    return Json(br);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(br);

        }

        [HttpPost]
        public ActionResult Logout()
        {
            try
            {
                WriteDBLog(LogFlag.LogOff, "退出/注销成功");
                Session.Clear();
                Session.Abandon();
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
    }
}

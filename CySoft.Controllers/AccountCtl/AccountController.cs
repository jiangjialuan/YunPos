using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Enums;
using CySoft.Model.Flags;
using CySoft.Model.Mapping;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;
using System.Linq;
using System.Reflection;
using CySoft.Model;
using CySoft.Model.Ts;
using System.Web;
using System.Text;

namespace CySoft.Controllers.AccountCtl
{

    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class AccountController : BaseController
    {


        private readonly string flagCyUserReg = System.Configuration.ConfigurationManager.AppSettings["FlagCyUserReg"];


        public ActionResult Browser()
        {
            return View();
        }


        /// <summary>
        /// 登录页
        /// </summary>
        [HttpGet]
        public ActionResult Login()
        {

            //var _browser = string.Format("{0}", Request.Browser.Browser).ToLower();
            //var _platform = Request.Browser.Platform;
            //var _version = Request.Browser.Version;
            //List<string> _browsers = new List<string>()
            //{
            //    "internetexplorer","chrome","firefox","mozilla"
            //};
            //if (_browsers.All(a => _browser != a))
            //{
            //    return RedirectToAction(actionName: "Browser", controllerName: "Account");
            //}
            //if ((_browser == "internetexplorer") && !(_version == "10.0" || _version == "11.0"))
            //{
            //    return RedirectToAction(actionName: "Browser", controllerName: "Account");
            //}

            if (Session["LoginInfo"] == null)
            {
                var loginCount = DataCache.Get<int>(Ip + "_login_count");
                ViewData["loginCount"] = loginCount;
                return View();
            }
            else
            {
                return RedirectToAction(actionName: "home", controllerName: "manager");
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        [HttpPost]
        public ActionResult Login(UserLogin model)
        {
            BaseResult br = new BaseResult();
            bool expires = false;//服务是否将要过期
            int cyDay = 0;//到期剩余天数
            string jumpUrl = "";//购买服务的地址
            try
            {
                if (string.IsNullOrWhiteSpace(model.username))
                {
                    br.Message.Add("请输入用户名");
                }
                if (string.IsNullOrWhiteSpace(model.password))
                {
                    br.Message.Add("请输入密码");
                }
                var loginCount = DataCache.Get<int>(Ip + "_login_count");
                if (loginCount >= 3)
                {
                    string CheckCode = "1";//AppConfig.GetValue("CheckCode") ?? "1";
                    if (CheckCode.Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrWhiteSpace(model.vaildcode))
                        {
                            br.Message.Add("请输入识别码");
                        }
                    }
                    if (br.Message.Count > 0)
                    {
                        br.Success = false;
                        br.Level = ErrorLevel.Warning;
                        return JsonString(br);
                    }
                    if (CheckCode.Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        object validCode = Session["SystemValidCode"];
                        if (validCode == null)
                        {
                            br.Success = false;
                            br.Message.Add("验证码已过期");
                            return JsonString(br);
                        }
                        Session.Remove("SystemValidCode");
                        if (model.vaildcode.ToLower() != validCode.ToString().ToLower())
                        {
                            br.Success = false;
                            br.Message.Add("验证码错误");
                            return JsonString(br);
                        }
                        if (br.Message.Count > 0)
                        {
                            br.Success = false;
                            return JsonString(br);
                        }
                    }
                }

                model.flag_lx = AccountFlag.standard;
                br = BusinessFactory.Account.LogOn(model);


                if (br.Success)
                {
                    Hashtable loginInfo = (Hashtable)br.Data;
                    var browserInfo = BrowserHelper.GetBrowserInfo(Request.UserAgent);
                    loginInfo.Add("flag_from", FromFlag.browser);
                    loginInfo.Add("client", browserInfo.browser);
                    loginInfo.Add("version_client", browserInfo.version);
                    loginInfo.Add("yanshi", model.yanshi.ToString().ToLower());
                    #region 验证购买服务是否符合

                    if (PublicSign.flagCheckService == "1")
                    {
                        var checkBr = BusinessFactory.Account.CheckServiceForLogin(loginInfo);

                        if (!checkBr.Success)
                        {
                            #region 购买服务不符合要求时
                            if (checkBr.Level == ErrorLevel.Error)
                            {
                                //获取服务异常
                                br.Success = false;
                                br.Message.Clear();
                                br.Message = checkBr.Message;
                                br.Level = ErrorLevel.Question;
                                if (checkBr.Data != null)
                                    br.Data = HttpUtility.UrlDecode(checkBr.Data.ToString(), Encoding.GetEncoding("UTF-8"));
                                return JsonString(br);
                            }
                            else if (checkBr.Level == ErrorLevel.Question)
                            {
                                //服务门店超过总门店数 需要管理员角色设置
                                if (loginInfo.ContainsKey("is_sysmanager") && loginInfo["is_sysmanager"].ToString() == "1")
                                {
                                    Session["LoginShopManager"] = loginInfo;
                                    //return RedirectToAction(actionName: "Set", controllerName: "ShopManager");
                                    br.Success = false;
                                    br.Message.Clear();
                                    br.Message = checkBr.Message;
                                    br.Level = ErrorLevel.Error;
                                    if (checkBr.Data != null)
                                        br.Data = HttpUtility.UrlDecode(checkBr.Data.ToString(), Encoding.GetEncoding("UTF-8"));
                                    return JsonString(br);

                                }
                                else
                                {
                                    //获取服务异常
                                    br.Success = false;
                                    br.Message.Clear();
                                    br.Message = checkBr.Message;
                                    br.Level = ErrorLevel.Question;
                                    if (checkBr.Data != null)
                                        br.Data = HttpUtility.UrlDecode(checkBr.Data.ToString(), Encoding.GetEncoding("UTF-8"));
                                    return JsonString(br);
                                }
                            }
                            else
                            {
                                //服务不够 需要跳转购买才可以进行的
                                br.Success = false;
                                br.Message.Clear();
                                br.Message = checkBr.Message;
                                br.Level = checkBr.Level;
                                if (checkBr.Data != null)
                                    br.Data = HttpUtility.UrlDecode(checkBr.Data.ToString(), Encoding.GetEncoding("UTF-8"));
                                return JsonString(br);

                            }
                            #endregion
                        }
                        else
                        {
                            #region 符合条件 检验是否 将要过期
                            //符合条件 检验是否 将要过期
                            Hashtable cyServiceHas = (Hashtable)checkBr.Data;
                            if (cyServiceHas != null && cyServiceHas.ContainsKey("cyServiceList") && cyServiceHas.ContainsKey("isExpire") && cyServiceHas.ContainsKey("isStop") && cyServiceHas.ContainsKey("buyUrl") && cyServiceHas.ContainsKey("endTime") && cyServiceHas.ContainsKey("buyUrl"))
                            {
                                var vEndData = cyServiceHas["endTime"].ToString();
                                DateTime d1 = DateTime.Parse(vEndData);
                                DateTime d2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                                if (d1 > DateTime.Parse("1900-01-01"))
                                {
                                    cyDay = ((int)(d1 - d2).TotalDays);//天数差     
                                    if (cyDay <= PublicSign.showMsgServiceDay)
                                    {
                                        expires = true;
                                    }
                                }
                                jumpUrl = HttpUtility.UrlDecode(cyServiceHas["buyUrl"].ToString(), Encoding.GetEncoding("UTF-8"));
                            }
                            #endregion

                            #region lz 2017-03-07 edit

                            if (cyServiceHas != null && cyServiceHas.ContainsKey("newVersion"))
                            {
                                if (cyServiceHas["newVersion"].ToString().Trim() == PublicSign.bm_yunpos_dd.ToString().Trim())
                                    loginInfo["version"] = "10";
                                else if (cyServiceHas["newVersion"].ToString().Trim() == PublicSign.bm_yunpos_ls.ToString().Trim())
                                    loginInfo["version"] = "20";
                                else if (cyServiceHas["newVersion"].ToString().Trim() == PublicSign.bm_yunpos_jt.ToString().Trim())
                                    loginInfo["version"] = "30";
                            }
                            #endregion

                        }
                    }
                    #endregion

                    Session["LoginInfo"] = loginInfo;

                    #region 验证后台权限等
                    var powerData = BusinessFactory.AccountFunction.GetPurview(loginInfo["id_user"] + "").Data as IList<ControllerModel>;
                    if (powerData == null || !powerData.Any())
                    {
                        br.Success = false;
                        br.Message.Clear();
                        br.Message.Add("没有后台登录权限");
                        return JsonString(br);
                    }
                    var ver = GetLoginInfo<int>("version");
                    if (ver <= 0)
                    {
                        br.Data = Url.RouteUrl("Default", new { controller = "Account", action = "SelectVer" });
                        return JsonString(br);
                    }
                    if (powerData.All(a => a.actions == null || !a.actions.Any()))
                    {
                        br.Success = false;
                        br.Message.Clear();
                        br.Message.Add("没有后台登录权限");
                        return JsonString(br);
                    }
                    br.Data = Url.RouteUrl("Default", new { controller = "Manager", action = "Home" });
                    DataCache.Remove(Ip + "_login_count");
                    WriteDBLog(LogFlag.LogOn, br.Message);
                    #endregion
                }
                else
                {
                    br.Level = ErrorLevel.NotLogin;
                    loginCount += 1;
                    DataCache.Remove(Ip + "_login_count");
                    DataCache.Add(Ip + "_login_count", loginCount);
                    br.Data = loginCount;
                }
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }

            if (br.Success && expires)
            {
                br.Level = ErrorLevel.Question;
                br.Message.Clear();
                br.Message.Add(" 您购买的服务有效期剩余 " + cyDay + " 天 即将到期！");
                if (!string.IsNullOrEmpty(jumpUrl))
                    br.Data = jumpUrl;
            }
            return JsonString(br);
        }

        /// <summary>
        /// 注册
        /// </summary>
        [HttpGet]
        public ActionResult Register()
        {

            var RegisterSource = string.Format("{0}", base.GetParameter("RegisterSource"));
            var type = typeof(Enums.FuncVersion);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            List<Ts_Flag> versionList = new List<Ts_Flag>();
            if (fields.Any())
            {
                foreach (var fieldInfo in fields)
                {
                    var des = fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
                    var name = des.Description;
                    var value = (int)fieldInfo.GetValue(fieldInfo);
                    versionList.Add(new Ts_Flag()
                    {
                        listdisplay = name,
                        listdata = value
                    });
                }
            }
            ViewData["versionList"] = versionList;
            ViewData["industryList"] = GetFlagList("industry").Data;
            ViewData["RegisterSource"] = RegisterSource;
            return View();
        }


        ///// <summary>
        ///// 注册
        ///// </summary>
        //[HttpGet]
        //public ActionResult SingleShopRegister()
        //{
        //    var type = typeof(Enums.FuncVersion);
        //    var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
        //    List<Ts_Flag> versionList = new List<Ts_Flag>();
        //    if (fields.Any())
        //    {
        //        foreach (var fieldInfo in fields)
        //        {
        //            var des = fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
        //            var name = des.Description;
        //            var value = (int)fieldInfo.GetValue(fieldInfo);
        //            versionList.Add(new Ts_Flag()
        //            {
        //                listdisplay = name,
        //                listdata = value
        //            });
        //        }
        //    }
        //    ViewData["versionList"] = versionList;
        //    ViewData["industryList"] = GetFlagList("industry").Data;
        //    return View();
        //}

        /// <summary>
        /// 选择版本
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectVer()
        {
            ViewData["redirect"] = GetParameter("redirect");
            var type = typeof(Enums.FuncVersion);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            List<Ts_Flag> versionList = new List<Ts_Flag>();
            if (fields.Any())
            {
                foreach (var fieldInfo in fields)
                {
                    var des = fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
                    var name = des.Description;
                    var value = (int)fieldInfo.GetValue(fieldInfo);
                    versionList.Add(new Ts_Flag()
                    {
                        listdisplay = name,
                        listdata = value
                    });
                }
            }
            ViewData["versionList"] = versionList;
            return View();
        }
        [HttpPost]
        public ActionResult SelectVer(byte version)
        {
            var res = HandleResult(() =>
            {
                Hashtable param = new Hashtable();
                param.Add("id", id_user_master);
                param.Add("version", version);
                return BusinessFactory.Account.ChangeUserVersion(param);
            });
            if (res.Success)
            {
                SetLoginInfo("version", version);
                res.Data = Url.RouteUrl("Default", new { controller = "Manager", action = "Home" });
            }
            return JsonString(res);
        }
        /// <summary>
        /// 注册
        /// </summary>
        [HttpPost]
        public ActionResult Register(UserRegister model)
        {
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            var RegisterSource = string.Format("{0}", param["RegisterSource"]);
            ParamVessel pv = new ParamVessel();
            pv.Add("phone", string.Empty, HandleType.ReturnMsg);
            pv.Add("password", string.Empty, HandleType.ReturnMsg);
            pv.Add("img_code", string.Empty, HandleType.ReturnMsg);
            pv.Add("phonevaildcode", string.Empty, HandleType.ReturnMsg);
            pv.Add("version", string.Empty, HandleType.ReturnMsg);
            try
            {
                #region 验证参数
                param = param.Trim(pv);
                var oldParam = (Hashtable)param.Clone();
                if (oldParam.ContainsKey("password"))
                    oldParam.Remove("password");
                string CheckCode = AppConfig.GetValue("CheckCode") ?? "1";//AppConfig.GetValue("CheckCode") ?? "1";
                if (CheckCode.Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    #region 验证验证码 手机验证码等

                    object validCode = Session["SystemValidCode"];
                    if (validCode == null)
                    {
                        br.Success = false;
                        br.Message.Add("验证码已过期");
                        return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                    }

                    Session.Remove("SystemValidCode");
                    if (!param["img_code"].ToString().Equals(validCode.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        br.Success = false;
                        br.Message.Add("图片识别码错误");
                        return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                    }
                    var validkey = "phone_register_" + param["phone"];//
                    object phone_register_code = DataCache.Get(validkey);
                    if (phone_register_code == null)
                    {
                        br.Success = false;
                        br.Message.Add("手机验证码已过期");
                        return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                    }
                    DataCache.Remove(validkey);
                    if (!param["phonevaildcode"].ToString().Equals(phone_register_code.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        br.Success = false;
                        br.Message.Add("手机验证码错误");
                        return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                    }
                    #endregion

                }
                if (!model.isAgree)
                {
                    br.Success = false;
                    br.Message.Add("请阅读服务协议!");
                    return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                }
                #endregion
                #region 检查用户号是否已注册到本系统

                var checkRes = BusinessFactory.Account.CheckHadRegister(model);
                if (checkRes.Success == false)
                {
                    return JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = checkRes.Message });
                }
                #endregion
                #region 获取用户系统用户ID
                //"http://192.168.1.140:8012/Auth/Reg";
                var reqUrl = string.Format("{0}", System.Configuration.ConfigurationManager.AppSettings["CyUserRegUrl"]);
                var Md5Key = string.Format("{0}", System.Configuration.ConfigurationManager.AppSettings["Md5Key"]);
                IDictionary<string, string> ps = new Dictionary<string, string>();
                ps.Add("phone", string.Format("{0}", param["phone"]));
                ps.Add("password", string.Format("{0}", param["password"]));
                //ps.Add("smscode", string.Format("{0}", param["phonevaildcode"]));
                //ps.Add("client", "window");
                //ps.Add("client_ver", "7");
                ps.Add("flag_from", "YUNPOS");
                ps.Add("service", "YUNPOS");
                if (!string.IsNullOrEmpty(model.dealer))
                {
                    ps.Add("dealer", model.dealer);
                }
                ps.Add("sign", SignUtils.SignRequestForCyUserSys(ps, Md5Key));


                //var webutils = new CySoft.Utility.WebUtils();
                //var respStr = webutils.DoPost(reqUrl, ps, 30000);
                //var respModel = JSON.Deserialize<ServiceResult>(respStr);

                var respModel = new ServiceResult()
                {
                    State = ServiceState.Done,
                    Data = JSON.Serialize(new CyUserModel { id = Guid.NewGuid().ToString() })
                };

                if (flagCyUserReg == "1")
                {
                    var webutils = new CySoft.Utility.WebUtils();
                    var respStr = webutils.DoPost(reqUrl, ps, 30000);
                    respModel = JSON.Deserialize<ServiceResult>(respStr);
                }

                if (respModel != null)
                {
                    #region MyRegion
                    //if (respModel.State != ServiceState.Done)
                    //{
                    //    br.Success = false;
                    //    br.Message.Add(respModel.Message);
                    //    return JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                    //}
                    //else
                    //{
                    //    if (!string.IsNullOrEmpty(respModel.Data.ToString()))
                    //    {
                    //        var cyUserModel = JSON.Deserialize<CyUserModel>(respModel.Data.ToString());
                    //        if (string.IsNullOrEmpty(cyUserModel.id))
                    //        {
                    //            br.Success = false;
                    //            br.Message.Add("注册失败!");
                    //            return JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                    //        }
                    //        else
                    //        {
                    //            model.id_cyuser = cyUserModel.id;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        br.Success = false;
                    //        br.Message.Add("注册失败!");
                    //        return JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                    //    }
                    //} 
                    #endregion
                    if (!string.IsNullOrEmpty(string.Format("{0}", respModel.Data)))
                    {
                        var cyUserModel = JSON.Deserialize<CyUserModel>(respModel.Data.ToString());
                        if (string.IsNullOrEmpty(cyUserModel.id))
                        {
                            br.Success = false;
                            br.Message.Add("注册失败!");
                            return JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                        }
                        else
                        {
                            model.id_cyuser = cyUserModel.id;
                        }
                    }
                    else
                    {
                        br.Success = false;
                        br.Message.Add("注册失败!");
                        return JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                    }
                }
                #endregion
                model.id_user = Guid.NewGuid().ToString();
                model.flag_from = "browser";
                //model.id_cyuser = Guid.NewGuid().ToString();
                br = BusinessFactory.Account.Register(model);
                if (br.Success)
                {
                    //WriteDBLog("注册", oldParam, br);
                    WriteDBLogForNoLogin("注册", model.id_user, oldParam, br);
                    if (RegisterSource.ToLower() == "singleshopregister")
                    {
                        br.Success = true;
                        //br.Message.Add("注册成功!");
                        return JsonString(new { status = Enums.ServiceStatusCode.Success.GetNum(), message = br.Message });
                    }
                    BaseResult br_login = BusinessFactory.Account.LogOn(new UserLogin() { username = model.phone, password = model.password, flag_lx = AccountFlag.standard });
                    if (br_login.Success)
                    {
                        Hashtable loginInfo = (Hashtable)br_login.Data;
                        var browserInfo = BrowserHelper.GetBrowserInfo(Request.UserAgent);
                        loginInfo.Add("flag_from", FromFlag.browser);
                        loginInfo.Add("client", browserInfo.browser);
                        loginInfo.Add("version_client", browserInfo.version);
                        Session["LoginInfo"] = loginInfo;
                        WriteDBLog("注册-登录", oldParam, br_login);
                    }
                }
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }

            if (br.Success)
            {
                return base.JsonString(new { status = Enums.ServiceStatusCode.Success.GetNum(), redirect_uri = "/manager/home" });
            }
            else
                return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });

        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Reset()
        {
            return View("Reset");
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Reset(UserReset model)
        {
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            ParamVessel pv = new ParamVessel();
            pv.Add("phone", string.Empty, HandleType.ReturnMsg);
            pv.Add("new_password", string.Empty, HandleType.ReturnMsg);
            pv.Add("img_code", string.Empty, HandleType.ReturnMsg);
            pv.Add("phonevaildcode", string.Empty, HandleType.ReturnMsg);

            #region 校验

            try
            {
                param = param.Trim(pv);
                string CheckCode = "1";//AppConfig.GetValue("CheckCode") ?? "1";
                if (CheckCode.Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    object validCode = Session["SystemValidCode"];
                    if (validCode == null)
                    {
                        br.Success = false;
                        br.Message.Add("图片识别码已过期");
                        return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                    }

                    Session.Remove("SystemValidCode");
                    if (!param["img_code"].ToString().Equals(validCode.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        br.Success = false;
                        br.Message.Add("图片识别码错误");
                        return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                    }

                    //PhoneVaild phoneVaild = (PhoneVaild)(Session["PhoneVaild"] ?? new PhoneVaild());
                    var validekey = "phone_reset_" + param["phone"];
                    object phone_reset = DataCache.Get(validekey);
                    if (phone_reset == null)
                    {
                        br.Success = false;
                        br.Message.Add("短信验证码已过期");
                        return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                    }
                    DataCache.Remove(validekey);
                    if (model.phonevaildcode != string.Format("{0}", phone_reset))
                    {
                        br.Message.Add("手机验证码无效！");
                        return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                    }
                }

                Hashtable param_query = new Hashtable()
                {
                    { "username", model.phone },
                    {"id_masteruser", "0"}
                };
                br = BusinessFactory.Account.Get(param_query);

                if (br.Success && br.Data != null)
                {
                    var account = br.Data as Tb_Account;
                    var user = new UserChangePWD()
                    {
                        id_user = account.id_user,
                        newPassword = model.new_password,
                        id_edit = account.id_user,
                        flag_from = "browser"
                    };
                    br = BusinessFactory.Account.ResetPassword(user);
                }
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Message.Add(ex.Message);
            }

            #endregion 校验

            if (br.Success)
            {

                return base.JsonString(new { status = Enums.ServiceStatusCode.Success.GetNum(), redirect_uri = "/account/login" });
            }
            else
            {
                return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
            }

        }


        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CZPwd(string id_user)
        {
            ViewData["id_user"] = id_user;
            return View("CZPwd");
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CZPwd(UserReset model)
        {
            var oldParam = new Hashtable();

            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            ParamVessel pv = new ParamVessel();
            pv.Add("id_user", string.Empty, HandleType.ReturnMsg);
            pv.Add("new_password", string.Empty, HandleType.ReturnMsg);
            pv.Add("img_code", string.Empty, HandleType.ReturnMsg);

            #region 校验
            try
            {
                param = param.Trim(pv);
                oldParam.Add("id_user", param["id_user"].ToString());
                object validCode = Session["SystemValidCode"];
                if (validCode == null)
                {
                    br.Success = false;
                    br.Message.Add("验证码已过期");
                    return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                }

                Session.Remove("SystemValidCode");
                if (!param["img_code"].ToString().Equals(validCode.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    br.Success = false;
                    br.Message.Add("图片识别码错误");
                    return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                }

                var user = new UserChangePWD()
                {
                    id_user = model.id_user,
                    newPassword = model.new_password,
                    id_edit = model.id_user,
                    flag_from = "browser"
                };

                if (user.newPassword.Length < 6)
                {
                    br.Success = false;
                    br.Message.Add("用户密码至少6位");
                    return base.JsonString(new { status = Enums.ServiceStatusCode.Error.GetNum(), message = br.Message });
                }

                br = BusinessFactory.Account.ResetPassword(user);

            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Message.Add(ex.Message);
            }

            #endregion 校验
            WriteDBLog("重置密码", oldParam, br);

            if (br.Success)
            {
                return base.JsonString(new
                {
                    status = "success",
                    message = "操作成功"
                });
            }
            else
            {
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }

        }


        /// <summary>
        /// 账号
        /// </summary>
        [LoginActionFilter]
        [ActionPurview(false)]
        public ActionResult MyInfo()
        {
            BaseResult br = new BaseResult();
            Hashtable result = new Hashtable();
            try
            {
                Hashtable param = new Hashtable();
                param["id"] = id_user;
                param.Add("id_masteruser", id_user_master);
                param.Add("id_platform_role", UserRole);
                br = BusinessFactory.Account.GetInfo(param);
                result = (Hashtable)br.Data;
                ViewData["roles"] = result["roles"];
                ViewData["modules"] = result["modules"];
                ViewData["user"] = result["user"];
            }
            catch (Exception)
            {
                br.Success = false;
            }
            return View();
        }

        /// <summary>
        /// 修改我的账号 
        /// </summary>
        [LoginActionFilter]
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult MyInfo(Tb_User model)
        {
            Hashtable param = base.GetParameters();
            BaseResult br = new BaseResult();
            try
            {
                if (model.name.IsEmpty())
                {
                    br.Success = false;
                    br.Message.Add("姓名不能为空！");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "name";
                    return base.JsonString(br);
                }
                //if (model.phone.IsEmpty())
                //{
                //    br.Success = false;
                //    br.Message.Add("手机号不能为空！");
                //    br.Level = ErrorLevel.Warning;
                //    br.Data = "phone";
                //    return JsonString(br);
                //}
                var phonecode = string.Format("{0}", param["phonecode"]);
                //手机号是否已修改，1：已修改 其它表示未修改
                var isChange = string.Format("{0}", param["isChange"]);
                if (isChange == "1")
                {
                    if (phonecode.IsEmpty())
                    {
                        br.Success = false;
                        br.Message.Add("短信验证码不能为空！");
                        br.Level = ErrorLevel.Warning;
                        br.Data = "phonecode";
                        return JsonString(br);
                    }
                    var key = "phone_changephone_" + model.phone;
                    string servicephonecode = string.Format("{0}", DataCache.Get(key));
                    if (servicephonecode.IsEmpty())
                    {
                        br.Success = false;
                        br.Message.Add("短信验证码已过期,请重新获取！");
                        br.Level = ErrorLevel.Warning;
                        br.Data = "phonecode";
                        return JsonString(br);
                    }
                    if (servicephonecode != phonecode)
                    {
                        br.Success = false;
                        br.Message.Add("短信验证码不正确！");
                        br.Level = ErrorLevel.Warning;
                        br.Data = "phonecode";
                        return JsonString(br);
                    }
                    DataCache.Remove(key);
                }
                else
                {
                    model.phone = null;
                }
                model.id = id_user;
                model.id_masteruser = id_user_master;
                br = BusinessFactory.Account.UpdataPart(model);
                if (br.Success)
                {
                    WriteDBLog("修改我的账号", param, br);
                }
            }
            catch (CySoftException ex)
            {
                br.Success = false;
                br.Message.Add(ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return JsonString(br);
        }















        /// <summary>
        /// 登录页
        /// </summary>
        public ActionResult LogOn()
        {
            ViewData["redirect"] = GetParameter("redirect");
            return View();
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        [HttpPost]
        public ActionResult LogOn(UserLogin model)
        {
            BaseResult br = new BaseResult();
            string redirect = GetParameter("redirect");
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
                string CheckCode = AppConfig.GetValue("CheckCode") ?? "1";
                if (CheckCode == "1")
                {
                    if (model.vaildcode == null || String.IsNullOrEmpty(model.vaildcode = model.vaildcode.Trim()))
                    {
                        br.Message.Add("请输入验证码");
                    }
                }
                if (br.Message.Count > 0)
                {
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    return base.JsonString(br);
                }
                if (CheckCode == "1")
                {
                    object validCode = Session["SystemValidCode"];
                    if (validCode == null)
                    {
                        br.Success = false;
                        br.Message.Add("验证码已过期");
                        br.Level = ErrorLevel.Warning;
                        return base.JsonString(br);
                    }
                    Session.Remove("SystemValidCode");
                    if (model.vaildcode.ToLower() != validCode.ToString().ToLower())
                    {
                        br.Success = false;
                        br.Message.Add("验证码错误");
                        br.Level = ErrorLevel.Warning;
                        return base.JsonString(br);
                    }
                    if (br.Message.Count > 0)
                    {
                        br.Success = false;
                        br.Level = ErrorLevel.Warning;
                        return base.JsonString(br);
                    }
                }
                model.flag_lx = AccountFlag.standard;
                br = BusinessFactory.Account.LogOn(model);
                if (br.Success)
                {
                    Hashtable loginInfo = (Hashtable)br.Data;
                    var browserInfo = BrowserHelper.GetBrowserInfo(Request.UserAgent);
                    loginInfo.Add("flag_from", FromFlag.browser);
                    loginInfo.Add("client", browserInfo.browser);
                    loginInfo.Add("version_client", browserInfo.version);
                    Session["LoginInfo"] = loginInfo;

                    if (!String.IsNullOrEmpty(redirect))
                    {
                        br.Data = redirect;
                    }
                    else
                    {
                        br.Data = Url.RouteUrl("Default", new { controller = "Main", action = "Index" });
                    }
                    WriteDBLog(LogFlag.LogOn, br.Message);
                    return base.JsonString(br);
                }
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return base.JsonString(br);

        }

        /// <summary>
        /// 推出登录
        /// </summary>
        #region public ActionResult LogOff()
        [ValidateInput(false)]
        public ActionResult Logout()
        {
            WriteDBLog(LogFlag.LogOff, "退出/注销成功");
            Session.Clear();
            Session.Abandon();
            //if (isCustomer)
            //{ ViewData["host"] = myHostInfo.Host; return RedirectToRoute("Default", new { controller = "Home" }); }
            return RedirectToRoute("Default", new { controller = "Account", action = "Login" });
        }
        #endregion

        /// <summary>
        /// 推出登录
        /// </summary>
        #region public ActionResult LogOff()
        [ValidateInput(false)]
        public ActionResult LogOff()
        {
            WriteDBLog(LogFlag.LogOff, "退出/注销成功");
            Session.Clear();
            Session.Abandon();
            //if (isCustomer)
            //{ ViewData["host"] = myHostInfo.Host; return RedirectToRoute("Default", new { controller = "Home" }); }
            return RedirectToRoute("Default", new { controller = "Account", action = "Login" });
        }
        #endregion

        /// <summary>
        /// 校验用户名是否已存在
        /// tim
        /// 2015-06-01
        /// </summary>
        //[HttpPost]
        //public ActionResult CheckRegisterAccount(string obj)
        //{
        //    BaseResult br = new BaseResult();
        //    try
        //    {
        //        Hashtable param = GetParameters();
        //        if (!string.IsNullOrWhiteSpace(obj)) param = JSON.Deserialize<Hashtable>(obj);
        //        ParamVessel p = new ParamVessel();
        //        p.Add("account", String.Empty, HandleType.ReturnMsg);
        //        param = param.Trim(p);
        //        br = BusinessFactory.Account.Get(param);
        //        br.Success = false;
        //        if (br.Data == null)
        //        {
        //            return base.JsonString(true);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        br.Success = false;
        //    }
        //    return base.JsonString(false);
        //}


        /// <summary>
        /// 校验用户名是否已存在
        /// </summary>
        [HttpPost]
        public ActionResult CheckRegisterUserName()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("username", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                br = BusinessFactory.Account.Get(param);
                br.Success = false;
                if (br.Data == null)
                {
                    return base.JsonString(true);
                }
            }
            catch (Exception)
            {
                br.Success = false;
            }
            return base.JsonString(false);
        }

        /// <summary>
        /// 校验电子邮件是否已存在
        /// </summary>
        [HttpPost]
        public ActionResult CheckRegisterEmail()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("account", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                br = BusinessFactory.Account.Get(param);
                br.Success = false;
                if (br.Data == null)
                {
                    return base.JsonString(true);
                }
            }
            catch (Exception)
            {
                br.Success = false;
            }
            return base.JsonString(false);
        }




        /// <summary>
        /// 修改我的密码UI 
        /// znt
        /// 2015-02-11
        /// </summary>
        [LoginActionFilter]
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult ChangePasswordView()
        {
            return PartialView("_ChangePasswordControl");
        }

        /// <summary>
        /// 修改密码 
        /// </summary>
        [LoginActionFilter]
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult ChangePassword(UserChangePWD model)
        {
            BaseResult br = new BaseResult();

            if (string.IsNullOrEmpty(model.oldPassword))
            {
                br.Success = false;
                br.Message.Add("旧密码不能为空！");
                br.Level = ErrorLevel.Warning;
                br.Data = null;
                return base.JsonString(br);
            }
            if (string.IsNullOrEmpty(model.newPassword))
            {
                br.Success = false;
                br.Message.Add("新密码不能为空！");
                br.Level = ErrorLevel.Warning;
                br.Data = null;
                return base.JsonString(br);
            }
            try
            {
                model.id_user = id_user;
                model.id_edit = id_user;
                br = BusinessFactory.Account.ChangePassword(model);
                if (br.Success)
                {
                    Hashtable param = new Hashtable();
                    param.Add("id_user", model.id_user);
                    WriteDBLog("修改密码", param, br);
                }
            }
            catch (CySoftException ex)
            {
                br.Success = false;
                br.Message.Add(ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (br.Success)
            {
                return base.JsonString(new
                {
                    status = "success",
                    message = "执行成功,正在载入页面...",
                });
            }
            else
            {
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message),
                });
            }
        }

        /// <summary>
        /// 重置密码UI
        /// </summary>
        [LoginActionFilter]
        [HttpPost]
        public ActionResult ResetPasswordView()
        {
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_user", String.Empty, HandleType.ReturnMsg);
                p.Add("username", String.Empty, HandleType.DefaultValue);
                param = param.Trim(p);
                ViewData["id_user"] = param["id_user"];
                ViewData["username"] = param["username"];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_ResetPasswordControl");
        }

        /// <summary>
        /// 创建用户名
        /// </summary>
        /// <returns></returns>
        [LoginActionFilter]
        [HttpPost]
        public ActionResult CreateAccount()
        {

            var br = new BaseResult();

            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", (long)0, HandleType.ReturnMsg);
                p.Add("account", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                var oldParam = (Hashtable)param.Clone();
                var account = param["account"].ToString();
                br = CyVerify.CheckUserName(account);
                if (!br.Success)
                    return base.JsonString(br);

                param.Add("id_user", GetLoginInfo<string>("id_user"));
                br = BusinessFactory.Account.CreateAccount(param);
                if (br.Success)
                    WriteDBLog("创建用户名", oldParam, br);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return base.JsonString(br);
        }

        /// <summary>
        /// 解除绑定UI
        /// </summary>
        [LoginActionFilter]
        [HttpPost]
        public ActionResult UnbingView()
        {
            var br = new BaseResult();
            var rs = new Hashtable();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", (long)0, HandleType.ReturnMsg);
                p.Add("username", String.Empty, HandleType.DefaultValue);
                p.Add("phone", String.Empty, HandleType.DefaultValue);
                p.Add("email", String.Empty, HandleType.DefaultValue);
                param = param.Trim(p);
                if (string.IsNullOrWhiteSpace(param["phone"].ToString()) && string.IsNullOrWhiteSpace(param["email"].ToString()))
                {
                    br.Success = false;
                    br.Message.Add("取消绑定条件不满足。");
                    br.Level = ErrorLevel.Warning;
                    return base.JsonString(br);
                }
                rs = param;
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_Unbing", rs);
        }
        /// <summary>
        /// 解除绑定
        /// </summary>
        [LoginActionFilter]
        [HttpPost]
        public ActionResult Unbing()
        {

            var br = new BaseResult();

            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", (long)0, HandleType.ReturnMsg);
                p.Add("username", String.Empty, HandleType.Remove);
                p.Add("phone", String.Empty, HandleType.Remove);
                p.Add("email", String.Empty, HandleType.Remove);
                p.Add("password", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);

                if (!(param.ContainsKey("phone") || param.ContainsKey("email")) || ((param.ContainsKey("phone") && param.ContainsKey("email"))))
                {
                    br.Success = false;
                    br.Message.Add("取消绑定的条件不满足。");
                    br.Level = ErrorLevel.Warning;
                    return base.JsonString(br);
                }
                param.Add("id_user", GetLoginInfo<string>("id_user"));
                br = BusinessFactory.Account.Unbing(param);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return base.JsonString(br);
        }


        /// <summary>
        /// 解除绑定
        /// </summary>
        [LoginActionFilter]
        [HttpPost]
        public ActionResult VaildEmail()
        {

            var br = new BaseResult();

            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", (long)0, HandleType.ReturnMsg);
                p.Add("validemail", String.Empty, HandleType.ReturnMsg);
                p.Add("vaildcode", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);

                string vaildcode = (param["vaildcode"] ?? "").ToString();
                object sysValidCode = Session["SystemValidCode"];
                if (sysValidCode == null)
                {
                    br.Success = false;
                    br.Message.Add("验证码错误。");
                    br.Level = ErrorLevel.Warning;
                    return base.JsonString(br);
                }
                if (vaildcode.ToLower() != sysValidCode.ToString().ToLower())
                {
                    br.Success = false;
                    br.Message.Add("验证码错误。");
                    br.Level = ErrorLevel.Warning;
                    return base.JsonString(br);
                }
                if (param["id"].ToString().Equals("0"))
                {
                    br.Success = false;
                    br.Message.Add("用户错误，请重新登录。");
                    br.Level = ErrorLevel.Warning;
                    return base.JsonString(br);
                }
                string email = (param["validemail"] ?? string.Empty).ToString();
                Regex rx = new Regex(@"^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$");

                if (!rx.IsMatch(email))
                {
                    br.Success = false;
                    br.Message.Add("邮箱格式不正确。");
                    br.Level = ErrorLevel.Warning;
                    return base.JsonString(br);
                }
                Session.Remove("SystemValidCode");
                Hashtable ht = new Hashtable();
                ht.Add("id", param["id"]);
                ht.Add("email", email);
                ht.Add("id_user", GetLoginInfo<string>("id_user"));
                br = BusinessFactory.Account.VaildEmail(ht);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return base.JsonString(br);
        }

        /// <summary>
        /// 绑定手机
        /// tim
        /// 2015-04-17
        /// </summary>
        [LoginActionFilter]
        [HttpPost]
        public ActionResult BingPhone()
        {
            var br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", (long)0, HandleType.ReturnMsg);
                p.Add("vaildcode", String.Empty, HandleType.ReturnMsg);
                p.Add("vaildphone", String.Empty, HandleType.ReturnMsg);
                p.Add("phonevaildcode", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);

                string vaildcode = (param["vaildcode"] ?? "").ToString();
                object sysValidCode = Session["SystemValidCode"];
                if (sysValidCode == null)
                {
                    br.Success = false;
                    br.Message.Add("验证码错误。");
                    br.Level = ErrorLevel.Warning;
                    return base.JsonString(br);
                }
                if (vaildcode.ToLower() != sysValidCode.ToString().ToLower())
                {
                    br.Success = false;
                    br.Message.Add("验证码错误。");
                    br.Level = ErrorLevel.Warning;
                    return base.JsonString(br);
                }

                string vaildphone = (param["vaildphone"] ?? "").ToString();
                string phonevaildcode = (param["phonevaildcode"] ?? "").ToString();
                PhoneVaild phoneVaild = (PhoneVaild)(Session["PhoneVaild"] ?? new PhoneVaild());

                if (vaildphone != phoneVaild.phone || phonevaildcode != phoneVaild.vaildcode)
                {
                    br.Success = false;
                    br.Message.Add("手机短信校验码错误或已失效，请重新输入或获取。");
                    br.Level = ErrorLevel.Warning;
                    return base.JsonString(br);
                }
                Session.Remove("SystemValidCode");
                Session.Remove("PhoneVaild");
                Hashtable ht = new Hashtable();
                ht.Add("id", param["id"]);
                ht.Add("phone", param["vaildphone"]);
                ht.Add("id_user", GetLoginInfo<string>("id_user"));
                br = BusinessFactory.Account.BingPhone(ht);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return base.JsonString(br);
        }

        /// <summary>
        /// 绑定邮箱
        /// </summary>
        public ActionResult BingEmail()
        {
            var br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("key", String.Empty, HandleType.ReturnMsg);
                p.Add("email", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                br = BusinessFactory.Account.BingEmail(param);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return base.JsonString(br);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        [LoginActionFilter]
        [HttpPost]
        public ActionResult ResetPassword(UserChangePWD model)
        {
            var oldParam = new Hashtable();
            oldParam.Add("id_user", model.id_user);
            BaseResult br = new BaseResult();
            if (string.IsNullOrEmpty(model.id_user))
            {
                br.Success = false;
                br.Message.Add("数据丢失，请刷新页面后再试！");
                br.Level = ErrorLevel.Warning;
                br.Data = null;
                return base.JsonString(br);
            }
            if (string.IsNullOrEmpty(model.newPassword))
            {
                br.Success = false;
                br.Message.Add("新密码不能为空！");
                br.Level = ErrorLevel.Warning;
                br.Data = null;
                return base.JsonString(br);
            }
            try
            {
                model.id_edit = GetLoginInfo<string>("id_user");
                model.flag_from = "browser";
                br = BusinessFactory.Account.ResetPassword(model);
                if (br.Success)
                {
                    Hashtable param = new Hashtable();
                    param.Add("id_user", model.id_user);
                }
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            WriteDBLog("重置密码", oldParam, br);
            return base.JsonString(br);
        }


        /// <summary>
        /// 员工账户
        /// lz
        /// 2016-08-16
        /// </summary>
        [LoginActionFilter]
        public ActionResult List()
        {
            try
            {
                #region 获取参数
                PageNavigate pn = new PageNavigate();
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("flag_stop", 0, HandleType.Remove);//是否停用
                p.Add("orderby", 1, HandleType.DefaultValue);//排序
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("start_rq_create", String.Empty, HandleType.Remove);
                p.Add("end_rq_create", String.Empty, HandleType.Remove);
                p.Add("_search_", "0", HandleType.DefaultValue);
                p.Add("page", 0, HandleType.DefaultValue);//当前页码
                p.Add("pageSize", base.PageSizeFromCookie, HandleType.DefaultValue);//每页大小
                param = param.Trim(p);
                int page = Convert.ToInt32(param["page"]);
                int pageSize = Convert.ToInt32(param["pageSize"]);

                PageList<Tb_User_Query> list = new PageList<Tb_User_Query>(pageSize);
                string orderby = param["orderby"].ToString();
                if (param["start_rq_create"] != null)
                    ViewData["start_rq_create"] = param["start_rq_create"];
                if (param["end_rq_create"] != null)
                    ViewData["end_rq_create"] = param["end_rq_create"];
                ViewData["orderby"] = param["orderby"];
                ViewData["keyword"] = GetParameter("keyword");
                switch (orderby)
                {
                    case "2":
                        param.Add("sort", "id");
                        param.Add("dir", "desc");
                        break;
                    case "3":
                        param.Add("sort", "name");
                        param.Add("dir", "asc");
                        break;
                    case "4":
                        param.Add("sort", "name");
                        param.Add("dir", "desc");
                        break;
                    default:
                        param.Add("sort", "id");
                        break;
                }
                if (param.ContainsKey("flag_stop"))
                {
                    ViewData["flag_stop"] = param["flag_stop"];
                }

                param.Add("limit", pageSize);
                param.Add("start", page * pageSize);


                param.Add("id_user_master", id_user_master);
                param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);

                #endregion

                #region 获取数据

                //用户管理门店
                if (id_shop != id_shop_master)//user_id_shops
                {
                    //var userShops = GetShop(Enums.ShopDataType.UserShopOnly); //GetUserShop();
                    var userShops = GetCurrentUserMgrShop(id_user, id_shop);
                    if (userShops.Any())
                    {
                        param.Add("user_id_shops", (from s in userShops select s.id_shop).ToArray());
                    }
                }

                //2017-03-10 改查自己下属门店的会员
                //param.Add("query_self_child", id_shop);

                pn = BusinessFactory.Account.GetPage(param);
                list = new PageList<Tb_User_Query>(pn, page, pageSize);
                ViewData["PageList"] = list;

                #endregion

                ViewData["id_user_master"] = id_user_master;
                if (param.ContainsKey("_search_") && param["_search_"].ToString() == "1")
                    return PartialView("_List");
                else
                    return View("List");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改、新增员工账户UI
        /// lz
        /// 2016-08-16
        /// </summary>
        [LoginActionFilter]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            BaseResult br = new BaseResult();
            Hashtable result = new Hashtable();
            try
            {
                Hashtable param = GetParameters();
                string id_shop_edit = id_shop;
                param["id"] = id;
                ParamVessel p = new ParamVessel();
                p.Add("flag_edit", "add", HandleType.DefaultValue);
                p.Add("id", String.Empty, HandleType.Remove);
                param = param.Trim(p);
                ViewData["flag_edit"] = param["flag_edit"];
                if (param["flag_edit"].ToString().ToLower() == "update")
                {
                    param.Remove("flag_edit");
                    param.Add("id_masteruser", id_user_master);
                    br = BusinessFactory.Account.GetInfo(param);
                    result = (Hashtable)br.Data;
                    ViewData["Model"] = result;
                    ViewData["ID"] = id;
                    if (result.ContainsKey("user"))
                    {
                        var userModel = result["user"] as Tb_User_Query;
                        if (userModel != null && !string.IsNullOrEmpty(userModel.id))
                            id_shop_edit = userModel.id_shop;
                    }
                        
                }
                else
                {
                    result["user"] = new Tb_User_Query();
                    param.Clear();
                    param.Add("flag_master", 1);
                    param.Add("_id_masteruser", id_user_master);
                    BaseResult br1 = BusinessFactory.RoleSetting.GetAll(param);
                    var dbRoles = (IList<Tb_Role>)br1.Data;
                    var roles = dbRoles.OrderBy(d => d.flag_type).ToList(m => new Tb_User_Role_Query() { id_role = m.id, isChecked = false, name_role = m.name });
                    result["roles"] = roles;
                    ViewData["Model"] = result;
                    ViewData["ID"] = 0;
                    if (result.ContainsKey("user"))
                    {
                        var userModel = result["user"] as Tb_User_Query;
                        if (userModel != null && !string.IsNullOrEmpty(userModel.id))
                            id_shop_edit = userModel.id_shop;
                    }
                }


                if (id_shop == id_shop_master)
                {
                    ViewData["shopList"] = GetShop(Enums.ShopDataType.All);
                }
                else
                {
                    ViewData["shopList"] = GetShop(Enums.ShopDataType.GetBJXJList, id_shop_edit);
                }

                //用户管理门店
                ViewData["userShopList"] = GetShop(Enums.ShopDataType.UserShopOnlyNone, id); //GetUserShop(id);

                ViewData["id_shop_master"] = id_shop_master;
                ViewData["id_shop"] = id_shop;
                ViewData["id_user_master"] = id_user_master;
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        /// <summary>
        /// 查询员工UI
        /// lz
        /// 2016-08-17
        /// </summary>
        [LoginActionFilter]
        [ActionAlias("account", "list")]
        public ActionResult Detail(string id)
        {
            BaseResult br = new BaseResult();
            Hashtable result = new Hashtable();
            try
            {
                Hashtable param = GetParameters();
                param["id"] = id;
                ParamVessel p = new ParamVessel();
                p.Add("id", String.Empty, HandleType.DefaultValue);
                param = param.Trim(p);
                if (string.IsNullOrEmpty(param["id"].ToString()))
                {
                    br.Success = false;
                    br.Message.Add(string.Format("参数错误!"));
                    return View();
                }
                param.Add("id_masteruser", id_user_master);
                br = BusinessFactory.Account.GetInfo(param);
                result = (Hashtable)br.Data;
                ViewData["Model"] = result;
                ViewData["ID"] = id;

                //用户管理门店
                ViewData["userShopList"] = GetShop(Enums.ShopDataType.UserShopOnlyNone, id);
                ViewData["id_shop_master"] = id_shop_master;

            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }


        /// <summary>
        ///  新增员工账户
        /// </summary>
        [HttpPost]
        public ActionResult Add(Tb_User_Edit model)
        {
            Hashtable param = base.GetParameters();
            var oldParam = (Hashtable)param.Clone();
            BaseResult br = new BaseResult();

            if (model.id_shop.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("门店不能为空!");
                br.Level = ErrorLevel.Warning;
                br.Data = "username";
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }
            if (model.username.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("登陆账号不能为空!");
                br.Level = ErrorLevel.Warning;
                br.Data = "username";
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }
            if (model.password.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("密码不能为空!");
                br.Level = ErrorLevel.Warning;
                br.Data = "password";
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }
            if (model.name.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("姓名不能为空!");
                br.Level = ErrorLevel.Warning;
                br.Data = "name";
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }

            //if (model.phone.IsEmpty())
            //{
            //    br.Success = false;
            //    br.Message.Add("手机号不能为空!");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = "phone";
            //    return base.JsonString(new
            //    {
            //        status = "error",
            //        message = string.Join(";", br.Message)
            //    });
            //}


            if (!model.phone.IsEmpty())
            {
                if (!CyVerify.IsPhone(model.phone))
                {
                    br.Success = false;
                    br.Message.Add("手机号格式不正确!");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "phone";
                    return base.JsonString(new
                    {
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }
            }



            if (!model.tel.IsEmpty() && CyVerify.IsIncludeChinese(model.tel))
            {
                br.Success = false;
                br.Message.Add("电话格式不正确!");
                br.Level = ErrorLevel.Warning;
                br.Data = "phone";
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }


            string confirmpwd = GetParameter("confirmpwd") ?? "";
            if (model.password.Trim() != confirmpwd.Trim())
            {
                br.Success = false;
                br.Message.Add("确认密码错误！");
                br.Level = ErrorLevel.Warning;
                br.Data = "confirmpwd";
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }

            try
            {
                model.id = Guid.NewGuid().ToString();
                model.id_create = id_user;
                model.id_edit = model.id_create;
                model.id_father = id_user_master;
                //model.flag_from = "browser";
                model.id_cyuser = id_cyuser;
                model.id_masteruser = id_user_master;
                model.flag_industry = flag_industry;
                byte result = 0;
                byte.TryParse(version, out result);
                model.version = result;
                if (param != null && param.ContainsKey("city_id") && !string.IsNullOrEmpty(param["city_id"].ToString()) && CyVerify.IsInt32(param["city_id"].ToString()))
                    model.id_city = int.Parse(param["city_id"].ToString());

                br = BusinessFactory.Account.Add(model);

                if (oldParam.ContainsKey("password"))
                    oldParam.Remove("password");
                if (oldParam.ContainsKey("confirmpwd"))
                    oldParam.Remove("confirmpwd");
                WriteDBLog("新增员工账户", oldParam, br);

                if (br.Success)
                {
                    return base.JsonString(new
                    {
                        status = "success",
                        message = "执行成功,正在载入页面..."
                    });
                }
                else
                {
                    return base.JsonString(new
                    {
                        status = "error",
                        message = string.Join(";", br.Message),
                        level = br.Level,
                        data = br.Data
                    });
                }
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // return base.JsonString(br);
        }


        /// <summary>
        ///  修改员工账户
        /// </summary>
        [LoginActionFilter]
        [ActionAlias("account", "edit")]
        public ActionResult Update(Tb_User_Edit model)
        {
            Hashtable param = base.GetParameters();
            var oldParam = (Hashtable)param.Clone();
            BaseResult br = new BaseResult();
            if (string.IsNullOrEmpty(model.id))
            {
                br.Success = false;
                br.Message.Add("账户信息丢失，请刷新后再试!");
                br.Level = ErrorLevel.Warning;
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message),
                });
            }

            if (model.id_shop.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("门店不能为空!");
                br.Level = ErrorLevel.Warning;
                br.Data = "username";
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }

            if (model.name.IsEmpty())
            {
                br.Success = false;
                br.Message.Add("姓名不能为空!");
                br.Level = ErrorLevel.Warning;
                br.Data = "name";
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message),
                });
            }

            if (model.id != id_user_master)
            {
                //if (model.phone.IsEmpty())
                //{
                //    br.Success = false;
                //    br.Message.Add("手机号不能为空!");
                //    br.Level = ErrorLevel.Warning;
                //    br.Data = "phone";
                //    return base.JsonString(new
                //    {
                //        status = "error",
                //        message = string.Join(";", br.Message),
                //    });
                //}

                if (!model.phone.IsEmpty())
                {
                    if (!CyVerify.IsPhone(model.phone))
                    {
                        br.Success = false;
                        br.Message.Add("手机号格式不正确!");
                        br.Level = ErrorLevel.Warning;
                        br.Data = "phone";
                        return base.JsonString(new
                        {
                            status = "error",
                            message = string.Join(";", br.Message)
                        });
                    }
                }
            }
            else
            {
                model.phone = phone_master;
            }

            if (!model.tel.IsEmpty() && CyVerify.IsIncludeChinese(model.tel))
            {
                br.Success = false;
                br.Message.Add("电话格式不正确!");
                br.Level = ErrorLevel.Warning;
                br.Data = "phone";
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }


            try
            {
                model.id_edit = id_user;
                model.id_masteruser = id_user_master;
                if (param != null && param.ContainsKey("city_id") && !string.IsNullOrEmpty(param["city_id"].ToString()) && CyVerify.IsInt32(param["city_id"].ToString()))
                    model.id_city = int.Parse(param["city_id"].ToString());

                br = BusinessFactory.Account.Update(model);

                if (oldParam.ContainsKey("password"))
                    oldParam.Remove("password");
                if (oldParam.ContainsKey("confirmpwd"))
                    oldParam.Remove("confirmpwd");
                WriteDBLog("修改员工账户", oldParam, br);

                if (br.Success)
                {
                    return base.JsonString(new
                    {
                        status = "success",
                        message = "执行成功,正在载入页面..."
                    });
                }
                else
                {
                    return base.JsonString(new
                    {
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }

            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        ///  删除员工账户
        /// </summary>
        [LoginActionFilter]
        public ActionResult Delete()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", string.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                var oldParam = (Hashtable)param.Clone();
                br = BusinessFactory.Account.Delete(param);
                if (br.Success)
                {
                    WriteDBLog("删除员工账户", oldParam, br);

                    return base.JsonString(new
                    {
                        status = "success",
                        message = "执行成功,正在载入页面..."
                    });
                }
                else
                {
                    return base.JsonString(new
                    {
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }

            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return base.JsonString(br);
        }


        /// <summary>
        ///  停用员工账户
        /// </summary>
        [LoginActionFilter]
        public ActionResult Stop()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", string.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                param.Add("id_edit", id_user);
                param.Add("id_masteruser", id_user_master);
                var oldParam = (Hashtable)param.Clone();
                br = BusinessFactory.Account.Stop(param);
                if (br.Success)
                {
                    WriteDBLog("停用员工账户", oldParam, br);

                    return base.JsonString(new
                    {
                        status = "success",
                        message = "执行成功,正在载入页面..."
                    });
                }
                else
                {
                    return base.JsonString(new
                    {
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return base.JsonString(br);
        }

        /// <summary>
        ///  启用员工账户
        /// </summary>
        [LoginActionFilter]
        public ActionResult Active()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", string.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                param.Add("id_edit", id_user);
                param.Add("id_masteruser", id_user_master);
                var oldParam = (Hashtable)param.Clone();
                br = BusinessFactory.Account.Active(param);
                if (br.Success)
                {
                    WriteDBLog("启用员工账户", oldParam, br);

                    return base.JsonString(new
                    {
                        status = "success",
                        message = "执行成功,正在载入页面..."
                    });
                }
                else
                {
                    return base.JsonString(new
                    {
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return base.JsonString(br);
        }

        /// <summary>
        /// 获得角色模块
        /// lxt
        /// 2015-04-17
        /// </summary>
        [LoginActionFilter]
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult GetFunctionsJSON()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_roleList", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                var id_roleList = param["id_roleList"].ToString().Trim().Split(',');
                long[] id_roles = new long[id_roleList.Length];
                for (var i = 0; i < id_roleList.Length; i++)
                {
                    id_roles[i] = Convert.ToInt64(id_roleList[i].Trim());
                }
                br = BusinessFactory.Account.GetRoleFunctions(id_roles);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return base.JsonString(br);
        }

        /// <summary>
        /// 分页查看业务员签到记录
        /// 2015-7-20 wzp
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [LoginActionFilter]
        [ActionPurview(false)]
        public ActionResult GetCheckInfo()
        {
            PageNavigate pn = new PageNavigate();
            int limit = 10;
            PageList<Tb_User_Checkin> list = new PageList<Tb_User_Checkin>(limit);
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_user", String.Empty, HandleType.ReturnMsg);//业务员Id            
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码     
                param = param.Trim(p);
                param.Add("sort", "id");
                param.Add("dir", "desc");
                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                param.Add("start", (pageIndex - 1) * limit);
                param.Add("limit", limit);
                ViewData["id_user"] = param["id_user"];

                pn = BusinessFactory.Account.GetCheckInfo(param);
                list = new PageList<Tb_User_Checkin>(pn, pageIndex, limit);
                return PartialView("_DialogListControl", list);
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// 找回密码
        /// mq 2016-05-19
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult ResetPwd()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                if (param.ContainsKey("vaildphone") && param.ContainsKey("newPwd") && param.ContainsKey("newPwd2"))
                {
                    string vaildphone = param["vaildphone"].ToString();
                    string newPwd = param["newPwd"].ToString();
                    string newPwd2 = param["newPwd2"].ToString();
                    if (!string.IsNullOrEmpty(vaildphone) && !string.IsNullOrEmpty(newPwd) && !string.IsNullOrEmpty(newPwd2))
                    {
                        if (newPwd == newPwd2)
                        {
                            param.Clear();
                            param.Add("username", vaildphone);
                            br = BusinessFactory.Account.Get(param);
                            if (br.Data != null)
                            {
                                UserChangePWD model = new UserChangePWD();
                                Tb_Account user = new Tb_Account();
                                user = (Tb_Account)br.Data;
                                model.id_user = user.id_user;
                                model.newPassword = newPwd2;
                                model.id_edit = user.id_user;
                                model.flag_from = "browser";
                                br = BusinessFactory.Account.ResetPassword(model);
                            }
                            else
                            {
                                br.Success = false;
                                br.Message.Add("该用户不存在或资料已丢失。");
                                br.Level = ErrorLevel.Warning;
                            }
                        }
                        else
                        {
                            br.Success = false;
                            br.Message.Add("两次输入的密码不一致。");
                            br.Level = ErrorLevel.Warning;
                        }
                    }
                    else
                    {
                        br.Success = false;
                        br.Message.Add("获取参数出错，请重试。");
                        br.Level = ErrorLevel.Warning;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return base.JsonString(br);
        }



        /// <summary>
        ///  恢复删除员工帐号
        /// </summary>
        [ActionAlias("account", "edit")]
        public ActionResult Init()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id", string.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                param.Add("id_edit", id_user);
                param.Add("id_masteruser", id_user_master);
                var oldParam = (Hashtable)param.Clone();
                br = BusinessFactory.Account.Init(param);
                if (br.Success)
                {
                    WriteDBLog("恢复删除员工帐号", oldParam, br);

                    return base.JsonString(new
                    {
                        status = "success",
                        message = "执行成功,正在载入页面..."
                    });
                }
                else
                {
                    return base.JsonString(new
                    {
                        status = "error",
                        message = string.Join(";", br.Message)
                    });
                }
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #region GetUserApi
        [ActionPurview(false)]
        public ActionResult GetUserApi()
        {
            //获取经办人api
            var user = GetUser();
            return JsonString(user);
        }
        #endregion



        [HttpGet]
        [ActionPurview(true)]
        [ActionAlias("shop", "shopinfo")]
        public ActionResult ChangeCompanyno()
        {
            ViewData["companyno"] = companyno;
            return View();
        }

        [HttpPost]
        [ActionPurview(true)]
        [ActionAlias("shop", "shopinfo")]
        public ActionResult ChangeCompanyno(string companyno)
        {
            ViewData["companyno"] = companyno;
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            param.Add("id_user", id_user);
            param.Add("companyno", companyno);
            var res = BusinessFactory.Account.ChangeCompanyno(param);
            return JsonString(res, 1);
        }

        #region Set
        [ActionPurview(false)]
        public ActionResult Set()
        {
            if (Session["LoginShopManager"] == null)
            {
                ViewData["is_sysmanager"] = "0";
            }
            else
            {

                var loginInfo = (Hashtable)Session["LoginShopManager"];
                var checkBr = BusinessFactory.Account.CheckServiceForLogin(loginInfo);
                if (!checkBr.Success)
                {
                    if (checkBr.Level == ErrorLevel.Error)
                    {
                        ViewData["is_sysmanager"] = "0";
                    }
                    else if (checkBr.Level == ErrorLevel.Question)
                    {
                        //服务门店超过总门店数 需要管理员角色设置
                        if (loginInfo["is_sysmanager"].ToString() == "1")
                        {
                            if (checkBr.Data != null && !string.IsNullOrEmpty(checkBr.Data.ToString()))
                            {
                                string url = HttpUtility.UrlDecode(checkBr.Data.ToString(), Encoding.GetEncoding("UTF-8"));//url
                                ViewData["url"] = url;
                            }

                            ViewData["is_sysmanager"] = "1";

                            var allStateList = new List<Tb_User_ShopWithShopMc>();
                            Hashtable param = new Hashtable();
                            param.Add("flag_delete", Enums.FlagDelete.NoDelete);
                            param.Add("id_masteruser", loginInfo["id_user_master"].ToString());
                            var vallstate = BusinessFactory.Tb_Shop.GetAll(param).Data as IList<Tb_Shop>;
                            if (vallstate.Any())
                            {
                                vallstate.ToList().ForEach(a =>
                                {
                                    allStateList.Add(new Tb_User_ShopWithShopMc()
                                    {
                                        id_shop = a.id,
                                        id_masteruser = a.id_masteruser,
                                        bm = a.bm,
                                        mc = a.mc
                                    });
                                });
                            }
                            ViewData["shopList"] = allStateList;

                            //用户管理门店
                            var userShopList = this.GetUserShopOnlyNone(loginInfo["id_shop_master"].ToString(), loginInfo["id_user"].ToString());
                            //添加自己门店
                            if (userShopList.Where(d => d.id_masteruser == loginInfo["id_user_master"].ToString() && d.id_user == loginInfo["id_user"].ToString()).Count() <= 0)
                            {
                                var selfModel = this.GetUserSelfShop(loginInfo["id_user_master"].ToString(), loginInfo["id_shop"].ToString(), loginInfo["id_user"].ToString());
                                if (selfModel != null)
                                    userShopList.Add(selfModel);
                            }
                            ViewData["userShopList"] = userShopList;
                            ViewData["id_shop_master"] = loginInfo["id_shop_master"].ToString();// id_shop_master;
                            ViewData["id_shop"] = loginInfo["id_shop"].ToString(); //id_shop;
                            ViewData["id_user_master"] = loginInfo["id_user_master"].ToString(); //id_user_master;//

                            var bm = BusinessFactory.Account.GetServiceBM(loginInfo["version"].ToString());
                            Hashtable ht = new Hashtable();
                            ht.Clear();
                            ht.Add("id_cyuser", loginInfo["id_cyuser"].ToString());
                            ht.Add("bm", bm);
                            ht.Add("service", "GetService");
                            ht.Add("id_masteruser", loginInfo["id_user_master"].ToString());
                            ht.Add("rq_create_master_shop", loginInfo["rq_create_master_shop"].ToString());
                            var cyServiceHas = BusinessFactory.Account.GetCYService(ht);
                            if (cyServiceHas != null && cyServiceHas.ContainsKey("cyServiceList") && cyServiceHas.ContainsKey("endTime"))
                            {
                                var cyServiceList = (List<Schedule_UserService>)cyServiceHas["cyServiceList"];
                                if (cyServiceList.Count() == 0)
                                {
                                    ViewData["buyCount"] = "0";
                                }
                                else
                                {
                                    ViewData["buyCount"] = cyServiceList.Where(d => d.bm == bm).Sum(d => d.sl).ToString();
                                }
                            }

                        }
                    }
                    else
                    {
                        //服务不够 需要跳转购买才可以进行的
                        return RedirectToAction("iframe", "index", new { url = checkBr.Data.ToString() });
                    }
                }
                else
                {
                    return RedirectToAction("Account", "Login");
                }
            }
            return View();
        }
        #endregion

        #region Set
        [HttpPost]
        public ActionResult Set(string ids)
        {
            BaseResult br = new BaseResult();
            if (Session["LoginShopManager"] == null)
            {
                br.Success = false;
                br.Message.Add("请先登录");
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message),
                    level = 3
                });
            }

            try
            {
                var loginInfo = (Hashtable)Session["LoginShopManager"];

                if (string.IsNullOrEmpty(ids))
                {
                    br.Success = false;
                    br.Message.Add("请选择门店先");
                    return base.JsonString(new
                    {
                        status = "error",
                        message = string.Join(";", br.Message),
                        level = 3
                    });
                }
                else
                {
                    #region 数据处理
                    if (ids.EndsWith(","))
                    {
                        ids = ids.Substring(0, ids.Length - 1);
                    }

                    var arr = ids.Split(',');
                    if (arr.Length <= 0)
                    {
                        br.Success = false;
                        br.Message.Add("请选择门店先");
                        return base.JsonString(new
                        {
                            status = "error",
                            message = string.Join(";", br.Message),
                            level = 3
                        });
                    }

                    if (!arr.Contains(loginInfo["id_shop_master"].ToString()))
                    {
                        ids += "," + loginInfo["id_shop_master"].ToString();
                    }
                    #endregion
                    #region 更新开启门店设置

                    var bm = BusinessFactory.Account.GetServiceBM(loginInfo["version"].ToString());
                    if (string.IsNullOrEmpty(bm))
                    {
                        return base.JsonString(new
                        {
                            status = "error",
                            message = "操作失败 获取服务编码异常 请检查版本是否正常！",
                            level = 3
                        });
                    }

                    Hashtable ht = new Hashtable();
                    ht.Add("id_cyuser", loginInfo["id_cyuser"].ToString());
                    ht.Add("bm", bm);
                    ht.Add("service", "GetService");
                    ht.Add("id_masteruser", loginInfo["id_user_master"].ToString());
                    ht.Add("rq_create_master_shop", loginInfo["rq_create_master_shop"].ToString());

                    var cyServiceHas = BusinessFactory.Account.GetCYService(ht);
                    if (cyServiceHas == null || !cyServiceHas.ContainsKey("cyServiceList"))
                    {
                        br.Message.Clear();
                        br.Message.Add(String.Format("获取购买服务信息失败 请重试！"));
                        br.Success = false;
                        br.Level = ErrorLevel.Warning;
                        return base.JsonString(new
                        {
                            status = "error",
                            message = string.Join(";", br.Message),
                            level = 3
                        });
                    }
                    var cyServiceList = (List<Schedule_UserService>)cyServiceHas["cyServiceList"];
                    var allowNum = cyServiceList.FirstOrDefault().sl;
                    ht.Clear();
                    ht.Add("id_masteruser", loginInfo["id_user_master"].ToString());
                    ht.Add("not_id_shop", loginInfo["id_shop_master"].ToString());
                    ht.Add("opened_ids", ids);
                    ht.Add("allow_number", allowNum);
                    br = BusinessFactory.Tb_Shop.ResetOpenShop(ht);
                    if (br.Success)
                    {
                        //修改成功后 
                        return base.JsonString(new
                        {
                            status = "success",
                            message = string.Join(";", br.Message),
                            level = 3
                        });

                    }
                    #endregion
                }

                return base.JsonString(new
                {
                    status = "error",
                    message = "操作失败",
                    level = 3
                });

            }
            catch (CySoftException ex)
            {
                br.Success = false;
                br.Data = "";
                br.Message.Clear();
                br.Message.Add(ex.Message);
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message),
                    level = 3
                });
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Data = "";
                br.Message.Add("系统异常 请重新登录后重试!");
                br.Level = ErrorLevel.Warning;
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }

        }

        #endregion

        #region 获取Tb_User_Shop信息
        /// <summary>
        /// 获取Tb_User_Shop信息
        /// lz 2016-11-07
        /// </summary>
        /// <returns></returns>
        protected List<Tb_User_ShopWithShopMc> GetUserShopOnlyNone(string id_user_master, string select_id_user)
        {
            #region 获取Tb_User_Shop信息
            Hashtable query_user_shop = new Hashtable();
            query_user_shop.Add("id_masteruser", id_user_master);
            query_user_shop.Add("id_user", select_id_user);

            var shopList = BusinessFactory.Tb_User_Shop.GetAll(query_user_shop).Data as List<Tb_User_ShopWithShopMc>;
            #endregion
            return shopList;
        }
        #endregion

        #region GetUserSelfShop
        protected Tb_User_ShopWithShopMc GetUserSelfShop(string id_user_master, string id_shop, string id_user)
        {
            Tb_User_ShopWithShopMc model = null;
            #region 添加登陆者当前门店信息
            Hashtable param = new Hashtable();
            param.Add("id_masteruser", id_user_master);
            param.Add("id", id_shop);
            var brSelfShop = BusinessFactory.Tb_Shop.Get(param);
            if (brSelfShop.Success)
            {
                Tb_Shop selfShop = (Tb_Shop)brSelfShop.Data;
                if (selfShop != null && !string.IsNullOrEmpty(selfShop.id))
                {
                    model = new Tb_User_ShopWithShopMc()
                    {
                        id_masteruser = id_user_master,
                        id_shop = id_shop,
                        id_user = id_user,
                        mc = selfShop.mc,
                        bm = selfShop.bm,
                        rq_create = selfShop.rq_create
                    };
                }
            }
            #endregion
            return model;
        }
        #endregion

    }
}

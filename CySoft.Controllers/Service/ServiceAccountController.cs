using System;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Service.Base;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Utility;
using CySoft.Model.Tb;
using System.Collections.Generic;
using ThoughtWorks.QRCode.Codec;
using System.Drawing;
using System.Drawing.Imaging;

namespace CySoft.Controllers.ServiceCtl
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceAccountController : ServiceBaseController
    {
        /// <summary>
        /// 用户登录
        /// lxt
        /// 2015-03-04
        /// </summary>
        [HttpPost]
        public ActionResult LogOn(string valid)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(valid);
                ParamVessel p = new ParamVessel();
                p.Add("clientvalid", String.Empty, HandleType.ReturnMsg);//客户端验证
                p.Add("username", String.Empty, HandleType.ReturnMsg);//用户名
                p.Add("password", String.Empty, HandleType.ReturnMsg);//密码
                p.Add("flag_from", "mobile", HandleType.DefaultValue);//来源
                p.Add("client", String.Empty, HandleType.DefaultValue);//客户端
                p.Add("version_client", String.Empty, HandleType.DefaultValue);//客户端版本
                param = param.Trim(p);
                string usernameLowerClone = param["username"].ToString().ToLower();
                string clientvalid = AppConfig.GetValue("clientvalid");
                if (clientvalid != null && clientvalid != param["clientvalid"].ToString())
                {
                    br.Success = false;
                    br.Message.Add("身份校验失败");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }

                FromFlag flag_from;
                if (!Enum.TryParse<FromFlag>(param["flag_from"].ToString(), out flag_from))
                {
                    br.Success = false;
                    br.Message.Add("来源类型无效");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }

                UserLogin model = JSON.Deserialize<UserLogin>(valid);
                model.flag_lx = AccountFlag.standard;
                br = BusinessFactory.Account.LogOn(model);
                if (br.Success)
                {
                    loginInfo = (Hashtable)br.Data;
                    loginInfo.Add("sessionid", Guid.NewGuid().ToString("N"));
                    loginInfo.Add("ServicePhone", "020-610-16066");
                    br.Data = loginInfo.Clone();

                    loginInfo.Add("flag_from", flag_from);
                    loginInfo.Add("client", param["client"]);
                    loginInfo.Add("version_client", param["version_client"]);
                    loginInfo.Add("password", param["password"]);
                    DataCache.Add("Service.Account." + usernameLowerClone, loginInfo, new TimeSpan(0, 5, 0));

                    WriteDBLog(LogFlag.LogOn, br.Message);
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
            return Json(br);
        }

        /// <summary>
        /// 获得单个完整对象
        /// lxt
        /// 2015-03-07
        /// </summary>
        [HttpPost]
        public ActionResult GetItem()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = new Hashtable();
                param.Add("id", GetLoginInfo<long>("id_user"));
                param.Add("picuri", String.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority));
                br = BusinessFactory.Account.Get(param);
                var uri = String.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority);
                if (br.Data != null)
                {
                    Tb_User user = (Tb_User)br.Data;
                    var id_master = GetLoginInfo<long>("id_user_master");

                    //if (string.IsNullOrEmpty(user.pic_erwei))
                    //{
                    //    string filename = "erwei_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                    //    string filepath = Server.MapPath("~\\UpLoad\\User\\Master") + "\\" + filename;
                    //    var destImg = string.Empty;
                    //    if (!string.IsNullOrEmpty(user.pic))
                    //        destImg = Server.MapPath(user.pic.Replace(uri, "")); //@"~\Images\\eweilogo.png");
                    //    else
                    //        destImg = Server.MapPath(@"~\Images\\eweilogo.png");

                    //    var id_des = DESEncrypt.EncryptDES(id_master.ToString());
                    //    var id = Base64Encrypt.EncodeBase64(id_des);
                    //    var data = param["picuri"] + "/ServiceCustomer/Scan/" + id;

                    //    QRCode.CreatQRCode(destImg, data, filepath);
                    //    user.pic_erwei = "/UpLoad/User/Master/" + filename;

                    //    param.Clear();
                    //    param.Add("id", GetLoginInfo<long>("id_user"));
                    //    param.Add("id_user", GetLoginInfo<long>("id_user"));
                    //    param.Add("id_user_master", GetLoginInfo<string>("id_user_master"));
                    //    param.Add("flag_from", GetLoginInfo<string>("flag_from"));
                    //    param.Add("pic_erwei", user.pic_erwei);
                    //    br = BusinessFactory.Account.Save(param);
                    //}

                    //user.pic_erwei = uri + user.pic_erwei;

                    //br.Data = new { user.id, user.name, user.job, user.phone, user.email, user.qq, user.pic, user.pic_erwei };

                    br.Data = new { user.id, user.name, user.job, user.phone, user.email, user.qq };
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
            return Json(br);
        }

        /// <summary>
        /// 更新用户信息
        /// lxt
        /// 2015-03-07
        /// </summary>
        [HttpPost]
        public ActionResult Updata(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Tb_User model = JSON.Deserialize<Tb_User>(obj);
                if (model.name.IsEmpty())
                {
                    br.Success = false;
                    br.Message.Add("姓名不能为空！");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                if (model.phone.IsEmpty())
                {
                    br.Success = false;
                    br.Message.Add("手机号不能为空！");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                model.id = GetLoginInfo<string>("id_user");
                //model.flag_from = GetLoginInfo<string>("flag_from");
                br = BusinessFactory.Account.UpdataPart(model);
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Base, br.Message);
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
            return Json(br);
        }

        [HttpPost]
        public ActionResult SaveCompany(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                var param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("id", 0, HandleType.ReturnMsg);//用户id               
                p.Add("companyname", String.Empty, HandleType.Remove);//公司名称               
                p.Add("tel", String.Empty, HandleType.Remove);//
                p.Add("fax", String.Empty, HandleType.Remove);//
                p.Add("zipcode", String.Empty, HandleType.Remove);//
                p.Add("address", String.Empty, HandleType.Remove);//
                p.Add("districts", String.Empty, HandleType.Remove);//所在区域
                p.Add("location", String.Empty, HandleType.Remove);//所在经纬度
                p.Add("location_des", String.Empty, HandleType.Remove);//所在地图描述
                param = param.Trim(p);

                if (!(param.Count > 1) || param["id"] == null || string.IsNullOrWhiteSpace(param["id"].ToString()))
                {
                    br.Success = false;
                    br.Message.Add("提交参数不完整.");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                if (GetLoginInfo<long>("id_user") == null
                    || GetLoginInfo<long>("id_user").Equals(0L)
                    || !GetLoginInfo<long>("id_user").ToString().Equals(param["id"].ToString()))
                {
                    br.Success = false;
                    br.Message.Add("提交参数错误.");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param.Add("id_user_master", GetLoginInfo<string>("id_user_master"));
                param.Add("flag_from", GetLoginInfo<string>("flag_from"));
                br = BusinessFactory.Account.Save(param);
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Base, br.Message);
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
            return Json(br);
        }
        /// <summary>
        /// xdh 
        /// 2015-7-31
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveUser(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                var param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("id", 0, HandleType.ReturnMsg);//用户id
                p.Add("name", String.Empty, HandleType.Remove);//姓名                
                p.Add("flag_sex", String.Empty, HandleType.Remove);//性别
                p.Add("job", String.Empty, HandleType.Remove);//工作
                p.Add("qq", String.Empty, HandleType.Remove);//QQ
                p.Add("email", String.Empty, HandleType.Remove);//
                p.Add("phone", String.Empty, HandleType.Remove);//   
                p.Add("pic", String.Empty, HandleType.Remove);//
                param = param.Trim(p);

                if (!(param.Count > 1) || param["id"] == null || string.IsNullOrWhiteSpace(param["id"].ToString()))
                {
                    br.Success = false;
                    br.Message.Add("提交参数不完整.");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                if (GetLoginInfo<long>("id_user") == null
                    || GetLoginInfo<long>("id_user").Equals(0L)
                    || !GetLoginInfo<long>("id_user").ToString().Equals(param["id"].ToString()))
                {
                    br.Success = false;
                    br.Message.Add("提交参数错误.");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                if (param.ContainsKey("pic") && !string.IsNullOrWhiteSpace(param["pic"].ToString()))
                {
                    var filename = param["pic"].ToString();
                    var sourfilename = Server.MapPath(filename);
                    if (System.IO.File.Exists(sourfilename))
                    {
                        var extname = Path.GetExtension(filename);

                        if (!Directory.Exists(ApplicationInfo.UserMePath))
                        {
                            Directory.CreateDirectory(ApplicationInfo.UserMePath);
                        }

                        var newfilename = String.Format("{0}/{1}_{2}{3}", ApplicationInfo.UserMeUri, GetLoginInfo<long>("id_user"), DateTime.Now.ToString("yyyyMMddHHmmssffff"), extname);
                        System.IO.File.Copy(sourfilename, Server.MapPath(newfilename), true);
                        param["pic"] = newfilename;
                        //清空二维码
                        param.Add("pic_erwei", "");
                    }
                    else
                        param.Remove("pic");
                }
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param.Add("id_user_master", GetLoginInfo<string>("id_user_master"));
                param.Add("flag_from", GetLoginInfo<string>("flag_from"));
                br = BusinessFactory.Account.Save(param);
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Base, br.Message);
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
            return Json(br);
        }

        /// <summary>
        /// 修改密码
        /// cxb
        /// 2015-4-2
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePassword(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                UserChangePWD model = JSON.Deserialize<UserChangePWD>(obj);
                if (string.IsNullOrEmpty(model.oldPassword))
                {
                    br.Success = false;
                    br.Message.Add("原密码不能为空！");
                    br.Data = null;
                    return Json(br);
                }
                if (string.IsNullOrEmpty(model.newPassword))
                {
                    br.Success = false;
                    br.Message.Add("新密码不能为空！");
                    br.Data = null;
                    return Json(br);
                }
                model.id_user = GetLoginInfo<string>("id_user");
                model.id_edit = GetLoginInfo<string>("id_user");
                br = BusinessFactory.Account.ChangePassword(model);
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Base, br.Message);
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
            return Json(br);
        }
        /// <summary>
        /// 创建帐号
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ActionResult CreateAccount(string obj)
        {

            var br = new BaseResult();

            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("id", (long)0, HandleType.ReturnMsg);
                p.Add("account", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);

                var account = param["account"].ToString();

                br = CyVerify.CheckUserName(account);
                if (!br.Success) return Json(br);
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                br = BusinessFactory.Account.CreateAccount(param);
                if (br.Success) WriteDBLog(LogFlag.Base, br.Message);
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
        /// 获取用户的系统参数
        /// </summary>
        /// <returns></returns>
        public ActionResult GetParam()
        {
            var br = new BaseResult();
            try
            {
                Hashtable param = new Hashtable();
                param["id_user_master"] = GetLoginInfo<long>("id_user_master");
                br = BusinessFactory.Setting.GetAll(param);
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
    }
}

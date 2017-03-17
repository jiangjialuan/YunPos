#region Imports
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Filters;
using CySoft.Frame.Common;
using CySoft.Frame.Core;
using CySoft.Frame.Utility;
using CySoft.Utility;
using CySoft.Controllers.Base;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Model;
using CySoft.Model.Other;
#endregion

namespace CySoft.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None)]
    public class UtilityController : BaseController
    {
        /// <summary>
        /// 验证码
        /// </summary>
        #region public ActionResult VaildCode()
        public ActionResult VaildCode()
        {
            //bool success = RedisHelper.Remove("3ahfeugdzm5uli4yrovskxnv_*");
            //bool success1 = SessionHelper.Set("Test1", "ASDFASDFASDF");
            //bool success2 = SessionHelper.Set("Test2", new { id = Guid.NewGuid(), date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss.fff"), code = 123 });
            //string result1 = SessionHelper.Get("Test1");
            //object result11 = SessionHelper.Get<object>("Test1");
            //string result2 = SessionHelper.Get("Test2");
            //object result4 = SessionHelper.Get<object>("Test2");
            var code = Rand.RandNum(4).ToUpper();

            MemoryStream vaildCode = ValidCode.GetValidateCode(code);
            byte[] buffer = vaildCode.GetBuffer();
            vaildCode.Close();
            vaildCode.Dispose();
            return File(buffer, "image/gif");
        }
        #endregion

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        [HttpPost]
        public ActionResult CheckVaildCode()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                string vaildcode = (param["vaildcode"] ?? "").ToString();
                if (String.IsNullOrEmpty(vaildcode = vaildcode.Trim()))
                {
                    br.Message.Add("请输入识别码");
                    br.Level = ErrorLevel.Warning;
                    br.Success = false;
                    br.Data = "101";
                    return JsonString(br.Success);
                }
                object validCode = Session["SystemValidCode"];
                if (validCode == null)
                {
                    br.Success = false;
                    br.Message.Add("识别码已过期");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "102";
                    return JsonString(br.Success);
                }
                if (vaildcode.ToLower() != validCode.ToString().ToLower())
                {
                    br.Success = false;
                    br.Message.Add("识别码错误");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "103";
                    return JsonString(br.Success);
                }
                br.Success = true;
            }
            catch (Exception)
            {
                br.Success = false;
                br.Level = ErrorLevel.Error;
                br.Message.Add("系统错误！");
                br.Data = "104";
            }
            return JsonString(br.Success);
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        #region public ActionResult UpLoadImage()
        [LoginActionFilter]
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult UpLoadImage()
        {
            BaseResult br = new BaseResult();
            try
            {
                HttpPostedFileBase postedFile = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (postedFile == null || postedFile.ContentLength <= 0)
                {
                    br.Success = false;
                    br.Level = ErrorLevel.Error;
                    br.Message.Add("<h5>上传失败</h5>");
                    br.Message.Add("");
                    br.Message.Add("未发现上传的文件！");
                    return JsonString(br);
                }
                if (!CyVerify.IsImage(postedFile.InputStream))
                {
                    br.Success = false;
                    br.Message.Add("<h5>上传失败</h5>");
                    br.Message.Add("");
                    br.Message.Add("上传的文件格式不正确，必须图片文件!");
                    br.Level = ErrorLevel.Warning;
                    return JsonString(br);
                }
                string extension = Path.GetExtension(postedFile.FileName);
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;
                string url = "/UpLoad/Temp/" + fileName;
                string fileFullName = ApplicationInfo.TempPath + "\\" + fileName;
                if (!Directory.Exists(ApplicationInfo.TempPath))
                {
                    Directory.CreateDirectory(ApplicationInfo.TempPath);
                }

                postedFile.SaveAs(fileFullName);
                br.Data = url;
                br.Message.Add("上传成功！");
                br.Success = true;

                //移除过期文件
                FileHelper.FilesClear(ApplicationInfo.TempPath, TimeSpan.FromDays(1));
            }
            catch (CySoftException cex)
            {
                br = cex.Result;
            }
            catch (Exception)
            {
                br.Success = false;
                br.Level = ErrorLevel.Error;
                br.Message.Add("<h5>上传失败</h5>");
                br.Message.Add("");
                br.Message.Add("请重试或与管理员联系！");
            }
            return Content(JSON.Serialize(br));
        }
        #endregion

        /// <summary>
        /// 上传文件
        /// </summary>
        #region public ActionResult UploadFile()
        [LoginActionFilter]
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult UploadFile()
        {
            BaseResult br = new BaseResult();
            try
            {
                HttpPostedFileBase postedFile = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (postedFile == null)
                {
                    br.Success = false;
                    br.Level = ErrorLevel.Error;
                    br.Message.Add("<h5>上传失败</h5>");
                    br.Message.Add("");
                    br.Message.Add("未发现上传的文件！");
                    return Json(br);
                }
                string ranx = "|.html|.htm|.aspx|.php|.asp|.axd|.shtml|.shtm|.exe|.cmd|.asmx|.cs|.cshtml|.vb|.c|.dll|.sh|.config|.js|.css|";
                int pos = postedFile.FileName.LastIndexOf('.') + 1;
                string extension = Path.GetExtension(postedFile.FileName);
                if (ranx.Contains("|" + extension.ToLower() + "|"))
                {
                    br.Success = false;
                    br.Message.Add("<h5>上传失败</h5>");
                    br.Message.Add("");
                    br.Message.Add("该文件属于禁止上传类型！");
                    br.Level = ErrorLevel.Error;
                    return Content(JSON.Serialize(br));
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;
                string url = "/UpLoad/Temp/" + fileName;
                string fileFullName = ApplicationInfo.TempPath + "\\" + fileName;
                if (!Directory.Exists(ApplicationInfo.TempPath))
                {
                    Directory.CreateDirectory(ApplicationInfo.TempPath);
                }
                postedFile.SaveAs(fileFullName);
                br.Data = url;
                br.Message.Add("上传成功！");
                br.Success = true;

                //移除过期文件
                FileHelper.FilesClear(ApplicationInfo.TempPath, TimeSpan.FromDays(1));
            }
            catch (CySoftException cex)
            {
                br = cex.Result;
            }
            catch (Exception)
            {
                br.Success = false;
                br.Level = ErrorLevel.Error;
                br.Message.Add("<h5>上传失败</h5>");
                br.Message.Add("");
                br.Message.Add("请重试或与管理员联系！");
            }
            return Content(JSON.Serialize(br));
        }
        #endregion  

        /// <summary>
        /// 上传文件
        /// </summary>
        #region public ActionResult UploadFileV2()
        [LoginActionFilter]
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult UploadFileV2()
        {
            BaseResult br = new BaseResult();
            try
            {
                HttpPostedFileBase postedFile = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (postedFile == null)
                {
                    br.Success = false;
                    br.Level = ErrorLevel.Error;
                    br.Message.Add("<h5>上传失败</h5>");
                    br.Message.Add("");
                    br.Message.Add("未发现上传的文件！");
                    return Json(br);
                }
                string ranx = "|.html|.htm|.aspx|.php|.asp|.axd|.shtml|.shtm|.exe|.cmd|.asmx|.cs|.cshtml|.vb|.c|.dll|.sh|.config|.js|.css|";
                int pos = postedFile.FileName.LastIndexOf('.') + 1;
                string extension = Path.GetExtension(postedFile.FileName);
                if (ranx.Contains("|" + extension.ToLower() + "|"))
                {
                    br.Success = false;
                    br.Message.Add("<h5>上传失败</h5>");
                    br.Message.Add("");
                    br.Message.Add("该文件属于禁止上传类型！");
                    br.Level = ErrorLevel.Error;
                    return Content(JSON.Serialize(br));
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;
                string url = "/UpLoad/Temp/" + fileName;
                string fileFullName = ApplicationInfo.TempPath + "\\" + fileName;
                if (!Directory.Exists(ApplicationInfo.TempPath))
                {
                    Directory.CreateDirectory(ApplicationInfo.TempPath);
                }
                postedFile.SaveAs(fileFullName);
                br.Data = new { url = url, size = postedFile.ContentLength };
                br.Message.Add("上传成功！");
                br.Success = true;

                //移除过期文件
                FileHelper.FilesClear(ApplicationInfo.TempPath, TimeSpan.FromDays(1));
            }
            catch (CySoftException cex)
            {
                br = cex.Result;
            }
            catch (Exception)
            {
                br.Success = false;
                br.Level = ErrorLevel.Error;
                br.Message.Add("<h5>上传失败</h5>");
                br.Message.Add("");
                br.Message.Add("请重试或与管理员联系！");
            }
            return Content(JSON.Serialize(br));
        }
        #endregion  
        [LoginActionFilter]
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult UploadImages()
        {
            Hashtable hash = new Hashtable();
            try
            {

                HttpPostedFileBase postedFile = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (postedFile == null)
                {
                    hash["error"] = 1;
                    hash["message"] = "未发现上传的文件";
                    return Content(JSON.Serialize(hash));
                }
                Hashtable extTable = new Hashtable();
                extTable.Add("image", "gif,jpg,jpeg,png,bmp");
                String fileName = postedFile.FileName;
                String fileExt = Path.GetExtension(fileName).ToLower();
                if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable["image"]).Split(','), fileExt.Substring(1).ToLower()) == -1)
                {
                    hash["error"] = 1;
                    hash["message"] = "上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable["image"]) + "格式。";
                    return Content(JSON.Serialize(hash));
                }
                fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExt;
                string url = "/UpLoad/MsgImages/" + fileName;
                string fileFullName = ApplicationInfo.ImagesPath + "\\" + fileName;
                if (!Directory.Exists(ApplicationInfo.ImagesPath))
                {
                    Directory.CreateDirectory(ApplicationInfo.ImagesPath);
                }
                postedFile.SaveAs(fileFullName);
                hash["error"] = 0;
                hash["url"] = url;
                return Content(JSON.Serialize(hash));

                //移除过期文件
                //FileHelper.FilesClear(ApplicationInfo.TempPath, TimeSpan.FromDays(1));
            }
            catch (CySoftException cex)
            {
                hash["error"] = 1;
                hash["message"] = cex.Result;
            }
            catch (Exception)
            {
                hash["error"] = 1;
                hash["message"] = "上传失败";
            }
            return Content(JSON.Serialize(hash));
        }
        /// <summary>
        /// 上传分账户资质材料
        /// </summary>
        /// <returns></returns>
        [LoginActionFilter]
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult UploadQualification(string type)
        {
            Hashtable hash = new Hashtable();
            try
            {

                HttpPostedFileBase postedFile = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (postedFile == null)
                {
                    hash["Level"] = ErrorLevel.Error;
                    hash["error"] = 1;
                    hash["message"] = "未发现上传的文件";
                    return Content(JSON.Serialize(hash));
                }
                Hashtable extTable = new Hashtable();
                extTable.Add("image", "jpg,jpeg,png");
                String fileName = postedFile.FileName;
                String fileExt = Path.GetExtension(fileName).ToLower();
                if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable["image"]).Split(','), fileExt.Substring(1).ToLower()) == -1)
                {
                    hash["Level"] = ErrorLevel.Error;
                    hash["error"] = 1;
                    hash["message"] = "上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable["image"]) + "格式。";
                    return Content(JSON.Serialize(hash));
                }
                string user_master = GetLoginInfo<long>("id_user_master").ToString();
                fileName = user_master + "_" + type + fileExt;
                string url = "/UpLoad/Qualifications/" + fileName;
                string fileFullName = ApplicationInfo.QualificationPath + "\\" + fileName;
                if (!Directory.Exists(ApplicationInfo.QualificationPath))
                {
                    Directory.CreateDirectory(ApplicationInfo.QualificationPath);
                }
                FileInfo info = new FileInfo(fileFullName);
                postedFile.SaveAs(fileFullName);
                hash["error"] = 0;
                hash["url"] = url;
                return Content(JSON.Serialize(hash));

            }
            catch (CySoftException cex)
            {
                hash["Level"] = ErrorLevel.Error;
                hash["error"] = 1;
                hash["message"] = cex.Result;
            }
            catch (Exception)
            {
                hash["Level"] = ErrorLevel.Error;
                hash["error"] = 1;
                hash["message"] = "上传失败";
            }
            return Content(JSON.Serialize(hash));
        }

        private string SendSMS(string vaildphone, string content)
        {
            var webutils = new CySoft.Utility.WebUtils();
            var reqUrl = string.Format("{0}", System.Configuration.ConfigurationManager.AppSettings["CyUserMsgUrl"]);
            var Md5Key = string.Format("{0}", System.Configuration.ConfigurationManager.AppSettings["Md5KeySMS"]);
            IDictionary<string, string> ps = new Dictionary<string, string>();
            Random random = new Random();
            var code = random.Next(1000, 9999);
            ps.Add("phone", vaildphone);
            ps.Add("content", content);
            ps.Add("client", "window");
            ps.Add("client_ver", "7");
            ps.Add("flag_from", "YUNPOS");
            ps.Add("sign", SignUtils.SignRequestForCyUserSys(ps, Md5Key));
            return webutils.DoPost(reqUrl, ps, 30000);
        }
        /// <summary>
        /// 发送手机验证码
        /// 增加type参数 用于区别注册和重置
        /// </summary>
        [HttpPost]
        public ActionResult PhoneVaildCode()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                string type = "register";
                if (param.ContainsKey("type"))
                {
                    type = param["type"].ToString();
                }
                #region 图形码验证

                ParamVessel pv = new ParamVessel();
                if (type != "changephone")
                {
                    pv.Add("vaildcode", String.Empty, HandleType.ReturnMsg);
                }
                pv.Add("vaildphone", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(pv);

                string vaildcode = string.Format("{0}", param["vaildcode"]);
                string vaildphone = param["vaildphone"].ToString();
                object sysValidCode = Session["SystemValidCode"];
                if (type != "changephone")
                {
                    if (sysValidCode == null)
                    {
                        br.Success = false;
                        br.Message.Add("识别码已过期");
                        br.Level = ErrorLevel.Warning;
                        br.Data = "vaildcode";
                        return Json(br);
                    }
                    if (vaildcode.ToLower() != sysValidCode.ToString().ToLower())
                    {
                        br.Success = false;
                        br.Message.Add("识别码错误");
                        br.Level = ErrorLevel.Warning;
                        br.Data = "vaildcode";
                        return Json(br);
                    }
                }

                #endregion 图形码验证
                
                

                br = ValidClient(vaildphone);
                if (!br.Success)
                    return JsonString(br);

                param.Clear();
                var code = Rand.RandNum(4);
                if (type == "register")
                {
                    string phonecode = string.Format("{0}", DataCache.Get("phone_register_" + vaildphone));
                    #region register
                    if (!string.IsNullOrEmpty(phonecode))
                    {
                        string passtimeStr = string.Format("{0}", DataCache.Get("phone_register_time_" + vaildphone));
                        DateTime now = DateTime.Now;
                        DateTime passtime = now;
                        DateTime.TryParse(passtimeStr, out passtime);
                        br.Success = true;
                        br.Message.Add("短信验证码已发送，请等待!");
                        br.Data = (passtime - now).Seconds;//"account";
                        br.Level = ErrorLevel.Drump;
                        return JsonString(br);
                    }
                    param.Add("username", vaildphone);
                    br = BusinessFactory.Account.Get(param);
                    if (br.Data != null)
                    {
                        br.Success = false;
                        br.Message.Add("该手机号已注册。");
                        br.Data = "account";
                        br.Level = ErrorLevel.Warning;
                        return JsonString(br);
                    }
                    var content = string.Format("欢迎注册云POS,验证码:{0}", code);
                    var respStr = SendSMS(vaildphone, content);
                    var respModel = JSON.Deserialize<ServiceResult>(respStr);
                    if (respModel != null)
                    {
                        if (respModel.State != ServiceState.Done)
                        {
                            br.Success = false;
                            br.Message.Add(respModel.Message);
                        }
                        else
                        {
                            br.Success = true;
                            var passTime = DateTime.Now.AddSeconds(90);
                            DataCache.Add("phone_register_" + vaildphone, code, passTime);
                            DataCache.Add("phone_register_time_" + vaildphone, passTime.ToString("yyyy-MM-dd HH:mm:ss"), passTime.AddSeconds(10));
                            br.Message.Add("短信已发送!");
                        }
                    }
                    else
                    {
                        br.Success = false;
                        br.Message.Add("获取短信验证码失败!");
                    } 
                    #endregion
                    return JsonString(br);
                }
                if (type == "changephone")
                {
                    string phonecode = string.Format("{0}", DataCache.Get("phone_changephone_" + vaildphone));
                    if (!string.IsNullOrEmpty(phonecode))
                    {
                        string passtimeStr = string.Format("{0}", DataCache.Get("phone_changephone_time_" + vaildphone));
                        DateTime now = DateTime.Now;
                        DateTime passtime = now;
                        DateTime.TryParse(passtimeStr, out passtime);
                        br.Success = true;
                        br.Message.Add("短信验证码已发送，请等待!");
                        br.Data = (passtime - now).Seconds;
                        br.Level = ErrorLevel.Drump;
                        return JsonString(br);
                    }
                    param.Add("username", vaildphone);
                    br = BusinessFactory.Account.Get(param);
                    if (br.Data != null)
                    {
                        br.Success = false;
                        br.Message.Add("该手机号已存在。");
                        br.Data = "account";
                        br.Level = ErrorLevel.Warning;
                        return JsonString(br);
                    }
                    var content = string.Format("云POS验证码：{0}，用于修改手机号。", code);
                    var respStr = SendSMS(vaildphone, content);
                    var respModel = JSON.Deserialize<ServiceResult>(respStr);
                    if (respModel != null)
                    {
                        if (respModel.State != ServiceState.Done)
                        {
                            br.Success = false;
                            br.Message.Add(respModel.Message);
                        }
                        else
                        {
                            br.Success = true;
                            var passTime = DateTime.Now.AddSeconds(90);
                            DataCache.Add("phone_changephone_" + vaildphone, code, passTime);
                            DataCache.Add("phone_changephone_time_" + vaildphone, passTime.ToString("yyyy-MM-dd HH:mm:ss"), passTime.AddSeconds(10));
                            br.Message.Add("短信已发送!");
                        }
                    }
                    else
                    {
                        br.Success = false;
                        br.Message.Add("获取短信验证码失败!");
                    }
                    return JsonString(br);
                }
                if (type == "reset")
                {
                    string phonecode = string.Format("{0}", DataCache.Get("phone_reset_" + vaildphone));
                    #region reset
                    if (!string.IsNullOrEmpty(phonecode))
                    {
                        string passtimeStr = string.Format("{0}", DataCache.Get("phone_reset_time_" + vaildphone));
                        DateTime now = DateTime.Now;
                        DateTime passtime = now;
                        DateTime.TryParse(passtimeStr, out passtime);
                        br.Success = true;
                        br.Message.Add("短信验证码已发送，请等待!");
                        br.Data = (passtime - now).Seconds;//"account";
                        br.Level = ErrorLevel.Drump;
                        return JsonString(br);
                    }
                    param.Add("username", vaildphone);
                    br = BusinessFactory.Account.Get(param);
                    if (br.Data == null)
                    {
                        br.Success = false;
                        br.Message.Add("该手机号未注册。");
                        br.Data = "account";
                        br.Level = ErrorLevel.Warning;
                        return JsonString(br);
                    }
                    var content = string.Format("云POS验证码：{0}，用于重置账号密码。", code);
                    var respStr = SendSMS(vaildphone, content);
                    var respModel = JSON.Deserialize<ServiceResult>(respStr);
                    if (respModel != null)
                    {
                        if (respModel.State != ServiceState.Done)
                        {
                            br.Success = false;
                            br.Message.Add(respModel.Message);
                        }
                        else
                        {
                            br.Success = true;
                            var passTime = DateTime.Now.AddSeconds(90);
                            DataCache.Add("phone_reset_" + vaildphone, code, passTime);
                            DataCache.Add("phone_reset_time_" + vaildphone, passTime.ToString("yyyy-MM-dd HH:mm:ss"), passTime.AddSeconds(10));
                            br.Message.Add("短信已发送!");
                        }
                    }
                    else
                    {
                        br.Success = false;
                        br.Message.Add("获取短信验证码失败!");
                    }
                    #endregion
                } 
                br.Success = true;
            }
            catch (Exception ex)
            {
                br.Success = false;
                br.Message.Add(ex.Message);
                br.Message.Add("验证码发送失败！");
            }
            return JsonString(br);
        }

        /// <summary>
        /// 校验手机验证码
        /// </summary>
        [HttpPost]
        public ActionResult CheckPhoneVaildCode()
        {
            BaseResult br = new BaseResult();
            //try
            //{
            //    Hashtable param = GetParameters();
            //    ParamVessel p = new ParamVessel();
            //    p.Add("vaildcode", String.Empty, HandleType.ReturnMsg);
            //    p.Add("vaildphone", String.Empty, HandleType.ReturnMsg);
            //    p.Add("phonevaildcode", String.Empty, HandleType.ReturnMsg);
            //    param = param.Trim(p);
            //    string vaildcode = (param["vaildcode"] ?? "").ToString();
            //    object sysValidCode = Session["SystemValidCode"];
            //    if (sysValidCode == null)
            //    {
            //        return JsonString(false);
            //    }
            //    if (vaildcode.ToLower() != sysValidCode.ToString().ToLower())
            //    {
            //        return JsonString(false);
            //    }
            //    string vaildphone = (param["vaildphone"] ?? "").ToString();
            //    string phonevaildcode = (param["phonevaildcode"] ?? "").ToString();
            //    PhoneVaild phoneVaild = (PhoneVaild)(Session["PhoneVaild"] ?? new PhoneVaild());
            //    if (vaildphone != phoneVaild.phone || phonevaildcode != phoneVaild.vaildcode)
            //    {
            //        return JsonString(false);
            //    }
            //    //Session.Remove("SystemValidCode");
            //    //Session.Remove("PhoneVaild");
            //    br.Success = true;
            //}
            //catch (Exception)
            //{
            //    br.Success = false;
            //}
            //return JsonString(br.Success);
            return JsonString(true);
        }

        private BaseResult ValidClient(string phonekey)
        {
            lock (PublicSign.lock_msg)
            {
                BaseResult br = new BaseResult();
                var ip = Request.UserHostAddress;
                var key = string.Format("$RegClientAccess_{0}", phonekey);
                var initconfig = new Hashtable();
                DateTime now = DateTime.Now;
                initconfig.Add("date", now);
                initconfig.Add("count", 1);
                try
                {
                    if (DataCache.ContainsKey(key))
                    {
                        var list = DataCache.Get<Hashtable>(key);
                        if (list == null)
                        {
                            list = initconfig;
                        }
                        else
                        {
                            var date = TypeConvert.ToDateTime(list["date"], now);
                            var count = TypeConvert.ToInt(list["count"], 0) + 1;

                            if (date != now && date.AddSeconds(20) >= now)
                            {
                                br.Success = false;
                                br.Data = "count";
                                br.Message.Add("您操作太过频繁，请稍后再试。");
                                br.Level = ErrorLevel.Warning;
                                return br;
                            }

                            var compssdate = date.AddDays(1);
                            if (compssdate >= now && count > 50)//一天内50次尝试注册
                            {
                                br.Success = false;
                                br.Data = "count";
                                br.Message.Add("今天您的注册次数过多过频繁，不能再注册，请明天再试。");
                                br.Level = ErrorLevel.Warning;
                                return br;
                            }
                            else if (compssdate < now)
                            {
                                date = now;
                                count = 1;
                            }
                            list["date"] = now;
                            list["count"] = count;
                        }
                        DataCache.Set(key, list);
                    }
                    else
                    {
                        DataCache.Add(key, initconfig, DateTime.Now.AddHours(12));
                    }
                }
                catch (Exception)
                {
                    br.Success = true;
                }
                br.Success = true;
                return br;
            }
        }

        //public ActionResult AppDownload()
        //{
        //    if (Request.UserAgent.ToLower().Contains("android"))
        //    {

        //    }
        //    else if (Request.UserAgent.ToLower().Contains("ios"))
        //    {

        //    }
        //    else { 
        //    }
        //    TextLogHelper.WriterExceptionLog(Request.UserAgent.ToLower());
        //    return Content(Request.UserAgent.ToLower());
        //}

        public ActionResult UploadFiles()
        {
            Hashtable param = GetParameters();
            if (param.ContainsKey("allowed") && !string.IsNullOrEmpty(param["allowed"].ToString()))
                ViewData["allowed"] = param["allowed"].ToString();
            else
                ViewData["allowed"] = "xls,xlsx";

            if (param.ContainsKey("callback") && !string.IsNullOrEmpty(param["callback"].ToString()))
                ViewData["callback"] = param["callback"].ToString();
            else
                ViewData["callback"] = "";

            return View();
        }

    }
}

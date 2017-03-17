using System;
using System.Collections;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Service.Base;
using CySoft.Frame.Core;
using CySoft.Frame.Common;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Utility;
using CySoft.Model.Tb;
using System.Collections.Generic;
using System.Text;

namespace CySoft.Controllers.Service
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceRegisterController : ServicePublicController
    {
        /// <summary>
        /// 注册
        /// tim
        /// 2015-06-03
        /// </summary>
        [HttpPost]
        public ActionResult Register(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                var model = JSON.Deserialize<UserRegister>(obj);
                if (model.password.IsEmpty())
                {
                    br.Success = false;
                    br.Message.Add("密码不能为空");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "102";
                    return Json(br);
                }
                if (model.phone.IsEmpty())
                {
                    br.Success = false;
                    br.Message.Add("手机号不能为空！");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "103";
                    return Json(br);
                }
                if (model.phonevaildcode.IsEmpty())
                {
                    br.Success = false;
                    br.Message.Add("手机效验码失效！");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "104";
                    return Json(br);
                }
                PhoneVaild phoneVaild = new PhoneVaild() { phone = model.phone, vaildcode = model.phonevaildcode };

                if (!DataCache.ContainsKey(phoneVaild.SeviceKey))
                {
                    br.Success = false;
                    br.Message.Add("手机效验码无效，请重新注册！");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "reg";
                    return Json(br);
                }
                model.flag_supplier = YesNoFlag.Yes;
                model.id_user =Guid.NewGuid().ToString();//BusinessFactory.Utilety.GetNextKey(typeof(Tb_User));
                //model.id_cgs = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Cgs));
                //if (model.flag_supplier == YesNoFlag.Yes)
                //{
                //    model.id_gys = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Gys));
                //    model.id_cgs_level = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Cgs_Level));
                //    model.id_spfl = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Spfl));
                //}
                model.flag_from = this.flag_from;
                br = BusinessFactory.Account.Register(model);               
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
        /// 发送手机验证码
        /// </summary>
       // [HttpPost]
        public ActionResult GetPhoneVaildCode(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                var param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("phone", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                string vaildphone = param["phone"].ToString();

                br = ValidClient();
                if (!br.Success) return Json(br);

                param.Clear();
                param.Add("account", vaildphone);
                br = BusinessFactory.Account.Get(param);
                if (br.Data != null)
                {
                    br.Success = false;
                    br.Message.Add("该手机号已注册，请登录解除手机绑定后，再用该手机注册。");
                    br.Data = "account";
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }

                PhoneVaild vaild = new PhoneVaild() { phone = vaildphone, vaildcode = CySoft.Frame.Common.Rand.RandNum(4) };

                Utility.SMS.ISMSHelper sender = new Utility.SMS.EmayHelper();
                sender.Send(vaild.phone, String.Format("校验码：{0}，用于开通订货易。", vaild.vaildcode));  
    
                DataCache.Add(vaild.SeviceKey, vaild, new TimeSpan(0, 2, 0));
                br.Success = true;
            }
            catch (Exception)
            {
                br.Success = false;
                br.Level = ErrorLevel.Error;
                br.Message.Add("验证码发送失败！");
            }
            return Json(br);
        }


        private BaseResult ValidClient()
        {
            BaseResult br = new BaseResult();
            var ip = Request.UserHostAddress;
            var key = string.Format("$RegClientAccess_{0}", ip);
            var initconfig = new Hashtable();
            initconfig.Add("date", DateTime.Now);
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
                        var date = TypeConvert.ToDateTime(list["date"], DateTime.Now);
                        var count = TypeConvert.ToInt(list["count"], 0) + 1;
                        var compssdate = date.AddDays(1);
                        if (compssdate >= DateTime.Now && count > 50)//一天内50次尝试注册
                        {
                            br.Success = false;
                            br.Data = "count";
                            br.Message.Add("今天您的注册次数过多过频繁，不能再注册，请明天再试。");
                            br.Level = ErrorLevel.Warning;
                            return br;
                        }
                        else if (compssdate < DateTime.Now)
                        {
                            date = DateTime.Now;
                            count = 1;
                        }
                        list["date"] = date;
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
}

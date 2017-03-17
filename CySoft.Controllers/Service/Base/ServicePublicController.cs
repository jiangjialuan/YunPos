using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Text;
using CySoft.Controllers.Base;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Utility;
using CySoft.Frame.Common;
using System.Security.Cryptography;


namespace CySoft.Controllers.Service.Base
{
    public abstract class ServicePublicController : Controller
    {
        public string flag_from =  FromFlag.mobile.ToString();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {           
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
            string actionName = filterContext.ActionDescriptor.ActionName.ToLower();

            HttpContextBase context = filterContext.HttpContext;
            BaseResult br = new BaseResult();
            string valid = context.Request["valid"];
            Hashtable param = JSON.Deserialize<Hashtable>(valid);
            ParamVessel p = new ParamVessel();
            p.Add("clientcode", String.Empty, HandleType.ReturnMsg);//客户端标识码        
            p.Add("clientvalid", String.Empty, HandleType.ReturnMsg);//客户端验证=  md5(客户端标识码+'-'+'cyorder'+'.'+yyyy-MM-dd+'_'+通讯码)
            p.Add("flag_from", "mobile", HandleType.DefaultValue);//来源
            p.Add("client", String.Empty, HandleType.DefaultValue);//客户端
            p.Add("version_client", String.Empty, HandleType.DefaultValue);//客户端版本
            param = param.Trim(p);
      
                FromFlag flag_from;
                if (!Enum.TryParse<FromFlag>(param["flag_from"].ToString(), out flag_from))
                {
                    br.Success = false;
                    br.Message.Add("来源类型无效");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "101";
                    throw new CySoftException(br);
                }
                string clientvalid = AppConfig.GetValue("clientvalid");
                string v = MD5Encrypt.Md5(string.Format("{0}-{1}.{2}_{3}", param["clientcode"], "cyorder", DateTime.Now.ToString("yyyy-MM-dd"), clientvalid));
                if (!v.ToLower().Equals(param["clientvalid"].ToString().ToLower()))
                {
                    br.Success = false;
                    br.Message.Add("身份校验失败");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "102";
                    throw new CySoftException(br);
                }
                this.flag_from = param["flag_from"].ToString();
            base.OnActionExecuting(filterContext);
        }
        
    }
}

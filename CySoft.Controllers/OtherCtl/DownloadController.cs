using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.Mvc;
using CySoft.Frame.Common;

namespace CySoft.Controllers.OtherCtl
{
    [OutputCache(Location = OutputCacheLocation.None)]
    public class DownloadController : Controller
    {
        public ActionResult Index()
        {
            var head = HttpContext.Request;
            //TextLogHelper.WriterExceptionLog("浏览器类型:"+head.Browser.Id);
            //TextLogHelper.WriterExceptionLog("浏览器名称:" + head.UserAgent);
            if (head.UserAgent.ToLower().Contains("micromessenger"))
            {
                return PartialView("_WeiXin");                              
            }
            if (head.UserAgent.ToLower().Contains("iphone"))
            {
                Response.Redirect("https://itunes.apple.com/us/app/ding-huo-yi/id1106219703?l=zh&ls=1&mt=8");
            }
            else
            {
                Response.Redirect("~/app/easydh.apk");
            }
            return View();
        }
    }
}

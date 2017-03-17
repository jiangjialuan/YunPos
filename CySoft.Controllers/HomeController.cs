using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CySoft.Controllers.Base;
using System.Web.UI;
using System.IO;

namespace CySoft.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None)]
    public class HomeController : BaseController
    {
        /// <summary>
        /// 首页面 cxb 2015-7-21
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() 
        {
            //if (isCustomer) { ViewData["host"] = myHostInfo.Host; return View(string.Format("~/Views/Home/Themes/Login.cshtml", myHostInfo.Host)); }
            return View(); //return RedirectToRoute("Default", new { controller = "Account", action = "Login" });
        }

        public ActionResult Register()
        {
            //if (isCustomer) { ViewData["host"] = myHostInfo.Host; return View(string.Format("~/Views/Home/Themes/Register.cshtml", myHostInfo.Host)); }
            return RedirectToRoute("Default", new { controller = "Account", action = "Register" });
        }

        /// <summary>
        /// 产品展示页 cxb 2015-7-21
        /// </summary>
        /// <returns></returns>
        public ActionResult dhy_Product() {
            return View();
        }

        /// <summary>
        /// 购买页 cxb 2015-7-21
        /// </summary>
        /// <returns></returns>
        public ActionResult Purchase() {
            return View();
        }

        /// <summary>
        /// 商家加盟 cxb 2015-7-21 
        /// </summary>
        /// <returns></returns>
        public ActionResult Affiliate() {
            return View();
        }

        /// <summary>
        /// 成功案例 cxb 2015-7-22
        /// </summary>
        /// <returns></returns>
        public ActionResult Case() {
            return View();
        }

        /// <summary>
        /// 关于我们 cxb 2015-7-22
        /// </summary>
        /// <returns></returns>
        public ActionResult AboutUs() {
            return View();
        }

        /// <summary>
        /// 联系我们 cxb 2015-7-22
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact() {
            return View();
        }

        /// <summary>
        /// 服务条款
        /// </summary>
        /// <returns></returns>
        public ActionResult RegAgreement()
        {
            return View();
        }

        public ActionResult Desktop(string t)
        {
            var str = new StringBuilder();
            str.AppendLine("[{000214A0-0000-0000-C000-000000000046}]");
            str.AppendLine("Prop3=19,2");
            str.AppendLine("[InternetShortcut]");
            str.AppendLine(string.Format("URL={0}", myHostInfo.GetUrlBase()));
            //str.AppendLine("IDList=");
            //str.AppendLine("IconFile=http://www.dhy.hk/Images/favicon.ico");
            str.AppendLine("IconIndex=1");

            byte[] array = Encoding.UTF8.GetBytes(str.ToString());

            MemoryStream stream = new MemoryStream(array);

            return File(stream, "application/octet-stream", string.Format("{0}.url", t));
        }
    }
}

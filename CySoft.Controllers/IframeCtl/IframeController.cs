using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model;
using CySoft.Model.Enums;
using CySoft.Model.Tb;
using CySoft.Utility.Mvc.Html;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using CySoft.Utility;
using System.IO;
using System.Data;
using System.Web;

namespace CySoft.Controllers.IframeCtl
{
    public class IframeController : BaseController
    {
        /// <summary>
        /// Iframe 页面
        /// lz
        /// 2017-01-04
        /// </summary>
        [ActionPurview(true)]
        public ActionResult Index()
        {
            string url = Request["url"] == null ? "" : HttpUtility.UrlDecode(Request["url"], Encoding.GetEncoding("UTF-8"));//url
            ViewData["url"] = url;
            return View();
        }

        /// <summary>
        /// ShowA 页面
        /// lz
        /// 2017-01-04
        /// </summary>
        [ActionPurview(true)]
        public ActionResult ShowA()
        {
            string msg = Request["msg"] == null ? "" : HttpUtility.UrlDecode(Request["msg"], Encoding.GetEncoding("UTF-8"));//msg
            string url = Request["url"] == null ? "" : HttpUtility.UrlDecode(Request["url"], Encoding.GetEncoding("UTF-8"));//url
            ViewData["msg"] = msg;
            ViewData["url"] = url;
            return View();
        }


        /// <summary>
        /// Iframe 设置门店
        /// lz
        /// 2017-01-04
        /// </summary>
        [ActionPurview(true)]
        public ActionResult SetShop()
        {
            string url = Request["url"] == null ? "" : HttpUtility.UrlDecode(Request["url"], Encoding.GetEncoding("UTF-8"));//url
            ViewData["url"] = url;
            return View();
        }



    }
}

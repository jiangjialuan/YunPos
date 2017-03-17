using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Frame.Core;
using CySoft.Utility.Mvc.Html;
using CySoft.Model.Tb;
using System.Collections;

namespace CySoft.Controllers.BulletinCtl
{
    /// <summary>
    /// 消息控制器
    /// </summary>
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]

    public class BulletinController : BaseController
    {
        /// <summary>
        /// 消息列表
        /// mq 2016-05-27 新增业务消息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult List(string type="")
        {
            Hashtable param = base.GetParameters();
            int limit = 10;
            PageList<Info_Query> lst = new PageList<Info_Query>(limit);
            //页码
            int pageIndex = param.ContainsKey("pageIndex") ? Convert.ToInt32(param["pageIndex"]) : 1;
            ViewData["pageIndex"] = pageIndex;
            param.Add("limit", limit);
            param.Add("start", (pageIndex - 1) * limit);
            string title = "";
            if (type == "yw")
            {
                title = "业务消息";
                param.Add("bm", "business");
            }
            if (type == "xt")
            {
                title = "系统公告";
                param.Add("bm", "system");
            }
            if (type == "sj")
            {
                title = "升级公告";
                param.Add("bm", "update");
            }


            param.Add("sort", "id");
            param.Add("dir", "desc");
            param.Add("id_user", GetLoginInfo<long>("id_user"));
            PageNavigate pn = BusinessFactory.Info.GetPage(param);
            lst = new PageList<Info_Query>(pn, pageIndex, limit);
            ViewBag.title = title;
            ViewBag.type = type;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListControl", lst);
            }
            return View(lst);
        }
        [ActionPurview(false)]
        public ActionResult Item(string type = "", string id = "")
        {
            string ParentTitle = "";
            Hashtable param = new Hashtable();
            BaseResult br = new BaseResult();
            long id_user = GetLoginInfo<long>("id_user");
            param.Add("id_user", id_user);
            param.Add("id_info", id);
            param.Add("flag_reade", 1);
            br = BusinessFactory.InfoUser.GetCount(param);
            var count = (int)br.Data;
            if (count <= 0)
            {
                Info_User info = new Info_User();
                info.id_user = id_user;
                info.id_info =Convert.ToInt64(id);
                info.flag_reade = 1;
                info.flag_from = "pc";
                info.rq = DateTime.Now;
                BusinessFactory.InfoUser.Add(info);
            }
            param.Clear();
            param.Add("id", id);
            
            if (type == "yw")
            {
                ParentTitle = "业务消息";
                param.Add("bm", "business");
            }
            if (type == "xt")
            {
                ParentTitle = "系统公告";
                param.Add("bm", "system");
            }
            if (type == "sj")
            {
                ParentTitle = "升级公告";
                param.Add("bm", "update");
            }
            br = BusinessFactory.Info.Get(param);
            ViewBag.ParentTitle = ParentTitle;
            ViewBag.type = type;
            return View(br.Data);
        }
    }
}

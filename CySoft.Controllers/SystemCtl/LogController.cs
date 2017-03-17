using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Controllers.Filters;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Frame.Core;
using CySoft.Model.Ts;
using CySoft.Utility.Mvc.Html;
using System.Collections;

#region 操作日志
#endregion

namespace CySoft.Controllers.SystemCtl
{
    [LoginActionFilter]
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class LogController : BaseController
    {
        /// <summary>
        /// 日志查看
        /// lxt
        /// 2015-03-24
        /// </summary>
        public ActionResult List()
        {
            PageNavigate pn = new PageNavigate();
            int limit = 10;
            PageList<Ts_Log_Query> list = new PageList<Ts_Log_Query>(limit);
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("flag_time", 0, HandleType.DefaultValue);//0全部
                p.Add("flag", 0, HandleType.DefaultValue);//0全部  1本人
                p.Add("orderby", 1, HandleType.DefaultValue);//排序
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("limit", 10, HandleType.DefaultValue);//每页大小
                param = param.Trim(p);
                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                limit = Convert.ToInt32(param["limit"]);
                string flag = param["flag"].ToString();
                string orderby = param["orderby"].ToString();

                ViewData["pageindex"] = pageIndex;
                ViewData["flag_time"] = param["flag_time"];
                ViewData["flag"] = param["flag"];
                ViewData["orderby"] = param["orderby"];
                ViewData["keyword"] = GetParameter("keyword");
                ViewData["limit"] = GetParameter("limit");

                param.Remove("flag");
                switch (orderby)
                {
                    case "2":
                        param.Add("sort", "id");
                        break;
                    default:
                        param.Add("sort", "id");
                        param.Add("dir", "desc");
                        break;
                }
                switch (flag)
                {
                    case "1":
                        param.Add("id_user", GetLoginInfo<long>("id_user"));
                        break;
                    default:
                        param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
                        break;
                }
                if (pageIndex < 1)
                {
                    pageIndex = 1;
                }
                if (limit < 1)
                {
                    param["limit"] = limit = 1;
                }
                else if (limit > 50)
                {
                    param["limit"] = limit = 50;
                }
                param.Add("start", (pageIndex - 1) * limit);
                pn = BusinessFactory.Log.GetPage(param);
                list = new PageList<Ts_Log_Query>(pn, pageIndex, limit);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListControl", list);
            }
            return View(list);
        }
    }
}

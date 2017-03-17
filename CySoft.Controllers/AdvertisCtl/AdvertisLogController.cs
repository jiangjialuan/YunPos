using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Tb;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;

namespace CySoft.Controllers.AdvertisCtl
{
    public class AdvertisLogController : BaseController
    {
        /// <summary>
        /// 保存点击广告信息用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(false)]
        [ValidateInput(false)]
        public ActionResult Save()
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();
            try
            {
                ParamVessel p = new ParamVessel();
                param.Add("id_adv", param["id"].ToString());
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param.Remove("id");
                br = BusinessFactory.Advertis_Log.Add(param);
                param.Clear();
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

        public ActionResult List()
        {
            ViewData["id_user_master"] = GetLoginInfo<long>("id_user_master");
            Hashtable param = GetParameters();
            ViewData["id_adv"] = param["id_adv"];
            ParamVessel p = new ParamVessel();
            p.Add("pageIndex", (long)1, HandleType.DefaultValue);
            p.Add("id_adv", param["id_adv"], HandleType.DefaultValue);
            if (param.ContainsKey("id_user"))
            {
                p.Add("id_user", param["id_user"].ToString(), HandleType.Remove);
            }
            if (param.ContainsKey("start_rq_create"))
            {
                p.Add("start_rq_create", param["start_rq_create"], HandleType.Remove);
            }
            if (param.ContainsKey("end_rq_create"))
            {
                p.Add("end_rq_create", param["end_rq_create"], HandleType.Remove);
            }
            param = param.Trim(p);
            PageList<Tb_Advertis_Log_Query> lst = GetPageData(param);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListControl", lst);
            }
            return View(lst);
        }

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private PageList<Tb_Advertis_Log_Query> GetPageData(Hashtable param)
        {
            int limit = 10;
            PageList<Tb_Advertis_Log_Query> lst = new PageList<Tb_Advertis_Log_Query>(limit);
            //页码
            int pageIndex = param.ContainsKey("pageIndex") ? Convert.ToInt32(param["pageIndex"]) : 1;
            ViewData["pageIndex"] = pageIndex;
            param.Add("limit", limit);
            param.Add("start", (pageIndex - 1) * limit);
            try
            {
                PageNavigate pn = BusinessFactory.Advertis_Log.GetPage(param);
                lst = new PageList<Tb_Advertis_Log_Query>(pn, pageIndex, limit);

            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lst;
        }
    }
}

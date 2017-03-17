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
    [LoginActionFilter]
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class AdvertisController : BaseController
    {
        public ActionResult Add()
        {
            ViewData["id_user_master"] = GetLoginInfo<long>("id_user_master");
            ViewData["id_edit"] = GetLoginInfo<long>("id_supplier");
            ViewData["id_cgs"] = GetLoginInfo<long>("id_buyer");
            ViewData["id_gys"] = GetLoginInfo<long>("id_supplier");
            return View();
        }

        /// <summary>
        /// 广告
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["id_user_master"] = GetLoginInfo<long>("id_user_master");
            Hashtable param = GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("pageIndex", (long)1, HandleType.DefaultValue);
            p.Add("flag_type", string.Empty, HandleType.Remove);
            param = param.Trim(p);
            param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
            PageList<Tb_Advertis> lst = GetPageData(param);
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
        private PageList<Tb_Advertis> GetPageData(Hashtable param)
        {
            int limit = 10;
            PageList<Tb_Advertis> lst = new PageList<Tb_Advertis>(limit);
            //页码
            int pageIndex = param.ContainsKey("pageIndex") ? Convert.ToInt32(param["pageIndex"]) : 1;
            ViewData["pageIndex"] = pageIndex;
            param.Add("limit", limit);
            param.Add("start", (pageIndex - 1) * limit);
            try
            {
                param.Add("sort", "sort");
                PageNavigate pn = BusinessFactory.Advertis.GetPage(param);
                lst = new PageList<Tb_Advertis>(pn, pageIndex, limit);

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
        /// <summary>
        /// 查询详情
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult AdvertisItem()
        {
            Hashtable param = GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("id", 0, HandleType.ReturnMsg);
            param = param.Trim(p);
            BaseResult br = new BaseResult();

            br = BusinessFactory.Advertis.Get(param);
            return View(br.Data);
        }

        /// <summary>
        /// 保存广告信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(false)]
        [ValidateInput(false)]
        public ActionResult Save()
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("Title", string.Empty, HandleType.ReturnMsg);
            p.Add("flag_type", (long)0, HandleType.DefaultValue);
            p.Add("info", string.Empty, HandleType.DefaultValue);
            p.Add("url", string.Empty, HandleType.DefaultValue);
            p.Add("filename", string.Empty, HandleType.DefaultValue);
            param = param.Trim(p);

            try
            {
                param.Add("id_edit", GetLoginInfo<long>("id_user"));
                param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
                long id_adv = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Advertis));//获取下一个Id自增值
                param.Add("id", id_adv);
                Hashtable paramcount = new Hashtable();
                paramcount.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
                paramcount.Add("isuse", 1);
                PageList<Tb_Advertis> lst = GetPageData(paramcount);
                if (lst.Count >= 5)
                {
                    param.Add("isuse", 0);
                }
                else
                {
                    param.Add("isuse", 1);
                }
                br = BusinessFactory.Advertis.Add(param);
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

        /// <summary>
        /// 删除广告
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult Delete()
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("id", 0, HandleType.ReturnMsg);
            param = param.Trim(p);
            br = BusinessFactory.Advertis.Delete(param);
            param.Clear();
            param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
            return PartialView("_ListControl", GetPageData(param));
        }

        [ActionPurview(false)]
        [ValidateInput(false)]
        public ActionResult Edit(long id)
        {
            BaseResult br = new BaseResult();
            try
            {

                Hashtable param = base.GetParameters();
                ViewData["id_cgs"] = GetLoginInfo<long>("id_buyer");
                ViewData["id_gys"] = GetLoginInfo<long>("id_supplier");
                long id_user_master = GetLoginInfo<long>("id_user_master");
                ViewData["id_user_master"] = id_user_master;
                param.Add("id_user_master", id_user_master);
                br = BusinessFactory.Advertis.Get(param);

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
            return View(br.Data);
        }

        /// <summary>
        /// 编辑广告信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(false)]
        [ValidateInput(false)]
        public ActionResult EditAdvertis()
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();
            try
            {
                param.Add("new_id_edit", GetLoginInfo<long>("id_user"));
                br = BusinessFactory.Advertis.Update(param);
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
        /// <summary>
        /// 广告位显示
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult MainDisplay()
        {
            BaseResult br = new BaseResult();

            try
            {
                Hashtable param = GetParameters();
                //param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                param.Add("sort", "sort");
                br = BusinessFactory.Advertis.GetAll(param);
                if (!br.Success)
                {
                    throw new CySoftException(br);
                }
            }
            catch (CySoftException ex)
            {

                throw ex;
            }
            return Json(br);
        }

        /// <summary>
        /// 广告位预览
        /// </summary>
        /// <returns></returns>
        public ActionResult Preview()
        {
            ViewData["id_user_master"] = GetLoginInfo<long>("id_user_master");
            Hashtable param = GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("pageIndex", (long)1, HandleType.DefaultValue);
            param = param.Trim(p);
            param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
            PageList<Tb_Advertis> lst = GetPageData(param);
            if (Request.IsAjaxRequest())
            {
                return PartialView("preview", lst);
            }
            return View(lst);
        }
        /// <summary>
        /// 点击广告位
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult UpdateClick()
        {
            BaseResult br = new BaseResult();

            try
            {
                Hashtable param = GetParameters();
                br = BusinessFactory.Advertis.Get(param);
                if (!br.Success)
                {
                    throw new CySoftException(br);
                }
                Tb_Advertis adv = (Tb_Advertis)br.Data;
                param.Add("new_click", adv.click + 1);
                br = new BaseResult();
                br = BusinessFactory.Advertis.Update(param);
                param.Remove("new_click");
                param.Add("id_adv", param["id"].ToString());
                //param.Add("id_user", GetLoginInfo<long>("id_user"));
                param.Add("id_user", GetLoginInfo<long>("id_user_master"));
                param.Remove("id");
                br = new BaseResult();
                br = BusinessFactory.Advertis_Log.Add(param);
            }
            catch (CySoftException ex)
            {

                throw ex;
            }
            return Json(br);
        }

        /// <summary>
        /// 广告上移下移
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult Update()
        {
            BaseResult br = new BaseResult();

            try
            {
                Hashtable param = GetParameters();
                br = BusinessFactory.Advertis.Update(param);
                if (br.Success)
                {
                    br = new BaseResult();
                    ParamVessel p = new ParamVessel();
                    p.Add("id", param["old_id"], HandleType.DefaultValue);
                    p.Add("new_sort", param["old_sort"], HandleType.DefaultValue);
                    param.Clear();
                    param = param.Trim(p);
                    br = BusinessFactory.Advertis.Update(param);
                }
            }
            catch (CySoftException ex)
            {

                throw ex;
            }
            return Json(br);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult UpdateModel()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                br = BusinessFactory.Advertis.Update(param);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            return Json(br);
        }
    }
}
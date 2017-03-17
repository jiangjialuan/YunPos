using System;
using System.Collections;
using System.Web.Mvc;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using CySoft.Utility.Mvc.Html;
using CySoft.Model.Enums;
using CySoft.Model.Td;
using CySoft.Utility;
using System.Collections.Generic;
using CySoft.Model.Flags;
using System.Linq;

//会员类别
namespace CySoft.Controllers.MemberCtl
{
    [LoginActionFilter]
    public class HyflController : BaseController
    {
        #region 会员类别-查询
        /// <summary>
        /// 会员类别-查询
        /// lz 2016-09-18
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult List()
        {
            Hashtable param = base.GetParameters();
            try
            {
                #region 获取参数
                int limit = base.PageSizeFromCookie;
                param.Add("id_masteruser", id_user_master);
                ParamVessel pv = new ParamVessel();
                pv.Add("_search_", "0", HandleType.DefaultValue);
                pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
                pv.Add("keyword", "", HandleType.Remove, true);
                pv.Add("page", 0, HandleType.DefaultValue);
                pv.Add("pageSize", limit, HandleType.DefaultValue);
                pv.Add("sort", "sort_id asc", HandleType.DefaultValue);
                param = param.Trim(pv);
                param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                int pageIndex = Convert.ToInt32(param["page"]);
                param.Add("limit", limit);
                param.Add("start", pageIndex * limit); 
                #endregion
                #region 获取数据
                PageNavigate pn = new PageNavigate();
                pn = BusinessFactory.Tb_Hyfl.GetPage(param);
                var plist = new PageList<Tb_Hyfl>(pn, pageIndex, limit);
                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
                ViewData["sort"] = param["sort"];       //排序规则需要返回前台 

                var yhlxBr = base.GetFlagList(Enums.TsFlagListCode.hyyhlx.ToString());
                if (yhlxBr.Success)
                    ViewBag.YHLXSelect = yhlxBr.Data;

                #endregion
                #region 返回
                if (param["_search_"].ToString().Equals("1"))
                    return PartialView("_List");
                else
                    return View(); 
                #endregion
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
            

        }
        #endregion

        #region 会员类别-新增
        /// <summary>
        /// 会员类别-新增 
        /// lz
        /// 2016-09-18
        /// </summary>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Add()
        {
            ViewData["option"] = "add";

            var yhlxBr = base.GetFlagList(Enums.TsFlagListCode.hyyhlx.ToString());
            if (yhlxBr.Success)
                ViewBag.YHLXSelect = yhlxBr.Data;

            return View("Edit");
        }
        #endregion

        #region 会员类别-Post新增
        /// <summary>
        /// 会员类别-Post新增
        /// lz
        /// 2016-09-18
        /// </summary>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Add(Tb_Hyfl model)
        {
            BaseResult br = new BaseResult();
            var oldParam = new Hashtable();
            try
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("bm", string.Empty, HandleType.DefaultValue);//bm
                p.Add("mc", string.Empty, HandleType.ReturnMsg);//mc
                p.Add("sort_id", "0", HandleType.DefaultValue);//sort_id
                p.Add("flag_yhlx", string.Empty, HandleType.ReturnMsg);//flag_yhlx
                p.Add("zk", 0m, HandleType.ReturnMsg);//zk

                param = param.Trim(p);
                param.Add("id_masteruser", id_user_master);
                param.Add("id_user", id_user);
                oldParam = (Hashtable)param.Clone();
                #endregion
                #region 新增
                br = BusinessFactory.Tb_Hyfl.Add(param);
                #endregion
                #region 返回
                WriteDBLog("会员类别-新增", oldParam, br);
                return base.JsonString(br, 1);
                #endregion
            }
            catch (Exception ex)
            {
                #region 异常返回
                br.Success = false;
                br.Data = "";
                br.Message.Add("数据不符合要求!");
                br.Level = ErrorLevel.Warning;
                WriteDBLog("会员类别-新增", oldParam, br);
                return base.JsonString(br, 1); 
                #endregion
            }
        }

        #endregion

        #region 会员类别-编辑
        /// <summary>
        /// 会员类别-编辑 
        /// lz
        /// 2016-09-18
        /// </summary>
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Edit()
        {
            try
            {
                ViewData["option"] = "edit";
                Hashtable param = base.GetParameters();
                param.Add("id_masteruser", id_user_master);
                ParamVessel p = new ParamVessel();
                p.Add("id_masteruser", id_user_master, HandleType.DefaultValue);
                p.Add("id", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                ViewData["item_edit"] = BusinessFactory.Tb_Hyfl.Get(param).Data;

                var yhlxBr = base.GetFlagList(Enums.TsFlagListCode.hyyhlx.ToString());
                if (yhlxBr.Success)
                    ViewBag.YHLXSelect = yhlxBr.Data;

                return View("Edit");
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
        }
        #endregion

        #region 会员类别-Post编辑
        /// <summary>
        /// 会员类别-Post编辑
        /// lz
        /// 2016-09-18
        /// </summary>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Edit(Tb_Hyfl model)
        {
            #region 获取参数
            var oldParam = new Hashtable();
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("bm", string.Empty, HandleType.DefaultValue);//bm
            pv.Add("mc", string.Empty, HandleType.ReturnMsg);//mc
            pv.Add("sort_id", "0", HandleType.DefaultValue);//sort_id
            pv.Add("id", string.Empty, HandleType.ReturnMsg);//id 
            pv.Add("flag_yhlx", string.Empty, HandleType.ReturnMsg);//flag_yhlx
            pv.Add("zk", 0m, HandleType.ReturnMsg);//zk
            #endregion
            #region 执行更新
            try
            {
                param_model = param.Trim(pv);
                oldParam = (Hashtable)param_model.Clone();
                br = BusinessFactory.Tb_Hyfl.Update(new Tb_Hyfl()
                {
                    id = param_model["id"].ToString(),
                    bm = param_model["bm"].ToString(),
                    mc = param_model["mc"].ToString(),
                    sort_id = int.Parse(param_model["sort_id"].ToString()),
                    id_masteruser = id_user_master,
                    id_edit = id_user,
                    flag_yhlx= byte.Parse(param_model["flag_yhlx"].ToString()),
                    zk =decimal.Parse(param_model["zk"].ToString())
                });
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            #endregion
            #region 返回
            WriteDBLog("会员类别-编辑", oldParam, br);
            return JsonString(br, 1); 
            #endregion
        } 
        #endregion

        #region 会员类别-删除
        /// <summary>
        ///会员类别-删除
        /// lz 2016-09-18
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult Delete()
        {
            var oldParam = new Hashtable();
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            param.Add("id_masteruser", id_user_master);
            pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            try
            {
                param_model = param.Trim(pv);
                oldParam = (Hashtable)param_model.Clone();
                br = BusinessFactory.Tb_Hyfl.Delete(param);
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            WriteDBLog("会员类别-删除", oldParam, br);
            return JsonString(br, 1);
        }
        #endregion
    }
}

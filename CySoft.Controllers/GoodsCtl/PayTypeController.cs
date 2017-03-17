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
using CySoft.Model.Enums;
using CySoft.Model.Tb;
using CySoft.Model.Ts;
using CySoft.Utility.Mvc.Html;

namespace CySoft.Controllers.GoodsCtl
{
    [LoginActionFilter]
    public class PayTypeController : BaseController
    {
        private List<Ts_Flag> GetFlagTypeList()
        {

            var res= GetFlagList("paytype").Data as  List<Ts_Flag>;
            return res?? new  List<Ts_Flag>();
        }


        [ActionPurview(true)]
        public ActionResult List()
        {
            Hashtable param = base.GetParameters();
            int limit = base.PageSizeFromCookie;
            param.Add("id_masteruser", id_user_master);
            ParamVessel p = new ParamVessel();
            p.Add("_search_", "0", HandleType.DefaultValue);
            p.Add("id_masteruser", String.Empty, HandleType.ReturnMsg);
            p.Add("s_mc", "", HandleType.Remove, true);
            p.Add("page", 0, HandleType.DefaultValue);
            p.Add("pageSize", limit, HandleType.DefaultValue);
            p.Add("sort", "flag_type asc", HandleType.DefaultValue);
            param = param.Trim(p);
            param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            int.TryParse(param["pageSize"].ToString(), out limit);
            PageNavigate pn = new PageNavigate();
            int pageIndex = Convert.ToInt32(param["page"]);
            param.Add("limit", limit);
            param.Add("start", pageIndex * limit);
            pn = BusinessFactory.Tb_Pay.GetPage(param);

            var plist = new PageList<Tb_Pay>(pn, pageIndex, limit);
            plist.PageIndex = pageIndex;
            plist.PageSize = limit;
            ViewData["List"] = plist;
            List<Ts_Flag> flagTypeList = GetFlagTypeList();
            ViewData["flagTypeList"] = flagTypeList;
            ViewData["IsMasterShop"] =!string.IsNullOrEmpty("id_shop") && id_shop == id_shop_master;
            if (param["_search_"].ToString().Equals("1"))
            {
                return PartialView("_List");
            }
            else
            {
                return View();
            }
        }
        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Add()
        {
            ViewData["option"] = "add";
            List<Ts_Flag> listFlag = GetFlagTypeList();
            SelectList selectListType = new SelectList(listFlag, "listdata", "listdisplay");
            List<Ts_Flag> listFlagStop = new List<Ts_Flag>()
                {
                     new Ts_Flag(){listdisplay = "启用",listdata =0},
                     new Ts_Flag(){listdisplay = "未启用",listdata =1}
                };
            SelectList selectListStop = new SelectList(listFlagStop, "listdata", "listdisplay");
            ViewBag.selectListType = selectListType;
            ViewBag.selectListStop = selectListStop;
            return View("_PayType_Edit");
        }

        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Edit()
        {
            try
            {
                Hashtable param = base.GetParameters();
                param.Add("id_masteruser", id_user_master);
                ParamVessel p = new ParamVessel();
                p.Add("id_masteruser", id_user_master, HandleType.DefaultValue);
                p.Add("id", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                var model = BusinessFactory.Tb_Pay.Get(param).Data as Tb_Pay;
                ViewData["item_edit"] = model;
                List<Ts_Flag> listFlagType = GetFlagTypeList();
                SelectList selectListType = new SelectList(listFlagType, "listdata", "listdisplay", model.flag_type);
                List<Ts_Flag> listFlagStop = new List<Ts_Flag>()
                {
                     new Ts_Flag(){listdisplay = "启用",listdata =0},
                     new Ts_Flag(){listdisplay = "禁用",listdata =1}
                };
                SelectList selectListStop = new SelectList(listFlagStop, "listdata", "listdisplay", model.flag_stop);
                ViewBag.selectListType = selectListType;
                ViewBag.selectListStop = selectListStop;
                ViewData["option"] = "edit";
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
            return View("_PayType_Edit");
        }


        /// <summary>
        /// 新增支付方式
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Add(Tb_Pay model)
        {
            Hashtable param = base.GetParameters();
            param.Add("id_masteruser", id_user_master);
            Hashtable param_model = null;
            BaseResult br = new BaseResult();
            Tb_Pay model_pay = new Tb_Pay();                             //新增对象

            try
            {
                ParamVessel pv = new ParamVessel();
                pv.Add("mc", string.Empty, HandleType.ReturnMsg);            //名称
                pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
                pv.Add("flag_type", 0, HandleType.ReturnMsg);
                pv.Add("flag_stop", 0, HandleType.ReturnMsg);
                param_model = param.Trim(pv);
                if (TryUpdateModel(model_pay))
                {
                    model_pay.id_masteruser = id_user_master;
                    model_pay.id_create = model_pay.id_edit = id_user;
                    model_pay.flag_system = 0;
                    br = BusinessFactory.Tb_Pay.Add(model_pay);
                }
                else
                {
                    br.Message.Add("参数有误!");
                }
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            return JsonString(br, 1);
        }

        /// <summary>
        /// 编辑支付方式
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Edit(Tb_Pay model)
        {
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("mc", string.Empty, HandleType.ReturnMsg);
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            pv.Add("flag_type", 0, HandleType.ReturnMsg);
            pv.Add("flag_stop", 0, HandleType.ReturnMsg);
            try
            {
                param_model = param.Trim(pv);

                br = BusinessFactory.Tb_Pay.Update(new Tb_Pay()
                {
                    id = param_model["id"].ToString(),
                    mc = param_model["mc"].ToString(),
                    flag_stop = Convert.ToByte(param_model["flag_stop"].ToString()),
                    flag_type = Convert.ToByte(param_model["flag_type"].ToString()),
                    id_masteruser = id_user_master,
                    id_edit = id_user
                });
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            return JsonString(br, 1);
        }


        /// <summary>
        /// 删除支付方式
        /// LD
        /// </summary>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult Delete()
        {
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
                br = BusinessFactory.Tb_Pay.Delete(param);
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            return JsonString(br, 1);
        }

        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Apply(int paytype,string id_shop)
        {
            if (paytype>0&&!string.IsNullOrEmpty(id_shop))
            {
                Hashtable param=new Hashtable();
                param.Add("id_shop", id_shop);
                param.Add("id_masteruser", id_user_master);
                param.Add("flag_type", paytype);
                ViewData["payConfigList"]= BusinessFactory.Tb_Pay_Config.GetAll(param).Data;
            }
            ViewData["id_shop"] = id_shop;
            ViewData["paytype"] = paytype;
            return View();
        }
    }
}

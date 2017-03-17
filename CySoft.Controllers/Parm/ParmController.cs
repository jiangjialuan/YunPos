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
using CySoft.Model.Ts;
using CySoft.Utility.Mvc.Html;

namespace CySoft.Controllers.Parm
{
    [LoginActionFilter]
    public class ParmController : BaseController
    {
        [ActionPurview(true)]
        public ActionResult List()
        {
            Hashtable param = base.GetParameters();
            try
            {
                int limit = 100000;//base.PageSizeFromCookie;
                param.Add("id_masteruser", id_user_master);
                ParamVessel pv = new ParamVessel();
                pv.Add("_search_", "0", HandleType.DefaultValue);
                pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
                pv.Add("s_parmname", "", HandleType.Remove, true);
                pv.Add("page", 0, HandleType.DefaultValue);
                pv.Add("pageSize", limit, HandleType.DefaultValue);
                pv.Add("sort", "sort_id asc", HandleType.DefaultValue);
                param = param.Trim(pv);
                int pageIndex = Convert.ToInt32(param["page"]);
                param.Add("limit", limit);
                param.Add("start", pageIndex * limit);
                PageNavigate pn = new PageNavigate();
                pn = BusinessFactory.Ts_Parm.GetPage(param);
                var plist = new PageList<Ts_Parm>(pn, pageIndex, limit);
                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
                param.Clear();
                param.Add("id_masteruser", id_user_master);
                param.Add("lg_flag_type", 15);

                param.Add("id_shop_zb_md", 1);
                param.Add("id_shop_zb", "0");
                param.Add("id_shop_md", id_shop);

                ViewData["ts_param_shops"] =BusinessFactory.Ts_Parm_Shop.GetAll(param).Data;
                ViewData["IsMasterShopUser"] = id_shop == id_shop_master?"1":"0";
                ViewData["shop_master"] = id_shop_master;
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
            if (string.Format("{0}", param["_search_"]).Equals("1"))
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
            return View("_Edit");
        }

        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Add(Ts_Parm model)
        {
            var res = HandleResult(() =>
            {
                model.id_masteruser = id_user_master;
                return BusinessFactory.Ts_Parm.Add(model);
            });
            return JsonString(res, 1);
        }

        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            Hashtable param = new Hashtable();
            param.Add("id", id);
            BusinessFactory.Ts_Parm.Get(param);
            ViewData["item_edit"] = BusinessFactory.Ts_Parm.Get(param).Data;
            ViewData["option"] = "edit";
            return View("_Edit");
        }

        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Edit(Ts_Parm model)
        {
            var res = HandleResult(() =>
            {
                if (id_shop!=id_shop_master)
                {
                    BaseResult br=new BaseResult(){Success = false};
                    br.Message.Add("非主门店下员工，不能修改此数据!");
                    return br;
                }
                model.id_masteruser = id_user_master;
                return BusinessFactory.Ts_Parm.Update(model);
            });

            if (res.Success)
            {
                RemoveCache();
            }
            return JsonString(res, 1);
        }
        [ActionPurview(true)]
        public ActionResult Delete(string id)
        {
            var res = HandleResult(() =>
            {
                Hashtable param=new Hashtable();
                param.Add("id",id);
                return BusinessFactory.Ts_Parm.Delete(param);
            });
            return JsonString(res, 1);
        }


    }
}

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
using CySoft.Model.Tb;
using CySoft.Model.Ts;
using CySoft.Utility.Mvc.Html;
using CySoft.Model.Enums;

namespace CySoft.Controllers.Parm
{
    public class ParmShopController : BaseController
    {
        [ActionPurview(true)]
        public ActionResult List()
        {
            Hashtable param = base.GetParameters();
            try
            {
                int limit = 10000;// base.PageSizeFromCookie;
                param.Add("id_masteruser", id_user_master);
                ParamVessel pv = new ParamVessel();
                pv.Add("_search_", "0", HandleType.DefaultValue);
                pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
                pv.Add("s_parmname", "", HandleType.Remove, true);
                pv.Add("id_shop", String.Empty, HandleType.Remove);
                pv.Add("page", 0, HandleType.DefaultValue);
                pv.Add("pageSize", limit, HandleType.DefaultValue);
                pv.Add("sort", "sort_id asc", HandleType.DefaultValue);
                param = param.Trim(pv);
                int pageIndex = Convert.ToInt32(param["page"]);
                param.Add("limit", limit);
                param.Add("start", pageIndex * limit);
                var shopList = GetShop(Enums.ShopDataType.UserShop); //GetUserShop();
                if (shopList.Any()&&!param.ContainsKey("id_shop"))
                {
                    var id_shop_list = (from sl in shopList select sl.id_shop).ToArray();
                    param.Add("id_shopList", id_shop_list);
                }
                ViewData["shoplist"] = shopList;
                PageNavigate pn = new PageNavigate();
                pn = BusinessFactory.Ts_Parm_Shop.GetPage(param);
                var plist = new PageList<Ts_Parm_ShopWithShopMc>(pn, pageIndex, limit);
                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
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
            ViewData["shoplist"]= GetShop(Enums.ShopDataType.UserShop); //GetUserShop();
            return View("_Edit");
        }

        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Add(Ts_Parm_Shop model)
        {
            var res = HandleResult(() =>
            {
                model.id_masteruser = id_user_master;
                return BusinessFactory.Ts_Parm_Shop.Add(model);
            });
            return JsonString(res, 1);
        }

        [ActionPurview(true)]
        [HttpGet]
        public ActionResult Edit(string id,string parmcode)
        {
            Hashtable param = new Hashtable();
            param.Add("id", id);
            BusinessFactory.Ts_Parm.Get(param);
            ViewData["item_edit"] = BusinessFactory.Ts_Parm_Shop.Get(param).Data;
            param.Clear();
            param.Add("id_masteruser",id_user_master);
            param.Add("parmcode", parmcode);
            ViewData["parmshoplist"]= BusinessFactory.Ts_Parm_Shop.GetAll(param).Data;
            ViewData["option"] = "edit";
            ViewData["shoplist"] = GetShop(Enums.ShopDataType.UserShop); //GetUserShop();
            return View("_Edit");
        }

        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Edit(UpdateParmShop model)
        {
            var res = HandleResult(() =>
            {
                //Hashtable param = base.GetParameters();
                //ParamVessel pv=new ParamVessel();
                //pv.Add("parmvalue",String.Empty,HandleType.ReturnMsg);
                //pv.Add("id_shops", String.Empty, HandleType.ReturnMsg);
                //pv.Add("id", String.Empty, HandleType.ReturnMsg);
                //pv.Add("validation_type", String.Empty, HandleType.ReturnMsg);
                ////model.id_masteruser = id_user_master;
                //param = param.Trim(pv);
                //param.Add("id_masteruser", id_user_master);
                model.id_masteruser = id_user_master;
                model.id_shop = id_shop;
                model.id_shop_master = id_shop_master;
                return BusinessFactory.Ts_Parm_Shop.Update(model);
            });
            return JsonString(res, 1);
        }
        [ActionPurview(true)]
        public ActionResult Delete(string id)
        {
            var res = HandleResult(() =>
            {
                Hashtable param = new Hashtable();
                param.Add("id", id);
                return BusinessFactory.Ts_Parm_Shop.Delete(param);
            });
            return JsonString(res, 1);
        }

    }
}

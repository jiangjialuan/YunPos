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
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Model.Ts;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;
using CySoft.Model.Enums;

namespace CySoft.Controllers.SystemCtl
{
    [LoginActionFilter]
    public class PayConfigController:BaseController
    {
        [ActionPurview(true)]
        [ActionAlias("paytype", "apply")]
        public ActionResult List()
        {
            Hashtable param = base.GetParameters();
            try
            {
                int limit = base.PageSizeFromCookie;
                param.Add("id_masteruser", id_user_master);
                ParamVessel pv = new ParamVessel();
                pv.Add("_search_", "0", HandleType.DefaultValue);
                pv.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);
                pv.Add("s_parmname", "", HandleType.Remove, true);
                pv.Add("id_shop", String.Empty, HandleType.Remove);
                pv.Add("flag_type", 0, HandleType.Remove);
                pv.Add("page", 0, HandleType.DefaultValue);//1
                pv.Add("pageSize", limit, HandleType.DefaultValue);//2
                pv.Add("sort", "parmcode asc", HandleType.DefaultValue);
                param = param.Trim(pv);
                int pageIndex = Convert.ToInt32(param["page"]);
                //param.Add("limit", limit);
                //param.Add("start", pageIndex * limit);
                var shopList = GetCurrentUserMgrShop(id_user, id_shop);//GetShop(Enums.ShopDataType.UserShop); //GetUserShop();
                ViewData["_row_total_"] = shopList.Count();
                ViewData["_page_size_"] = limit;
                ViewData["_current_page_"] = pageIndex;
                if (shopList.Any() && !param.ContainsKey("id_shop"))
                {
                    var id_shop_list = (from sl in shopList select sl.id_shop).ToList();
                    id_shop_list = id_shop_list.Skip(pageIndex*limit).Take(limit).ToList();
                    ViewData["CurrentShopIds"] = id_shop_list;
                    param.Add("id_shopList", id_shop_list.ToArray());
                }
                ViewData["shoplist"] = shopList;
                var br = BusinessFactory.Tb_Pay_Config.GetAll(param);
                var plist = br.Data;
                ViewData["List"] = plist;
                ViewData["flag_type"] = param["flag_type"];
                ViewData["payTypeList"] = GetFlagList("paytype");
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
            if (param["_search_"].ToString().Equals("1"))
            {
                return PartialView("_List1");
            }
            else
            {
                return View("List1");
            }
            //return View("List1");
        }

        [ActionPurview(false)]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            Hashtable param=new Hashtable();
            param.Add("id",id);
            ViewData["item_edit"]= BusinessFactory.Tb_Pay_Config.Get(param).Data;
            ViewData["option"] = "edit";
            return View("_Edit");
        }
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult Edit(Tb_Pay_Config model)
        {
            var res = HandleResult(() => BusinessFactory.Tb_Pay_Config.Update(model));
            return JsonString(res,1);
        }
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult EditList(string models)
        {
            var res = HandleResult(() =>
            {
               var list= JSON.Deserialize<List<Tb_Pay_Config>>(models);
               return BusinessFactory.Tb_Pay_Config.Update(list);
            });
            return JsonString(res, 1);
        } 
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult SetPayConfig(SetPayConfigModel model)
        {
            var res = HandleResult(() =>
            {
                model.id_masteruser = id_user_master;
                return BusinessFactory.Tb_Pay_Config.Update(model);
            });
            return JsonString(res, 1);
        }
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult OpenPay(OpenPayConfigModel model)
        {
            var res = HandleResult(() =>
            {
                model.id_masteruser = id_user_master;
                return BusinessFactory.Tb_Pay_Config.Update(model);
            });
            return JsonString(res, 1);
        }

    }
}

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

namespace CySoft.Controllers.PromoteCtl
{
    [LoginActionFilter]
    public class PromoteSortController:BaseController
    {
        [ActionPurview(true)]
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
                pv.Add("page", 0, HandleType.DefaultValue);
                pv.Add("pageSize", limit, HandleType.DefaultValue);
                pv.Add("sort", "sort_id asc", HandleType.DefaultValue);
                pv.Add("style","",HandleType.Remove);
                param = param.Trim(pv);
                int pageIndex = Convert.ToInt32(param["page"]);
                param.Add("limit", limit);
                param.Add("start", pageIndex * limit);
                PageNavigate pn = new PageNavigate();
                pn = BusinessFactory.Tb_Promote_Sort.GetPage(param);
                var plist = new PageList<Tb_Promote_Sort>(pn, pageIndex, limit);
                plist.PageIndex = pageIndex;
                plist.PageSize = limit;
                ViewData["List"] = plist;
                ViewData["sort"] = param["sort"];       //排序规则需要返回前台
                ViewData["promote_sort_name"] = GetFlagList();
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

        private List<Ts_Flag> GetFlagList()
        {
            List<Ts_Flag> list=new List<Ts_Flag>()
            {
                new Ts_Flag(){listcode = "dp",listdisplay = "单品促销"},
                new Ts_Flag(){listcode = "dzsp",listdisplay = "单组促销"},
                new Ts_Flag(){listcode = "zh",listdisplay = "组合促销"},
                new Ts_Flag(){listcode = "spfl",listdisplay = "商品分类促销"},
                new Ts_Flag(){listcode = "bill",listdisplay = "整单促销"}
            };
            
            return list;
        }

        [ActionPurview(true)]
        [HttpPost]
        public ActionResult Edit(Tb_Promote_Sort model)
        {
            var res = HandleResult(() =>
            {
                model.id_masteruser = id_user_master;
                return BusinessFactory.Tb_Promote_Sort.Update(model);
            });
            return JsonString(res, 1);
        }

    }
}

using System;
using System.Collections;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CySoft.Controllers.AdminCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class UserManageController : BaseController
    {
       
        public ActionResult Index()
        {
            PageNavigate pn = new PageNavigate();
            int limit = 20;
            PageList<Tb_User_Master> list = new PageList<Tb_User_Master>(limit);
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                //p.Add("id_city", 0, HandleType.DefaultValue);//是否停用
                //p.Add("id_province", 0, HandleType.DefaultValue);
                //p.Add("id_county", 0, HandleType.DefaultValue);
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("username", String.Empty, HandleType.Remove, true);
                p.Add("companyname", String.Empty, HandleType.Remove, true);
                p.Add("tel", String.Empty, HandleType.Remove, true);
                p.Add("name", String.Empty, HandleType.Remove, true);
                p.Add("phone", String.Empty, HandleType.Remove, true);
                p.Add("flag_account", 0, HandleType.Remove);
                p.Add("flag_phone_check", 0, HandleType.Remove);
                p.Add("rq_start_reg", DateTime.Now, HandleType.Remove);//排序
                p.Add("rq_end_reg", DateTime.Now, HandleType.Remove);//排序
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("limit", 20, HandleType.DefaultValue);//每页大小
                p.Add("sort", "rq_create", HandleType.DefaultValue);
                p.Add("dir", "desc", HandleType.DefaultValue);
                param = param.Trim(p);
                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                limit = Convert.ToInt32(param["limit"]);
                ViewData["pageindex"] = pageIndex;
                ViewData["sort"] = param["sort"];
                ViewData["dir"] = param["dir"];
                ViewData["limit"] = GetParameter("limit");

                //if (param["id_city"].ToString().Equals("0")) param.Remove("id_city");
                //if (param["id_province"].ToString().Equals("0")) param.Remove("id_province");
                //if (param["id_county"].ToString().Equals("0")) param.Remove("id_county");
                if (pageIndex < 1)
                {
                    pageIndex = 1;
                }
                if (limit < 1)
                {
                    param["limit"] = limit = 1;
                }
                param.Add("start", (pageIndex - 1) * limit);
                pn = BusinessFactory.UserMaster.GetPage(param);
                list = new PageList<Tb_User_Master>(pn, pageIndex, limit);
                // return Json(list, JsonRequestBehavior.AllowGet);
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
                return PartialView("_ListUserMasterControl", list);
            }
            return View(list);
        }
    }
}

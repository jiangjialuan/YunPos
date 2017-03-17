using System;
using System.Collections;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Frame.Common;
using CySoft.Frame.Core;

namespace CySoft.Controllers.MemberCtl
{
    [OutputCache(Location = OutputCacheLocation.None)]
    public class MemberController : BaseController
    {
        public ActionResult List()
        {
            Hashtable param_i = base.GetParameters();
            Hashtable param_q = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("_search_", "0", HandleType.DefaultValue);

            try
            {
                param_q = param_i.Trim(pv);
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                return RedirectToAction(actionName: "Index", controllerName: "Error", routeValues: new { id = "500" });
            }

            if (param_q["_search_"].ToString().Equals("1"))
            {
                return PartialView("../PartialView/MemberList");
            }
            else
            {
                return View("List");
            }
        }

        public ActionResult Add()
        {
            return View("Add");
        }
    }
}

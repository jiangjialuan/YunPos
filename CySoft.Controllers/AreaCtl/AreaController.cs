using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using CySoft.Controllers.Base;

namespace CySoft.Controllers.MemberCtl
{
    [OutputCache(Location = OutputCacheLocation.None)]
    public class AreaController : BaseController
    {
        public ActionResult GetName()
        {
            return null;
        }

        public ActionResult Index()
        {
            Hashtable param = new Hashtable();
            param.Add("fatherId", 0);
            var br = BusinessFactory.Districts.GetAll(param);
            if (br.Success)
            {
                ViewData["AreaData"] = br.Data;
            }
            return PartialView("../PartialView/AreaIndex");
        }

        public ActionResult GetCity()
        {
            Hashtable param = base.GetParameters();
            if (param != null && param.ContainsKey("province_id") && !string.IsNullOrEmpty(param["province_id"].ToString()))
            {
                Hashtable ht = new Hashtable();
                ht.Add("fatherId", param["province_id"].ToString());
                var br = BusinessFactory.Districts.GetAll(ht);
                if (br.Success)
                {
                    ViewData["AreaData"] = br.Data;
                }
            }
            return PartialView("../PartialView/AreaGetCity");
        }
        public ActionResult GetCounty()
        {
            Hashtable param = base.GetParameters();
            if (param != null && param.ContainsKey("city_id") && !string.IsNullOrEmpty(param["city_id"].ToString()))
            {
                Hashtable ht = new Hashtable();
                ht.Add("fatherId", param["city_id"].ToString());
                var br = BusinessFactory.Districts.GetAll(ht);
                if (br.Success)
                {
                    ViewData["AreaData"] = br.Data;
                }
            }
            return PartialView("../PartialView/AreaGetCounty");
        }
    }
}

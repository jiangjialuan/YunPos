using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Controllers.Filters;
using System.Web.UI;
using System.Web.Mvc;
using CySoft.Controllers.Base;

#region 销售退货单
#endregion

namespace CySoft.Controllers.SaleCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class SaleReturnController : BaseController
    {
        public ActionResult List()
        {
            return View();
        }
    }
}

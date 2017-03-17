#region Imports
using System;
using System.Collections;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Frame.Core;
#endregion

#region 错误页
#endregion

namespace CySoft.Controllers.OtherCtl
{
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ErrorController : BaseController
    {
        public ActionResult Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) id = string.Empty;
            return View(id as object);
        }

        public ActionResult Mobile()
        {
            return View();
        }
    }
}
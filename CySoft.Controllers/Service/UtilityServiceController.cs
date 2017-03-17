using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CySoft.Controllers.Service.Base;
using CySoft.Frame.Core;

namespace CySoft.Controllers.Service
{
    public class UtilityServiceController : ServiceBaseController
    {
        public ActionResult DownLoad()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "Down\\YUNPOS.exe";
            return File(path, "application/octet-stream", "YUNPOS.exe");
        }

    }
}

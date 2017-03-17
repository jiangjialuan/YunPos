using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Controllers.Base;
using CySoft.Frame.Common;
using CySoft.Frame.Core;
using System.Web;
using System.Web.Mvc;
using CySoft.Model.Tb;

namespace CySoft.Controllers.TestCtl
{
    public class TestController : BaseController
    {
        public ActionResult Do(string merchantId)
        {
            return Content(Utility.JSON.Serialize(new { AA = "aa", BB = 123 }), "application/json", Encoding.UTF8);
        }

        public ActionResult Tree()
        {
            BaseResult br = BusinessFactory.Tb_Spfl.GetTree(new Hashtable
            {
                { "id_cyuser", base.UserData["ID"] },
                { "childId", "0" },
                { "sort", "mc" },
                { "dir", "asc" },
            });

            if (br.Success)
            {
                List<Tb_Spfl_Tree> list_spfl = br.Data as List<Tb_Spfl_Tree>;
                ViewData["list_spfl"] = list_spfl;
            }
            return View("SP");
        }
    }
}

using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Tz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.BLL.BusinessBLL
{
    public class Tz_Ys_JscBLL : BaseBLL
    {

        #region GetPage
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Tz_Ys_Jsc), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tz_Ys_Jsc_QueryModel>(typeof(Tz_Ys_Jsc), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }
        #endregion

    }
}

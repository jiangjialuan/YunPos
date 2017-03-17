using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.Model.Enums;
using CySoft.Model.Td;
using CySoft.Model.Tz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Utility;
using CySoft.Model.Tb;

//购销结算池
namespace CySoft.BLL.BusinessBLL
{

    public class Tz_Yf_Jsc_GxBLL : BaseBLL
    {

        #region 获取分页数据
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Tz_Yf_Jsc_Gx), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tz_Yf_Jsc_Gx_QueryModel>(typeof(Tz_Yf_Jsc_Gx), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }
        #endregion



    }
}

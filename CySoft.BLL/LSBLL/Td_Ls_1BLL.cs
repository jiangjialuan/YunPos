using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.IDAL;
using CySoft.Model.Td;

namespace CySoft.BLL.LSBLL
{
    public class Td_Ls_1BLL : BaseBLL
    {
        public ITd_Ls_1DAL Td_Ls_1DAL { get; set; }

        public override BaseResult Get(Hashtable param)
        {
            BaseResult res=new BaseResult(){Success = true};
            GetHomePageData(res, param);
            return res;
        }
        /// <summary>
        /// 查询首页显示数据
        /// </summary>
        /// <param name="res"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private bool GetHomePageData(BaseResult res,Hashtable param)
        {
            var ht = Td_Ls_1DAL.GetHomePageData(param) as Hashtable;
            ht = ht ?? new Hashtable();
            var payList= Td_Ls_1DAL.QueryHomePagePays(param);
            var kcje = Td_Ls_1DAL.QueryHomePageShopKcj(param);
            var cxsps = Td_Ls_1DAL.QueryHomePageCxsp(param);



            var zxsps = Td_Ls_1DAL.QueryHomePageZxsp(param);
            ht.Add("payList",payList);
            ht.Add("kcje", kcje);
            ht.Add("cxsps", cxsps);
            ht.Add("zxsps", zxsps);
            res.Data = ht;
            return true;

        }


    }
}

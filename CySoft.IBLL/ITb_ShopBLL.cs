using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Frame.Core;
using CySoft.IBLL.Base;
using CySoft.Model.Other;

namespace CySoft.IBLL
{
    public interface ITb_ShopBLL:IBaseBLL
    {
        #region 接口服务方法
        BaseResult QueryShopSelectModels(Hashtable param);
        BaseResult GetPosShopInfo(Hashtable param);

        BaseResult GetMaxBMInfo(Hashtable param);

        int GetUserShopCount(Hashtable param);

        BaseResult CloseShopWithOutMaster(Hashtable param);

        BaseResult ResetOpenShop(Hashtable param);

        BaseResult QueryShopListWithFatherId(Hashtable param);

        BaseResult ResetYZM(Hashtable param);

        #endregion
    }
}

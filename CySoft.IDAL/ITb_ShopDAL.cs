using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.IDAL.Base;
using CySoft.Model.Other;
using CySoft.Model.Tb;

namespace CySoft.IDAL
{
    public interface ITb_ShopDAL : IBaseDAL
    {
        IList<ShopSelectModel> QueryShopSelectModels(Type type, IDictionary param, string database = null);
        PosShopInfoModel GetPosShopInfo(Type type, IDictionary param, string database = null);
        Tb_Shop GetMaxBMInfo(Type type, IDictionary param = null, string database = null);
        int CloseShopWithOutMaster(Type type, IDictionary param = null, string database = null);
        int ResetOpenShop(Type type, IDictionary param = null, string database = null);

        IList<Tb_ShopWithFatherId> QueryShopListWithFatherId(Type type, IDictionary param, string database = null);
        
    }
}

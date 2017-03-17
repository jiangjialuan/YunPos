#region Imports
using CySoft.IDAL.Base;
using CySoft.Model.Tb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Model.Other;

#endregion

namespace CySoft.IDAL
{
    public interface ITb_ShopspDAL : IBaseDAL
    {
        IList<ShopspList_Query> GetShopspList(Type type, IDictionary param, string database = null);
        IList<Tb_Shopsp_Query_For_Ps> GetShopspListForPs(Type type, IDictionary param, string database = null);
        IList<SelectSpModel> GetPageList(Type type, IDictionary param, string database = null);
        IList<Tb_Shopsp_Query_For_Ps> GetPageListForPs(Type type, IDictionary param, string database = null);
        int GetMaxBarcodeInfo(Type type, IDictionary param = null, string database = null);
        IList<ShopspList_Query> GetShopspDwList(Type type, IDictionary param, string database = null);
        int GetCountSel(Type type, IDictionary param = null, string database = null);
        IList<TResult> QueryPageSel<TResult>(Type type, IDictionary param, string database = null);


    }
}


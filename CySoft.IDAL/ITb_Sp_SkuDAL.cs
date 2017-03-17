using System;
using System.Collections;
using System.Collections.Generic;
using CySoft.IDAL.Base;
using CySoft.Model.Tb;
using CySoft.Model.Other;

namespace CySoft.IDAL
{
    public interface ITb_Sp_SkuDAL : IBaseDAL
    {
        IList<Tb_Sp_Sku_Query> QueryList1(Type type, IDictionary param, string database = null);
        IList<SkuData> QueryListExport(Type type, IDictionary param, string database = null);
    }
}

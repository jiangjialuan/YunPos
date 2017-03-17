using System;
using System.Collections;
using System.Collections.Generic;
using CySoft.IDAL.Base;
using CySoft.Model.Tb;
using CySoft.Model.Other;

namespace CySoft.IDAL
{
    public interface ITb_SpDAL : IBaseDAL
    {
        IList<Tb_Sp_Query> QueryPageOfService(Type type, IDictionary param, string database = null);
        IList<Tb_Sp_Query> QueryPageOfOrder(Type type, IDictionary param, string database = null);
        int QueryCountOfService(Type type, IDictionary param, string database = null);
        int QueryCountOfOrder(Type type, IDictionary param, string database = null); 
        IList<Tb_Gys_Sp> QueryList1(Type type, IDictionary param, string database = null);
        IList<Tb_Sp_Query> QueryPageOfServiceForSearch(Type type, IDictionary param, string database = null);
        IList<Tb_Sp_Info> QueryInfoAll(Type type, IDictionary param, string database = null);
        int UpSp(Type type, IDictionary param, string database = null);

        //分词查询
        IList<SkuData> QueryAnalysisPage(Type type, IDictionary param, string database = null);
        int QueryAnalysisCount(Type type, IDictionary param, string database = null);
        
    }
}

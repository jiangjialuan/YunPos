using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IDAL.Base;
using CySoft.Model.Tb;
using System.Collections;

namespace CySoft.IDAL
{
    public interface ITb_Gys_CgsDAL : IBaseDAL
    {
        IList<Tb_Gys_Cgs_Query> QueryPageOfBuyer(Type type, IDictionary param, string database = null);
        int QueryCountOfBuyer(Type type, IDictionary param, string database = null);
        IList<Tb_Gys_Cgs_Query> QueryListOfBuyer(Type type, IDictionary param, string database = null);
        IList<Tb_Gys_Cgs_Query> QueryListOfSupplier(Type type, IDictionary param, string database = null);
        //IList<Tb_Gys_Query> FindSupplier(Type type, IDictionary param, string database = null);
    }
}

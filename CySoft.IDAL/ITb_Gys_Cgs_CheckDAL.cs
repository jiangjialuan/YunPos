using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IDAL.Base;
using CySoft.Model.Tb;
using System.Collections;

namespace CySoft.IDAL
{
    public interface ITb_Gys_Cgs_CheckDAL : IBaseDAL
    {
        IList<Tb_Gys_Cgs_Check_Query> QueryListOfBuyerAttention(Type type, IDictionary param, string database = null);
        IList<Tb_Gys_Cgs_Check_Query> QueryPageOfSupplierAttention(Type type, IDictionary param, string database = null);
    }
}

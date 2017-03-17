using System;
using System.Collections;
using System.Collections.Generic;
using CySoft.IDAL.Base;
using CySoft.Model.Tb;

namespace CySoft.IDAL
{
    public interface ITb_Sp_DjDAL : IBaseDAL
    {
        IList<Tb_Sp_Dj_Query> QueryList1(Type type, IDictionary param, string database = null);
    }
}

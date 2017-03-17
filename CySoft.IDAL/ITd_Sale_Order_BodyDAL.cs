using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IDAL.Base;
using System.Collections;
using CySoft.Model.Td;
using CySoft.Model.Tb;
using CySoft.Model.Other;

namespace CySoft.IDAL
{
    public interface ITd_Sale_Order_BodyDAL : IBaseDAL
    {
        IList<Td_Sale_Order_Body_Query> QueryStatisticsPage(Type type, System.Collections.IDictionary param, string database = null);
        int QueryStatisticsCount(Type type, System.Collections.IDictionary param, string database = null);
        Td_Report QueryStatisticsInfo(Type type, IDictionary param, string database = null);
        int Data_reset(Type type, IDictionary param, string database = null);
    }
}

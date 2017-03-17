using System;
using System.Collections;
using System.Collections.Generic;
using CySoft.IDAL.Base;
using CySoft.Model.Tb;
using CySoft.Model.Td;

namespace CySoft.IDAL
{
    public interface ITd_sale_payDAL : IBaseDAL
    {
        IList<Td_sale_pay_Query> QueryPageforView(Type type, IDictionary param, string database = null);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Td;

namespace CySoft.IDAL
{
    public interface ITd_Kc_Kspd_2DAL
    {
        IList<Td_Kc_Kspd_2> QureyKspd2LeftJoinKspd1(Type type, IDictionary param, string database = null);
    }
}

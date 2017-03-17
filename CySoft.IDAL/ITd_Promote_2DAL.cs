using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.IDAL.Base;
using CySoft.Model.Td;

namespace CySoft.IDAL
{
    public interface ITd_Promote_2DAL : IBaseDAL
    {
        List<Td_Promote_2_Query> QueryListWithSpfl(Hashtable param);

        List<Td_Promote_2_Query> QueryListWithSp(Hashtable param);
    }
}

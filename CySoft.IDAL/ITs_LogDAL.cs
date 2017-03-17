using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IDAL.Base;
using CySoft.Model.Ts;

namespace CySoft.IDAL
{
    public interface ITs_LogDAL : IBaseDAL
    {
        Ts_Log QueryItem(Type type, System.Collections.IDictionary param, string database = null);
    }
}

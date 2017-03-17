using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IDAL.Base;

namespace CySoft.IDAL
{
    public interface ITs_param_businessDAL : IBaseDAL
    {
        int Insert_yw(Type type, System.Collections.IDictionary param, string database = null);
        int Insert_SystemSet(Type type, System.Collections.IDictionary param, string database = null);
    }
}

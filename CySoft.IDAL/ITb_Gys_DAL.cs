using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IDAL.Base;
using CySoft.Model.Tb;

namespace CySoft.IDAL
{
    public interface ITb_Gys_DAL : IBaseDAL
    {
        Tb_Cgs QueryGysOfCgs(Type type, IDictionary param, string database = null);
    }
}

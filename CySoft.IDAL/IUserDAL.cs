using System;
using System.Collections.Generic;
using CySoft.IDAL.Base;
using CySoft.Model.Tb;
using System.Collections;


namespace CySoft.IDAL
{
    public interface IUserDAL : IBaseDAL
    {
        IList<Tb_User_Master> PageUserMaster(Type type, IDictionary param, string database = null);
    }
}

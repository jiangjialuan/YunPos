using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IDAL.Base;
using CySoft.Model.Tb;

namespace CySoft.IDAL
{
    public interface ITb_KhflDAL : IBaseDAL
    {
        IList<Tb_Khfl> QuerySubListByPath(string id_masteruser, string fatherPath);
    }

}

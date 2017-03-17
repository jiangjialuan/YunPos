using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.IDAL.Base;
using CySoft.Model;
using CySoft.Model.Tb;

namespace CySoft.IDAL 
{
    public interface ITb_GysflDAL : IBaseDAL
    {
        IList<Tb_Gysfl> QuerySubListByPath(string id_masteruser, string fatherPath);
    }
}

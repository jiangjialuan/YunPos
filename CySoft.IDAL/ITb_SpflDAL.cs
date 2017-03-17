using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.IDAL.Base;
using CySoft.Model;

namespace CySoft.IDAL 
{
    public interface ITb_SpflDAL : IBaseDAL
    {
        IList<Tb_Spfl> QuerySubListByPath(string id_cyuser, string fatherPath);
    }
}

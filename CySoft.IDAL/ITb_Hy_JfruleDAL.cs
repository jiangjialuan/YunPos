using CySoft.IDAL.Base;
using CySoft.Model.Ts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.IDAL
{
    public interface ITb_Hy_JfruleDAL : IBaseDAL
    {
        void AddWithExists(Ts_HykDbjf model);
    }
}

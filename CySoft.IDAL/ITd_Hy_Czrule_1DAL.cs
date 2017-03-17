
#region Imports
using CySoft.IDAL.Base;
using CySoft.Model.Ts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace CySoft.IDAL
{
    public interface ITd_Hy_Czrule_1DAL : IBaseDAL
    {
        void AddWithExists(Ts_HykDbjf model);
    }
}
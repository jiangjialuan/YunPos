#region Imports
using CySoft.IDAL.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace CySoft.IDAL
{
    public interface ITz_Hy_Je_FlowDAL : IBaseDAL
    {
        int AddFlowWithExists(Type type, IDictionary param, string database = null);

        int AddFlowForXFWithExists(Type type, IDictionary param, string database = null);
        
    }
}
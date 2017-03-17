using CySoft.IBLL.Base;
using CySoft.Model.Tb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Frame.Core;

namespace CySoft.IBLL
{
    public interface IFunctionBll : IBaseBLL
    {
        IList<Tb_Function_Tree> GetFunctionTree(Hashtable param);

        BaseResult MoveNode(Hashtable param);

    }
}

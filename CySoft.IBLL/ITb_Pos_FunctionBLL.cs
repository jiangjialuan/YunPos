using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.IBLL.Base;
using CySoft.Model.Tb;

namespace CySoft.IBLL
{
    public interface ITb_Pos_FunctionBLL:IBaseBLL
    {
        IList<Tb_Pos_Function_Tree> GetFunctionTree(Hashtable param);
    }
}

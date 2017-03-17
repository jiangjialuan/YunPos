using CySoft.IBLL.Base;
using CySoft.Model.Tb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.IBLL
{
    public interface IRoleModuleBll : IBaseBLL
    {
        IList<Tb_Role_Module_Tree> GetRoleModuleTree(Hashtable param);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Frame.Core;
using CySoft.IBLL.Base;
using System.Collections;

namespace CySoft.IBLL
{
    public interface IProcessFunBLL : IBaseBLL
    {
        BaseResult GetProcess(long id_user);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IBLL.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Model.Tb;
namespace CySoft.IBLL
{
    public interface IProcessBLL : IBaseBLL
    {
        BaseResult CompareProcess(Hashtable param);
    }
}

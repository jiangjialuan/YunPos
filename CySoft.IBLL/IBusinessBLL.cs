using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IBLL.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Model.Tb;
using System.Data;
using CySoft.Model.Other;
namespace CySoft.IBLL
{
    public interface IBusinessBLL : IBaseBLL
    {

        BaseResult ProcedureQuery(Hashtable param);

        BaseResult ProcedureOutQuery(Hashtable param);

        BaseResult ShydhQuery(Hashtable param);
        

    }
}

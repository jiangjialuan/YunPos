#region Imports
using CySoft.Frame.Core;
using CySoft.IBLL.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace CySoft.IBLL
{
    public interface ITd_Jh_1BLL : IBaseBLL
    {
        BaseResult ImportIn(Hashtable param);
        
    }
}
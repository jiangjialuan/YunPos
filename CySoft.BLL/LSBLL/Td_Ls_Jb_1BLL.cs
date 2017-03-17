using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Other;
using CySoft.Model.Td;

namespace CySoft.BLL.LSBLL
{
    public class Td_Ls_Jb_1BLL : BaseBLL 
    {
        public override BaseResult Get(Hashtable param)
        {
            BaseResult res=new BaseResult(){Success = true};
            
            //DAL.GetItem(typeof(Td_Ls_Jb_1), param);
            return res;
        }

       
    }
}

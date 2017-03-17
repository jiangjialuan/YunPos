using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Td;

namespace CySoft.BLL.PSBLL
{
    public class Td_Ps_Sq_2BLL : BaseBLL
    {
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult res = new BaseResult() {Success = true};
            res.Data= DAL.QueryList<Td_Ps_Sq_2_Query>(typeof(Td_Ps_Sq_2), param).ToList();
            return res;
        }
    }
}

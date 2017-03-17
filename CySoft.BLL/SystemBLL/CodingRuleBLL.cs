using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Model.Ts;

namespace CySoft.BLL.SystemBLL
{
    public class CodingRuleBLL : BaseBLL
    {
        /// <summary>
        ///  规则
        ///  znt 2015-03-17
        /// </summary>

        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            br.Data=DAL.GetItem(typeof(Ts_Codingrule), param);
            br.Success = true;
            return br;

        }

    }
}

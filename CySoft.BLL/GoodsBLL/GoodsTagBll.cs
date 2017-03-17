using CySoft.BLL.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Model.Tb;
using CySoft.Frame.Core;

namespace CySoft.BLL.GoodsBLL
{
    public class GoodsTagBll:BaseBLL
    {
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Tb_Gys_Tag>(typeof(Tb_Gys_Tag), param);
            br.Success = true;
            return br;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Other;

namespace CySoft.BLL.GoodsBLL
{
    public class Tb_Sp_JgzhBLL : BaseBLL 
    {
        public override BaseResult Add(dynamic entity)
        {
            BaseResult res=new BaseResult(){Success = true};
            var model= entity as SpzhcfModel;


            return res;
        }
    }
}

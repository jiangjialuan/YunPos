using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.BLL.GoodsBLL
{
    public class Tb_Shopsp_ExportdataBLL : BaseBLL
    {
        public override BaseResult GetAll(Hashtable param)
        {
            BaseResult br = new BaseResult();
            var sp = DAL.QueryList<Tb_Shopsp_Exportdata>(typeof(Tb_Shopsp_Exportdata), param);
            br.Data = sp;
            br.Success = true;
            return br;
        }
    }
}

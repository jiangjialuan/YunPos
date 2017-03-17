using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Frame.Core;
using CySoft.IBLL.Base;
using CySoft.Model.Tb;

namespace CySoft.IBLL
{
    public interface IGoodsRecommendBll : IBaseBLL
    {
        BaseResult Pass(Tb_Sp_Sku_Push info);

        BaseResult NoPass(Tb_Sp_Sku_Push info);

        BaseResult Invalid(Tb_Sp_Sku_Push info);

    }
}

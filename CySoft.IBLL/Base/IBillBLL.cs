using System;
using System.Collections;
using CySoft.Frame.Core;
using CySoft.Model.Td;

namespace CySoft.IBLL.Base
{
    public interface IBillBLL : IBaseBLL
    {
        BaseResult Check(Hashtable param);
        BaseResult UnCheck(Hashtable param);
        BaseResult Copy(Hashtable param);
        BaseResult Cancel(Hashtable param);
        BaseResult Invalid(Hashtable param);
        BaseResult CheckOrder(Hashtable param);
        BaseResult CheckFinance(Hashtable param);
        BaseResult SetOrderAddress(Hashtable param);
        BaseResult SetOrderInvoice(Hashtable param);
        BaseResult Edit(dynamic entity);
        BaseResult PayTotal(Hashtable param);
        BaseResult GysHomeTotal(Hashtable param);
        PageNavigate GetStatistics(Hashtable param);
        BaseResult GetStatisticsInfo(Hashtable param);
    }
}

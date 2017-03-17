using CySoft.Frame.Core;
using CySoft.IBLL.Base;
using CySoft.Model.Pay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.IBLL
{
    public interface IPayBLL : IBaseBLL
    {
        BaseResult OutOrderPay(Hashtable param);
        BaseResult AliPayQuery(Hashtable param);
        BaseResult WXPayQuery(Hashtable param);
        

    }
}

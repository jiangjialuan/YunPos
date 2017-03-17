using System;
using System.Collections.Generic;
using CySoft.IBLL.Base;
using CySoft.Frame.Core;
using System.Collections;

namespace CySoft.BLL.Base
{
    public class BillBLL : BaseBLL, IBillBLL
    {
        public virtual BaseResult Check(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual BaseResult UnCheck(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual BaseResult Copy(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual BaseResult Cancel(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual BaseResult Invalid(Hashtable param)
        {
            throw new NotImplementedException();
        }
        public virtual BaseResult CheckOrder(Hashtable param)
        {
            throw new NotImplementedException();
        }
        public virtual BaseResult CheckFinance(Hashtable param)
        {
            throw new NotImplementedException();
        }
         public virtual BaseResult SetOrderAddress(Hashtable param)
        {
            throw new NotImplementedException();
        }
         public virtual BaseResult SetOrderInvoice(Hashtable param)
        {
            throw new NotImplementedException();
        }

         public virtual BaseResult Edit(dynamic entity)
         {
             throw new NotImplementedException();
         }
         public virtual BaseResult PayTotal(Hashtable param)
        {
            throw new NotImplementedException();
        }
         public virtual BaseResult GysHomeTotal(Hashtable param)
         {
             throw new NotImplementedException();
         }
         public virtual PageNavigate GetStatistics(Hashtable param) 
         {
             throw new NotImplementedException();
         }
         public virtual BaseResult GetStatisticsInfo(Hashtable param)
         {
             throw new NotImplementedException();
         }
    }
}

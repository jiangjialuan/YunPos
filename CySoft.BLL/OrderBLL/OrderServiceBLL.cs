using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.IBLL;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.IDAL;
using CySoft.Model.Td;

namespace CySoft.BLL.OrderBLL
{
    public class OrderServiceBLL : BaseBLL, IOrderServiceBLL
    {
        public ITd_Sale_Order_HeadDAL Td_Sale_Order_HeadDAL { get; set; }

        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = Td_Sale_Order_HeadDAL.QueryCountOfService(typeof(Td_Sale_Order_Head), param);
            if (pn.TotalCount > 0)
            {
                pn.Data = Td_Sale_Order_HeadDAL.QueryPageOfService(typeof(Td_Sale_Order_Head), param);
            }
            else
            {
                pn.Data = new List<Td_Sale_Order_Head_Query>();
            }
            pn.Success = true;
            return pn;
        }

        public virtual BaseResult GetCount(Hashtable param = null)
        {
            throw new NotImplementedException();
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Td;

namespace CySoft.BLL.PromoteBLL
{
    public class Td_Promote_ShopBLL : BaseBLL
    {
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult res=new BaseResult(){Success = true};
            if (param == null || !param.ContainsKey("id_masteruser") || !param.ContainsKey("id_bill"))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            DAL.QueryList(typeof(Td_Promote_Shop), param);
            return res;
        }
    }
}

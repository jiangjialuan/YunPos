using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using System.Collections;
using CySoft.IBLL;

#region 采购商
#endregion

namespace CySoft.BLL.BuyerBLL
{
    public class BuyerBLL : BaseBLL,IBuyerBLL
    {
        /// <summary>
        /// 采购商 收货地址
        /// </summary>
        /// <returns></returns>
        public BaseResult RecieverAddress(int cgsid)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            param.Add("id_cgs", cgsid);
            br.Data = DAL.QueryList<Tb_Cgs_Shdz_Query>(typeof(Tb_Cgs_Shdz), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 获取 采购商单体
        /// znt 2015-3-19
        /// </summary>
        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.GetItem<Tb_Cgs>(typeof(Tb_Cgs), param);
            br.Success = true;
            return br;
        }
    }
}

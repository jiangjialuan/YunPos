using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Tb;

namespace CySoft.BLL.GoodsBLL
{
    public class Tb_Shop_ShopBLL : BaseBLL
    {

        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult res = new BaseResult() {Success = true};
            res.Data=DAL.QueryList<Tb_Shop_ShopWithMc>(typeof(Tb_Shop_Shop), param).ToList();
            return res;
        }

    }
}

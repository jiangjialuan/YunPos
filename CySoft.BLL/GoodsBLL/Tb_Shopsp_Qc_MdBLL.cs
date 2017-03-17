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
    public class Tb_Shopsp_Qc_MdBLL : BaseBLL
    {
        #region GetPage
        public override PageNavigate GetPage(Hashtable param)
        {
            PageNavigate pn = new PageNavigate();
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", param["id_masteruser"]);
            ht.Add("keyword", param["keyword"]);
            ht.Add("flag_delete", (byte)CySoft.Model.Enums.Enums.FlagDelete.NoDelete);
            if (param.ContainsKey("id_shop"))
                ht.Add("id_shop", param["id_shop"]);

            pn.TotalCount = DAL.GetCount(typeof(Tb_Shopsp_Qc_Md), ht);
            if (pn.TotalCount > 0)
            {
                var list = DAL.QueryPage<Tb_Shopsp_Qc_Md>(typeof(Tb_Shopsp_Qc_Md), param);
                pn.Data = list;
            }

            pn.Success = true;
            return pn;
        }
        #endregion
    }
}

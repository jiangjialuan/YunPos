#region Imports
using CySoft.Frame.Core;
using CySoft.IBLL.Base;
using CySoft.Model.Tb;
using CySoft.Model.Td;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace CySoft.IBLL
{
    public interface ITb_ShopspBLL : IBaseBLL
    {
        BaseResult BatchSave(Hashtable param);
        BaseResult Copy(Hashtable param);
        BaseResult ImportIn(Hashtable param);
        BaseResult GetShopspList(Hashtable param);
        PageNavigate GetPageList(Hashtable param);
        BaseResult CheckParam(Hashtable param, Td_Sp_Qc qcModel, List<Tb_Shopsp_DBZ> dbzList);

        PageNavigate GetPageListForPs(Hashtable param);

        BaseResult GetShopspListForPs(Hashtable param);

        BaseResult GetMaxBarcodeInfo(Hashtable param);

        BaseResult GetShopspDwList(Hashtable param);

    }
}
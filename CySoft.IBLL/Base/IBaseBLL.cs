using System;
using System.Collections;
using CySoft.Frame.Core;
using CySoft.Model.Other;
using System.Collections.Generic;
using CySoft.Model.Tb;
using System.Data;
using CySoft.Model;

namespace CySoft.IBLL.Base
{
    public interface IBaseBLL
    {
        BaseResult Add(dynamic entity);
        BaseResult Delete(Hashtable param);
        BaseResult Update(dynamic entity);
        BaseResult Get(Hashtable param);
        BaseResult GetAll(Hashtable param = null);
        BaseResult GetCount(Hashtable param = null);
        PageNavigate GetPage(Hashtable param = null);
        PageNavigate GetPage<TType,TQuery>(Hashtable param = null);
        BaseResult Save(dynamic entity);
        BaseResult Export(Hashtable param = null);
        BaseResult Stop(Hashtable param);
        BaseResult Active(Hashtable param);
        BaseResult Init(Hashtable param);
        BaseResult CheckStock(Hashtable param);
        int isAdmin(Hashtable param);
        void Add(Info_User info);
        int GetGGCount(Hashtable param);
        int GetByID(Hashtable param);
        string GetNewDH(string id_masteruser, string id_shop, Model.Enums.Enums.FlagDJLX type);
        Hashtable GetParm(string id_user_master);
        Hashtable GetShopParm(string id_user_master,string id_shop);
        void ClearShopParm(string id_user_master, string id_shop);
        void ClearTbShop(string id_shop);
        BaseResult GetHy_ShopShare(string id_shop_select, string id_user_master);
        Hashtable GetAutoAudit(string id_user_master, string id_shop, string id_shop_master);
        Hashtable GetCYService(Hashtable param);
        string GetBuyServiceUrl(Hashtable param);
        string GetServiceBM(string version);
        string GetBarcodePic(Tb_Shopsp_Service model);
        PageNavigate GetPageSel(Hashtable param = null);

    }
}

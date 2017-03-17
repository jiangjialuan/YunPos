using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Enums;
using CySoft.Model.Tb;
using CySoft.Utility;
using Spring.Collections;

namespace CySoft.Controllers
{
    [LoginActionFilter]
    public class SearchConditionController:BaseController
    {
        /// <summary>
        /// 获取门店相关
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult GetShop(string type)
        {
            var shoplist = new List<Tb_User_ShopWithShopMc>();
            if (string.IsNullOrEmpty(type))
            {
                //if (id_shop == id_shop_master)
                //{
                //    shoplist = GetShop(Enums.ShopDataType.UserShop);
                //}
                //else
                //{
                //    shoplist = GetShop(Enums.ShopDataType.UserShopOnly);
                //}
                shoplist = GetCurrentUserMgrShop(id_user, id_shop);
            }
            else
            {
                if (type == "all")
                {
                    shoplist=GetShop(Enums.ShopDataType.All);
                }
            }
            if (shoplist.Any())
            {
                var obj = (from l in shoplist
                           select new
                           {
                               id_shop = l.id_shop,
                               mc = l.mc
                           }).ToList();
                return JsonString(obj);
            }
            return JsonString(new List<Tb_Shop>());
        }
        /// <summary>
        /// 获取经办人
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult GetUser(string type)
        {
            var userlist=new List<Tb_User>();
            if (string.IsNullOrEmpty(type))
            {
                userlist=GetUser();
                if (userlist.Any())
                {
                    var ul= from u in userlist
                    select new 
                    {
                        id=u.id,
                        username=u.name
                    };
                    return JsonString(ul);
                }
            }
            return JsonString(new List<Tb_User>());
        }
        /// <summary>
        /// 获取供应商
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult GetGys(string type)
        {
            return JsonString(null);
        }

        /// <summary>
        /// 根据门店ID获取对应的配送中心
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult GetPsShop(string id)
        {
            var br=HandleResult(() =>
            {
                BaseResult res=new BaseResult();
                Hashtable param = new Hashtable();
                if (!id.IsEmpty())
                {
                    param.Add("id", id);
                    param.Add("id_masteruser",id_user_master);
                    param.Add("flag_state", (int)Enums.ShopShopFlagState.Use);
                    param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                    res = BusinessFactory.Tb_Shop.Get(param);
                    var shop = res.Data as Tb_Shop;
                    if (shop != null)
                    {
                        if (shop.id_shop_ps.IsEmpty())
                        {
                            param.Clear();
                            param.Add("id_shop_child",id);
                            param.Add("id_masteruser", id_user_master);
                            param.Add("flag_state", (int)Enums.ShopShopFlagState.Use);
                            param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                            var shopshopList= BusinessFactory.Tb_Shop_Shop.GetAll(param).Data as List<Tb_Shop_ShopWithMc>;
                            if (shopshopList != null && shopshopList.Any())
                            {
                                res.Success = true;
                                res.Data = shopshopList.FirstOrDefault().id_shop_father;
                            }
                            else
                            {
                                res.Success = false;
                                res.Message.Clear();
                                res.Message.Add("此门店已不存在或停用!");
                            }
                        }
                        else
                        {
                            res.Success = true;
                            res.Data = shop.id_shop_ps;
                        }
                    }
                    else
                    {
                        res.Success = false;
                        res.Message.Clear();
                        res.Message.Add("此门店已不存在或停用!");
                    }
                }
                if (res.Success)
                {
                    param.Clear();
                    param.Add("id", string.Format("{0}",res.Data));
                    param.Add("id_masteruser",id_user_master);
                    param.Add("flag_state", (int)Enums.ShopShopFlagState.Use);
                    param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                    var psShop= BusinessFactory.Tb_Shop.Get(param).Data as Tb_Shop;
                    if (psShop == null)
                    {
                        res.Success = false;
                        res.Message.Clear();
                        res.Message.Add("未查询到配送门店!");
                    }
                    else
                    {
                        res.Data = new {id_shop=psShop.id,mc=psShop.mc};
                    }
                }
                return res;
            });
            return JsonString(br);
        }
        /// <summary>
        /// 根据传入的门店ID查询下一层级门店
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult GetSubShop(string id)
        {
            if (!id.IsEmpty())
            {
                Hashtable param = new Hashtable();
                param.Add("id_masteruser", id_user_master);
                param.Add("id_shop_father_self", id);
                param.Add("tb_shop_state", (int)Enums.ShopShopFlagState.Use);
                param.Add("tb_shop_delete", (int)Enums.FlagDelete.NoDelete);
                var list = BusinessFactory.Tb_Shop_Shop.GetAll(param).Data as List<Tb_Shop_ShopWithMc>;
                if (list != null)
                {
                    var backlist = from l in list
                                   select new
                                   {
                                       id_shop = l.id_shop_child,
                                       mc = l.mc,
                                       bm = l.bm,
                                       flag_type = l.flag_type
                                   };
                    return JsonString(new { Success = true, Data = backlist, Message = new List<string>() });
                }
            }
            return JsonString(new { Success = true, Data = new List<Tb_Shop>(), Message = new List<string>() });
        }

        /// <summary>
        /// 根据传入的门店ID查询下一层级门店以及本身
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult GetSubShopAndSelf(string id)
        {
            if (!id.IsEmpty())
            {
                Hashtable param = new Hashtable();
                param.Add("id_masteruser", id_user_master);
                param.Add("id_shop_father_self", id);
                param.Add("tb_shop_state", (int)Enums.ShopShopFlagState.Use);
                param.Add("tb_shop_delete", (int)Enums.FlagDelete.NoDelete);
                var list = BusinessFactory.Tb_Shop_Shop.GetAll(param).Data as List<Tb_Shop_ShopWithMc>;
                if (list != null)
                {
                    var backlist = from l in list
                                   select new
                                   {
                                       id_shop = l.id_shop_child,
                                       mc = l.mc,
                                       bm = l.bm,
                                       flag_type = l.flag_type
                                   };
                    return JsonString(new { Success = true, Data = backlist, Message = new List<string>() });
                }
            }
            return JsonString(new { Success = true, Data = new List<Tb_Shop>(), Message = new List<string>() });
        }

        /// <summary>
        /// 获取配送中心下的所有门店
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult GetPzxSubShop(string id)
        {
            if (!id.IsEmpty())
            {
                Hashtable param = new Hashtable();
                param.Add("id_masteruser", id_user_master);
                param.Add("id_shop_ps", id);
                param.Add("flag_state", (int)Enums.ShopShopFlagState.Use);
                param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                var list = BusinessFactory.Tb_Shop.QueryShopListWithFatherId(param).Data as List<Tb_ShopWithFatherId>;
                if (list != null)
                {
                    var backlist = from l in list
                                   select new
                                   {
                                       id_shop = l.id,
                                       mc = l.mc,
                                       bm = l.bm,
                                       flag_type = l.flag_type,
                                       id_shop_father = l.id_shop_father
                                   };
                    return JsonString(new { Success = true, Data = backlist, Message = new List<string>() });
                }
            }
            return JsonString(new { Success = true, Data = new List<Tb_Shop>(), Message = new List<string>() });
        }


        /// <summary>
        /// 获取带有上下级关系的门店信息
        /// </summary>
        /// <param name="id">如何有值，返回这个ID的下一级节点</param>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult GetTreeShop(string id)
        {
            Hashtable param=new Hashtable();
            param.Add("id_masteruser",id_user_master);
            param.Add("tb_shop_state", 1);
            param.Add("tb_shop_delete", 0);
            if (!id.IsEmpty())
            {
                param.Add("id_shop_father", id);
            }
            var shopList = BusinessFactory.Tb_Shop_Shop.GetAll(param).Data as List<Tb_Shop_ShopWithMc>;
            if (shopList!=null&&shopList.Any())
            {
                var res= from s in shopList
                select new
                {
                    id = s.id_shop_child,
                    parent = s.id_shop_father== "0" ? "#" : s.id_shop_father,
                    flag_type = s.flag_type,
                    text = s.mc
                };
                return JsonString(new {Success=true,Data=res,Message=new List<string>()});
            }
            return JsonString(new { Success = true, Data = new List<string>(), Message = new List<string>() });
        }
        /// <summary>
        /// 查询用户管理的所有客户
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult GetKh()
        { 
            var list = GetUserKh("1");
            if (list != null)
            {
                var backlist = from l in list
                               select new
                               {
                                   mc=l.mc,
                                   id_kh=l.id
                               };
                return JsonString(new { Success = true, Data = backlist, Message = new List<string>() });
            }
            return JsonString(new { Success = true, Data = new List<Tb_Shop>(), Message = new List<string>() });
        }

    }
}

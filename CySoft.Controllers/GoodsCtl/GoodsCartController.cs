using System;
using System.Collections.Generic;
using CySoft.Controllers.Filters;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Model.Td;
using CySoft.Utility;

#region 购物车
#endregion

namespace CySoft.Controllers.GoodsCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class GoodsCartController : BaseController
    {
        /// <summary>
        ///  添加
        ///  znt 2015-03-20
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult Save(string goods)
        {
            BaseResult br = new BaseResult();
            if (string.IsNullOrEmpty(goods) || goods == "[]")
            {
                br.Success = false;
                br.Message.Add("操作失败！请刷新页面后再试。");
                br.Level = ErrorLevel.Warning;
                return Json(br);
            }
            try
            {
               List<Td_Sale_Cart> list =JSON.Deserialize<List<Td_Sale_Cart>>(goods);
               if(list.Count<0)
               {
                   if (list[0].id_gys < 1)
                   {
                       br.Success = false;
                       br.Message.Add("操作失败！请刷新页面后再试。");
                       br.Level = ErrorLevel.Warning;
                       return Json(br);
                   }
                   if (list[0].id_sp < 1)
                   {
                       br.Success = false;
                       br.Message.Add("操作失败！请刷新页面后再试。");
                       br.Level = ErrorLevel.Warning;
                       return Json(br);
                   }
                   if (list[0].id_sku < 1)
                   {
                       br.Success = false;
                       br.Message.Add("操作失败！请刷新页面后再试。");
                       br.Level = ErrorLevel.Warning;
                       return Json(br);
                   }
                   if (list[0].sl < 1)
                   {
                       br.Success = false;
                       br.Message.Add("操作失败！数量必须大于0。");
                       br.Level = ErrorLevel.Warning;
                       return Json(br);
                   }
               }
               foreach (Td_Sale_Cart item in list)
               {
                   item.id_cgs = GetLoginInfo<long>("id_buyer");
                   item.id_user = GetLoginInfo<long>("id_user");
                   br = BusinessFactory.GoodsCart.Save(item);
               }
               Hashtable param = new Hashtable();
               param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
               param.Add("id_user", GetLoginInfo<long>("id_user"));
               BaseResult br1 = BusinessFactory.GoodsCart.GetCount(param);
               br.Data = br1.Data;
               
            }
            catch (CySoftException ex)
            {

                throw ex;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            return Json(br);
        }

        /// <summary>
        /// 购物车列表 待定
        /// znt 2015-03-14  
        /// </summary>
        [ActionPurview(false)]
        public ActionResult List()
        {
            BaseResult br = new BaseResult();
            try
            {
                long id_buyer = GetLoginInfo<long>("id_buyer");
                Hashtable param = new Hashtable();
                param.Add("id_cgs", id_buyer);
                param.Add("flag_cart", 1);
                br = BusinessFactory.Supplier.GetAll(param);

                param.Clear();
                param.Add("id_cgs", id_buyer);
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                BaseResult cartBr = BusinessFactory.GoodsCart.GetAll(param);
                ViewData["cartList"] = cartBr.Data;
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(br.Data);

        }

        /// <summary>
        /// 删除 购物车记录 
        /// znt 2015-03-16
        ///  2015-03-14 lxt 修改
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult Delete()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("skuList", String.Empty, HandleType.ReturnMsg);
                p.Add("id_gys", (long)0, HandleType.ReturnMsg);
                param = param.Trim(p);
                if (param.ContainsKey("skuList"))
                {
                    string[] split = param["skuList"].ToString().Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    param.Remove("skuList");
                    long[] id_skuList = new long[split.Length];
                    for (int i = 0; i < split.Length;i++ )
                    {
                        id_skuList[i] = Convert.ToInt64(split[i].Trim());
                    }
                    param["id_skuList"] = id_skuList;
                }
                long id_buyer = GetLoginInfo<long>("id_buyer");
                long id_user = GetLoginInfo<long>("id_user");
                param.Add("id_cgs", id_buyer);
                param.Add("id_user", id_user);
                br = BusinessFactory.GoodsCart.Delete(param);
                if (br.Success)
                {
                    param.Clear();
                    param.Add("id_cgs", id_buyer);
                    param.Add("id_user", id_user);
                    BaseResult br1 = BusinessFactory.GoodsCart.GetCount(param);
                    br.Data = br1.Data;
                }
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Json(br);
        }

        /// <summary>
        /// 清空
        /// lxt
        /// 2015-03-30
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult Clear()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_gys", (long)0, HandleType.Remove);
                param = param.Trim(p);
                long id_buyer = GetLoginInfo<long>("id_buyer");
                long id_user = GetLoginInfo<long>("id_user");
                param.Add("id_cgs", id_buyer);
                param.Add("id_user", id_user);
                br = BusinessFactory.GoodsCart.Delete(param);
                if (br.Success)
                {
                    param.Clear();
                    param.Add("id_cgs", id_buyer);
                    param.Add("id_user", id_user);
                    BaseResult br1 = BusinessFactory.GoodsCart.GetCount(param);
                    br.Data = br1.Data;
                }
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(br);
        }

        /// <summary>
        ///  购物车_添加
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult SaveCart()
        {
            Hashtable param = GetParameters();
            BaseResult br = new BaseResult();

            ParamVessel p = new ParamVessel();
            p.Add("id_gys", (long) 0, HandleType.ReturnMsg);
            p.Add("id_sp", (long) 0, HandleType.ReturnMsg);
            p.Add("id_sku", (long) 0, HandleType.ReturnMsg);
            p.Add("sl", (long) 0, HandleType.ReturnMsg);
            param = param.Trim(p);
            try
            {
                Td_Sale_Cart model = new Td_Sale_Cart()
                {
                    id_cgs = GetLoginInfo<long>("id_buyer"),
                    id_user = GetLoginInfo<long>("id_user"),

                    id_gys = Convert.ToInt32(param["id_gys"]),
                    id_sku = Convert.ToInt32(param["id_sku"]),
                    id_sp = Convert.ToInt32(param["id_sp"]),
                    sl = Convert.ToDecimal(param["sl"]),
                };

                br = BusinessFactory.GoodsCart.Save(model);
                if (br.Success)
                {
                    param.Clear();
                    param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                    param.Add("id_user", GetLoginInfo<long>("id_user"));
                    BaseResult br1 = BusinessFactory.GoodsCart.GetCount(param);
                    br.Data = br1.Data;

                }
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch
                (Exception ex)
            {

                throw ex;
            }
            return Json(br);
            }
        }

    
}

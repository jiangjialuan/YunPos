using System;
using System.Collections;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Controllers.Service.Base;
using CySoft.Frame.Core;
using CySoft.Model.Td;
using CySoft.Utility;

#region 购物车
#endregion

namespace CySoft.Controllers.GoodsCtl
{
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceGoodsCartController : ServiceBaseController
    {
        /// <summary>
        ///  添加
        ///  lxt
        ///  2015-04-02
        /// </summary>
        [HttpPost]
        public ActionResult Save(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Td_Sale_Cart model = JSON.Deserialize<Td_Sale_Cart>(obj);
                if (model.id_gys < 1)
                {
                    br.Success = false;
                    br.Message.Add("操作失败！提交的数据不完整。");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                if (model.id_sp < 1)
                {
                    br.Success = false;
                    br.Message.Add("操作失败！提交的数据不完整。");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                if (model.id_sku < 1)
                {
                    br.Success = false;
                    br.Message.Add("操作失败！提交的数据不完整。");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                if (model.sl < 1)
                {
                    br.Success = false;
                    br.Message.Add("操作失败！数量必须大于0。");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                model.id_cgs = GetLoginInfo<long>("id_buyer");
                model.id_user = GetLoginInfo<long>("id_user");
                br = BusinessFactory.GoodsCart.Save(model);
                if (br.Success)
                {
                    Hashtable param = new Hashtable();
                    param.Add("id_cgs", model.id_cgs);
                    param.Add("id_user", model.id_user);
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
        /// 删除
        /// lxt
        /// 2015-04-02
        /// </summary>
        [HttpPost]
        public ActionResult Delete(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
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
        public ActionResult Clear(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
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
        /// 购物车列表
        /// znt 2015-04-29
        /// </summary>
        /// <returns></returns>
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
                long id_user = GetLoginInfo<long>("id_user");
                param.Clear();
                param.Add("id_cgs", id_buyer);
                param.Add("id_user", id_user);
                param.Add("baseurl", Request.Url.Scheme + "://" + Request.Url.Authority);
                BaseResult cartBr = BusinessFactory.GoodsCart.GetAll(param);
                br.Data = new { cupplier = br.Data, cart = cartBr.Data };
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
    }
}

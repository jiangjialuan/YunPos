using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Controllers.Mobile.Base;
using CySoft.Frame.Core;
using CySoft.Model.Td;
using CySoft.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CySoft.Controllers.Mobile
{
    public class MobileGoodsCartController : MobileBaseController
    {
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        ///  添加
        ///  wwc 2016-07-21
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
                List<Td_Sale_Cart> list = JSON.Deserialize<List<Td_Sale_Cart>>(goods);
                if (list.Count < 0)
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
    }
}

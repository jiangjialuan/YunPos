using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Controllers.Service.Base;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Frame.Core;
using CySoft.Utility;
using System.Collections;
using CySoft.Controllers.Base;
using CySoft.Controllers.GoodsCtl;
using CySoft.Model.Td;
using CySoft.Model.Flags;
using CySoft.Model.Ts;
using CySoft.Model.Other;
using CySoft.Model.Tb;

#region 订货单接口
#endregion

namespace CySoft.Controllers.Service
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceOrderController : ServiceBaseController
    {

        /// <summary>
        /// 新增订货单
        /// znt 2015-04-21
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(string obj)
        {
            BaseResult br = new BaseResult();

            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                ParamVessel p = new ParamVessel();
                p.Add("orderData", String.Empty, HandleType.ReturnMsg); // 订单来源
                p.Add("invoiceFlag", String.Empty, HandleType.ReturnMsg); // 发票类型
                p.Add("soure", (long)3, HandleType.DefaultValue); // 订单来源 App端 购物车
                param = param.Trim(p);

                Td_Sale_Order_Head_Query model = JSON.Deserialize<Td_Sale_Order_Head_Query>(param["orderData"].ToString());
                if (string.IsNullOrEmpty(model.id_gys.ToString()))
                {
                    br.Success = false;
                    br.Message.Add("校验失败！请重新刷新页面。");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "none";
                    return Json(br);
                }

                if (model.order_body.Count <= 0)
                {
                    br.Success = false;
                    br.Message.Add("商品不能为空！");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "sku";
                    return Json(br);
                }
                if (string.IsNullOrEmpty(model.shr))
                {
                    br.Success = false;
                    br.Message.Add("收货人不能为空！");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "shr";
                    return Json(br);
                }
                if (string.IsNullOrEmpty(model.phone))
                {
                    br.Success = false;
                    br.Message.Add("联系号码不能为空！");
                    br.Level = ErrorLevel.Warning;
                    br.Data = "phone";
                    return Json(br);
                }

                // 单号 赋值 
                br = BusinessFactory.Utilety.GetNextDH(model, typeof(Td_Sale_Order_Head));

                if (!br.Success)
                {
                    return Json(br);
                }

                // 发票类型 
                string invoiceFlag = param["invoiceFlag"].ToString();
                switch (invoiceFlag)
                {
                    case "2":
                        model.invoiceFlag = InvoiceFlag.General;
                        break;
                    case "3":
                        model.invoiceFlag = InvoiceFlag.Vat;
                        break;
                    default:
                        model.invoiceFlag = InvoiceFlag.None;
                        break;
                }

                // 订单来源
                int soure = Convert.ToInt32(param["soure"]);

                model.id_cgs = GetLoginInfo<long>("id_buyer");
                model.id_user_bill = GetLoginInfo<long>("id_user");
                model.id_user_master = GetLoginInfo<long>("id_user_master");

                model.id_create = GetLoginInfo<long>("id_user");
                model.id_edit = GetLoginInfo<long>("id_user");

                //订单入参
                Hashtable param2 = new Hashtable();
                param2.Clear();
                param2.Add("id_user", GetLoginInfo<long>("id_user"));
                param2.Add("model", model);

                switch (soure)
                {
                    case (int)OrderSourceFlag.AppClone:
                        param2.Add("OrderSource", OrderSourceFlag.AppClone);
                        break;
                    default:
                        param2.Add("OrderSource", OrderSourceFlag.AppCart);
                        break;
                }

                br = BusinessFactory.Order.Add(param2);

                if (br.Success)
                {
                    br.Data = model.dh;
                    WriteDBLog(LogFlag.Bill, br.Message);
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
        ///  获取填写订单页面信息
        ///  znt 
        /// </summary>
        [HttpPost]
        public ActionResult GetItem(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                ParamVessel p = new ParamVessel();
                p.Add("id_gys", (long)0, HandleType.ReturnMsg); // 订单来源
                param = param.Trim(p);

                long id_gys = Convert.ToInt64(param["id_gys"]);
                long id_cgs = GetLoginInfo<long>("id_buyer");

                Td_Sale_Order_Head_Query model = new Td_Sale_Order_Head_Query();

                model.id_gys = id_gys;
                model.id_cgs = id_cgs;

                #region 清单列表
                param.Clear();
                param.Add("id_gys", id_gys);
                param.Add("id_cgs", id_cgs);
                param.Add("baseurl", Request.Url.Scheme + "://" + Request.Url.Authority);
                br = BusinessFactory.GoodsCart.GetAll(param);
                if (br.Data != null)
                {
                    List<Td_Sale_Cart_Query> list = br.Data as List<Td_Sale_Cart_Query>;
                    model.order_body = new List<Td_Sale_Order_Body_Query>();
                    foreach (var item in list)
                    {
                        Td_Sale_Order_Body_Query body = new Td_Sale_Order_Body_Query();

                        body.dj = item.dj;
                        body.dj_base = item.dj_base;
                        body.bm = item.bm_Interface;
                        body.formatname = string.Format("{1}【{2}】", item.bm_Interface, item.mc, item.gg.TrimEnd(','));
                        body.id_sku = item.id_sku.Value;
                        body.id_sp = item.id_sp.Value;
                        body.sl = item.sl.Value;
                        body.sl_dh_min = item.sl_dh_min;
                        body.unit = item.unit;
                        body.photo = item.photo;
                        model.je_hs += item.dj * item.sl.Value;
                        model.je_pay += item.dj * item.sl.Value;
                        model.order_body.Add(body);
                    }

                }
                #endregion

                #region 供应商发票类型 / 采购商发票信息

                //param.Clear();
                //param.Add("id", id_gys);
                //br = BusinessFactory.Supplier.Get(param);

                //Tb_Gys_Edit tb_gys = br.Data as Tb_Gys_Edit;
                //if (tb_gys != null)
                //{

                //    model.tax = tb_gys.tax; // 普通税
                //    model.vat = tb_gys.vat; // 增值税
                //}

                param.Clear();
                param.Add("id", id_cgs);
                br = BusinessFactory.Buyer.Get(param);
                Tb_Cgs tb_cgs = br.Data as Tb_Cgs;
                if (!br.Success || tb_cgs == null)
                {
                    br.Success = false;
                    br.Message.Add("采购商不存在！");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                model.title_invoice = tb_cgs.title_invoice;
                model.account_bank = tb_cgs.account_bank;
                model.name_bank = tb_cgs.name_bank;
                model.no_tax = tb_cgs.no_tax;
                #endregion

                #region 获取 采购商收货地址
                br = BusinessFactory.Buyer.RecieverAddress(Convert.ToInt32(id_cgs));
                List<Tb_Cgs_Shdz_Query> list_shdz = br.Data as List<Tb_Cgs_Shdz_Query>;
                if (list_shdz != null && list_shdz.Count > 0)
                {

                    Tb_Cgs_Shdz_Query Shdz = list_shdz.Where(m => m.flag_default == YesNoFlag.Yes).FirstOrDefault();
                    if (Shdz != null)
                    {
                        model.shr = Shdz.shr;
                        model.phone = Shdz.phone;
                        model.id_province = Shdz.id_province;
                        model.id_city = Shdz.id_city;
                        model.id_county = Shdz.id_county;
                        model.address = Shdz.address;
                        model.province_name = Shdz.province_name;
                        model.city_name = Shdz.city_name;
                        model.county_name = Shdz.county_name;
                    }
                    else
                    {
                        if (list_shdz.Count == 1)
                        {
                            Shdz = list_shdz.FirstOrDefault();

                            model.shr = Shdz.shr;
                            model.phone = Shdz.phone;
                            model.id_province = Shdz.id_province;
                            model.id_city = Shdz.id_city;
                            model.id_county = Shdz.id_county;
                            model.address = Shdz.address;
                            model.province_name = Shdz.province_name;
                            model.city_name = Shdz.city_name;
                            model.county_name = Shdz.county_name;
                        }
                    }
                }
                #endregion

                br.Success = true;
                br.Data = model;

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
        /// 获取供应商订单参数
        /// tim 
        /// 2015-07-30
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetOrderParams(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                ParamVessel p = new ParamVessel();
                p.Add("id_gys", String.Empty, HandleType.ReturnMsg);// 供应商ID
                param = param.Trim(p);
                var id_gys = param["id_gys"].ToString();
                if (id_gys.IsEmpty())
                {
                    br.Success = false;
                    br.Message.Add("供应商参数错误。");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                param.Clear();
                param.Add("id", id_gys);
                br = BusinessFactory.Supplier.Get(param);
                var tb_gys = br.Data as Tb_Gys_Edit;
                if (!br.Success || tb_gys == null)
                {
                    br.Success = false;
                    br.Message.Add("供应商不存在。");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                param.Clear();
                param.Add("id_user_master", tb_gys.id_user_master);
                br = BusinessFactory.Setting.GetAll(param);
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
        /// 采购商 订单列表
        /// znt
        /// 2015-03-27
        /// </summary>
        [HttpPost]
        public ActionResult GetPage(string obj)
        {
            PageNavigate pn = new PageNavigate();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                ParamVessel p = new ParamVessel();
                p.Add("id_cgs", GetLoginInfo<long>("id_buyer"), HandleType.DefaultValue);//采购商Id
                p.Add("id_gys", (long)0, HandleType.Remove);//供应商Id
                p.Add("orderType", 0, HandleType.Remove);//订单类别 默认是订货单（0 全部，1 订货单，2 退货单）  
                p.Add("start_rq_create", String.Empty, HandleType.Remove); // 开始时间
                p.Add("end_rq_create", String.Empty, HandleType.Remove); // 结束时间
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("limit", 20, HandleType.DefaultValue);//当前页码
                p.Add("flag_delete", (long)0, HandleType.DefaultValue); // 默认 未删除状态
                p.Add("sort", "rq_create", HandleType.DefaultValue);
                p.Add("dir", "desc", HandleType.DefaultValue);
                param = param.Trim(p);

                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                if (pageIndex < 1)
                {
                    pageIndex = 1;
                }
                int limit = Convert.ToInt32(param["limit"]);
                if (limit < 1)
                {
                    limit = 20;
                }
                if (limit > 200)
                {
                    limit = 200;
                }

                param.Add("start", (pageIndex - 1) * limit);
                //param.Add("baseurl", Request.Url.Scheme + "://" + Request.Url.Authority);
                pn = BusinessFactory.Order.GetPage(param);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(pn);
        }

        /// <summary>
        /// 清单列表
        /// znt
        /// 2015-04-20
        /// </summary>
        [HttpPost]
        public ActionResult DetailedList(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.Remove);// 订货单号
                param = param.Trim(p);
                param.Add("baseurl", Request.Url.Scheme + "://" + Request.Url.Authority);
                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                if (pageIndex < 1)
                {
                    pageIndex = 1;
                }
                int limit = Convert.ToInt32(param["limit"]);
                if (limit < 1)
                {
                    limit = 20;
                }
                if (limit > 200)
                {
                    limit = 200;
                }

                br = BusinessFactory.Order.GetAll(param);
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
        /// 备注信息
        /// znt
        /// 2015-04-20
        /// </summary>
        [HttpPost]
        public ActionResult RemarkList(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                // 待定
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
        /// 复制订单
        /// znt 2015-04-22
        /// </summary>
        [HttpPost]
        public ActionResult OrderClone(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);
                p.Add("id_user", GetLoginInfo<long>("id_user"), HandleType.DefaultValue);
                p.Add("id_cgs", GetLoginInfo<long>("id_buyer"), HandleType.DefaultValue);
                param = param.Trim(p);

                long id_gys = Convert.ToInt64(param["id_gys"]);
                long id_cgs = GetLoginInfo<long>("id_buyer");

                br = BusinessFactory.Order.AppOrderClone(param);

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
        /// 操作日志
        /// znt
        /// 2015-04-20
        /// </summary>
        [HttpPost]
        public ActionResult LogList(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                br = BusinessFactory.OrderLog.GetAll(param);
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

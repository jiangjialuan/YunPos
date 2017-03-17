using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Controllers.Service.Base;
using System.Web.UI;
using CySoft.Utility;
using CySoft.Controllers.Base;
using CySoft.Model.Td;
using CySoft.Model.Flags;
using CySoft.Model.Ts;
using CySoft.Model.Other;
using CySoft.Model.Tb;

#region 销售订单接口
#endregion

namespace CySoft.Controllers.Service
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceSaleOrderController : ServiceBaseController
    {
        /// <summary>
        /// 详情
        /// znt 2015-05-05
        /// </summary>
        [HttpPost]
        public ActionResult Item(string obj)
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
                br = BusinessFactory.Order.Get(param);
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
        /// 代客新增订货单
        /// tim
        /// 2015-07-29
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
                param = param.Trim(p);

                Td_Sale_Order_Head_Query model = JSON.Deserialize<Td_Sale_Order_Head_Query>(param["orderData"].ToString());
                if (string.IsNullOrEmpty(model.id_cgs.ToString()))
                {
                    br.Success = false;
                    br.Message.Add("请选择客户下单。");
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

                model.id_gys = GetLoginInfo<long>("id_supplier");

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


               
                model.id_user_bill = GetLoginInfo<long>("id_user");
                model.id_user_master = GetLoginInfo<long>("id_user_master");

                model.id_create = GetLoginInfo<long>("id_user");
                model.id_edit = GetLoginInfo<long>("id_user");

                param.Clear();
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param.Add("model", model);

                br = BusinessFactory.SaleOrder.Add(param);
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
        /// 供应商 订单列表
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
                p.Add("id_gys", GetLoginInfo<long>("id_supplier"), HandleType.DefaultValue);//供应商Id
                p.Add("orderType", 0, HandleType.Remove);//订单类别 默认是订货单 （0 全部，1 订货单，2 退货单）  
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
        /// 订单/销售单作废 cxb 2015-5-22
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Invalid(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);
                p.Add("remark", String.Empty, HandleType.ReturnMsg);
                p.Add("id_supplier",String.Empty,HandleType.ReturnMsg);
                param = param.Trim(p);
                param["newOrderLogId"] = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
                //param.Add("id_supplier", GetLoginInfo<long>("id_supplier"));
                br = BusinessFactory.SaleOrder.Invalid(param);
                if (br.Success)
                {
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
                br.Success = true;
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
        /// 订单审核
        /// znt 2015-05-05
        /// </summary>
        [HttpPost]
        public ActionResult CheckOrder(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);// 订货单号
                p.Add("jsonarray", String.Empty, HandleType.Remove);
                param = param.Trim(p);

                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param["newOrderLogId"] = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));

                br = BusinessFactory.SaleOrder.CheckOrder(param);

                if (br.Success)
                {
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
        /// 财务审核
        /// znt 2015-04-17
        /// </summary>
        [HttpPost]
        public ActionResult CheckFinance(string obj)
        {
            BaseResult br = new BaseResult();

            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);// 订货单号
                param = param.Trim(p);

                param.Add("id_user", GetLoginInfo<long>("id_user"));
                param["newOrderLogId"] = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                br = BusinessFactory.SaleOrder.CheckFinance(param);

                if (br.Success)
                {
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
        /// 金额数量总计
        /// znt 2015-05-06
        /// </summary>
        [HttpPost]
        public ActionResult GysHomeTotal(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                param.Clear();
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                br = BusinessFactory.SaleOrder.GysHomeTotal(param);
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
        /// 金额数量总计
        /// znt 2015-05-06
        /// 已停用，接口调用方法写错，暂时保留给旧的调用方使用，新的接口使用GysHomeTotal
        /// tim 2015-11-4
        /// </summary>
        [HttpPost]
        public ActionResult PayTotal(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                ParamVessel p = new ParamVessel();
                p.Add("id_gys", string.Empty, HandleType.Remove);
                p.Add("id_cgs", string.Empty, HandleType.Remove);
                p.Add("flag_delete", 0, HandleType.DefaultValue);
                p.Add("not_flag_state", 0, HandleType.DefaultValue);
                p.Add("start_rq_create", string.Empty, HandleType.Remove);
                p.Add("end_rq_create", string.Empty, HandleType.Remove);
                param = param.Trim(p);
                if (param.ContainsKey("start_rq_create") && !string.IsNullOrWhiteSpace(param["start_rq_create"].ToString()) && param.ContainsKey("end_rq_create") && !string.IsNullOrWhiteSpace(param["end_rq_create"].ToString()))
                {
                    var start = param["start_rq_create"].ToString();
                    var end = param["end_rq_create"].ToString();
                    try
                    {
                        if (DateTime.Compare(DateTime.Parse(start), DateTime.Parse(end)) > 0)
                        {
                            //调用者参数提交错了，这里更正
                            param["start_rq_create"] = end;
                        }
                    }
                    catch { }
                }
                br = BusinessFactory.SaleOrder.PayTotal(param);
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

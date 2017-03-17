using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Controllers.Service.Base;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Frame.Core;
using CySoft.Model.Td;
using CySoft.Utility;
using CySoft.Controllers.Base;
using CySoft.Model.Flags;
using System.Collections;
using CySoft.Model.Other;

#region 供应商付款
#endregion
namespace CySoft.Controllers.Service
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceSalePayController : ServiceBaseController
    {
        /// <summary>
        ///  添加付款
        ///  znt 2015-05-06
        /// </summary>
        [HttpPost]
        public ActionResult Add(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Td_Sale_Pay model = JSON.Deserialize<Td_Sale_Pay>(obj);

                if (model.dh_order.IsEmpty())
                {
                    br.Success = false;
                    br.Message.Add(string.Format("订单号校验失败，请重新刷新页面。"));
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                if (model.rq_create.Value == null)
                {
                    br.Success = false;
                    br.Message.Add(string.Format("付款日期不能为空。"));
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }

                br = BusinessFactory.Utilety.GetNextDH(model, typeof(Td_Sale_Pay));
                if (!br.Success)
                {
                    return Json(br);
                }

                switch (model.flag_pay)
                {
                    case "onlink":      // 线上付款
                        break;
                    case "platform":    // 平台付款
                        break;
                    default:            // 默认线下付款
                        model.dh_pay = "";
                        break;
                }
                model.id_create = GetLoginInfo<long>("id_user");
                model.id_edit = GetLoginInfo<long>("id_user");
                br = BusinessFactory.Funds.PayForGys(model);
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
        /// 订单 收款/付款记录
        /// znt 2015-05-06
        /// </summary>
        [HttpPost]
        public ActionResult List(string obj)
        {
            BaseResult br = new BaseResult();
            Query_Pay model = new Query_Pay();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                ParamVessel p = new ParamVessel();
                p.Add("dh_order", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);

                string dh = param["dh_order"].ToString();

                param.Add("sort", "rq_create");
                param.Add("dir", "desc");
                List<Td_sale_pay_Query> list = BusinessFactory.Funds.GetAll(param).Data as List<Td_sale_pay_Query>;
                model.listPay = list;

                param.Clear();
                param.Add("dh", dh);
                Td_Sale_Order_Head_Query head = BusinessFactory.Order.Get(param).Data as Td_Sale_Order_Head_Query;
                if (head != null)
                {
                    model.je_pay = head.je_pay.Value;
                    model.je_payed = head.je_payed.Value;
                    model.je_nopay = head.je_pay - head.je_payed <= 0 ? (decimal)0.00 : (head.je_pay - head.je_payed).Value; 
                }

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
        /// 确认付款
        /// znt 2015-05-08
        /// </summary>
        [HttpPost]
        public ActionResult Confirm( string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);

                param.Add("id_edit", GetLoginInfo<long>("id_user"));
                br = BusinessFactory.Funds.PayConfirm(param);

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
        /// 取消收款记录 
        /// znt 2015-04-29
        /// </summary>
        [HttpPost]
        public ActionResult CancelRecord(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                ParamVessel p = new ParamVessel();
                p.Add("dh", string.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);

                param.Add("id_edit", GetLoginInfo<long>("id_user"));
                br = BusinessFactory.Funds.CancelRecord(param);
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
        /// 删除未付款记录
        /// znt 2015-05-11
        /// </summary>
        [HttpPost]
        public ActionResult Delete(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                ParamVessel p = new ParamVessel();
                p.Add("dh", string.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);

                br = BusinessFactory.Funds.Remove(param);
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

    }
}

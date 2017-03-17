using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CySoft.Controllers.Filters;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Model.Td;

namespace CySoft.Controllers.OrderCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class SaleOrderPayController : BaseController
    {
        /// <summary>
        /// 收款记录详情列表
        /// znt 2015-04-29
        /// </summary>
        public ActionResult List(string id)
        {
            List<Td_sale_pay_Query> model = new List<Td_sale_pay_Query>();
            try
            {
                Hashtable param = new Hashtable();
                param.Add("dh", id);
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty ,HandleType.ReturnMsg);
                param = param.Trim(p);
                string dh = id;
                ViewData["dh"] = dh; // 销售单号
                ViewData["je_pay"] ="0.00"; // 应付金额
                ViewData["je_payed"] ="0.00"; // 已付金额
                ViewData["je_nopay"] = "0.00"; // 尚未付款

                param.Clear();
                param.Add("dh",dh);
                Td_Sale_Order_Head_Query head = BusinessFactory.Order.Get(param).Data as Td_Sale_Order_Head_Query;
                if (head != null)
                {
                    ViewData["je_pay"] = head.je_pay;
                    ViewData["je_payed"] = head.je_payed;
                    ViewData["je_nopay"] = head.je_pay - head.je_payed < 0 ? "0.00" : (head.je_pay - head.je_payed).Value.ToString(); // 已付金额
                }

                #region 收款记录

                param.Clear();
                param.Add("dh_order", dh);
                param.Add("flag_delete",0); // 未删除
                model = BusinessFactory.Funds.GetAll(param).Data as List<Td_sale_pay_Query>;

                #endregion

            }
            catch (CySoftException ex)
            {

                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View(model);
        }
    }
}

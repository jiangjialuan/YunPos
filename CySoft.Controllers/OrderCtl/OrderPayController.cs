//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web.Mvc;
//using CySoft.Controllers.Filters;
//using System.Web.UI;
//using CySoft.Controllers.Base;
//using CySoft.Model.Td;
//using System.Collections;
//using CySoft.Frame.Core;
//using CySoft.Model.Tb;
//using CySoft.Utility;
//using CySoft.Model.Pay;

//namespace CySoft.Controllers.OrderCtl
//{
//    [LoginActionFilter]
//    [OutputCache(Location = OutputCacheLocation.None)]
//    public class OrderPayController : BaseController
//    {
//        /// <summary>
//        /// 付款记录详情列表
//        /// znt 2015-04-29
//        /// </summary>
//        public ActionResult List(string id)
//        {
//            List<Td_sale_pay_Query> model = new List<Td_sale_pay_Query>();
//            Hashtable param = new Hashtable();
//            param.Add("dh", id);
//            try
//            {
//                ParamVessel p = new ParamVessel();
//                p.Add("dh", String.Empty, HandleType.ReturnMsg);
//                param = param.Trim(p);
//                string dh = id;
//                ViewData["dh"] = dh; // 订货单号
//                ViewData["id_gys"] = (long)0;
//                ViewData["je_pay"] = "0.00"; // 应付金额
//                ViewData["je_payed"] = "0.00"; // 已付金额
//                ViewData["je_nopay"] = "0.00"; // 尚未付款
//                ViewData["isSettle"] = false;  // 是否已结清付款

//                ViewData["khr"]=string.Empty; // 开户人
//                ViewData["name_bank"]=string.Empty; ; // 开户银行
//                ViewData["account_bank"] = string.Empty; // 银行账号

//                param.Clear();
//                param.Add("dh", dh);
//                Td_Sale_Order_Head_Query head = BusinessFactory.Order.Get(param).Data as Td_Sale_Order_Head_Query;
//                if (head != null)
//                {
//                    ViewData["id_gys"] = head.id_gys;
//                    ViewData["je_pay"] = head.je_pay;
//                    ViewData["je_payed"] = head.je_payed;
//                    ViewData["je_nopay"] = head.je_pay - head.je_payed <= 0 ? "0.00" : (head.je_pay - head.je_payed).Value.ToString(); // 已付金额
//                    ViewData["isSettle"] = head.je_pay - head.je_payed <= 0 ? true : false;
//                }


//                if (!(bool)ViewData["isSettle"] && head != null)
//                {
//                    param.Clear();
//                    param.Add("id_gys", head.id_gys);
//                    Tb_Gys_Account Bank = BusinessFactory.BankAccount.GetDefault(param).Data as Tb_Gys_Account;
//                    if (Bank != null)
//                    {
//                        ViewData["khr"] = Bank.khr;
//                        ViewData["name_bank"] = Bank.name_bank;
//                        ViewData["account_bank"] = Bank.account_bank;
//                    }
//                }

//                #region 收款记录

//                param.Clear();
//                param.Add("dh_order", dh);
//                param.Add("flag_delete", 0); // 未删除
//                model = BusinessFactory.Funds.GetAll(param).Data as List<Td_sale_pay_Query>;

//                #endregion

//                #region 判断供应商是否开通在线支付
//                param.Clear();
//                BaseResult br = new BaseResult();
//                bool yeePay = false;
//                bool wxPay = false;
//                param["id_user_master"] = head.id_user_master_gys;
//                //br = BusinessFactory.Pay.CheckPayAccount(param, "yeePay");
//                //if (br.Data != null)
//                //{

//                //}
//                br = BusinessFactory.Pay.CheckPayAccount(param, "wxPay");
//                if (br.Data != null)
//                {
//                    WeChatAccount wx = (WeChatAccount)br.Data;
//                    if (wx.flag_state == 1)
//                    {
//                        wxPay = true;
//                    }
//                }
//                string md5Gys = MD5Encrypt.Encrypt(head.id_user_master_gys.ToString());
//                ViewBag.dh = dh;
//                ViewBag.md5Gys = md5Gys;
//                ViewBag.yeePay = yeePay;
//                ViewBag.wxPay = wxPay;
//                #endregion

//            }
//            catch (CySoftException ex)
//            {

//                throw ex;
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//            return View(model);
//        }
//    }
//}

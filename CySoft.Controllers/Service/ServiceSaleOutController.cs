using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Td;
using CySoft.Model.Ts;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;
using CySoft.Model.Flags;
using System.Linq;
using CySoft.Controllers.Service.Base;
using CySoft.Model.Other;

#region 销售出库单
#endregion

namespace CySoft.Controllers.SaleCtl
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceSaleOutController : ServiceBaseController
    {
        /// <summary>
        /// 出库/出货记录 cxb 2015-3-3 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult List()
        {
            PageNavigate pn = new PageNavigate();
            int limit = 10;
            PageList<Td_Sale_Out_Head_Query> list = new PageList<Td_Sale_Out_Head_Query>(limit);
            try
            {
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("from", String.Empty, HandleType.Remove);//来源
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                param = param.Trim(p);

                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                ViewData["keyword"] = GetParameter("keyword");
                ViewData["orderby"] = param["orderby"];
                ViewData["pageIndex"] = pageIndex;
                ViewData["limit"] = limit;

                param.Add("limit", limit);
                param.Add("start", (pageIndex - 1) * limit);
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                pn = BusinessFactory.ShippingRecord.GetPage(param);
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
        /// 出库
        /// cxb
        /// 2015-3-20 
        /// </summary>
        [HttpPost]
        public ActionResult Out(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("head", null, HandleType.ReturnMsg);
                p.Add("body", null, HandleType.ReturnMsg);
                p.Add("ShoppingInfo", null, HandleType.Remove);
                param = param.Trim(p);
                Td_Sale_Out_Head head = JSON.ConvertToType<Td_Sale_Out_Head>(param["head"]);
                head.id_gys = GetLoginInfo<long>("id_supplier");//int.Parse(param["id_supplier"].ToString());
                List<Td_Sale_Out_Body> body = JSON.ConvertToType<List<Td_Sale_Out_Body>>(param["body"]);
                ShoppingInfo shoppinginfo = JSON.ConvertToType<ShoppingInfo>(param["ShoppingInfo"]);
                Ts_Sale_Order_Log ts = new Ts_Sale_Order_Log();//日志

                //单号 赋值 
                if (head.id_cgs <= 0)
                {
                    br.Success = false;
                    br.Message.Add("未知采购商，请刷新后再试！");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                if (head.dh_order.IsEmpty())
                {
                    br.Success = false;
                    br.Message.Add("订单号丢失，请刷新后再试！");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                if (head.id_gys <= 0)
                {
                    br.Success = false;
                    br.Message.Add("未知供应商，请刷新后再试！");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                if (body.Count < 1)
                {
                    br.Success = false;
                    br.Message.Add("<h5>参数错误</h5>");
                    br.Message.Add("没有可保存的商品！");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                int xh = 1;
                foreach (var item in body)
                {
                    if (item != null)
                    {
                        if (item.id_sku < 1)
                        {
                            br.Success = false;
                            br.Message.Add("<h5>参数错误</h5>");
                            br.Message.Add(String.Format("[代码:303]，第{0}行数据中商品id有误！", xh));
                            br.Level = ErrorLevel.Warning;
                            return Json(br);
                        }
                        if (item.sl <= 0m)
                        {
                            br.Success = false;
                            br.Message.Add("<h5>参数错误</h5>");
                            br.Message.Add(String.Format("[代码:303]，第{0}行数据中商品数量有误！", xh));
                            br.Level = ErrorLevel.Warning;
                            return Json(br);
                        }
                        //if (item.zhl <= 0m)
                        //{
                        //    br.Success = false;
                        //    br.Message.Add("<h5>参数错误</h5>");
                        //    br.Message.Add(String.Format("[代码:303]，第{0}行数据中包装数有误！", xh));
                        //    br.Level = ErrorLevel.Warning;
                        //    return Json(br);
                        //}
                        xh++;
                    }
                }
                br = BusinessFactory.Utilety.GetNextDH(head, typeof(Td_Sale_Out_Head));

                if (!br.Success)
                {
                    return Json(br);
                }
                ts.id = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                ts.id_user = GetLoginInfo<long>("id_user");
                ts.id_user_master = GetLoginInfo<long>("id_user_master");
                ts.flag_type = 4;
                head.id_create = GetLoginInfo<long>("id_user");
                head.id_edit = GetLoginInfo<long>("id_user");
                head.flag_state = CySoft.Model.Flags.OrderFlag.WaitDelivery;
                param.Clear();
                param.Add("head", head);
                param.Add("body", body);
                param.Add("Ts", ts);
                param.Add("id_user_master", GetLoginInfo<long>("id_user_master"));
                param.Add("ShoppingInfo", shoppinginfo);
                br = BusinessFactory.ShippingRecord.Add(param);

                return Json(br);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 发货
        /// cxb
        /// 2015-3-25
        /// 2015-04-16 lxt 改
        /// </summary>
        [HttpPost]
        public ActionResult Deliver(string obj)
        {

            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            DateTime dt = new DateTime(1900, 1, 1);
            Ts_Sale_Order_Log ts = new Ts_Sale_Order_Log();//日志
            try
            {
                Td_Sale_Out_Head model = JSON.Deserialize<Td_Sale_Out_Head>(obj);
                ts.id = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                ts.id_user = GetLoginInfo<long>("id_user");
                ts.id_user_master = GetLoginInfo<long>("id_user_master");
                ts.flag_type = 5;
                ts.dh = model.dh;
                ts.content = "订单已通过发货确认";
                if (model.rq_fh == dt)
                {
                    model.rq_fh = DateTime.Now;
                }
                model.id_fh = GetLoginInfo<long>("id_user");
                model.id_gys = GetLoginInfo<long>("id_supplier");
                param["id_user_master"] = GetLoginInfo<long>("id_user_master");
                param["model"] = model;
                br = BusinessFactory.ShippingRecord.Update(param);
                if (br.Success)
                {
                    BusinessFactory.OrderLog.Add(ts);
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
        /// 作废
        /// cxb
        /// 2015-3-26 
        /// </summary>
        [HttpPost]
        public ActionResult Invalid(string obj)
        {

            BaseResult br = new BaseResult();
            Ts_Sale_Order_Log ts = new Ts_Sale_Order_Log();//日志
            try
            {
                Td_Sale_Out_Head model = JSON.Deserialize<Td_Sale_Out_Head>(obj);
                ts.id = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
                ts.id_user = GetLoginInfo<long>("id_user");
                ts.id_user_master = GetLoginInfo<long>("id_user_master");
                ts.flag_type = 6;
                ts.rq = DateTime.Now;
                ts.dh = model.dh;
                ts.content = "出库单已作废";
                model.id_edit = GetLoginInfo<long>("id_user_master");
                br = BusinessFactory.ShippingRecord.Save(model);
                if (br.Success)
                {
                    BusinessFactory.OrderLog.Add(ts);
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
        /// cxb
        /// 2015-3-26
        /// </summary>
        [HttpPost]
        public ActionResult Delete()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("dh", String.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                param["id_edit"] = GetLoginInfo<long>("id_user_master");
                br = BusinessFactory.ShippingRecord.Delete(param);



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
        /// cxb 2015-3-18 
        /// </summary>
        [HttpPost]
        public ActionResult Item(string obj)
        /// 出库/出货记录详
        /// cxb 
        {
            BaseResult br = new BaseResult();
            Hashtable paramli = new Hashtable();

            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                //param.Add("dh", "DH20150504000020");
                Hashtable model = new Hashtable();
                //ParamVessel p = new ParamVessel();
                //p.Add("dh", String.Empty, HandleType.ReturnMsg);
                //param = param.Trim(p);
                br = BusinessFactory.Order.Get(param);
                Td_Sale_Order_Head_Query list0 = (Td_Sale_Order_Head_Query)br.Data;
                list0.order_body = new List<Td_Sale_Order_Body_Query>();
                //获取订单单体
                param["sort"] = "sl_ck";
                param["dir"] = "desc";
                br = BusinessFactory.Order.GetAll(param);
                ViewData["OrderbodyList"] = br.Data;
                List<Td_Sale_Order_Body_Query> list1 = (List<Td_Sale_Order_Body_Query>)br.Data;
                List<Td_Sale_Order_Body_Query> body_list0 = list1.Where(e => e.dh == list0.dh).ToList();
                list0.order_body = body_list0;
                if (list0.ToString() == "")
                {
                    model["order"] = "{}";
                }
                else
                {
                    model["order"] = list0;
                }
                //获取出库单头
                paramli["dh_order"] = param["dh"];
                paramli["sort"] = "flag_state asc, rq_fh desc  , rq_create";
                paramli["dir"] = "desc";
                paramli["not_flag_state"] = OrderFlag.Deleted;
                br = BusinessFactory.ShippingRecord.Get(paramli);
                List<Td_Sale_Out_Head_Query> list2 = (List<Td_Sale_Out_Head_Query>)br.Data;


                //获取出库单单体
                paramli["sort"] = "dh";
                br = BusinessFactory.ShippingRecord.GetAll(paramli);
                List<Td_Sale_Out_Body_Query> list3 = (List<Td_Sale_Out_Body_Query>)br.Data;
                foreach (Td_Sale_Out_Head_Query item in list2)
                {
                    List<Td_Sale_Out_Body_Query> body_list = list3.Where(e => e.dh == item.dh).ToList();
                    foreach (Td_Sale_Out_Body_Query item0 in body_list)
                    {
                        item.out_body.Add(item0);
                    }
                }
                model["out"] = list2;
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
            return Json(br.Data);
        }
    }
}

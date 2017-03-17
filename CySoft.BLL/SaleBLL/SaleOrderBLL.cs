//using System;
//using System.Collections;
//using CySoft.BLL.Base;
//using CySoft.Frame.Attributes;
//using CySoft.Frame.Core;
//using CySoft.Model.Flags;
//using CySoft.Model.Tb;
//using CySoft.Model.Td;
//using CySoft.Model.Ts;
//using System.Collections.Generic;
//using CySoft.Utility;
//using CySoft.Model.Other;
//using CySoft.IDAL;
//using System.Linq;
//using CySoft.BLL.GoodsBLL;
//using CySoft.BLL.SystemBLL.SettingBLL;

//#region 销售订单
//#endregion

//namespace CySoft.BLL.SaleBLL
//{
//    public class SaleOrderBLL : BillBLL
//    {
//        protected static readonly Type saleOrderHeadType = typeof(Td_Sale_Order_Head);
//        protected static readonly Type saleOrderBodyType = typeof(Td_Sale_Order_Body);
//        protected static readonly Type saleOrderLogType = typeof(Ts_Sale_Order_Log);

//        public ITd_Sale_Order_HeadDAL Td_Sale_Order_HeadDAL { get; set; }
//        public ITd_Sale_Order_BodyDAL Td_Sale_Order_BodyDAL { get; set; }
//        /// <summary>
//        /// 新增
//        /// lxt
//        /// 2015-04-16
//        /// </summary>
//        [Transaction]
//        public override BaseResult Add(dynamic entity)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = (Hashtable)entity;

//            Td_Sale_Order_Head_Query head = (Td_Sale_Order_Head_Query)param["model"];
//            long id_user = Convert.ToInt64(param["id_user"]);

//            GoodsSkuFdBLL Fd = new CySoft.BLL.GoodsBLL.GoodsSkuFdBLL();
//            Fd.DAL = DAL;
//            var id_sku_list = head.order_body.Select(item => item.id_sku).ToList();
//            var id_gys = Convert.ToInt32(head.id_gys);

//            //检查是否存在分单 商品
//            var goods_fd = Fd.Check_Order_Goods_Fd(id_sku_list, id_gys);

//            //如果有返回数据，表示存在分单商品
//            if (goods_fd.Count > 0)
//            {
//                Hashtable SourceParam = new Hashtable();
//                var dh = head.dh;

//                //备份原入参
//                SourceParam.Add("orderData", param["orderData"]);
//                SourceParam.Add("invoiceFlag", param["invoiceFlag"]);
//                SourceParam.Add("soure", param["soure"]);

//                param.Remove("orderData");
//                param.Remove("invoiceFlag");
//                param.Remove("soure");

//                //进入分单
//                br = Fd.Order_Fd_Add2(dh, dh, id_gys, head, param, SourceParam);
//            }
//            else
//            {

//                head.flag_delete = 0;
//                head.flag_state = (short)OrderFlag.Submitted;
//                head.je_payed = 0m;
//                head.je_hs = 0m;

//                // 获取 供应商信息
//                param.Clear();
//                param.Add("id_gys", head.id_gys);
//                param.Add("id_cgs", head.id_cgs);
//                var gys_cgs = DAL.GetItem<Tb_Gys_Cgs>(typeof(Tb_Gys_Cgs), param);

//                if (gys_cgs == null)
//                {
//                    br.Success = false;
//                    br.Level = ErrorLevel.Warning;
//                    br.Message.Add("采购关系不存在，无法销售");
//                    return br;
//                }

//                if (gys_cgs.flag_stop.Equals(YesNoFlag.Yes))
//                {
//                    br.Success = false;
//                    br.Level = ErrorLevel.Warning;
//                    br.Message.Add(string.Format("[{0}]和[{1}]关注已取消，无法销售。", gys_cgs.mc_gys, gys_cgs.mc_cgs));
//                    return br;
//                }

//                IList<Td_Sale_Order_Body_Query> body = head.order_body;
//                short xh = 1;
//                foreach (var item in body)
//                {
//                    item.dh = head.dh;
//                    item.xh = xh++;
//                    item.agio = 1m;
//                    item.dj_bhs = item.dj_bhs.Digit(DigitConfig.dj);
//                    item.slv = head.slv.Digit(DigitConfig.slv);
//                    item.dj_hs = (item.dj_bhs * (1 + item.slv / 100)).Digit(DigitConfig.dj);
//                    item.sl = item.sl.Digit(DigitConfig.sl);
//                    item.sl_ck = 0m;
//                    item.sl_fh = 0m;
//                    item.je_hs = (item.dj_hs * item.sl).Digit(DigitConfig.je);
//                    if (head.flag_tj != 1)
//                    {
//                        item.je_pay = item.je_hs;
//                        head.je_pay += item.je_pay;
//                    }
//                    head.je_hs += item.je_hs;
//                }


//                // 处理 申请特价
//                if (head.flag_tj == 1)
//                {
//                    decimal sumAmount = 0;
//                    int length = body.Count;
//                    for (int i = 0; i < length; i++)
//                    {
//                        if (i == length - 1)
//                        {
//                            body[i].je_pay = head.je_pay - sumAmount;
//                        }
//                        else
//                        {
//                            body[i].je_pay = Math.Round(((body[i].je_hs / head.je_hs) * head.je_pay.Digit(DigitConfig.je)).Value, 2, MidpointRounding.AwayFromZero);
//                            sumAmount += body[i].je_pay.Value;
//                        }
//                    }
//                }

//                DAL.Add(head);
//                DAL.AddRange(body);


//                Ts_Sale_Order_Log orderLog = new Ts_Sale_Order_Log();
//                orderLog.dh = head.dh;
//                orderLog.flag_type = 1;
//                orderLog.id_user = id_user;
//                orderLog.id_user_master = head.id_user_master;
//                orderLog.content = "已提交订单，等待订单审核";
//                orderLog.rq = DateTime.Now;
//                DAL.Add(orderLog);
//            }
//            br.Success = true;
//            return br;
//        }

//        /// <summary>
//        /// 新增
//        /// lxt
//        /// 2015-04-16
//        /// </summary>
//        [Transaction]
//        public BaseResult Add_Fd(dynamic entity)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = (Hashtable)entity;
//            Td_Sale_Order_Head_Query head = (Td_Sale_Order_Head_Query)param["model"];
//            long id_user = Convert.ToInt64(param["id_user"]);

//            head.flag_delete = 0;
//            head.flag_state = (short)OrderFlag.Submitted;
//            head.je_payed = 0m;
//            head.je_hs = 0m;

//            // 获取 供应商信息
//            //param.Clear();
//            //param.Add("id_gys", head.id_gys);
//            //param.Add("id_cgs", head.id_cgs);
//            //var gys_cgs = DAL.GetItem<Tb_Gys_Cgs>(typeof(Tb_Gys_Cgs), param);

//            //if (gys_cgs == null)
//            //{
//            //    br.Success = false;
//            //    br.Level = ErrorLevel.Warning;
//            //    br.Message.Add("采购关系不存在，无法销售");
//            //    return br;
//            //}

//            //if (gys_cgs.flag_stop.Equals(YesNoFlag.Yes))
//            //{
//            //    br.Success = false;
//            //    br.Level = ErrorLevel.Warning;
//            //    br.Message.Add(string.Format("[{0}]和[{1}]关注已取消，无法销售。", gys_cgs.mc_gys, gys_cgs.mc_cgs));
//            //    return br;
//            //}

//            IList<Td_Sale_Order_Body_Query> body = head.order_body;
//            short xh = 1;
//            foreach (var item in body)
//            {
//                item.dh = head.dh;
//                item.xh = xh++;
//                item.agio = 1m;
//                item.dj_bhs = item.dj_bhs.Digit(DigitConfig.dj);
//                item.slv = head.slv.Digit(DigitConfig.slv);
//                item.dj_hs = (item.dj_bhs * (1 + item.slv / 100)).Digit(DigitConfig.dj);
//                item.sl = item.sl.Digit(DigitConfig.sl);
//                item.sl_ck = 0m;
//                item.sl_fh = 0m;
//                item.je_hs = (item.dj_hs * item.sl).Digit(DigitConfig.je);
//                if (head.flag_tj != 1)
//                {
//                    item.je_pay = item.je_hs;
//                    head.je_pay += item.je_pay;
//                }
//                head.je_hs += item.je_hs;
//            }


//            // 处理 申请特价
//            if (head.flag_tj == 1)
//            {
//                decimal sumAmount = 0;
//                int length = body.Count;
//                for (int i = 0; i < length; i++)
//                {
//                    if (i == length - 1)
//                    {
//                        body[i].je_pay = head.je_pay - sumAmount;
//                    }
//                    else
//                    {
//                        body[i].je_pay = Math.Round(((body[i].je_hs / head.je_hs) * head.je_pay.Digit(DigitConfig.je)).Value, 2, MidpointRounding.AwayFromZero);
//                        sumAmount += body[i].je_pay.Value;
//                    }
//                }
//            }

//            DAL.Add(head);
//            DAL.AddRange(body);

//            Ts_Sale_Order_Log orderLog = new Ts_Sale_Order_Log();
//            orderLog.dh = head.dh;
//            orderLog.flag_type = 1;
//            orderLog.id_user = id_user;
//            orderLog.id_user_master = head.id_user_master;
//            orderLog.content = "已提交订单，等待订单审核";
//            orderLog.rq = DateTime.Now;
//            DAL.Add(orderLog);

//            br.Success = true;
//            return br;
//        }

//        /// <summary>
//        /// 获取单个
//        /// znt 2015-04-22
//        /// mq 2016-05 修改：从业务消息跳转进来的把该消息设为已读
//        /// </summary>
//        public override BaseResult Get(Hashtable param)
//        {
//            BaseResult br = new BaseResult();
//            if (param.ContainsKey("id_info"))
//            {
//                Hashtable info = new Hashtable();
//                info["id_info"] = param["id_info"];
//                info["new_flag_reade"] = 1;
//                DAL.UpdatePart(typeof(Info_User), info);
//                param.Remove("id_info");
//            }
//            Td_Sale_Order_Head_Query model = DAL.GetItem<Td_Sale_Order_Head_Query>(saleOrderHeadType, param);
//            if (model == null)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("未找到订单【{0}】！请检查后再试。", param["dh"]));
//                br.Level = ErrorLevel.Warning;
//                return br;
//            }

//            //switch (model.flag_state)
//            //{
//            //    case 10:
//            //        model.mc_flag_state = string.Format("待订单审核");
//            //        break;
//            //    case 20:
//            //        model.mc_flag_state = string.Format("待财务审核");
//            //        break;
//            //    case 30:
//            //    case 40:
//            //        model.mc_flag_state = string.Format("待出库审核");
//            //        break;
//            //    case 50:
//            //    case 60:
//            //        model.mc_flag_state = string.Format("待发货确认");
//            //        break;
//            //    case 70:
//            //        model.mc_flag_state = string.Format("待收货确认");
//            //        break;
//            //    case 80:
//            //        model.mc_flag_state = string.Format("已完成");
//            //        break;
//            //    case 0:
//            //        model.mc_flag_state = string.Format("已作废");
//            //        break;
//            //}

//            model.mc_flag_state = GetMcFlagState(model.flag_state);

//            #region 付款状态
//            if (model.je_pay == 0)
//            {
//                model.mc_payment_state = string.Format("已付款");
//            }
//            else if (model.je_pay > 0 && model.je_payed == 0 && model.count_nopay == 0)
//            {
//                model.mc_payment_state = string.Format("未付款");
//            }
//            else if (model.je_pay > model.je_payed && model.count_nopay == 0)
//            {
//                model.mc_payment_state = string.Format("部分付款");
//            }
//            else if (model.je_pay > model.je_payed && model.count_nopay > 0)
//            {
//                model.mc_payment_state = string.Format("待付款审核");
//            }
//            else
//            {
//                model.mc_payment_state = string.Format("已付款");
//            }

//            model.mc_payment_state = GetMcPaymentState(model.je_pay, model.je_payed, model.count_nopay);
//            #endregion

//            model.order_body = DAL.QueryList<Td_Sale_Order_Body_Query>(saleOrderBodyType, param);
//            param.Add("sort", "rq");
//            param.Add("dir", "desc");
//            model.order_Log = DAL.QueryList<Ts_Sale_Order_Log_Query>(saleOrderLogType, param);

//            //已分单 则 查询子单
//            if (model.flag_fd == 1)
//            {
//                long id_gys = 0;
//                if (param.ContainsKey("id_gys"))
//                {
//                    id_gys = (long)param["id_gys"];
//                }
//                param.Clear();
//                param.Add("tsof_dh_father", model.dh);

//                model.children_list = DAL.QueryListByStatementName<Td_Sale_Order_Head_Query>(saleOrderHeadType, param, "GetChildrenList");

//                if (model.children_list != null && model.children_list.Count > 0)
//                {
//                    foreach (var item in model.children_list)
//                    {
//                        param.Clear();
//                        param.Add("dh", item.dh);
//                        item.mc_flag_state = GetMcFlagState(item.flag_state);
//                        item.mc_payment_state = GetMcPaymentState(item.je_pay, item.je_payed, item.count_nopay);
//                        item.order_body = DAL.QueryList<Td_Sale_Order_Body_Query>(saleOrderBodyType, param);
//                        param.Add("sort", "rq");
//                        param.Add("dir", "desc");
//                        item.order_Log = DAL.QueryList<Ts_Sale_Order_Log_Query>(saleOrderLogType, param);
//                        //if (id_gys == item.id_gys)
//                        //{
//                        //    foreach (var body in item.order_body)
//                        //    {
//                        //        foreach (var it in model.order_body)
//                        //        {
//                        //            if (body.id_sku == it.id_sku)
//                        //            {
//                        //                it.dj_bhs = body.dj_bhs;
//                        //                it.dj_hs = body.dj_hs;
//                        //                it.je_hs = body.je_hs;
//                        //                it.je_pay = body.je_pay;
//                        //                DAL.Update<Td_Sale_Order_Body>(it);
//                        //            }
//                        //        }
//                        //    }
//                        //}
//                    }
//                }
//            }

//            br.Data = model;
//            br.Success = true;
//            return br;
//        }

//        private string GetMcFlagState(short? flagState)
//        {
//            switch (flagState)
//            {
//                case 10:
//                    return "待订单审核";
                    
//                case 20:
//                    return "待财务审核";
                    
//                case 30:
//                case 40:
//                    return "待出库审核";
                    
//                case 50:
//                case 60:
//                    return "待发货确认";
                    
//                case 70:
//                    return "待收货确认";
                    
//                case 80:
//                    return "已完成";
                    
//                case 0:
//                    return "已作废";
//                default:
//                    return "";
//            }
//        }

//        private string GetMcPaymentState(decimal? jePay,decimal? jePayed,int countNopay)
//        {
//            #region 付款状态
//            if (jePay == 0)
//            {
//                return "已付款";
//            }
//            else if (jePay > 0 && jePayed == 0 && countNopay == 0)
//            {
//                return "未付款";
//            }
//            else if (jePay > jePayed && countNopay == 0)
//            {
//                return "部分付款";
//            }
//            else if (jePay > jePayed && countNopay > 0)
//            {
//                return "待付款审核";
//            }
//            else
//            {
//                return "已付款";
//            }
//            #endregion
//        }

//        /// <summary>
//        /// 审核
//        /// lxt
//        /// 2015-04-15
//        /// 2015-04-17 znt
//        /// </summary>
//        [Transaction]
//        public override BaseResult Check(Hashtable param)
//        {
//            BaseResult br = new BaseResult();
//            string dh = param["dh"].ToString();
//            OrderFlag new_flag_state = (OrderFlag)Convert.ToInt32(param["new_flag_state"]);
//            long id_user = Convert.ToInt64(param["id_user"]);
//            long id_user_master = Convert.ToInt64(param["id_user_master"]);
//            Td_Sale_Order_Head_Query model = DAL.GetItem<Td_Sale_Order_Head_Query>(saleOrderHeadType, param);
//            if (model == null)
//            {
//                br.Success = false;
//                br.Message.Add(String.Format("未找到订单【{0}】！请检查后再试。", dh));
//                br.Level = ErrorLevel.Warning;
//                return br;
//            }

//            // 待订单审核+申请特价  更新审批价
//            if (model.flag_state == 10 && model.flag_tj == 1)
//            {
//                List<Td_Sale_Order_Body> orderBodyList = JSON.Deserialize<List<Td_Sale_Order_Body>>(param["jsonarray"].ToString());

//                if (orderBodyList != null && orderBodyList.Count > 0)
//                {
//                    foreach (var item in orderBodyList)
//                    {
//                        param.Clear();
//                        param.Add("dh", dh);
//                        param.Add("xh", item.xh);
//                        param.Add("new_je_pay", item.je_pay);
//                        DAL.UpdatePart(typeof(Td_Sale_Order_Body), param);
//                    }
//                }

//            }

//            // 待财务审核时 检查出库状态( 若已出库 表明该订单为已出库的退回单 )
//            if (new_flag_state == OrderFlag.FinanceCheck)
//            {
//                // 订单已出库时 且 订单待发货或部分发货 =》 直接跳到待发货状态 
//                if (model.flag_out == 2 && model.flag_fh != 2)
//                {
//                    new_flag_state = OrderFlag.WaitDelivery;
//                }
//            }

//            DateTime dbDateTime = DAL.GetDbDateTime();

//            param.Clear();
//            param.Add("dh", dh);
//            param.Add("new_flag_state", new_flag_state);
//            param.Add("new_id_edit", id_user);
//            param.Add("new_rq_edit", dbDateTime);
//            DAL.UpdatePart(typeof(Td_Sale_Order_Head), param);

//            Ts_Sale_Order_Log orderLog = new Ts_Sale_Order_Log();
//            orderLog.dh = model.dh;
//            orderLog.id_user = id_user;
//            orderLog.id_user_master = id_user_master;
//            orderLog.rq = DateTime.Now;
//            switch (new_flag_state)
//            {
//                case OrderFlag.OrderCheck:
//                    orderLog.flag_type = 2;
//                    orderLog.content = "订单已通过审核";
//                    break;
//                case OrderFlag.FinanceCheck:
//                    orderLog.flag_type = 3;
//                    orderLog.content = "订单已通过财务审核";
//                    break;
//            }



//            DAL.Add(orderLog);

//            switch (new_flag_state)
//            {
//                case OrderFlag.OrderCheck:
//                    br.Message.Add(String.Format("销售订单审核。信息：单号：{0}", model.dh));
//                    break;
//                case OrderFlag.FinanceCheck:
//                    br.Message.Add(String.Format("销售订单财务审核。", model.dh));
//                    break;
//            }
//            br.Success = true;
//            return br;
//        }

//        /// <summary>
//        /// 退回
//        /// lxt
//        /// 2015-04-15
//        /// </summary>
//        [Transaction]
//        public override BaseResult UnCheck(Hashtable param)
//        {
//            BaseResult br = new BaseResult();
//            string dh = param["dh"].ToString();
//            long id_user = Convert.ToInt64(param["id_user"]);
//            long id_user_master = Convert.ToInt64(param["id_user_master"]);
//            long newOrderLogId = Convert.ToInt64(param["newOrderLogId"]);
//            long id_supplier = Convert.ToInt64(param["id_supplier"]);
//            string remark = (param["remark"] ?? "").ToString();
//            param.Clear();
//            param.Add("dh", dh);
//            param.Add("id_gys", id_supplier);
//            Td_Sale_Order_Head_Query model = DAL.GetItem<Td_Sale_Order_Head_Query>(typeof(Td_Sale_Order_Head), param);
//            if (model == null)
//            {
//                br.Success = false;
//                br.Message.Add(String.Format("未找到销售订单【{0}】！请检查后再试。", dh));
//                br.Level = ErrorLevel.Warning;
//                return br;
//            }

//            DateTime dbDateTime = DAL.GetDbDateTime();

//            param.Clear();
//            param.Add("dh", model.dh);
//            param.Add("new_flag_state", OrderFlag.Submitted);
//            param.Add("new_id_edit", id_user);
//            param.Add("new_rq_edit", dbDateTime);
//            DAL.UpdatePart(typeof(Td_Sale_Order_Head), param);

//            Ts_Sale_Order_Log orderLog = new Ts_Sale_Order_Log();
//            orderLog.id = newOrderLogId;
//            orderLog.dh = model.dh;
//            orderLog.flag_type = 7;
//            orderLog.id_user = id_user;
//            orderLog.id_user_master = id_user_master;
//            orderLog.rq = DateTime.Now;
//            switch (model.flag_state)
//            {
//                case 20:
//                    orderLog.content = "订单未通过财务审核，退回重审";
//                    break;
//                case 30:
//                    orderLog.content = "订单未通过出库审核,退回重审";
//                    break;
//                default:
//                    orderLog.content = "订单未发货确认，退回重审";
//                    break;
//            }
//            DAL.Add(orderLog);

//            br.Message.Add(String.Format("销售订单退回重审。信息：单号:{0}", model.dh));
//            br.Success = true;
//            return br;
//        }

//        /// <summary>
//        /// 作废
//        /// lxt
//        /// 2015-04-15
//        /// </summary>
//        [Transaction]
//        public override BaseResult Invalid(Hashtable param)
//        {
//            BaseResult br = new BaseResult();
//            string dh = param["dh"].ToString();
//            long id_user = Convert.ToInt64(param["id_user"]);
//            long id_user_master = Convert.ToInt64(param["id_user_master"]);
//            long newOrderLogId = Convert.ToInt64(param["newOrderLogId"]);
//            long id_supplier = Convert.ToInt64(param["id_supplier"]);
//            string remark = (param["remark"] ?? "").ToString();
//            param.Clear();
//            param.Add("dh", dh);
//            param.Add("id_gys", id_supplier);
//            Td_Sale_Order_Head_Query model = DAL.GetItem<Td_Sale_Order_Head_Query>(typeof(Td_Sale_Order_Head), param);
//            if (model == null)
//            {
//                br.Success = false;
//                br.Message.Add(String.Format("未找到销售订单【{0}】！请检查后再试。", dh));
//                br.Level = ErrorLevel.Warning;
//                return br;
//            }
//            if (model.flag_state > 20 && model.flag_state != 80)//状态80为已收货订单（即 已完成）
//            {
//                br.Success = false;
//                br.Data = "none";
//                br.Message.Add(string.Format("该订单已审核，不能作废！"));
//                return br;
//            }

//            DateTime dbDateTime = DAL.GetDbDateTime();

//            param.Clear();
//            param.Add("dh", model.dh);
//            param.Add("new_flag_state", OrderFlag.Invalided);
//            param.Add("new_id_edit", id_user);
//            param.Add("new_rq_edit", dbDateTime);
//            DAL.UpdatePart(typeof(Td_Sale_Order_Head), param);

//            Ts_Sale_Order_Log orderLog = new Ts_Sale_Order_Log();
//            orderLog.id = newOrderLogId;
//            orderLog.dh = model.dh;
//            orderLog.flag_type = 7;
//            orderLog.id_user = id_user;
//            orderLog.id_user_master = id_user_master;
//            orderLog.rq = DateTime.Now;
//            switch (model.flag_state)
//            {
//                case 10:
//                    orderLog.content = "订货单未通过订单审核，退回作废";
//                    break;
//                case 70:
//                    orderLog.content = "订货作废";
//                    break;
//            }

//            DAL.Add(orderLog);

//            #region 分单-批量修改发货状态

//            var Fd = new GoodsSkuFdBLL();
//            Fd.DAL = DAL;
//            //检查当前单号是否存在上级单号
//            var Order_Fd = Fd.Query_Sale_Order_Fd(dh);
//            //存在上级订单
//            if (Order_Fd != null)
//            {

//                Fd.SaleOut_UpdateFlag_Fd(dh, OrderFlag.Invalided);
//            }

//            #endregion

//            br.Message.Add(String.Format("销售订单作废。信息：单号:{0}", model.dh));
//            br.Success = true;
//            return br;
//        }

//        /// <summary>
//        /// 修改信息
//        /// lxt
//        /// 2015-04-15
//        /// </summary>
//        public override BaseResult Update(dynamic entity)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = (Hashtable)entity;
//            param["new_rq_edit"] = DAL.GetDbDateTime();
//            DAL.UpdatePart(typeof(Td_Sale_Order_Head), param);
//            br.Success = true;
//            return br;
//        }

//        /// <summary>
//        /// 订单审核
//        /// znt 2015-04-17
//        /// cxb 2015-7-7 改
//        /// </summary>
//        /// 待定： 1 读取当前供应商的设置的订单流程  2 读取订单当前的状态 进行业务处理
//        [Transaction]
//        public override BaseResult CheckOrder(Hashtable param)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param0 = new Hashtable();
//            string dh = param["dh"].ToString();
//            long id_gys = Convert.ToInt64(param["id_gys"]); //  当供应商订单流程可以设置后 此id有用
//            long id_user = Convert.ToInt64(param["id_user"]);
//            long id_user_master = Convert.ToInt64(param["id_user_master"]);
//            long newOrderLogId = Convert.ToInt64(param["newOrderLogId"]);
//            string jsonarray = param["jsonarray"] == null ? String.Empty : param["jsonarray"].ToString();
//            OrderFlag new_flag_state = OrderFlag.OrderCheck;
//            Td_Sale_Order_Head_Query model = DAL.GetItem<Td_Sale_Order_Head_Query>(saleOrderHeadType, param);
//            if (model == null)
//            {
//                br.Success = false;
//                br.Message.Add(String.Format("未找到订单【{0}】！请检查后再试。", model.dh));
//                br.Level = ErrorLevel.Warning;
//                return br;
//            }

//            // 尚未设计供应商订单流程设置  此处暂时订单提交后就是待订单审核 
//            if (model.flag_state != (short)OrderFlag.Submitted && model.flag_delete == (short)YesNoFlag.No)
//            {
//                br.Success = false;
//                br.Message.Add(String.Format("当前状态不在待订单审核中【{0}】！请检查或刷新页面后再试。", model.dh));
//                br.Level = ErrorLevel.Warning;
//                return br;
//            }

//            // 待订单审核+申请特价  更新审批价
//            if (model.flag_tj == 1 && !string.IsNullOrEmpty(jsonarray))
//            {
//                List<Td_Sale_Order_Body> orderBodyList = JSON.Deserialize<List<Td_Sale_Order_Body>>(jsonarray);

//                if (orderBodyList != null && orderBodyList.Count > 0)
//                {
//                    foreach (var item in orderBodyList)
//                    {
//                        param.Clear();
//                        param.Add("dh", dh);
//                        param.Add("xh", item.xh);
//                        param.Add("new_je_pay", item.je_pay);
//                        DAL.UpdatePart(typeof(Td_Sale_Order_Body), param);
//                    }
//                }
//            }
//            Ts_Param_Business item0 = new Ts_Param_Business();
//            param0["id_user_master"] = id_user_master;
//            IList<Ts_Param_Business> list = DAL.QueryList<Ts_Param_Business>(typeof(Ts_Param_Business), param0);

//            IList<Ts_Param_Business> list0 = list.Where(m => m.bm.IndexOf("ywlc") != -1 && m.val != "0").ToList();
//            if (list0.Where(m => m.bm == "order_ywlc_cwsh").Count() > 0)
//            {
//                item0.bm = "order_ywlc_cwsh";
//            }
//            else if (list0.Where(m => m.bm == "order_ywlc_cksh").Count() > 0)
//            {
//                item0.bm = "order_ywlc_cksh";
//            }
//            else if (list0.Where(m => m.bm == "order_ywlc_fhqr").Count() > 0)
//            {
//                item0.bm = "order_ywlc_fhqr";
//            }
//            else
//            {
//                item0.bm = "order_ywlc_shqr";
//            }
//            switch (item0.bm)
//            {
//                case "order_ywlc_cwsh":
//                    new_flag_state = OrderFlag.OrderCheck;
//                    break;
//                case "order_ywlc_cksh":
//                    // 待财务审核时 检查出库状态( 若已出库 表明该订单为已出库的退回单 )
//                    // 订单已出库时 且 订单待发货或部分发货 =》 直接跳到待发货状态 
//                    if (model.flag_out == 2 && model.flag_fh != 2)
//                    {
//                        new_flag_state = OrderFlag.WaitDelivery;
//                    }
//                    else
//                    {
//                        new_flag_state = OrderFlag.WaitOutputCheck;
//                    }
//                    break;
//                case "order_ywlc_fhqr":
//                    new_flag_state = OrderFlag.WaitDelivery;
//                    break;
//                case "order_ywlc_shqr":
//                    new_flag_state = OrderFlag.Delivered;
//                    break;
//                default:
//                    new_flag_state = OrderFlag.Receipted;
//                    break;
//            }
//            param.Clear();
//            param.Add("dh", dh);
//            param.Add("new_flag_state", new_flag_state);
//            if (new_flag_state == OrderFlag.Receipted)
//            {
//                param.Add("new_flag_out", 2);
//                param.Add("new_flag_fh", 2);
//            }
//            param.Add("new_id_edit", id_user);
//            param.Add("new_rq_edit", DateTime.Now);
//            int result = DAL.UpdatePart(saleOrderHeadType, param);
//            if (result > 0)
//            {
//                br.Success = false;
//                br.Message.Add(String.Format("销售订单【{0}】订单审核时操作失败！", model.dh));
//                br.Level = ErrorLevel.Error;
//                return br;
//            }

//            Ts_Sale_Order_Log orderLog = new Ts_Sale_Order_Log();
//            orderLog.id = newOrderLogId;
//            orderLog.dh = model.dh;
//            orderLog.id_user = id_user;
//            orderLog.id_user_master = id_user_master;
//            orderLog.rq = DateTime.Now;
//            orderLog.flag_type = 2;
//            orderLog.content = "已通过订单审核";
//            DAL.Add(orderLog);

//            br.Success = true;
//            br.Message.Add(String.Format(String.Format("销售订单审核。信息：单号：{0}", model.dh)));
//            return br;
//        }

//        /// <summary>
//        /// 财务审核 
//        /// znt 2015-04-17
//        /// cxb 2015-7-7
//        /// </summary>
//        /// 待定： 1 读取当前供应商的设置的订单流程  2 读取订单当前的状态 进行业务处理
//        [Transaction]
//        public override BaseResult CheckFinance(Hashtable param)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param0 = new Hashtable();
//            string dh = param["dh"].ToString();
//            long id_gys = Convert.ToInt64(param["id_gys"]); //  当供应商订单流程可以设置后 此id有用
//            long id_user = Convert.ToInt64(param["id_user"]);
//            long id_user_master = Convert.ToInt64(param["id_user_master"]);
//            OrderFlag new_flag_state = OrderFlag.FinanceCheck;
//            long newOrderLogId = Convert.ToInt64(param["newOrderLogId"]);

//            Td_Sale_Order_Head_Query model = DAL.GetItem<Td_Sale_Order_Head_Query>(saleOrderHeadType, param);
//            if (model == null)
//            {
//                br.Success = false;
//                br.Message.Add(String.Format("未找到订单【{0}】！请检查后再试。", model.dh));
//                br.Level = ErrorLevel.Warning;
//                return br;
//            }

//            // 尚未设计供应商订单流程设置  此处暂时已审核后就是待财务审核 
//            if (model.flag_state != (short)OrderFlag.OrderCheck && model.flag_delete == (short)YesNoFlag.No)
//            {
//                br.Success = false;
//                br.Message.Add(String.Format("当前状态不在待财务审核中【{0}】！请检查或刷新页面后再试。", model.dh));
//                br.Level = ErrorLevel.Warning;
//                return br;
//            }
//            Ts_Param_Business item = new Ts_Param_Business();
//            param0["id_user_master"] = param["id_user_master"];
//            IList<Ts_Param_Business> list = DAL.QueryList<Ts_Param_Business>(typeof(Ts_Param_Business), param0);
//            IList<Ts_Param_Business> list0 = list.Where(m => m.bm.IndexOf("ywlc") != -1 && m.val != "0" && m.bm.IndexOf("order_ywlc_cwsh") == -1).OrderBy(m => m.sort_id).ToList();
//            if (list0.Where(m => m.bm == "order_ywlc_cksh").Count() > 0)
//            {
//                item.bm = "order_ywlc_cksh";
//            }
//            else if (list0.Where(m => m.bm == "order_ywlc_fhqr").Count() > 0)
//            {
//                item.bm = "order_ywlc_fhqr";
//            }
//            else
//            {
//                item.bm = "order_ywlc_shqr";
//            }
//            switch (item.bm)
//            {
//                case "order_ywlc_cksh":
//                    // 待财务审核时 检查出库状态( 若已出库 表明该订单为已出库的退回单 )
//                    // 订单已出库时 且 订单待发货或部分发货 =》 直接跳到待发货状态 
//                    if (model.flag_out == 2 && model.flag_fh != 2)
//                    {
//                        new_flag_state = OrderFlag.WaitDelivery;
//                    }
//                    else
//                    {
//                        new_flag_state = OrderFlag.FinanceCheck;
//                    }
//                    break;
//                case "order_ywlc_fhqr":
//                    new_flag_state = OrderFlag.WaitDelivery;
//                    break;
//                case "order_ywlc_shqr":
//                    new_flag_state = OrderFlag.Delivered;
//                    break;
//                default:
//                    new_flag_state = OrderFlag.Receipted;
//                    break;
//            }

//            param.Clear();
//            param.Add("dh", dh);
//            param.Add("new_flag_state", new_flag_state);
//            if (new_flag_state == OrderFlag.Receipted)
//            {
//                param.Add("new_flag_out", 2);
//                param.Add("new_flag_fh", 2);
//            }
//            param.Add("new_id_edit", id_user);
//            param.Add("new_rq_edit", DateTime.Now);
//            int result = DAL.UpdatePart(saleOrderHeadType, param);
//            if (result > 0)
//            {
//                br.Success = false;
//                br.Message.Add(String.Format("销售订单【{0}】财务审核时 操作失败！", model.dh));
//                br.Level = ErrorLevel.Error;
//                return br;
//            }

//            Ts_Sale_Order_Log orderLog = new Ts_Sale_Order_Log();
//            orderLog.id = newOrderLogId;
//            orderLog.dh = model.dh;
//            orderLog.id_user = id_user;
//            orderLog.id_user_master = id_user_master;
//            orderLog.rq = DateTime.Now;
//            orderLog.flag_type = 3;
//            orderLog.content = "已通过财务审核";
//            DAL.Add(orderLog);

//            br.Success = true;
//            br.Message.Add(String.Format(String.Format("销售订单审核。信息：单号：{0}", model.dh)));
//            return br;
//        }

//        /// <summary>
//        ///  删除
//        ///  znt 2015-04-20
//        /// </summary>
//        public override BaseResult Stop(Hashtable param)
//        {
//            BaseResult br = new BaseResult();


//            Td_Sale_Order_Head model = DAL.GetItem<Td_Sale_Order_Head>(typeof(Td_Sale_Order_Head), param);

//            if (model == null)
//            {
//                br.Success = false;
//                br.Data = "none";
//                br.Message.Add(string.Format("未找到订单【{0}】！请检查后再试。", param["dh"]));
//                return br;
//            }

//            if (model.flag_state != 0)
//            {
//                br.Success = false;
//                br.Data = "none";
//                br.Message.Add(string.Format("订单【{0}】当前状态不符合删除要求！", param["dh"]));
//                return br;
//            }

//            param.Add("new_flag_delete", 1);

//            int result = DAL.UpdatePart(typeof(Td_Sale_Order_Head), param);
//            if (result >= 0)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("订单【{0}】删除失败！", param["dh"]));
//                return br;
//            }

//            br.Success = true;
//            br.Message.Add(string.Format("订单【{0}】删除成功！", param["dh"]));
//            return br;
//        }

//        /// <summary>
//        /// 设置 收货地址
//        /// znt 2015-04-21
//        /// </summary>
//        public override BaseResult SetOrderAddress(Hashtable param)
//        {
//            BaseResult br = new BaseResult();
//            Td_Sale_Order_Head model = (Td_Sale_Order_Head)param["model"];

//            param.Clear();
//            param.Add("dh", model.dh);
//            if (DAL.GetCount(saleOrderHeadType, param) <= 0)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("未找到订单【{0}】！请检查后再试。", model.dh));
//                br.Level = ErrorLevel.Warning;
//                br.Data = "dh";
//                return br;
//            }

//            param.Clear();
//            param.Add("dh", model.dh);
//            param.Add("new_shr", model.shr);
//            param.Add("new_id_province", model.id_province);
//            param.Add("new_id_city", model.id_city);
//            param.Add("new_id_county", model.id_county);
//            param.Add("new_address", model.address);
//            param.Add("new_phone", model.phone);
//            param.Add("new_id_edit", model.id_edit);
//            param.Add("new_rq_edit", model.rq_edit);

//            int result = DAL.UpdatePart(saleOrderHeadType, param);
//            if (result >= 0)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("修改订单【{0}】收货地址失败", model.dh));
//                return br;
//            }
//            br.Success = true;
//            br.Message.Add(string.Format("修改订单【{0}】收货地址成功", model.dh));
//            return br;
//        }

//        /// <summary>
//        /// 设置 订单发票
//        /// znt 2015-04-21
//        /// </summary>
//        public override BaseResult SetOrderInvoice(Hashtable param)
//        {
//            BaseResult br = new BaseResult();
//            Td_Sale_Order_Head model = (Td_Sale_Order_Head)param["model"];

//            param.Clear();
//            param.Add("dh", model.dh);
//            if (DAL.GetCount(saleOrderHeadType, param) <= 0)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("未找到订单【{0}】！请检查后再试。", model.dh));
//                br.Level = ErrorLevel.Warning;
//                br.Data = "dh";
//                return br;
//            }

//            param.Clear();
//            param.Add("dh", model.dh);

//            if (model.flag_invoice.ToLower() == "general")
//            {
//                param.Add("new_title_invoice", model.title_invoice);
//                param.Add("new_content_invoice", model.content_invoice);
//            }
//            else
//            {
//                param.Add("new_title_invoice", model.title_invoice);
//                param.Add("new_content_invoice", model.content_invoice);
//                param.Add("new_name_bank", model.name_bank);
//                param.Add("new_account_bank", model.account_bank);
//                param.Add("new_no_tax", model.no_tax);
//            }

//            param.Add("new_id_edit", model.id_edit);
//            param.Add("new_rq_edit", model.rq_edit);

//            int result = DAL.UpdatePart(saleOrderHeadType, param);
//            if (result >= 0)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("修改订单【{0}】发票信息失败", model.dh));
//                return br;
//            }
//            br.Success = true;
//            br.Message.Add(string.Format("修改订单【{0}】发票信息成功", model.dh));
//            return br;
//        }

//        /// <summary>
//        /// 销售单修改 
//        /// znt 2015-05-05
//        /// </summary>
//        [Transaction]
//        public override BaseResult Edit(dynamic entity)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = (Hashtable)entity;
//            Td_Sale_Order_Head_Query head = (Td_Sale_Order_Head_Query)param["model"];
//            long id_user = Convert.ToInt64(param["id_user"]);
//            long newOrderLogId = Convert.ToInt64(param["newOrderLogId"]);
//            long id_user_master = Convert.ToInt64(param["id_user_master"]);
//            long id_cgs = Convert.ToInt64(param["id_cgs"]);
//            param.Clear();
//            param.Add("dh", head.dh);
//            Td_Sale_Order_Head model = DAL.GetItem<Td_Sale_Order_Head>(typeof(Td_Sale_Order_Head), param);
//            if (model == null)
//            {
//                br.Success = false;
//                br.Data = "none";
//                br.Message.Add(string.Format("未找到订单【{0}】！请检查后再试。", head.dh));
//                return br;
//            }

//            if (model.flag_state != (int)CySoft.Model.Flags.OrderFlag.Submitted)
//            {
//                br.Success = false;
//                br.Level = ErrorLevel.Error;
//                br.Message.Add(string.Format("订单【{0}】当前状态不符合修改订单商品内容要求！", head.dh));
//                return br;
//            }

//            IList<Td_Sale_Order_Body_Query> body = head.order_body;
//            long je_hs = (long)model.je_hs;
//            long je_pay = (long)model.je_pay;
//            // 清空 原有订单体
//            param.Clear();
//            param.Add("dh", head.dh);
//            DAL.Delete(typeof(Td_Sale_Order_Body), param);

//            // 重新计算金额
//            model.je_hs = (long)0;
//            model.je_pay = (long)0;

//            short xh = 1;
//            foreach (var item in body)
//            {
//                item.dh = model.dh;
//                item.xh = xh++;
//                item.agio = 1m;
//                item.dj_bhs = item.dj_bhs.Digit(DigitConfig.dj);
//                item.slv = model.slv.Digit(DigitConfig.slv);
//                item.dj_hs = (item.dj_bhs * (1 + item.slv / 100)).Digit(DigitConfig.dj);
//                item.sl = item.sl.Digit(DigitConfig.sl);
//                item.sl_ck = 0m;
//                item.sl_fh = 0m;
//                item.je_hs = (item.dj_hs * item.sl).Digit(DigitConfig.je);
//                if (model.flag_tj != 1)
//                {
//                    item.je_pay = item.je_hs;
//                    model.je_pay += item.je_pay;
//                }
//                model.je_hs += item.je_hs;
//            }


//            // 处理 申请特价
//            if (model.flag_tj == 1)
//            {
//                model.je_pay = head.je_pay;
//                decimal sumAmount = 0;
//                int length = body.Count;
//                for (int i = 0; i < length; i++)
//                {
//                    if (i == length - 1)
//                    {
//                        body[i].je_pay = model.je_pay - sumAmount;
//                    }
//                    else
//                    {
//                        if (model.je_hs == 0) model.je_hs = 1;
//                        body[i].je_pay = Math.Round(((body[i].je_hs / model.je_hs) * model.je_pay.Digit(DigitConfig.je)).Value, 2, MidpointRounding.AwayFromZero);
//                        sumAmount += body[i].je_pay.Value;
//                    }
//                }
//            }

//            DAL.AddRange(body);

//            var Fd = new GoodsSkuFdBLL();
//            Fd.DAL = DAL;
//            var Order_Fd = Fd.Query_Sale_Order_Fd(head.dh);
//            if (Order_Fd != null)
//            {
//                param.Clear();
//                param.Add("dh", Order_Fd.dh_father);
//                IList<Td_Sale_Order_Body> body_fd = DAL.QueryList<Td_Sale_Order_Body>(typeof(Td_Sale_Order_Body), param);
//                if (body_fd != null && body_fd.Count > 0)
//                {
//                    foreach (var item in body_fd)
//                    {
//                        foreach (var it in body)
//                        {
//                            if (item.id_sku == it.id_sku)
//                            {
//                                item.je_hs = it.je_hs;
//                                item.je_pay = it.je_pay;
//                                item.dj_bhs = it.dj_bhs;
//                                item.dj_hs = it.dj_hs;
//                                DAL.Update<Td_Sale_Order_Body>(item);
//                            }
//                        }
//                    }
//                }

//                if (model.id_cgs == id_cgs)
//                {
//                    Td_Sale_Order_Head head_fd = DAL.GetItem<Td_Sale_Order_Head>(typeof(Td_Sale_Order_Head), param);
//                    if (head_fd != null)
//                    {
//                        head_fd.je_hs = head_fd.je_hs - je_hs + model.je_hs;
//                        head_fd.je_pay = head_fd.je_pay - je_pay + model.je_pay;
//                        DAL.Update<Td_Sale_Order_Head>(head_fd);
//                    }
//                }
//            }
//            Ts_Sale_Order_Log orderLog = new Ts_Sale_Order_Log();
//            orderLog.id = newOrderLogId;
//            orderLog.dh = model.dh;
//            orderLog.flag_type = 8;
//            orderLog.id_user = id_user;
//            orderLog.id_user_master = id_user_master;
//            orderLog.content = "修改了订单的商品内容";
//            orderLog.rq = DateTime.Now;
//            DAL.Add(orderLog);

//            param.Clear();
//            param.Add("dh", model.dh);
//            param.Add("new_je_hs", model.je_hs);
//            param.Add("new_je_pay", model.je_pay);
//            param.Add("new_je_payed", model.je_payed);
//            param.Add("new_id_edit", id_user);
//            param.Add("new_rq_edit", DateTime.Now);
//            int result = DAL.UpdatePart(typeof(Td_Sale_Order_Head), param);
//            if (result > 0)
//            {
//                br.Success = false;
//                br.Level = ErrorLevel.Error;
//                br.Message.Add(string.Format("修改订单商品内容失败！流水号：{0}", head.dh));
//                return br;
//            }

//            br.Success = true;
//            br.Message.Add(string.Format("修改订单商品内容成功！流水号：{0}", head.dh));
//            return br;

//        }

//        /// <summary>
//        /// 统计金额、数量
//        /// znt 2015-05-06
//        /// </summary>
//        public override BaseResult PayTotal(Hashtable param)
//        {
//            BaseResult br = new BaseResult();
//            Query_Pay_Total model = Td_Sale_Order_HeadDAL.QueryPayTotal(typeof(Td_Sale_Order_Head), param);
//            if (model != null)
//            {
//                br.Data = model;
//            }
//            else
//            {
//                br.Data = new Query_Pay_Total();
//            }
//            br.Success = true;
//            return br;
//        }

//        public override BaseResult GysHomeTotal(Hashtable param)
//        {
//            BaseResult br = new BaseResult();
//            Query_Pay_Total model = Td_Sale_Order_HeadDAL.GysHomeTotal(typeof(Td_Sale_Order_Head), param);
//            if (model != null)
//            {
//                br.Data = model;
//            }
//            else
//            {
//                br.Data = new Query_Pay_Total();
//            }
//            br.Success = true;
//            return br;
//        }

//        /// <summary>
//        /// 获取统计汇总的数据 cxb 2015-6-4
//        /// </summary>
//        /// <param name="param"></param>
//        /// <returns></returns>
//        public override BaseResult GetStatisticsInfo(Hashtable param)
//        {
//            BaseResult br = new BaseResult();
//            br.Data = Td_Sale_Order_BodyDAL.QueryStatisticsInfo(typeof(Td_Sale_Order_Body), param);
//            br.Success = true;
//            return br;
//        }


//        /// <summary>
//        /// 获取订单数 cxb 2015-6-10 09:37:52
//        /// </summary>
//        /// <param name="param"></param>
//        /// <returns></returns>
//        public override BaseResult GetCount(Hashtable param = null)
//        {
//            BaseResult br = new BaseResult();
//            br.Data = DAL.GetCount(typeof(Td_Sale_Order_Head), param);
//            return br;
//        }

//        /// <summary>
//        /// 获取商品统计数据 cxb 2015-6-1
//        /// </summary>
//        /// <returns></returns>
//        public override PageNavigate GetStatistics(Hashtable param)
//        {
//            PageNavigate pn = new PageNavigate();
//            pn.TotalCount = Td_Sale_Order_BodyDAL.QueryStatisticsCount(typeof(Td_Sale_Order_Body), param);
//            if (pn.TotalCount > 0)
//            {
//                IList<Td_Sale_Order_Body_Query> List = Td_Sale_Order_BodyDAL.QueryStatisticsPage(typeof(Td_Sale_Order_Body), param);
//                foreach (var item in List)
//                {
//                    switch (item.flag_state)
//                    {
//                        case 10:
//                            item.mc_flag_state = string.Format("待订单审核");
//                            break;
//                        case 20:
//                            item.mc_flag_state = string.Format("待财务审核");
//                            break;
//                        case 30:
//                        case 40:
//                            item.mc_flag_state = string.Format("待出库审核");
//                            break;
//                        case 50:
//                        case 60:
//                            item.mc_flag_state = string.Format("待发货确认");
//                            break;
//                        case 70:
//                            item.mc_flag_state = string.Format("待收货确认");
//                            break;
//                        case 80:
//                            item.mc_flag_state = string.Format("已完成");
//                            break;
//                        case 0:
//                            item.mc_flag_state = string.Format("已作废");
//                            break;
//                    }
//                    pn.Data = List;
//                }
//            }
//            else
//            {
//                pn.Data = new List<Td_Sale_Order_Body_Query>();
//            }
//            pn.Success = true;
//            return pn;
//        }
//    }
//}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using CySoft.BLL.Base;
//using CySoft.Frame.Core;
//using CySoft.Model.Tb;
//using CySoft.Model.Td;
//using System.Collections;
//using CySoft.BLL.GoodsBLL;
//using CySoft.Model.Other;
//using CySoft.Model.Flags;
//using CySoft.BLL.SystemBLL;
//using CySoft.Model.Ts;
//using CySoft.Frame.Attributes;
//using CySoft.IBLL.Base;
//using CySoft.IBLL;
//using CySoft.Utility;
//using CySoft.IDAL;

//namespace CySoft.BLL.OrderBLL
//{
//    public class OrderBLL : BaseBLL, IOrderBLL
//    {
//        protected static readonly Type SaleOrderHead = typeof(Td_Sale_Order_Head);
//        protected static readonly Type SaleOrderBody = typeof(Td_Sale_Order_Body);
//        protected UtiletyBLL utilety = new UtiletyBLL();
//        public IInfo_UserDAL Info_UserDAL { get; set; }
//        private ITd_Sale_Order_HeadDAL Td_Sale_Order_HeadDAL { get; set; }
//        /// <summary>
//        /// 获取订单单头
//        /// cxb 2015-3-20
//        /// znt 2015-04-01 修改
//        /// mq 2016-05 修改：从业务消息跳转进来的把该消息设为已读
//        /// </summary>
//        public override BaseResult Get(Hashtable param)
//        {
//            BaseResult br = new BaseResult();
//            Td_Sale_Order_Head_Query model = DAL.GetItem<Td_Sale_Order_Head_Query>(SaleOrderHead, param);
//            if (param.ContainsKey("id_info"))
//            {
//                Hashtable info = new Hashtable();
//                info["id_info"] = param["id_info"];
//                info["new_flag_reade"] = 1;
//                DAL.UpdatePart(typeof(Info_User), info);
//                param.Remove("id_info");
//            }
//            if (model == null)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("未找到订单【{0}】！请检查后再试。", param["dh"]));
//                br.Level = ErrorLevel.Warning;
//                return br;
//            }

//            #region 订单状态
//            switch (model.flag_state)
//            {
//                case 10:
//                    model.mc_flag_state = string.Format("待订单审核");
//                    break;
//                case 20:
//                    model.mc_flag_state = string.Format("待财务审核");
//                    break;
//                case 30:
//                case 40:
//                    model.mc_flag_state = string.Format("待出库审核");
//                    break;
//                case 50:
//                case 60:
//                    model.mc_flag_state = string.Format("待发货确认");
//                    break;
//                case 70:
//                    model.mc_flag_state = string.Format("收货确认");
//                    break;
//                case 80:
//                    model.mc_flag_state = string.Format("已完成");
//                    break;
//                case 0:
//                    model.mc_flag_state = string.Format("已作废");
//                    break;
//            } 
//            #endregion

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
//            #endregion

//            br.Data = model;
//            br.Success = true;
//            return br;
//        }

//        /// <summary>
//        /// 获取订单金额和笔数汇总
//        /// </summary>
//        /// <param name="param"></param>
//        /// <returns></returns>
//        public BaseResult GetAccount(Hashtable param) {
//            BaseResult br = new BaseResult();
//            br.Data = Td_Sale_Order_HeadDAL.QueryOrderStatisticsInfo(typeof(Td_Sale_Order_Head),param);
//            br.Success = true;
//            return br;
//        }

//        /// <summary>
//        /// 获取订单体
//        /// cxb
//        /// 2015-02-11
//        /// </summary>
//        public override BaseResult GetAll(Hashtable param = null)
//        {
//            BaseResult br = new BaseResult();
//            br.Data = DAL.QueryList<Td_Sale_Order_Body_Query>(SaleOrderBody,param);
//            br.Success = true;
//            return br;
//        }

//        /// <summary>
//        /// 获取订单头列表
//        /// YYM
//        /// 2016-05-26
//        /// </summary>
//        /// <param name="param"></param>
//        /// <returns>订单头列表</returns>
//        public BaseResult Order_Head_GetAll(Hashtable param = null)
//        {
//            BaseResult br = new BaseResult();
//            br.Data = DAL.QueryList<Td_Sale_Order_Head>(typeof(Td_Sale_Order_Head), param);
//            br.Success = true;
//            return br;
//        }

//        /// <summary>
//        ///  新增 分单订单
//        ///  znt 2015-03-20
//        /// </summary>
//        [Transaction]
//        public override BaseResult Add(dynamic entity)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = (Hashtable)entity;
//            Td_Sale_Order_Head_Query head = (Td_Sale_Order_Head_Query)param["model"];
//            OrderSourceFlag source = (OrderSourceFlag)param["OrderSource"];
//            long id_user = Convert.ToInt64(param["id_user"]);

//            GoodsSkuFdBLL Fd = new CySoft.BLL.GoodsBLL.GoodsSkuFdBLL();
//            Fd.DAL = DAL;
//            Fd.Info_UserDAL = Info_UserDAL;
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
//                br = Fd.Order_Fd_Add(dh, dh, id_gys, head, param, SourceParam);
//            }
//            else
//            {
//                head.flag_delete = 0;
//                head.flag_state = (short)OrderFlag.Submitted;
//                head.je_payed = 0m;

//                // 获取 供应商信息
//                param.Clear();
//                param.Add("id_gys", head.id_gys);
//                param.Add("id_cgs", head.id_cgs);
//                var gys_cgs = DAL.GetItem<Tb_Gys_Cgs>(typeof(Tb_Gys_Cgs), param);

//                if (gys_cgs == null)
//                {
//                    br.Success = false;
//                    br.Level = ErrorLevel.Warning;
//                    br.Message.Add("采购关系不存在，无法订货");
//                    return br;
//                }

//                if (gys_cgs.flag_stop.Equals(YesNoFlag.Yes))
//                {
//                    br.Success = false;
//                    br.Level = ErrorLevel.Warning;
//                    br.Message.Add(string.Format("[{0}]和[{1}]关注已取消，无法订货", gys_cgs.mc_cgs, gys_cgs.mc_gys));
//                    return br;
//                }

//                //if (head.invoiceFlag == InvoiceFlag.General && head.slv==0)
//                //{
//                //    head.slv = tb_gys.tax;
//                //}
//                //if (head.invoiceFlag == InvoiceFlag.Vat && head.slv==0)
//                //{
//                //    head.slv = tb_gys.vat;
//                //}

//                head.je_hs = 0m;
//                IList<Td_Sale_Order_Body_Query> body = head.order_body;
//                short xh = 1;
//                foreach (var item in body)
//                {
//                    item.dh = head.dh;
//                    item.xh = xh++;
//                    item.agio = 1m;
//                    item.dj_bhs = item.dj_bhs.Digit(DigitConfig.dj);
//                    item.slv = head.slv.Digit(DigitConfig.slv);
//                    item.dj_hs = (item.dj_bhs * (1 + head.slv / 100)).Digit(DigitConfig.dj);
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
//                            body[i].je_pay = Math.Round(((body[i].je_hs / (head.je_hs == 0 ? 1 : head.je_hs)) * head.je_pay.Digit(DigitConfig.je)).Value, 2, MidpointRounding.AwayFromZero);
//                            sumAmount += body[i].je_pay.Value;
//                        }
//                    }
//                }

//                DAL.Add(head);
//                DAL.AddRange(body);

//                if (source == OrderSourceFlag.PcCart || source == OrderSourceFlag.AppCart)
//                {
//                    param.Clear();
//                    param.Add("id_cgs", head.id_cgs);
//                    param.Add("id_gys", head.id_gys);
//                    DAL.Delete(typeof(Td_Sale_Cart), param);
//                }

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
//        ///  新增 订单
//        ///  YYM 2016-04-20
//        /// </summary>
//        [Transaction]
//        public BaseResult Add_Fd(dynamic entity)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = (Hashtable)entity;

//            Td_Sale_Order_Head_Query head = (Td_Sale_Order_Head_Query)param["model"];
//            OrderSourceFlag source = (OrderSourceFlag)param["OrderSource"];
//            long id_user = Convert.ToInt64(param["id_user"]);

//            head.flag_delete = 0;
//            head.flag_state = (short)OrderFlag.Submitted;
//            head.je_payed = 0m;


//            // 获取 供应商信息
//            //param.Clear();
//            //param.Add("id_gys", head.id_gys);
//            //param.Add("id_cgs", head.id_cgs);
//            //var gys_cgs = DAL.GetItem<Tb_Gys_Cgs>(typeof(Tb_Gys_Cgs), param);

//            //if (gys_cgs == null)
//            //{
//            //    br.Success = false;
//            //    br.Level = ErrorLevel.Warning;
//            //    br.Message.Add("采购关系不存在，无法订货");
//            //    return br;
//            //}

//            //if (gys_cgs.flag_stop.Equals(YesNoFlag.Yes))
//            //{
//            //    br.Success = false;
//            //    br.Level = ErrorLevel.Warning;
//            //    br.Message.Add(string.Format("[{0}]和[{1}]关注已取消，无法订货", gys_cgs.mc_cgs, gys_cgs.mc_gys));
//            //    return br;
//            //}

//            //if (head.invoiceFlag == InvoiceFlag.General && head.slv==0)
//            //{
//            //    head.slv = tb_gys.tax;
//            //}
//            //if (head.invoiceFlag == InvoiceFlag.Vat && head.slv==0)
//            //{
//            //    head.slv = tb_gys.vat;
//            //}

//            head.je_hs = 0m;
//            IList<Td_Sale_Order_Body_Query> body = head.order_body;
//            short xh = 1;
//            foreach (var item in body)
//            {
//                item.dh = head.dh;
//                item.xh = xh++;
//                item.agio = 1m;
//                item.dj_bhs = item.dj_bhs.Digit(DigitConfig.dj);
//                item.slv = head.slv.Digit(DigitConfig.slv);
//                item.dj_hs = (item.dj_bhs * (1 + head.slv / 100)).Digit(DigitConfig.dj);
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
//                        body[i].je_pay = Math.Round(((body[i].je_hs / (head.je_hs == 0 ? 1 : head.je_hs)) * head.je_pay.Digit(DigitConfig.je)).Value, 2, MidpointRounding.AwayFromZero);
//                        sumAmount += body[i].je_pay.Value;
//                    }
//                }
//            }

//            DAL.Add(head);
//            DAL.AddRange(body);

//            if (source == OrderSourceFlag.PcCart || source == OrderSourceFlag.AppCart)
//            {
//                param.Clear();
//                param.Add("id_cgs", head.id_cgs);
//                param.Add("id_gys", head.id_gys);
//                DAL.Delete(typeof(Td_Sale_Cart), param);
//            }

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
//        ///  订单状态 订单下一步操作
//        ///  znt 2015-04-02
//        /// </summary>
//        [Transaction]
//        public BaseResult NextState(dynamic entity)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = (Hashtable)entity;
//            utilety.DAL = DAL;

//            string dh = param["dh"].ToString();
//            int new_flag_state = Convert.ToInt32(param["new_flag_state"]);
//            int id_user = Convert.ToInt32(param["id_user"]);
//            int id_user_master = Convert.ToInt32(param["id_user_master"]);

//            if (string.IsNullOrEmpty(dh))
//            {
//                br.Success = false;
//                br.Data = "dh";
//                br.Message.Add(string.Format("数据验证失败！请重新刷新页面。"));
//                return br;
//            }

//            Td_Sale_Order_Head_Query model = DAL.GetItem<Td_Sale_Order_Head_Query>(typeof(Td_Sale_Order_Head), param);
//            if (model == null)
//            {
//                br.Success = false;
//                br.Data = "count";
//                br.Message.Add(string.Format("订单不存在！订单号:{0}", dh));
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
//            if (new_flag_state == 30)
//            {
//                // 订单已出库时 且 订单待发货或部分发货 =》 直接跳到待发货状态 
//                if (model.flag_out == 2 && model.flag_fh != 2)
//                {
//                    new_flag_state = 60;
//                }
//            }

//            param.Clear();
//            param.Add("dh", dh);
//            param.Add("new_flag_state", new_flag_state);
//            int result = DAL.UpdatePart(typeof(Td_Sale_Order_Head), param);
//            if (result >= 0)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("修改订单失败！流水号:{0}", dh));
//                return br;
//            }

//            #region 添加 订单操作日志

//            Ts_Sale_Order_Log orderLog = new Ts_Sale_Order_Log();
//            orderLog.id = utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
//            orderLog.dh = model.dh;
//            orderLog.id_user = id_user;
//            orderLog.id_user_master = id_user_master;
//            orderLog.rq = DateTime.Now;

//            switch (new_flag_state)
//            {
//                case 20:
//                    orderLog.flag_type = 2;
//                    orderLog.content = string.Format("订货单已通过订货单审核");
//                    break;
//                case 30:
//                    orderLog.flag_type = 3;
//                    orderLog.content = string.Format("订货单已通过财务审核");
//                    break;
//            }

//            DAL.Add(orderLog);

//            #endregion

//            br.Success = true;
//            br.Message.Add(string.Format("修改订单状态成功！流水号:{0}", dh));
//            return br;

//        }

//        /// <summary>
//        ///  订单状态 订单退回操作
//        ///  znt 2015-04-02
//        /// </summary>
//        [Transaction]
//        public BaseResult BackState(dynamic entity)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = (Hashtable)entity;
//            utilety.DAL = DAL;

//            string dh = param["dh"].ToString();
//            int flag_state = Convert.ToInt32(param["flag_state"]);
//            int id_user = Convert.ToInt32(param["id_user"]);
//            int id_user_master = Convert.ToInt32(param["id_user_master"]);

//            if (string.IsNullOrEmpty(dh))
//            {
//                br.Success = false;
//                br.Data = "dh";
//                br.Message.Add(string.Format("数据验证失败！请重新刷新页面。"));
//                return br;
//            }

//            Td_Sale_Order_Head_Query model = DAL.GetItem<Td_Sale_Order_Head_Query>(typeof(Td_Sale_Order_Head), param);
//            if (model == null)
//            {
//                br.Success = false;
//                br.Data = "count";
//                br.Message.Add(string.Format("订单不存在！订单号:{0}", dh));
//                return br;
//            }

//            param.Clear();
//            param.Add("dh", dh);
//            param.Add("new_flag_state", 10);
//            int result = DAL.UpdatePart(typeof(Td_Sale_Order_Head), param);
//            if (result >= 0)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("修改订单失败！流水号:{0}", dh));
//                return br;
//            }

//            #region 添加 订单操作日志

//            Ts_Sale_Order_Log orderLog = new Ts_Sale_Order_Log();
//            orderLog.id = utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
//            orderLog.dh = model.dh;
//            orderLog.flag_type = 7;
//            orderLog.id_user = id_user;
//            orderLog.id_user_master = id_user_master;
//            orderLog.content = string.Format("已提交订货单，等待订货单审核");

//            switch (flag_state)
//            {
//                case 20:
//                    orderLog.content = string.Format("订货单未通过财务审核，退回重审");
//                    break;
//                case 30:
//                    orderLog.content = string.Format("订货单未通过出库审核,退回重审");
//                    break;
//                default:
//                    orderLog.content = string.Format("订货单未发货确认，退回重审");
//                    break;
//            }

//            orderLog.rq = DateTime.Now;

//            DAL.Add(orderLog);

//            #endregion

//            br.Success = true;
//            br.Message.Add(string.Format("修改订单状态成功！流水号:{0}", dh));
//            return br;

//        }

//        /// <summary>
//        /// 订单状态  作废操作
//        /// znt 2015-04-03
//        /// </summary>
//        [Transaction]
//        public BaseResult Invalid(dynamic entity)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = (Hashtable)entity;
//            utilety.DAL = DAL;

//            string dh = param["dh"].ToString();
//            int id_user = Convert.ToInt32(param["id_user"]);
//            int id_user_master = Convert.ToInt32(param["id_user_master"]);
//            int flag_state = Convert.ToInt32(param["flag_state"]);

//            if (string.IsNullOrEmpty(dh))
//            {
//                br.Success = false;
//                br.Data = "dh";
//                br.Message.Add(string.Format("数据验证失败！请重新刷新页面。"));
//                return br;
//            }

//            Td_Sale_Order_Head model = DAL.GetItem<Td_Sale_Order_Head>(typeof(Td_Sale_Order_Head), param);

//            if (model == null)
//            {
//                br.Success = false;
//                br.Data = "none";
//                br.Message.Add(string.Format("数据验证失败！请重新刷新页面。"));
//                return br;
//            }

//            if(model.flag_state>20){
//                br.Success = false;
//                br.Data = "none";
//                br.Message.Add(string.Format("该订单已审核，不能作废！"));
//                return br;
//            }

//            // 1 修改订单状态 等级为0
//            param.Clear();
//            param.Add("dh", dh);
//            param.Add("new_flag_state", 0);
//            int result = DAL.UpdatePart(typeof(Td_Sale_Order_Head), param);
//            if (result >= 0)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("订单作废失败！流水号:{0}", dh));
//                return br;
//            }

//            //#region 商品库存数量回滚
//            //param.Clear();
//            //param.Add("dh", dh);

//            //List<Td_Sale_Order_Body_Query> body_list = DAL.QueryList<Td_Sale_Order_Body_Query>(typeof(Td_Sale_Order_Body), param).ToList();
//            //if (body_list != null)
//            //{
//            //    foreach (var item in body_list)
//            //    {
//            //        param.Clear();
//            //        param.Add("id_sku", item.id_sku);
//            //        param.Add("id_gys", model.id_gys);
//            //        var gys_sp=DAL.GetItem<Tb_Gys_Sp>(typeof(Tb_Gys_Sp),param);

//            //        if(gys_sp!=null)
//            //        {
//            //            param.Add("new_sl_kc", gys_sp.sl_kc + item.sl_ck);
//            //            int res = DAL.UpdatePart(typeof(Tb_Gys_Sp),param);
//            //            if (result >= 0)
//            //            {
//            //                br.Success = false;
//            //                br.Message.Add(string.Format("订单作废失败！流水号:{0}", dh));
//            //                return br;
//            //            }
//            //        }

//            //    }
//            //}

//            //#endregion



//            #region 添加 订单操作日志

//            Ts_Sale_Order_Log orderLog = new Ts_Sale_Order_Log();
//            orderLog.id = utilety.GetNextKey(typeof(Ts_Sale_Order_Log));
//            orderLog.dh = model.dh;
//            orderLog.flag_type = 6;
//            orderLog.id_user = id_user;
//            orderLog.id_user_master = id_user_master;
//            orderLog.rq = DateTime.Now;

//            switch (flag_state)
//            {
//                case 10:
//                    orderLog.content = string.Format("订货单未通过订单审核，退回作废");
//                    break;
//                case 70:
//                    orderLog.content = string.Format("订货单系统作废");
//                    break;
//            }

//            DAL.Add(orderLog);

//            #endregion

//            br.Success = true;
//            br.Message.Add(string.Format("订单作废成功！流水号:{0}", dh));
//            return br;

//        }

//        /// <summary>
//        ///  商品sku备注 
//        ///  znt 2015-04-07
//        /// </summary>
//        public BaseResult SpSkuRemark(dynamic entity)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = (Hashtable)entity;
//            string dh = param["dh"].ToString();

//            if (string.IsNullOrEmpty(dh))
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("订单商品单号不能为空!"));
//                br.Data = "none";
//                return br;
//            }

//            if (DAL.GetCount(typeof(Td_Sale_Order_Body), param) <= 0)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("订单商品不存在！流水号:{0}", dh));
//                br.Data = "none";
//                return br;
//            }

//            int result = DAL.UpdatePart(typeof(Td_Sale_Order_Body), param);
//            if (result >= 0)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("订单商品备注失败！流水号:{0}", dh));
//                return br;
//            }

//            br.Success = true;
//            br.Message.Add(string.Format("订单商品备注成功！流水号:{0}", dh));
//            return br;
//        }

//        /// <summary>
//        /// 分页数据
//        /// znt 2015-03-30
//        /// </summary>
//        public override PageNavigate GetPage(Hashtable param = null)
//        {
//            PageNavigate pn = new PageNavigate();

//            pn.TotalCount = DAL.GetCount(typeof(Td_Sale_Order_Head), param);
//            if (pn.TotalCount > 0)
//            {
//                List<Td_Sale_Order_Head_Query> list = DAL.QueryPage<Td_Sale_Order_Head_Query>(typeof(Td_Sale_Order_Head), param).ToList();
//                foreach (var item in list)
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
//                             item.mc_flag_state = string.Format("待出库审核");
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

//                    if (item.je_pay == 0)
//                    {
//                        item.mc_payment_state = string.Format("已付款");
//                    }
//                    else if (item.je_pay > 0 && item.je_payed == 0 && item.count_nopay == 0)
//                    {
//                        item.mc_payment_state = string.Format("未付款");
//                    }
//                    else if (item.je_pay > item.je_payed && item.count_nopay == 0)
//                    {
//                        item.mc_payment_state = string.Format("部分付款");
//                    }
//                    else if (item.je_pay > item.je_payed && item.count_nopay > 0)
//                    {
//                        item.mc_payment_state = string.Format("待付款审核");
//                    }
//                    else
//                    {
//                        item.mc_payment_state = string.Format("已付款");
//                    }

//                }

//                pn.Data = list;
//                pn.Success = true;
//                return pn;
//            }

//            pn.Data = new List<Td_Sale_Order_Head_Query>();
//            pn.Success = true;
//            return pn;

//        }

//        /// <summary>
//        /// 跟新订单信息 
//        /// znt 2015-04-14
//        /// </summary>
//        public override BaseResult Update(dynamic entity)
//        {
//            BaseResult br = new BaseResult();
//            Hashtable param = (Hashtable)entity;

//            if (string.IsNullOrEmpty(param["dh"].ToString()))
//            {
//                br.Success = false;
//                br.Data = "dh";
//                br.Message.Add(string.Format("参数校验失败", param["dh"]));
//                return br;
//            }

//            if (DAL.GetCount(typeof(Td_Sale_Order_Head), param) <= 0)
//            {
//                br.Success = false;
//                br.Data = "none";
//                br.Message.Add(string.Format("跟新失败！流水号:{0}", param["dh"]));
//                return br;
//            }

//            int result = DAL.UpdatePart(typeof(Td_Sale_Order_Head), param);
//            if (result >= 0)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("跟新失败！流水号:{0}", param["dh"]));
//                return br;
//            }

//            br.Success = true;
//            br.Message.Add(string.Format("跟新成功！流水号:{0}", param["dh"]));
//            return br;
//        }

//        /// <summary>
//        ///  删除
//        ///  znt 2015-04-20
//        /// </summary>
//        public override BaseResult Stop(Hashtable param)
//        {
//            BaseResult br = new BaseResult();

//            if (string.IsNullOrEmpty(param["dh"].ToString()))
//            {
//                br.Success = false;
//                br.Data = "dh";
//                br.Message.Add(string.Format("参数校验失败", param["dh"]));
//                return br;
//            }

//            Td_Sale_Order_Head model = DAL.GetItem<Td_Sale_Order_Head>(typeof(Td_Sale_Order_Head), param);

//            if (model == null)
//            {
//                br.Success = false;
//                br.Data = "none";
//                br.Message.Add(string.Format("删除失败！流水号:{0}", param["dh"]));
//                return br;
//            }

//            if (model.flag_state != 0)
//            {
//                br.Success = false;
//                br.Data = "none";
//                br.Message.Add(string.Format("订货单当前状态不符合删除要求！流水号:{0}", param["dh"]));
//                return br;
//            }

//            param.Add("new_flag_delete", 1);

//            string dh = param["dh"].ToString();

//            int result = DAL.UpdatePart(typeof(Td_Sale_Order_Head), param);
//            if (result >= 0)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("跟新失败！流水号:{0}", param["dh"]));
//                return br;
//            }

//            var Fd = new GoodsBLL.GoodsSkuFdBLL();
//            Fd.DAL = DAL;
//            Fd.SaleOut_UpdateFlag_Fd(dh, OrderFlag.Deleted);



//            br.Success = true;
//            br.Message.Add(string.Format("跟新成功！流水号:{0}", param["dh"]));
//            return br;
//        }

//        /// <summary>
//        /// 设置 收货地址
//        /// znt 2015-04-21
//        /// </summary>
//        public BaseResult SetOrderAddress(Hashtable param)
//        {
//            BaseResult br = new BaseResult();
//            Td_Sale_Order_Head model = (Td_Sale_Order_Head)param["model"];

//            param.Clear();
//            param.Add("dh", model.dh);
//            if (DAL.GetCount(SaleOrderHead, param) <= 0)
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

//            int result = DAL.UpdatePart(SaleOrderHead, param);
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
//        public BaseResult SetOrderInvoice(Hashtable param)
//        {
//            BaseResult br = new BaseResult();
//            Td_Sale_Order_Head model = (Td_Sale_Order_Head)param["model"];

//            param.Clear();
//            param.Add("dh", model.dh);
//            if (DAL.GetCount(SaleOrderHead, param) <= 0)
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

//            int result = DAL.UpdatePart(SaleOrderHead, param);
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
//        /// 获取出库单数量 cxb 2015-6-24
//        /// </summary>
//        /// <param name="param"></param>
//        /// <returns></returns>
//        public override BaseResult GetCount(Hashtable param = null)
//        {
//            BaseResult br = new BaseResult();
//            br.Data = DAL.GetCount(typeof(Td_Sale_Out_Head),param);
//            br.Success = true;
//            return br;
//        }

//        /// <summary>
//        /// App复制订单
//        /// znt 2015-04-22
//        /// </summary>
//        [Transaction]
//        public BaseResult AppOrderClone(Hashtable param)
//        {
//            BaseResult br = new BaseResult();

//            string dh = param["dh"].ToString();
//            long id_cgs = Convert.ToInt64(param["id_cgs"]);
//            long id_user = Convert.ToInt64(param["id_user"]);

//            param.Clear();
//            param.Add("dh", dh);
//            Td_Sale_Order_Head head = DAL.GetItem<Td_Sale_Order_Head>(typeof(Td_Sale_Order_Head), param);

//            if (head == null)
//            {
//                br.Success = false;
//                br.Message.Add(string.Format("未找到订单【{0}】！请检查后再试。", dh));
//                return br;
//            }

//            // 清空购物车
//            param.Clear();
//            param.Add("id_user", id_user);
//            param.Add("id_gys", head.id_gys);
//            param.Add("id_cgs", id_cgs);
//            DAL.Delete(typeof(Td_Sale_Cart), param);

//            //获取单体
//            param.Clear();
//            param.Add("dh", dh);
//            List<Td_Sale_Order_Body> bodyList = DAL.QueryList<Td_Sale_Order_Body>(typeof(Td_Sale_Order_Body), param).ToList();
//            List<Td_Sale_Cart> cartList = new List<Td_Sale_Cart>();
//            foreach (var item in bodyList)
//            {
//                param.Clear();
//                param.Add("id_gys", head.id_gys);
//                param.Add("id_sku", item.id_sku);
//                Tb_Gys_Sp gsModel = DAL.GetItem<Tb_Gys_Sp>(typeof(Tb_Gys_Sp), param);
//                if (gsModel != null)
//                {
//                    if (gsModel.flag_stop == YesNoFlag.No)
//                    {
//                        Td_Sale_Cart cart = new Td_Sale_Cart();
//                        cart.id_user = id_user;
//                        cart.id_cgs = id_cgs;
//                        cart.id_gys = head.id_gys;
//                        cart.id_sp = item.id_sp;
//                        cart.id_sku = item.id_sku;
//                        cart.sl = item.sl;
//                        cartList.Add(cart);
//                    }
//                }

//            }

//            if (cartList.Count > 0)
//            {
//                DAL.AddRange(cartList);
//            }

//            br.Message.Add(String.Format(string.Format("订单【{0}】复制成功。", dh)));
//            br.Success = true;
//            return br;
//        }

//    }
//}

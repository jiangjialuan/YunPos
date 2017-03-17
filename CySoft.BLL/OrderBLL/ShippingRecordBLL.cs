using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Model.Tb;
using CySoft.BLL.Base;
using CySoft.Model.Td;
using CySoft.Frame.Attributes;
using CySoft.Frame.Common;
using CySoft.IDAL;
using CySoft.Model.Ts;
using System.Linq;
using CySoft.BLL.GoodsBLL;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.IBLL;

namespace CySoft.BLL.OrderBLL
{
    public class ShippingRecordBLL : BaseBLL, IShippingRecordBLL
    {

        protected static readonly Type SaleOutHead = typeof(Td_Sale_Out_Head_Query);
        protected static readonly Type SaleOutBody=typeof(Td_Sale_Out_Body);
        public ITd_Sale_Out_BodyDAL Td_Sale_Out_BodyDAL { get; set; }
        /// <summary>
        /// 出库/发货记录分页查询
        /// cxb
        /// 2015-3-17
        /// </summary>
        #region public override PageNavigate GetPage(Hashtable param = null)
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = DAL.QueryCount(typeof(Td_Sale_Out_Head),param);
            if (pn.TotalCount > 0)
            {
                pn.Data = DAL.QueryPage<Td_Sale_Out_Head_Query>(SaleOutHead,param);
            }
            else
            {
                pn.Data = new List<Td_Sale_Out_Head_Query>();
            }
            pn.Success = true;
            return pn;
        }
        #endregion

        /// <summary>
        /// 获取出库单单头
        /// cxb
        /// 2015-3-20
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        #region public override BaseResult Get(Hashtable param)
        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Td_Sale_Out_Head_Query>(SaleOutHead, param);
            if (br.Data == null)
            {
                br.Success = false;
                br.Message.Add("未找到该单头，请刷新后再试！");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            br.Success = true;
            return br;
        }
        #endregion

        /// <summary>
        /// 获取所有出库单
        /// cxb
        /// 2015-3-19
        /// </summary>
        #region public override BaseResult GetAll(Hashtable param = null)
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Td_Sale_Out_Body_Query>(SaleOutBody, param);
            br.Success = true;
            return br;
        }
        #endregion

        /// <summary>
        /// 获取所有出库单数量 cxb 2015-6-18
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult GetCount(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.GetCount(typeof(Td_Sale_Out_Head), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 发货
        /// cxb 
        /// 2015-3-25 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        #region public override BaseResult Update(dynamic entity)
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Td_Sale_Out_Head model = (Td_Sale_Out_Head)entity["model"];
            Hashtable param = new Hashtable();
            Hashtable param0 = new Hashtable();
            var ht = new Hashtable();
            param["dh"] = model.dh;
            param["new_company_logistics"] = model.company_logistics;
            param["new_no_logistics"] = model.no_logistics;
            param["new_rq_fh_logistics"] = model.rq_fh;
            param["new_id_fh"] = model.id_fh;
            param["new_flag_state"] = CySoft.Model.Flags.OrderFlag.Delivered;
            DAL.UpdatePart(typeof(Td_Sale_Out_Head),param);
            bool flag = true;
            param0["id_user_master"] = entity["id_user_master"];
            IList<Ts_Param_Business> list = DAL.QueryList<Ts_Param_Business>(typeof(Ts_Param_Business), param0);
            if (list.Where(e => e.bm == "order_ywlc_shqr"&&e.val!="0").Count() > 0)
            {
                flag = true;
            }
            else {
                flag = false;
            }
            param.Clear();
            param["dh"] = model.dh;
            param["dh_order"] = model.dh_order;
            Td_Sale_Out_BodyDAL.Updatefh(typeof(Td_Sale_Out_Body), param);
            if (!flag)
            {
                ///更新出库单状态为已完成
                ht.Add("id_gys", model.id_gys);
                ht.Add("dh", model.dh);
                ht.Add("new_flag_state", 80);
                ht.Add("new_id_edit", param["id_user"]);
                ht.Add("new_rq_edit", DateTime.Now);
                DAL.UpdatePart(typeof(Td_Sale_Out_Head), ht);


                ///更新订单状态为已完成
                ht.Remove("dh");
                ht.Add("dh", model.dh_order);
                ht.Add("flag_state", 70);
                DAL.UpdatePart(typeof(Td_Sale_Order_Head), ht);
                br.Success = true;
                param["id_gys"] = model.id_gys;
                //Td_Sale_Out_BodyDAL.Updatefh(typeof(Td_Sale_Out_Body), param);
                //Td_Sale_Out_BodyDAL.UpdateBatchConfirm(typeof(Td_Sale_Out_Body), param);
            }

            #region 分单-批量修改发货状态

            var Fd = new GoodsSkuFdBLL();
            Fd.DAL = DAL;
            //检查当前单号是否存在上级单号
            var Order_Fd = Fd.Query_Sale_Order_Fd(model.dh_order);
            //存在上级订单
            if (Order_Fd != null)
            {
                Fd.SaleOut_FH_Fd(model.dh_order);
            }

            #endregion

            br.Success = true;
            br.Message.Add(String.Format("发货成功。信息：单号：{0}", model.dh));
            return br;
        }
        #endregion

        /// <summary>
        /// 采购商确认发货 cxb 2015-4-1
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Active(Hashtable param)
        {
            BaseResult br = new BaseResult();
            var dh = param["dh_order"].ToString();

            Hashtable param2 = new Hashtable();
            param2.Add("dh",param["dh"]);
            Td_Sale_Out_Head OutHead = DAL.GetItem<Td_Sale_Out_Head>(SaleOutHead, param2);

            //如果本发货单的状态为 已发货
            if (OutHead.flag_state == OrderFlag.Delivered)
            {
                param["new_flag_state"] = CySoft.Model.Flags.OrderFlag.Receipted;
                DAL.UpdatePart(typeof (Td_Sale_Out_Head), param);
            }

            //查已收货 的发货单
            param["flag_state"] = 80;
            param["temp"] = param["dh"];
            param.Remove("dh");
            br.Data = DAL.QueryList<Td_Sale_Out_Head_Query>(SaleOutHead, param);
            List<Td_Sale_Out_Head_Query> Sale_Order_Head = (List<Td_Sale_Out_Head_Query>)br.Data;
            
            //统计已收货的总数量
            decimal? Sale_Order_Head_num = Sale_Order_Head.Sum(e => e.sl_sum).Value;

            //查询订货商品资料
            param["dh"] = param["dh_order"];
            IList<Td_Sale_Order_Body_Query> Sale_Order_Body = (IList<Td_Sale_Order_Body_Query>)DAL.QueryList<Td_Sale_Order_Body_Query>(typeof(Td_Sale_Order_Body), param);
            //统计订货总数量
            decimal? Sale_Order_Body_num = Sale_Order_Body.Sum(e => e.sl).Value;
            
            //如果发货总数量 等于 订货总数量
            if (Sale_Order_Head_num == Sale_Order_Body_num)
            {
                param.Remove("flag_state");
                param.Remove("dh_order");
                param["dh"] = param["dh"];
                //改变订货单 头状态为：80 已收货
                DAL.UpdatePart(typeof(Td_Sale_Order_Head), param);
            }

            #region 分单-批量修改发货状态

                var Fd = new GoodsSkuFdBLL();
                Fd.DAL = DAL;
                var rs = Fd.Query_Sale_Order_Head(dh);
                if (rs.flag_fd != 1)
                {
                    Fd.SaleOut_ShouHuo_Fd(dh);
                }

            #endregion

            br.Success = true;
            br.Message.Add(String.Format("确认收货成功。信息：出库单号：{0}，订单号：{1}", param["temp"], param["dh"]));

            return br;
        }

        /// <summary>
        /// 批量收货确认 cxb 2015-6-18
        /// tim 2015-7-23
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
         [Transaction]
        public BaseResult ConfirmBatch(Hashtable param) 
        {
            BaseResult br = new BaseResult();
            if (!param.ContainsKey("id_gys")||!param.ContainsKey("id_user"))
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("参数错误。");
                return br;
            }
            var ht = new Hashtable();
            ///更新出库单状态为已完成
            ht.Add("id_gys", param["id_gys"]);
            ht.Add("new_flag_state",80);
            ht.Add("new_id_edit", param["id_user"]);
            ht.Add("new_rq_edit",DateTime.Now);
            DAL.UpdatePart(typeof(Td_Sale_Out_Head),ht);

            
            ///更新订单状态为已完成
            ht.Add("flag_state", 70);
            DAL.UpdatePart(typeof(Td_Sale_Order_Head), ht);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 收货确认越过出库发货 cxb 2015-6-17
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public BaseResult ConfirmSh(Hashtable param) {
            BaseResult br = new BaseResult();
            param["new_flag_state"] = OrderFlag.Receipted;
            br.Data = DAL.UpdatePart(typeof(Td_Sale_Order_Head), param);
            br.Success = true;
            br.Message.Add(String.Format("确认收货成功。信息：订单号：{0}", param["dh"]));
            return br;
        }

        /// <summary>
        /// 作废
        /// cxb
        /// 2015-3-26
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
       [Transaction]
        public override BaseResult Save(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Td_Sale_Out_Head model = (Td_Sale_Out_Head)entity;

            Hashtable param = new Hashtable();
            Hashtable param0 = new Hashtable();

            OrderFlag new_flag_state = CySoft.Model.Flags.OrderFlag.WaitDelivery;
            param["dh"] = model.dh;
            param["new_id_edit"] = model.id_edit;
            param["new_rq_edit"] = DateTime.Now;
            param["new_remark"] = model.remark;
            param["new_flag_state"] = CySoft.Model.Flags.OrderFlag.Invalided;
            DAL.UpdatePart(typeof(Td_Sale_Out_Head), param);

            param.Clear();
            param["dh_order"] =model.dh_order;
            param0["id_user_master"] = model.id_edit;
            param0["val"] = 1;
            param0["not_bm"] = "order_ywlc_cwsh";
            param0.Add("sort", "sort_id");
            param0.Add("dir", "asc");
            IList<Ts_Param_Business> list = DAL.QueryList<Ts_Param_Business>(typeof(Ts_Param_Business), param0);

            Ts_Param_Business item0 = list.First();
            
            //是否出库审核
            if (list.Where(m => m.bm == "order_ywlc_cksh").Count() > 0) 
            {
                item0.bm = "order_ywlc_cksh";
            }
            //是否发货确认
            else if (list.Where(m => m.bm == "order_ywlc_fhqr").Count() > 0)
            {
                item0.bm = "order_ywlc_fhqr";
            }
            
            switch (item0.bm)
            {
                case "order_ywlc_cksh":
                    new_flag_state = OrderFlag.WaitOutputCheck;
                    break;
                case "order_ywlc_fhqr":
                    new_flag_state = OrderFlag.WaitDelivery;
                    break;
            }
            param["flag_state"] = new_flag_state;
            int flag = Td_Sale_Out_BodyDAL.Updatezf(typeof(Td_Sale_Out_Body), param);

            #region 分单-批量修改发货状态

                var Fd = new GoodsSkuFdBLL();
                Fd.DAL = DAL;
                //检查当前单号是否存在上级单号
                var Order_Fd = Fd.Query_Sale_Order_Fd(model.dh_order);
                //存在上级订单
                if (Order_Fd != null)
                {

                    Fd.SaleOut_UpdateFlag_Fd(model.dh_order, new_flag_state);
                }

            #endregion

            br.Success = true;
            br.Message.Add(String.Format("作废成功。信息：单号：{0}", model.dh));

            return br;
        }

        /// <summary>
        /// 删除
        /// cxb
        /// 2015-3-26 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult br = new BaseResult();
            param["new_rq_edit"] = DateTime.Now;
            param["new_flag_delete"] = 1;
            DAL.UpdatePart(typeof(Td_Sale_Out_Head), param);
            br.Success = true;
            br.Message.Add(String.Format("删除成功。信息：单号：{0}", param["dh"]));
            return br;
        }

        /// <summary> 
        /// 新增
        /// 作者:cxb
        /// 日期:2014-02-27
        /// </summary>
        #region public override BaseResult Add(dynamic entity)
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = (Hashtable)entity;
            Hashtable param0 = new Hashtable();
            OrderFlag new_flag_state = CySoft.Model.Flags.OrderFlag.WaitDelivery;
            Td_Sale_Out_Head head = (Td_Sale_Out_Head)param["head"];
            List<Td_Sale_Out_Body> body =(List<Td_Sale_Out_Body>)param["body"];
            Ts_Sale_Order_Log ts =(Ts_Sale_Order_Log)param["Ts"];
            ShoppingInfo shopping = (ShoppingInfo)param["ShoppingInfo"];
            short xh = 1;
            int dj_digit = 2;
            int je_digit = 2;
            foreach (Td_Sale_Out_Body item in body)
            {
                xh = xh++;
                item.dh = head.dh;//设置单号
                item.xh = xh;
                item.sl = item.sl.Digit(dj_digit);                  //数量        
                item.sl_ck = item.sl_ck.Digit(dj_digit);
                item.sl_fh = item.sl_fh.Digit(dj_digit); //折扣
                head.sl_sum = +item.sl;
                head.flag_delete = 0;
                head.rq_fh = DateTime.Now;
                xh++;
            }
            Ts_Param_Business item0= new Ts_Param_Business();
            param0["id_user_master"] = param["id_user_master"];
            IList<Ts_Param_Business> list = DAL.QueryList<Ts_Param_Business>(typeof(Ts_Param_Business), param0);
            list = list.Where(m => m.bm.IndexOf("ywlc") != -1 && m.val != "0" && m.bm.IndexOf("order_ywlc_cwsh") == -1).ToList();
            if (list.Where(m => m.bm=="order_ywlc_cksh").Count() > 0) {
                item0.bm = "order_ywlc_cksh";
            }
            else if (list.Where(m => m.bm == "order_ywlc_fhqr").Count() > 0)
            {
                item0.bm = "order_ywlc_fhqr";
            }
            
            switch (item0.bm)
            {
                case "order_ywlc_cksh":
                    if (list.Where(m => m.bm != "order_ywlc_cksh").Count() > 0)
                    {
                        if (list.Where(m => m.bm != "order_ywlc_cksh").First().bm == "order_ywlc_shqr")
                        {
                            new_flag_state = OrderFlag.Delivered;
                        }
                        else
                        {
                            new_flag_state = OrderFlag.WaitDelivery;
                        }
                    }
                    else {
                        if (list.Count == 0)
                        {
                            new_flag_state = OrderFlag.WaitDelivery;
                        }
                        else
                        {
                            new_flag_state = OrderFlag.Receipted;
                        }
                    }
                    break;
                case "order_ywlc_fhqr":
                    if (list.Where(m => m.bm != "order_ywlc_fhqr").Count()==0)
                    {
                        new_flag_state = OrderFlag.Receipted;
                    }
                    else
                    {
                        new_flag_state = OrderFlag.Delivered;
                    }
                    break;
                case "order_ywlc_shqr":
                    new_flag_state = OrderFlag.Delivered;
                    break;
                default:
                    new_flag_state = OrderFlag.Receipted;
                    break;
            }
            head.flag_state = new_flag_state;
            DAL.AddRange(body);
            DAL.Add(head);
            param.Clear();
            param["dh_order"] = head.dh_order;
            param["dh"] = head.dh;
            int flag = Td_Sale_Out_BodyDAL.UpdateBody(typeof(Td_Sale_Out_Body), param);
            if (item0.bm == "order_ywlc_fhqr")
            {
                param.Clear();
                param["dh"] = head.dh;
                param["new_company_logistics"] = shopping.company_logistics;
                param["new_no_logistics"] = shopping.no_logistics;
                param["new_rq_fh_logistics"] = shopping.rq_fh;
                param["new_id_fh"] =head.id_create;
                DAL.UpdatePart(typeof(Td_Sale_Out_Head), param);
                param.Clear();
                param["dh"] = head.dh;
                param["dh_order"] = head.dh_order;
                Td_Sale_Out_BodyDAL.UpdatefhCrossingck(typeof(Td_Sale_Out_Body), param);
            }
            else if (list.Where(m => m.bm != "order_ywlc_cksh").Count()==0)
            {
                param["dh"] = head.dh;
                param.Clear();
                param["new_company_logistics"] = shopping.company_logistics;
                param["new_no_logistics"] = shopping.no_logistics;
                param["new_rq_fh_logistics"] = shopping.rq_fh;
                param["new_id_fh"] = head.id_create;
                DAL.UpdatePart(typeof(Td_Sale_Out_Head), param);
                param.Clear();
                param["dh"] = head.dh;
                param["dh_order"] = head.dh_order;
                Td_Sale_Out_BodyDAL.Updatefh(typeof(Td_Sale_Out_Body), param);
            }
            else if (list.Where(m => m.bm != "order_ywlc_cksh").First().bm == "order_ywlc_shqr")
            {
                param["dh"] = head.dh;
                param.Clear();
                param["new_company_logistics"] = shopping.company_logistics;
                param["new_no_logistics"] = shopping.no_logistics;
                param["new_rq_fh_logistics"] = shopping.rq_fh;
                param["new_id_fh"] =head.id_create;
                DAL.UpdatePart(typeof(Td_Sale_Out_Head), param);
                param.Clear();
                param["dh"] = head.dh;
                param["dh_order"] = head.dh_order;
                Td_Sale_Out_BodyDAL.Updatefh(typeof(Td_Sale_Out_Body), param);
            }

            #region 跳入分单批量插入出库单

            var Fd = new GoodsSkuFdBLL();
            Fd.DAL = DAL;
            var rs = Fd.Query_Sale_Order_Head(head.dh_order);
            if (rs.flag_fd != 1)
            {
                //检查当前单号是否存在上级单号
                var Order_Fd = Fd.Query_Sale_Order_Fd(head.dh_order);
                //存在上级订单
                if (Order_Fd != null)
                {
                    Fd.SaleOut_Out_Fd(head.dh_order, head.dh);
                }
            }

            #endregion

            ts.dh=head.dh_order;
            ts.content = "订单已通过出库审核";
            DAL.Add(ts);
            br.Success = true;
            br.Data = head;
            br.Message.Add(String.Format("新增出库单。信息：单号：{0}", head.dh));
            return br;
        }
        #endregion
    }
}

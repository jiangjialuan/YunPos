using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using CySoft.Model.Td;
using System.Collections;
using CySoft.IDAL;
using CySoft.IBLL;
using CySoft.Frame.Attributes;
using CySoft.Utility;
using CySoft.Model.Other;

namespace CySoft.BLL.FundsBLL
{
    public class FundsBLL : BaseBLL, IFundsBLL
    {
        public ITd_sale_payDAL Td_sale_payDAL { get; set; }
        /// <summary>
        /// 分页查询 cxb 2015-4-22
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PageNavigate GetPage(System.Collections.Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = DAL.QueryCount(typeof(Td_Sale_Pay), param);
            if (pn.TotalCount > 0)
            {
                pn.Data = Td_sale_payDAL.QueryPageforView(typeof(Td_Sale_Pay), param);
            }
            else
            {
                pn.Data = new List<Td_sale_pay_Query>();
            }
            pn.Success = true;
            return pn;
        }

        /// <summary>
        /// 取消收款记录 cxb 2015-4-22
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult Update(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = (Hashtable)entity;
            param["flag_state"] = 1;
            if (DAL.GetCount(typeof(Td_Sale_Pay), param) > 0)
            {
                param.Remove("flag_state");
                br.Data = DAL.UpdatePart(typeof(Td_Sale_Pay), param);
            }
            br.Success = true;
            br.Message.Add(String.Format("修改收款记录。流水号：{0}", param["id"]));
            return br;
        }

        /// <summary>
        /// 删除收款记录 cxb 2015-4-22
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.Delete(typeof(Td_Sale_Pay), param);
            return br;
        }


        /// <summary>
        /// 新增 
        /// znt 2015-04-27
        /// </summary>
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            Td_Sale_Pay model = (Td_Sale_Pay)entity;

            param.Clear();
            param.Add("dh", model.dh_order);
            Td_Sale_Order_Head head = DAL.GetItem<Td_Sale_Order_Head>(typeof(Td_Sale_Order_Head), param);

            if (head == null)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("添加付款记录失败，该订货单号【{0}】不存在或资料已缺失！", model.dh_order));
                br.Data = "none";
                return br;
            }

            // 计算尚未付款金额
            decimal notYetPay = head.je_pay.Value - head.je_payed.Value;

            // 判断订单是否完全付款
            if (notYetPay <= 0)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("付款提示，该单号【{0}】已完成付款，不需要重复付款", model.dh_order));
                return br;
            }

            if (model.je > notYetPay)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("添加付款记录失败，总金额不允许大于订单待付款金额!", model.dh_order));
                return br;
            }

            model.id_cgs = head.id_cgs;
            model.id_gys = head.id_gys;
            model.flag_delete = 0;

            DAL.Add(model);
            br.Success = true;
            return br;
        }

        /// <summary>
        ///  确认收款
        /// znt 2015-04-27
        /// </summary>
        [Transaction]
        public BaseResult PayConfirm(Hashtable param)
        {
            BaseResult br = new BaseResult();
            string dh = param["dh"].ToString();
            long id_edit = Convert.ToInt64(param["id_edit"]);

            param.Clear();
            param.Add("dh", dh);
            Td_Sale_Pay model = DAL.GetItem<Td_Sale_Pay>(typeof(Td_Sale_Pay), param);
            if (model == null)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("付款失败，该付款单号【{0}】不存在或资料已缺失！", dh));
                br.Data = "none";
                return br;
            }

            if (model.flag_state != 1)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("付款失败，该付款单号【{0}】当前状态不符合付款要求！", dh));
                br.Data = "none";
                return br;
            }

            param.Clear();
            param.Add("dh", model.dh_order);
            Td_Sale_Order_Head head = DAL.GetItem<Td_Sale_Order_Head>(typeof(Td_Sale_Order_Head), param);

            if (head == null)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("付款失败，该单号【{0}】不存在或资料已缺失！", model.dh_order));
                return br;
            }

            // 尚未付款金额
            decimal notYetPay = head.je_pay.Value - head.je_payed.Value;

            // 判断订单是否完全付款
            if (notYetPay <= 0)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("付款提示，该单号【{0}】已完成付款，不需要重复付款", model.dh_order));
                return br;
            }

            // 检查差额
            if (model.je.Value >= notYetPay)
            {
                model.remark = string.Format("【原收款单金额为:{0}】", model.je.Value.ToString("0.00")) + model.remark;
                model.je = notYetPay;

                // 剩下未审核的付款单直接作废 
                param.Clear();
                param.Add("dh_order",head.dh);
                param.Add("not_dh", model.dh); // 排除当前操作的付款单
                param.Add("flag_state",1);
                param.Add("new_flag_state", 0);
                param.Add("new_id_edit", id_edit);
                param.Add("new_rq_edit", DateTime.Now);
                DAL.UpdatePart(typeof(Td_Sale_Pay),param);

                
            }

            // 更新订单的付款金额
            param.Clear();
            param.Add("dh", head.dh);
            param.Add("new_je_payed", head.je_payed.Digit(DigitConfig.je) + model.je.Digit(DigitConfig.je));
            param.Add("new_id_edit", id_edit);
            param.Add("new_rq_edit", DateTime.Now);
            DAL.UpdatePart(typeof(Td_Sale_Order_Head), param);

            // 跟新当前付款单的状态和金额
            param.Clear();
            param.Add("dh", model.dh);
            param.Add("new_je",model.je);
            param.Add("new_flag_state", 2);
            param.Add("new_id_edit", id_edit);
            param.Add("new_rq_edit", DateTime.Now);
            param.Add("new_remark", model.remark);

            int result = DAL.UpdatePart(typeof(Td_Sale_Pay), param);
            if (result > 0)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("付款失败，流水号：{0}", dh));
                br.Data = "none";
                return br;
            }

            br.Success = true;
            br.Message.Add(string.Format("付款成功，流水号：{0}", dh));
            return br;
        }

        /// <summary>
        /// 取消收款确认
        /// znt 2015-04-29
        /// </summary>
        [Transaction]
        public BaseResult CancelRecord(Hashtable param)
        {
            BaseResult br = new BaseResult();
            string dh = param["dh"].ToString(); // 收款单号
            long id_edit = Convert.ToInt64(param["id_edit"]);

            param.Clear();
            param.Add("dh", dh);
            Td_Sale_Pay model = DAL.GetItem<Td_Sale_Pay>(typeof(Td_Sale_Pay), param);
            if (model == null)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("取消收款失败，收款流水号【{0}】不存在或资料已缺失！", dh));
                br.Data = "none";
                return br;
            }

            if (model.flag_state != 2)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("取消收款失败，该付款单号【{0}】当前状态不符合取消收款确认要求！", dh));
                br.Data = "none";
                return br;
            }

            param.Clear();
            param.Add("dh", model.dh_order);
            Td_Sale_Order_Head head = DAL.GetItem<Td_Sale_Order_Head>(typeof(Td_Sale_Order_Head), param);
            if (head == null)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("取消收款失败，该收款流水号【{0}】的销售单【{1}】不存在或资料已缺失！", model.dh, model.dh_order));
                br.Data = "none";
                return br;
            }

            #region 更新订单已付金额

            param.Clear();
            param.Add("dh", head.dh);
            param.Add("new_je_payed", head.je_payed - model.je);
            param.Add("new_id_edit", model.id_edit);
            param.Add("new_rq_edit", DateTime.Now);
            DAL.UpdatePart(typeof(Td_Sale_Order_Head), param);

            #endregion

            param.Clear();
            param.Add("dh", model.dh);
            param.Add("new_flag_state", 0);
            param.Add("new_id_edit", id_edit);
            param.Add("new_rq_edit", DateTime.Now);

            int result = DAL.UpdatePart(typeof(Td_Sale_Pay), param);
            if (result > 0)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("取消收款确认失败，流水号：{0}", dh));
                br.Data = "none";
                return br;
            }

            br.Success = true;
            br.Message.Add(string.Format("取消付款确认成功，流水号：{0}", dh));
            return br;

        }

        /// <summary>
        ///  供应商 添加付款记录
        ///  znt 2015-04-28
        /// </summary>
        [Transaction]
        public BaseResult PayForGys(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            Td_Sale_Pay model = (Td_Sale_Pay)entity;

            param.Clear();
            param.Add("dh", model.dh_order);
            Td_Sale_Order_Head head = DAL.GetItem<Td_Sale_Order_Head>(typeof(Td_Sale_Order_Head), param);

            if (head == null)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("添加付款记录失败，该销售单号【{0}】不存在或资料已缺失！", model.dh_order));
                br.Data = "none";
                return br;
            }

            // 尚未付款金额
            decimal notYetPay = head.je_pay.Value - head.je_payed.Value;

            // 判断订单是否完全付款
            if (notYetPay <= 0)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("付款提示，该单号【{0}】已完成付款，不需要重复付款", model.dh_order));
                return br;
            }

            // 检查差额
            if (model.je.Value >= notYetPay)
            {
                model.je = notYetPay;

                //未审核的付款单直接作废 
                param.Clear();
                param.Add("dh_order", head.dh);
                param.Add("flag_state", 1);
                param.Add("new_flag_state", 0);
                param.Add("new_id_edit", model.id_edit);
                param.Add("new_rq_edit", DateTime.Now);
                DAL.UpdatePart(typeof(Td_Sale_Pay), param);

            }

            // 更新订单状态
            param.Clear();
            param.Add("dh", head.dh);
            param.Add("new_je_payed", head.je_payed.Digit(DigitConfig.je) + model.je.Digit(DigitConfig.je));
            param.Add("new_id_edit", model.id_edit);
            param.Add("new_rq_edit", DateTime.Now);
            DAL.UpdatePart(typeof(Td_Sale_Order_Head), param);


            model.id_cgs = head.id_cgs;
            model.id_gys = head.id_gys;
            model.flag_delete = 0;
            model.flag_state = 2; // 直接已审核

            DAL.Add(model);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 不分页获取所有
        /// znt 2015-04-27
        /// </summary>
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            List<Td_sale_pay_Query> list = DAL.QueryList<Td_sale_pay_Query>(typeof(Td_Sale_Pay), param).ToList();
            if (list != null)
            {
                br.Data = list;
            }
            else
            {
                br.Data = new List<Td_sale_pay_Query>();
            }

            br.Success = true;
            return br;
        }

        /// <summary>
        /// 删除单个付款记录
        /// znt 2015-05-11
        /// </summary>
        public BaseResult Remove(Hashtable param)
        {
            BaseResult br = new BaseResult();
            string dh = param["dh"].ToString();

            param.Clear();
            param.Add("dh", dh);
            Td_Sale_Pay model = DAL.GetItem<Td_Sale_Pay>(typeof(Td_Sale_Pay), param);

            // 未审核 删除记录 
            if (model!=null && model.flag_state != 1)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add(string.Format("删除失败，该付款单号【{0}】当前状态不符合删除要求！", dh));
                br.Data = "none";
                return br;
            }

            param.Clear();
            param.Add("dh", dh);
            DAL.Delete(typeof(Td_Sale_Pay), param);

            br.Success = true;
            br.Message.Add(string.Format("删除付款单成功，流水号：{0}",dh));
            return br;
        }

    }
}

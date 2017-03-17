using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Tb;
using CySoft.Model.Td;
using CySoft.Model.Ts;
using CySoft.Utility;

namespace CySoft.Controllers.GoodsCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class GoodsFdController : BaseController
    {
        /// <summary>
        /// 新增 引用商品 [单个]
        /// YYM 2016-04-20
        /// 【json】
        /// </summary>
        /// <param name="obj">obj = {"id_sp":"4946746","id_gys":"5345"}</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ActionPurview(false)]
        public ActionResult Add(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                if (obj.Trim() == string.Empty)
                {
                    br.Success = false;
                    br.Message.Add("获取商品信息失败！请重试试。");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                Hashtable param = new Hashtable();
                Hashtable p1 = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();

                ParamVessel p = new ParamVessel();
                p.Add("id_gys", String.Empty, HandleType.ReturnMsg);//供应商
                p.Add("id_mc", String.Empty, HandleType.ReturnMsg);//商品名称
                p.Add("id_sku", String.Empty, HandleType.ReturnMsg);//sku
                p.Add("bm_Interface", string.Empty, HandleType.ReturnMsg);//编码
                p.Add("id_spfl", string.Empty, HandleType.ReturnMsg);//分类
                param = p1.Trim(p);

                param.Add("dj_dh", p1["dj_dh"] ?? 0);//单价
                param.Add("sl_dh_min", p1["sl_dh_min"] ?? 0);//最少订货量

                //获取 当前用户 的 供应商编码
                param.Add("id_gys_fd", GetLoginInfo<long>("id_supplier"));
                param.Add("id_user", GetLoginInfo<long>("id_user"));

                //插入 商品分单关系
                br = BusinessFactory.GoodsSkuFd.Add_Sdk_Fd(param);

                if (br.Success)
                {
                    WriteDBLog(LogFlag.Bill, br.Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(br);
        }

        /// <summary>
        /// 新增分单sku商品关系 [批量]
        /// YYM 2016-04-20
        /// 【json】【未完成】
        /// </summary>
        /// <param name="obj">obj=[{"id_sp":"4950942","id_gys":"5345"},{"id_sp":"4946747","id_gys":"5345"},{"id_sp":"4946746","id_gys":"5345"}]</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ActionPurview(false)]
        [Transaction]
        public ActionResult Add_List(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                //获取商品资料 //id_sku gys_fd id_gys
                List<Tb_sp_sku_fd> list = JSON.Deserialize<List<Tb_sp_sku_fd>>(obj);

                if (list.Count > 0)
                {
                    br.Success = false;
                    br.Message.Add("获取商品信息失败！请重试试。");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                //获取 当前用户 的 供应商编码
                long id_supplier = GetLoginInfo<long>("id_supplier");

                //循环商品列表
                foreach (var item in list)
                {
                    if (item.id_sku.ToString() == string.Empty)
                    {
                        br.Success = false;
                        br.Message.Add("获取商品sku失败！请重试试。");
                        br.Level = ErrorLevel.Warning;
                        return Json(br);
                    }
                    if (item.id_gys.ToString() == string.Empty)
                    {
                        br.Success = false;
                        br.Message.Add("获取商品供应商失败！请重试试。");
                        br.Level = ErrorLevel.Warning;
                        return Json(br);
                    }

                    //获取 当前用户 的 供应商编码
                    item.id_gys_fd = (int)id_supplier;
                    br = BusinessFactory.GoodsSkuFd.Add_Sdk_Fd(list);
                }

                if (br.Success)
                {
                    WriteDBLog(LogFlag.Base, br.Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(br);
        }

        /// <summary>
        /// 采购商 提交新增
        /// znt 2015-03-19
        /// mq 2016-05-30 修改，新增订单提交成功后通知采购商
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        [Transaction]
        public ActionResult Submit()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                int gys_send = Convert.ToInt32(param["gys_send"]);
                ParamVessel p = new ParamVessel();
                p.Add("orderData", String.Empty, HandleType.ReturnMsg); // 订单头信息
                p.Add("invoiceFlag", String.Empty, HandleType.ReturnMsg); // 发票类型
                p.Add("soure", (long)0, HandleType.DefaultValue); // 订单来源 默认PcNew
                param = param.Trim(p);

                //订单头信息
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
                //订单编码 赋值 
                br = BusinessFactory.Utilety.GetNextDH(model, typeof(Td_Sale_Order_Head));

                if (!br.Success)
                {
                    return Json(br);
                }

                #region 发票类型
                string invoiceFlag = param["invoiceFlag"].ToString();
                int soure = Convert.ToInt32(param["soure"]);
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

                #endregion
                model.id_cgs = GetLoginInfo<long>("id_buyer");
                model.id_user_bill = GetLoginInfo<long>("id_user");
                model.id_user_master = GetLoginInfo<long>("id_user_master");

                model.id_create = GetLoginInfo<long>("id_user");
                model.id_edit = GetLoginInfo<long>("id_user");

                long newOrderLogId = BusinessFactory.Utilety.GetNextKey(typeof(Ts_Sale_Order_Log));

                Hashtable param2 = new Hashtable();
                param2.Clear();
                //下单入参
                param2.Add("newOrderLogId", newOrderLogId);
                param2.Add("id_user", GetLoginInfo<long>("id_user"));
                param2.Add("model", model);
                //备份原入参
                param2.Add("orderData", param["orderData"]);
                param2.Add("invoiceFlag", param["invoiceFlag"]);
                param2.Add("soure", param["soure"]);

                switch (soure)
                {        //PC端 购物车
                    case (int)OrderSourceFlag.PcCart:
                        param2.Add("OrderSource", OrderSourceFlag.PcCart);
                        break;
                    //PC端 复制订单
                    case (int)OrderSourceFlag.PcClone:
                        param2.Add("OrderSource", OrderSourceFlag.PcClone);
                        break;
                    default:
                        param2.Add("OrderSource", OrderSourceFlag.PcNew);
                        break;
                }
                br = BusinessFactory.Order.Add(param2);

                if (br.Success)
                {
                    br.Data = model.dh;
                    WriteDBLog(LogFlag.Bill, br.Message);
                    Hashtable loginInfo = Session["LoginInfo"] != null ? (Hashtable)Session["LoginInfo"] : new Hashtable();
                    if (string.IsNullOrEmpty(loginInfo["companyname"].ToString()))
                    {
                        return Json(br);
                    }
                    string name = loginInfo["companyname"].ToString();
                    BaseResult br1 = new BaseResult();
                    long id_info = BusinessFactory.Utilety.GetNextKey(typeof(Info));//获取下一个Id自增值
                    param.Add("gys_send", gys_send);
                    param.Add("id", id_info);
                    param.Add("Title", "客户【"+name+"】下单了，" + model.dh);
                    param.Add("content", "sales,"+model.dh);
                    param.Add("filename", "");
                    param.Add("fileSize", "");
                    param.Add("id_info_type", 0);
                    param.Add("id_create", GetLoginInfo<long>("id_user"));
                    param.Add("id_master", GetLoginInfo<long>("id_user_master"));
                    param.Add("flag_from", "pc");
                    param.Add("bm", "business");
                    br1 = BusinessFactory.Info.Add(param);
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

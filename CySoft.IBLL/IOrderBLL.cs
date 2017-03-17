using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IBLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using System.Collections;

namespace CySoft.IBLL
{
    public interface IOrderBLL : IBaseBLL
    {
        /// <summary>
        /// 订单_验证并修改状态
        /// </summary>
        /// <param name="entity">单据信息</param>
        /// <returns>状态</returns>
        BaseResult NextState(dynamic entity);
        /// <summary>
        /// 退单_验证并修改状态
        /// </summary>
        /// <param name="entity">单据信息</param>
        /// <returns>状态</returns>
        BaseResult BackState(dynamic entity);
        /// <summary>
        /// 作废订单_验证并修改状态
        /// </summary>
        /// <param name="entity">单据信息</param>
        /// <returns>状态</returns>
        BaseResult Invalid(dynamic entity);
        /// <summary>
        /// 商品备注_sku
        /// </summary>
        /// <param name="entity">订单信息</param>
        /// <returns>状态</returns>
        BaseResult SpSkuRemark(dynamic entity);
        /// <summary>
        /// 设置收货地址
        /// </summary>
        /// <param name="param">订单单头</param>
        /// <returns></returns>
        BaseResult SetOrderAddress(Hashtable param);
        /// <summary>
        /// 设置订单发票
        /// </summary>
        /// <param name="param">订单单头</param>
        /// <returns></returns>
        BaseResult SetOrderInvoice(Hashtable param);
        /// <summary>
        /// 复制订单_APP
        /// </summary>
        /// <param name="param">订单信息</param>
        /// <returns></returns>
        BaseResult AppOrderClone(Hashtable param);
        /// <summary>
        /// 获取订单金额和笔数汇总
        /// </summary>
        /// <param name="param">订单单头</param>
        /// <returns></returns>
        BaseResult GetAccount(Hashtable param);
        /// <summary>
        /// 分单使用的 新增 订单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        BaseResult Add_Fd(dynamic entity);

        /// <summary>
        /// 获取订单头列表
        /// YYM
        /// 2016-05-26
        /// </summary>
        /// <param name="param"></param>
        /// <returns>订单头列表</returns>
        BaseResult Order_Head_GetAll(Hashtable param = null);

    }
}

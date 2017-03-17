using System.Collections;
using System.Collections.Generic;
using CySoft.Frame.Attributes;
using CySoft.IBLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Tb;
using CySoft.Model.Td;

namespace CySoft.IBLL
{
    public interface IGoodsSkuFdBLL : IBaseBLL
    {
        /// <summary>
        /// 引用 SKU 分单商品
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        BaseResult Add_Sdk_Fd(dynamic entity);

        /// <summary>
        /// 检查订单是否存在 分单商品
        /// </summary>
        /// <param name="id_skuList">sku商品列表</param>
        /// <param name="id_gys_fd">分单者 的 供应商编码</param>
        /// <returns>分单商品关系数据</returns>
        List<Tb_sp_sku_fd> Check_Order_Goods_Fd(List<long?> id_skuList, long id_gys_fd);

        /// <summary>
        /// 查询订单分单关系
        /// 【单个】
        /// </summary>
        /// <param name="dh"></param>
        /// <returns></returns>
        Td_Sale_Order_Fd Query_Sale_Order_Fd(string dh);

        /// <summary>
        /// 分单函数 采购商-订货下单
        /// </summary>
        /// <param name="dh">母单ID</param>
        /// <param name="path">订单路径</param>
        /// <param name="id_gys">当前供应商</param>
        /// <param name="OrderData">对应的商品资料</param>
        /// <param name="param3">提交的数据</param>
        /// <param name="param">原入参</param>
        /// <returns></returns>
        [Transaction]
        BaseResult Order_Fd_Add(string dh, string path, long id_gys, Td_Sale_Order_Head_Query OrderData, Hashtable param3, Hashtable param);

        /// <summary>
        /// 分单函数 供应商-代客下单
        /// </summary>
        /// <param name="dh">母单ID</param>
        /// <param name="path">订单路径</param>
        /// <param name="id_gys">当前供应商</param>
        /// <param name="OrderData">对应的商品资料</param>
        /// <param name="param3">提交的数据</param>
        /// <param name="param">原入参</param>
        /// <returns></returns>
        [Transaction]
        BaseResult Order_Fd_Add2(string dh, string path, long id_gys, Td_Sale_Order_Head_Query OrderData, Hashtable param3, Hashtable param);

        /// <summary>
        /// 分单-出库-更新状态
        /// </summary>
        /// <param name="dh">当前订单号</param>
        /// <param name="ck_dh">出库单号</param>
        /// <returns></returns>
        [Transaction]
        BaseResult SaleOut_Out_Fd(string dh, string ck_dh);
        /// <summary>
        /// 分单-发货-更新状态 **
        /// </summary>
        /// <param name="dh">当前订单号</param>
        /// <returns></returns>
        [Transaction]
        BaseResult SaleOut_FH_Fd(string dh);

        /// <summary>
        /// 分单--更新--收货状态
        /// </summary>
        /// <param name="dh">收货单号</param>
        /// <returns></returns>
        [Transaction]
        BaseResult SaleOut_ShouHuo_Fd(string dh);

        /// <summary>
        /// 主要用途 更新 作废/删除 状态
        /// </summary>
        /// <param name="dh">单号</param>
        /// <param name="op">状态</param>
        /// <returns></returns>
        [Transaction]
        BaseResult SaleOut_UpdateFlag_Fd(string dh, OrderFlag op);

        /// <summary>
        /// 查询订单头内容
        /// </summary>
        /// <param name="dh"></param>
        /// <returns></returns>
        Td_Sale_Order_Head Query_Sale_Order_Head(string dh);

        /// <summary>
        /// 根据订单号 获取所有子单单号
        /// </summary>
        /// <param name="dh">母单单号</param>
        /// <returns>子单列表</returns>
        List<string> Get_Fd_Sale_Order_List(string dh);

        /// <summary>
        /// 检查SKU分单商品 是否 已存在关联
        /// </summary>
        /// <param name="id_gys_fd">当前用户的 供应商编码</param>
        /// <param name="id_sku">商品的sku编码</param>
        /// <returns>true/false 存在/不存在</returns>
        bool Check_Sdk_Fd(int id_gys_fd, int id_sku);

        /// <summary>
        /// 查询发货头列表
        /// </summary>
        /// <param name="dh">订单号</param>
        /// <returns></returns>
        Td_Sale_Out_Head Get_Sale_Out_Head_Query(string dh);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Utility.Pay.YeePay
{
    /// <summary>
    /// API返回结果对象
    /// </summary>
    public class RespondJson
    {
        /// <summary>
        ///加密的响应结果
        /// </summary>
        public string data;
    }
    #region 返回结果公有属性
    /// <summary>
    /// 返回结果公有属性
    /// </summary>
    public class BaseRespondJson
    {
        /// <summary>
        /// 主账户商户编号
        /// </summary>
        public string customernumber { get; set; }
        /// <summary>
        /// 返回码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 子账户编号
        /// </summary>
        public string ledgerno { get; set; }
        /// <summary>
        /// 签名信息
        /// </summary>
        public string hmac { get; set; }
    } 
    #endregion

    #region 返回失败结果
    /// <summary>
    /// 返回失败结果
    /// </summary>
    public class FailJson
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 错误描述
        /// </summary>
        public string msg { get; set; }
    } 
    #endregion

    #region 注册返回结果
    /// <summary>
    /// 注册返回结果
    /// </summary>
    public class RegisterResultJson : BaseRespondJson
    {
        /// <summary>
        /// 注册请求号
        /// </summary>
        public string requestid { get; set; }
        /// <summary>
        /// 错误描述
        /// </summary>
        public string msg { get; set; }
    } 
    #endregion

    #region 资质上传返回结果
    /// <summary>
    /// 资质上传返回结果
    /// </summary>
    public class CertifiedResultJson : BaseRespondJson
    {
        /// <summary>
        /// 照片类型
        /// </summary>
        public string filetype { get; set; }
        /// <summary>
        /// 分账状态
        /// </summary>
        public bool active { get; set; }

    } 
    #endregion

    #region 余额查询返回结果
    /// <summary>
    /// 余额查询返回结果
    /// </summary>
    public class QueryBalanceJson : BaseRespondJson
    {
        /// <summary>
        /// 主账户余额
        /// </summary>
        public string balance { get; set; }
        /// <summary>
        /// 子账户余额
        /// </summary>
        public string ledgerbalance { get; set; }
    } 
    #endregion

    #region 易宝支付请求实体
    /// <summary>
    /// 易宝支付请求实体
    /// </summary>
    public class YeePayJson
    {
        /// <summary>
        /// 商户编号
        /// </summary>
        public string customernumber { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string requestid { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public string amount { get; set; }
        /// <summary>
        /// 是否担保1是0否
        /// </summary>
        public string assure { get; set; }
        /// <summary>
        /// 商品名
        /// </summary>
        public string productname { get; set; }
        /// <summary>
        /// 商品类别
        /// </summary>
        public string productcat { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public string productdesc { get; set; }
        /// <summary>
        /// 分账信息
        /// </summary>
        public string divideinfo { get; set; }
        /// <summary>
        /// 后台通知地址
        /// </summary>
        public string callbackurl { get; set; }
        /// <summary>
        /// 页面通知地址
        /// </summary>
        public string webcallbackurl { get; set; }
        /// <summary>
        /// 银行编码
        /// </summary>
        public string bankid { get; set; }
        /// <summary>
        /// 担保有效期
        /// </summary>
        public string period { get; set; }
        /// <summary>
        /// 商户备注
        /// </summary>
        public string memo { get; set; }
        /// <summary>
        /// 签名加密
        /// </summary>
        public string hmac { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string payproducttype { get; set; }

        #region 一键支付,非必填
        /// <summary>
        /// 用户标识/公众号用户openId,当公众号支付时必填
        /// </summary>
        public string userno { get; set; }
        /// <summary>
        /// 用户 IP 地址
        /// </summary>
        public string ip { get; set; }
        /// <summary>
        /// 持卡人姓名
        /// </summary>
        public string cardname { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string idcard { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string bankcardnum { get; set; }
        #endregion
        /// <summary>
        /// 供应商主用户
        /// </summary>
        public long id_user_master_gys { get; set; }
        /// <summary>
        /// 采购商主用户
        /// </summary>
        public long id_user_master_cgs { get; set; }
    } 
    #endregion

    #region 支付请求成功返回实体
    /// <summary>
    /// 支付请求成功返回实体
    /// </summary>
    public class PayRequestJson : BaseRespondJson
    {

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string requestid { get; set; }

        /// <summary>
        /// 易宝交易流水号
        /// </summary>
        public string externalid { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>

        public string amount { get; set; }

        /// <summary>
        /// 支付链接
        /// </summary>
        public string payurl { get; set; }

        /// <summary>
        /// 绑卡 ID  
        /// </summary>
        public string bindid { get; set; }

        /// <summary>
        /// 银行编码
        /// </summary>
        public string bankcode { get; set; }

    } 
    #endregion

    #region 支付操作返回结果实体
    /// <summary>
    /// 支付操作返回结果实体
    /// </summary>
    public class PayResultJson
    {

        /// <summary>
        /// 商户编号
        /// </summary>
        public string customernumber { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string requestid { get; set; }

        /// <summary>
        /// 返回码成功返回：1
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 易宝交易流水号
        /// </summary>
        public string externalid { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>

        public string amount { get; set; }

        /// <summary>
        /// 通知类型
        /// </summary>
        public string notifytype { get; set; }

        /// <summary>
        /// 暂时没有启用
        /// </summary>
        public string cardno { get; set; }

        /// <summary>
        /// 银行编码
        /// </summary>
        public string bankcode { get; set; }

        /// <summary>
        /// 签名信息
        /// </summary>
        public string hmac { get; set; }


    } 
    #endregion

    #region 订单查询返回结果实体
    /// <summary>
    /// 订单查询返回结果实体
    /// </summary>
    public class queryOrderJson : BaseRespondJson
    {
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string requestid { get; set; }
        /// <summary>
        /// 易宝交易流水号
        /// </summary>
        public string externalid { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public string amount { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string productname { get; set; }
        /// <summary>
        /// 商品类别
        /// </summary>
        public string productcat { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public string productdesc { get; set; }
        /// <summary>
        /// 订单状态  INIT： 未支付,SUCCESS： 已支付 FAIL： 支付失败
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 订单类型 SALES： 正常交易  REFUND： 差错退款
        /// </summary>
        public string ordertype { get; set; }
        /// <summary>
        /// 业务类型 COMMON： 普通交易  ASSURE： 担保交易
        /// </summary>
        public string busitype { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public string orderdate { get; set; }
        /// <summary>
        /// 订单创建时间
        /// </summary>
        public string createdate { get; set; }
        /// <summary>
        /// 支付成功时间
        /// </summary>
        public string paydate { get; set; }
        /// <summary>
        /// 银行通道编号
        /// </summary>
        public string bankid { get; set; }
        /// <summary>
        /// 银行编码
        /// </summary>
        public string bankcode { get; set; }
        /// <summary>
        /// 绑卡 id
        /// </summary>
        public string bindid { get; set; }
        /// <summary>
        /// 商户手续费
        /// </summary>
        public string fee { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 银行卡后四位  
        /// </summary>
        public string lastno { get; set; }
        /// <summary>
        /// 银行卡类型  DEBIT： 储蓄卡  CREDIT： 信用卡
        /// </summary>
        public string cardtype { get; set; }
        /// <summary>
        /// 支付产品类型
        /// </summary>
        public string payproducttype { get; set; }
        /// <summary>
        /// 订单错误码
        /// </summary>
        public string errorcode { get; set; }
        /// <summary>
        /// 错误码描述 
        /// </summary>
        public string errormsg { get; set; }
    } 
    #endregion
}

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CySoft.Controllers.Base;
using CySoft.Controllers.Service.Base;
using CySoft.Model;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Model.Ts;
using CySoft.Utility;
using CySoft.Frame.Core;
using CySoft.Controllers.Filters;
using CySoft.Model.CySoft.Model.Td;

//商业服务
namespace CySoft.Controllers.Service
{
    public class PayServiceController : ServiceBaseController
    {

        #region OutOrderPay 非本地订单支付
        /// <summary>
        /// OutOrderPay 非本地订单支付
        /// lz
        /// 2016-12-07
        /// </summary>
        //[HttpPost]
        [ActionPurview(false)]
        public ActionResult OutOrderPay()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("dh", string.Empty, HandleType.ReturnMsg);//dh
                p.Add("json_param", string.Empty, HandleType.ReturnMsg);//json_param
                p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(p);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }

                #endregion
                #region 转换payList
                try
                {
                    var payList = Utility.JSON.Deserialize<List<Money_OrderPayList>>(param["json_param"].ToString());
                    param.Add("payList", payList);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = "json_param参数格式不正确.";
                    return res;
                } 
                #endregion
                #region 读取ticket
                //读取ticket
                Hashtable ht = new Hashtable();
                ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                var ticketBr = BusinessFactory.Tb_Ticket.Get(ht);
                if (!ticketBr.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.S0001;
                    return res;
                }

                var ticketModel = (Tb_Ticket)ticketBr.Data;
                if (ticketModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0003;
                    return res;
                }

                var ticket = ticketModel.ticket;
                #endregion
                #region 验证签名
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "json_param", "dh" });

                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion
                #region 执行操作
                var br = BusinessFactory.Pay.OutOrderPay(param);
                #endregion
                #region 返回

                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = br.Message.FirstOrDefault();
                res.Data = br.Data;
                return res;
                #endregion
            });
            return JsonString(sr);
        }
        #endregion

        #region AliPayQuery 支付宝订单查询
        /// <summary>
        /// AliPayQuery 支付宝订单查询
        /// lz
        /// 2016-12-08
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult AliPayQuery()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("out_trade_no", string.Empty, HandleType.DefaultValue);//out_trade_no
                p.Add("trade_no", string.Empty, HandleType.DefaultValue);//trade_no
                p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(p);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }

                #endregion
                #region 读取ticket
                //读取ticket
                Hashtable ht = new Hashtable();
                ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                var ticketBr = BusinessFactory.Tb_Ticket.Get(ht);
                if (!ticketBr.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.S0001;
                    return res;
                }

                var ticketModel = (Tb_Ticket)ticketBr.Data;
                if (ticketModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0003;
                    return res;
                }

                var ticket = ticketModel.ticket;
                #endregion
                #region 验证签名
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "out_trade_no", "trade_no" });

                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion
                #region 检验支付宝交易号，和商户订单号不能同时为空

                if (string.IsNullOrEmpty(param["out_trade_no"].ToString()) && string.IsNullOrEmpty(param["trade_no"].ToString()))
                {
                    res.State = ServiceState.Fail;
                    res.Message = "支付宝交易号，和商户订单号不能同时为空.";
                    return res;
                } 
                #endregion
                #region 执行操作
                var br = BusinessFactory.Pay.AliPayQuery(param);
                #endregion
                #region 返回

                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = br.Message.FirstOrDefault();
                res.Data = br.Data;
                return res;
                #endregion
            });
            return JsonString(sr);
        }
        #endregion

        #region WXPayQuery 微信订单查询
        /// <summary>
        /// WXPayQuery 微信订单查询
        /// lz
        /// 2016-12-08
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult WXPayQuery()
        {
            var sr = RequestResult(res =>
            {
                #region 获取参数
                Hashtable param = base.GetParameters();
                ParamVessel p = new ParamVessel();
                p.Add("id_shop", string.Empty, HandleType.ReturnMsg);//id_shop
                p.Add("id_masteruser", string.Empty, HandleType.ReturnMsg);//id_masteruser
                p.Add("out_trade_no", string.Empty, HandleType.ReturnMsg);//out_trade_no
                p.Add("sign", string.Empty, HandleType.ReturnMsg);//sign 
                #endregion
                #region 验证参数
                try
                {
                    param = param.Trim(p);
                }
                catch (Exception ex)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0001;
                    return res;
                }

                #endregion
                #region 读取ticket
                //读取ticket
                Hashtable ht = new Hashtable();
                ht.Add("key_y", param["id_masteruser"].ToString() + "_" + param["id_shop"].ToString());
                var ticketBr = BusinessFactory.Tb_Ticket.Get(ht);
                if (!ticketBr.Success)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.S0001;
                    return res;
                }

                var ticketModel = (Tb_Ticket)ticketBr.Data;
                if (ticketModel == null)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0003;
                    return res;
                }

                var ticket = ticketModel.ticket;
                #endregion
                #region 验证签名
                IDictionary<string, string> dic = base.GetParameters(new string[] { "id_shop", "id_masteruser", "out_trade_no" });

                var validSign = SignUtils.SignRequest(dic, ticket);
                //验证签名
                if (param["sign"].ToString() != validSign)
                {
                    res.State = ServiceState.Fail;
                    res.Message = ServiceFailCode.A0002;
                    return res;
                }
                #endregion
                #region 检验支付宝交易号，和商户订单号不能同时为空

                if (string.IsNullOrEmpty(param["out_trade_no"].ToString()))
                {
                    res.State = ServiceState.Fail;
                    res.Message = "商户订单号不能为空.";
                    return res;
                }
                #endregion
                #region 执行操作
                var br = BusinessFactory.Pay.WXPayQuery(param);
                #endregion
                #region 返回

                res.State = br.Success ? ServiceState.Done : ServiceState.Fail;
                res.Message = br.Message.FirstOrDefault();
                res.Data = br.Data;
                return res;
                #endregion
            });
            return JsonString(sr);
        }
        #endregion

    }
}

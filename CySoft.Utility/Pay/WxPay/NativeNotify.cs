using CySoft.Frame.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace CySoft.Utility.Pay.WxPay
{
    /// <summary>
    /// 扫码支付模式一回调处理类
    /// 接收微信支付后台发送的扫码结果，调用统一下单接口并将下单结果返回给微信支付后台
    /// </summary>
    public class NativeNotify : Notify
    {
        public NativeNotify(Page page)
            : base(page)
        {

        }

        public override void ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData();
            WxPayData res = new WxPayData();
            if (!notifyData.IsSet("sub_mch_id"))
            {
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "回调数据异常");
                Log.Info(this.GetType().ToString(), "The data WeChat post is error : " + res.ToXml());
            }
            else
            {
                //检查transaction_id和out_trade_no是否返回
                if (!notifyData.IsSet("transaction_id") || !notifyData.IsSet("out_trade_no"))
                {
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "回调数据异常");
                    Log.Info(this.GetType().ToString(), "The data WeChat post is error : " + res.ToXml());
                }
                else
                {
                    //调订单查询接口，获得订单结果
                    string sub_mch_id = notifyData.GetValue("sub_mch_id").ToString();
                    string transaction_id = notifyData.GetValue("transaction_id").ToString();
                    string out_trade_no = notifyData.GetValue("out_trade_no").ToString();
                    WxPayData OrderResult = new WxPayData();
                    try
                    {
                        OrderResult = SelectOrder(sub_mch_id, transaction_id, out_trade_no);
                        Log.Info("查看订单查询结果", OrderResult.ToXml());
                    }
                    catch (Exception ex)//若在调统一下单接口时抛异常，立即返回结果给微信支付后台
                    {
                        res.SetValue("return_code", "FAIL");
                        res.SetValue("return_msg", "订单查询失败");
                        Log.Error(this.GetType().ToString(), "Order failure : " + res.ToXml());
                    }
                    //若查询失败，则立即返回结果给微信支付后台
                    if (OrderResult.GetValue("return_code").ToString() != "SUCCESS" && OrderResult.GetValue("result_code").ToString() != "SUCCESS")
                    {
                        res.SetValue("return_code", "FAIL");
                        res.SetValue("return_msg", "订单查询失败");
                        Log.Error(this.GetType().ToString(), "Order failure : " + res.ToXml());
                    }
                    else
                    {
                        Log.Info("成功", "");
                        //成功,则返回成功结果给微信支付后台
                        res.SetValue("return_code", "SUCCESS");
                        res.SetValue("return_msg", "OK");
                        res.SetValue("appid", WxPayConfig.APPID);
                        res.SetValue("mch_id", WxPayConfig.MCHID);
                        res.SetValue("sub_mch_id", sub_mch_id);
                        res.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
                        res.SetValue("result_code", "SUCCESS");
                        res.SetValue("err_code_des", "OK");
                        res.SetValue("sign", res.MakeSign());
                        Log.Info(this.GetType().ToString(), "Order success , send data to WeChat : " + res.ToXml());
                    }
                }
            }
            page.Response.Write(res.ToXml());
            page.Response.End();
        }

        private WxPayData SelectOrder(string sub_mch_id,string transaction_id, string out_trade_no)
        {
            //查询订单结果
            WxPayData req = new WxPayData();
            req.SetValue("appid", WxPayConfig.APPID);
            req.SetValue("mch_id", WxPayConfig.MCHID);
            req.SetValue("sub_mch_id", sub_mch_id);
            req.SetValue("transaction_id", transaction_id);
            req.SetValue("out_trade_no", out_trade_no);
            WxPayData result = WxPayApi.OrderQuery(req);
            return result;
        }
    }
}

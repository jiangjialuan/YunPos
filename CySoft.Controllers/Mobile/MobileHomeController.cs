//using CySoft.Controllers.Mobile.Base;
//using CySoft.Utility.WeChat;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web.Mvc;
//using CySoft.Controllers.Base;
//using System.Collections;
//using CySoft.Model.Pay;
//using CySoft.Model.Tb;

//namespace CySoft.Controllers.Mobile
//{
//    public class MobileHomeController : MobileBaseController
//    {
//        public void Index(string id)
//        {
//            if (string.IsNullOrEmpty(id))
//            {
//                return;
//            }

//            //除了微信接入 其余发送的消息均为 用户触发 消息中可获取openid
//            if (Request.RequestType.ToLower() == "get")
//            {
//                string echostr = HttpContext.Request.QueryString["echostr"];
//                string signature = HttpContext.Request.QueryString["signature"];
//                string timestamp = HttpContext.Request.QueryString["timestamp"];
//                string nonce = HttpContext.Request.QueryString["nonce"];

//                if (string.IsNullOrEmpty(echostr) || string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(nonce))
//                {
//                    return;
//                }

//                Hashtable param = new Hashtable();
//                WeChatAccount gzhInfo = new WeChatAccount();

//                //验证 id 判断供应商是否存在于订货易系统中 id=admin 为 直接 订货易 公众号
//                //没有在订货易开通微信的 不进行验证处理 查询公众号信息
//                if (id.Equals("admin", StringComparison.InvariantCultureIgnoreCase))
//                {
//                    param.Add("id_user_master", 1);
//                    gzhInfo = BusinessFactory.Pay.CheckPayAccount(param, "wxPay").Data as WeChatAccount;
//                }
//                else
//                {
//                    param.Add("bm", id.Trim());
//                    var gysInfo = BusinessFactory.Supplier.Get(param).Data as Tb_Gys_Edit;
//                    if (gysInfo == null)
//                    {
//                        return;
//                    }
//                    param.Clear();
//                    param.Add("id_user_master", gysInfo.id_user_master);
//                    gzhInfo = BusinessFactory.Pay.CheckPayAccount(param, "wxPay").Data as WeChatAccount;
//                }

//                if (gzhInfo == null || string.IsNullOrEmpty(gzhInfo.appid) || string.IsNullOrEmpty(gzhInfo.appsecret) || string.IsNullOrEmpty(gzhInfo.token))
//                {
//                    return;
//                }

//                //如果是微信接入验证
//                if (WeChatHelper.IsWeChatSwitchingIn(echostr, signature, timestamp, nonce, gzhInfo.token))
//                {
//                    //输出echostr
//                    HttpContext.Response.Write(echostr);
//                    HttpContext.Response.End();
//                    //初始化微信菜单按钮
//                    WeChatHelper.CreateWeChatMenu(HttpContext.Server.MapPath("~") + "/Template/WeChat_Menu.txt", GetAccessToken(gzhInfo.appid, gzhInfo.appsecret));
//                    return;
//                }
//            }
//            else
//            {
//                //是否 来自微信的请求
//                //if (IsRequestFromWeChat())
//                //{

//                //}
//                //获取用户发送信息
//                //启用openid登录
//                //判断采购商是否已经登录 是：直接进入订货页面 否：系统帮其登录
//                var requestXml = WeChatHelper.RequestMsg();
//                var resXml = "";
//                switch (requestXml.MsgType)
//                {
//                    //当收到文本信息的时候回复信息
//                    case "text":
//                        resXml = "<xml><ToUserName><![CDATA[" + requestXml.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXml.ToUserName + "]]></FromUserName><CreateTime>" + DateTime.Now.ToString("yyyyMMdd") + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + "文本" + "]]></Content><FuncFlag>0</FuncFlag></xml>";
//                        break;
//                    //当接收推送事件时回复的信息
//                    case "event":
//                        switch (requestXml.Event.ToLower())
//                        {
//                            //关注的时候回复信息
//                            case "subscribe":
//                                resXml = "<xml><ToUserName><![CDATA[" + requestXml.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXml.ToUserName + "]]></FromUserName><CreateTime>" + DateTime.Now.ToString("yyyyMMdd") + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + "第一次关注" + "]]></Content><FuncFlag>0</FuncFlag></xml>"; ;
//                                break;
//                            //自定义菜单的时候回复信息
//                            case "click":
//                                resXml = "<xml><ToUserName><![CDATA[" + requestXml.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXml.ToUserName + "]]></FromUserName><CreateTime>" + DateTime.Now.ToString("yyyyMMdd") + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + "单击事件" + "]]></Content><FuncFlag>0</FuncFlag></xml>"; ;
//                                break;
//                            case "view":
//                                //判断请求的域名跟当前系统的域名是否一致
//                                //是：判断其openid是否已关联系统的账号如果 是 直接登录系统 否：跳转到登录页面
//                                //否：不做处理
//                                break;
//                        }
//                        break;
//                }

//            }


//        }
//    }
//}

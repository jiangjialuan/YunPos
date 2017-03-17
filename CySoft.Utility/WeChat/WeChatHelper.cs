using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Xml;

namespace CySoft.Utility.WeChat
{
    public class WeChatHelper
    {
        /// <summary>
        /// 获取微信服务access_token
        /// </summary>
        /// <param name="appid">公众号 应用id</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetAccessToken(string appid, string appsecret)
        {
            if (string.IsNullOrEmpty(appid) || string.IsNullOrEmpty(appsecret))
            {
                return null;
            }

            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, appsecret);
            string respText = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream resStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(resStream, Encoding.Default);
                respText = reader.ReadToEnd();
                resStream.Close();
            }
            JavaScriptSerializer Jss = new JavaScriptSerializer();
            Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
            //通过键access_token获取值
            //oldAccessToken = respDic["access_token"].ToString();
            //AccessTokenExpireTime = DateTime.Now.AddSeconds(int.Parse(respDic["expires_in"].ToString()) - 200);
            return respDic;
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string Post(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                return content;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 判断是否微信接入 不是：返回null 是：返回 echostr 值
        /// </summary>
        /// <returns></returns>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        public static bool IsWeChatSwitchingIn(string echostr, string signature, string timestamp, string nonce, string token)
        {
            if (string.IsNullOrEmpty(echostr) || string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(nonce) || string.IsNullOrEmpty(token))
            {
                return false;
            }

            //从微信服务器接收传递过来的数据
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);//将三个字符串组成一个字符串
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");//进行sha1加密
            tmpStr = tmpStr.ToLower();
            //加过密的字符串与微信发送的signature进行比较，一样则通过微信验证，否则失败。
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 创建 菜单按钮
        /// </summary>
        public static void CreateWeChatMenu(string menuServerPath, string access_token)
        {
            using (FileStream fsMenu = new FileStream(menuServerPath, FileMode.Open))
            {
                using (StreamReader srMenu = new StreamReader(fsMenu, Encoding.UTF8))
                {
                    string menu = srMenu.ReadToEnd();
                    //创建菜单
                    Post("https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + access_token, menu);
                }
            }
        }

        /// <summary>
        /// 接收微信信息
        /// </summary>
        public static RequestXml RequestMsg()
        {
            //接收信息流
            Stream requestStream = System.Web.HttpContext.Current.Request.InputStream;
            byte[] requestByte = new byte[requestStream.Length];
            requestStream.Read(requestByte, 0, (int)requestStream.Length);
            //转换成字符串
            string requestStr = Encoding.UTF8.GetString(requestByte);
            //将XML文件封装到实体类requestXml中
            RequestXml requestXml = new RequestXml();

            if (!string.IsNullOrEmpty(requestStr))
            {
                //封装请求类到xml文件中
                XmlDocument requestDocXml = new XmlDocument();
                requestDocXml.LoadXml(requestStr);
                XmlElement rootElement = requestDocXml.DocumentElement;

                requestXml.ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText;//开发者微信号
                requestXml.FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText;//发送方微信号
                requestXml.CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText;//消息发送信息
                requestXml.MsgType = rootElement.SelectSingleNode("MsgType").InnerText;
                requestXml.MsgId = rootElement.SelectSingleNode("MsgId").InnerText;

                //获取接收信息的类型
                switch (requestXml.MsgType)
                {
                    //接收普通信息
                    case "text"://文本信息
                        requestXml.Content = rootElement.SelectSingleNode("Content").InnerText;
                        break;
                    case "image"://图片信息
                        requestXml.PicUrl = rootElement.SelectSingleNode("PicUrl").InnerText;
                        break;
                    case "location"://地理位置信息
                        requestXml.Location_X = rootElement.SelectSingleNode("Location_X").InnerText;
                        requestXml.Location_Y = rootElement.SelectSingleNode("Location_Y").InnerText;
                        break;
                    //接收事件推送
                    //大概包括有：关注/取消关注事件、扫描带参数二维码事件、上报地理位置事件、自定义菜单事件、点击菜单拉取消息时的事件推送、点击菜单跳转链接时的事件推送
                    case "event":
                        requestXml.Event = rootElement.SelectSingleNode("Event").InnerText;
                        requestXml.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;
                        break;
                }
            }

            return requestXml;
        }

        /// <summary>
        /// 回复消息(微信信息返回)
        /// </summary>
        /// <param name="weixinXML"></param>
        private static string ResponseMsg(RequestXml requestXml)
        {
            string resXml = "";

            try
            {
                switch (requestXml.MsgType)
                {
                    //当收到文本信息的时候回复信息
                    case "text":
                        resXml = "<xml><ToUserName><![CDATA[" + requestXml.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXml.ToUserName + "]]></FromUserName><CreateTime>" + DateTime.Now.ToString("yyyyMMdd") + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + "文本" + "]]></Content><FuncFlag>0</FuncFlag></xml>";
                        break;
                    //当接收推送事件时回复的信息
                    case "event":
                        switch (requestXml.Event.ToLower())
                        {
                            //关注的时候回复信息
                            case "subscribe":
                                resXml = "<xml><ToUserName><![CDATA[" + requestXml.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXml.ToUserName + "]]></FromUserName><CreateTime>" + DateTime.Now.ToString("yyyyMMdd") + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + "第一次关注" + "]]></Content><FuncFlag>0</FuncFlag></xml>"; ;
                                break;
                            //自定义菜单的时候回复信息
                            case "click":
                                resXml = "<xml><ToUserName><![CDATA[" + requestXml.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXml.ToUserName + "]]></FromUserName><CreateTime>" + DateTime.Now.ToString("yyyyMMdd") + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + "单击事件" + "]]></Content><FuncFlag>0</FuncFlag></xml>"; ;
                                break;
                            case "view":
                                break;
                        }
                        break;
                }

                return resXml;

            }
            catch
            {
                return "";
            }

        }

        public class RequestXml
        {
            public string ToUserName { get; set; }
            public string FromUserName { get; set; }
            public string CreateTime { get; set; }
            public string MsgType { get; set; }
            public string Content { get; set; }
            public string PicUrl { get; set; }
            public string Location_X { get; set; }
            public string Location_Y { get; set; }
            public string Event { get; set; }
            public string EventKey { get; set; }
            public string MsgId { get; set; }
        }
    }
}

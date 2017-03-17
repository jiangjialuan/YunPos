using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CySoft.Frame.Core;
using System.Net.Http;
using System.Diagnostics;

namespace CySoft.Utility.SMS
{

    public class EmayHelper : ISMSHelper
    {
        private static HttpClient _httpClient;
        private readonly static string cdkey = "9SDK-EMY-0999-JEVNM";
        private readonly static string passwd = "904440";
        private readonly static string Uri_Send = "http://sdk999ws.eucp.b2m.cn:8080/sdkproxy/sendsms.action";
        private readonly static string Uri_Reg = "http://sdk999ws.eucp.b2m.cn:8080/sdkproxy/regist.action";
        private readonly static string Uri_LogOut = "http://sdk999ws.eucp.b2m.cn:8080/sdkproxy/logout.action";
        private readonly static string Uri_QueryAmount = "http://sdk999ws.eucp.b2m.cn:8080/sdkproxy/querybalance.action";
        private readonly static IDictionary<string, string> HttpParam = new Dictionary<string, string>() { { "cdkey", cdkey }, { "password", passwd } };
        private static HttpClient httpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = new HttpClient();
                    // 设置请求头信息  
                    _httpClient.DefaultRequestHeaders.Add("Host", "sdk999ws.eucp.b2m.cn:8080");
                    _httpClient.DefaultRequestHeaders.Add("Method", "Post");
                    _httpClient.DefaultRequestHeaders.Add("KeepAlive", "false");   // HTTP KeepAlive设为false，防止HTTP连接保持  
                    _httpClient.DefaultRequestHeaders.Add("UserAgent",
                        "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.95 Safari/537.11");
                }
                return _httpClient;
            }
        }


        public EmayHelper() { Init(); }

        public BaseResult Init(Hashtable param = null)
        {
            return Register();
        }

        public BaseResult Send(string phone, string msg)
        {
            Hashtable ht = new Hashtable();
            ht.Add("phone", phone);
            ht.Add("msg", msg);
            return Send(ht);
        }

        public BaseResult Send(Hashtable param = null)
        {
            BaseResult br = new BaseResult() { Success = true };
            if (param == null
                || !param.ContainsKey("phone")
                || !param.ContainsKey("msg")
                || string.IsNullOrWhiteSpace(param["phone"].ToString())
                || string.IsNullOrWhiteSpace(param["msg"].ToString()))
            {
                br.Success = false;
                br.Message.Add("发送内容不符合条件");
                br.Level = ErrorLevel.Warning;
                return br;
            }

            var parma = new Dictionary<string, string>();

            parma.Add("phone", param["phone"].ToString());
            parma.Add("message", param["msg"].ToString());
            parma.Add("addserial", "");

            HttpContent postContent = new FormUrlEncodedContent(parma.Concat(HttpParam));
            Post(Uri_Send, postContent);
            return br;
        }

        public BaseResult Register(Hashtable param = null)
        {
            BaseResult br = new BaseResult() { Success = true };
            HttpContent postContent = new FormUrlEncodedContent(HttpParam);
            Post(Uri_Reg, postContent);
            return br;
        }

        public BaseResult Logout(Hashtable param = null)
        {
            BaseResult br = new BaseResult() { Success = true };
            HttpContent postContent = new FormUrlEncodedContent(HttpParam);
            Post(Uri_LogOut, postContent);
            return br;
        }

        public BaseResult QueryAmount(Hashtable param = null)
        {
            BaseResult br = new BaseResult() { Success = true };
            HttpContent postContent = new FormUrlEncodedContent(HttpParam);
            Post(Uri_QueryAmount, postContent);
            return br;
        }

        public BaseResult PayAmount(Hashtable param = null)
        {
            throw new NotImplementedException();
        }

        public BaseResult Status(Hashtable param = null)
        {
            throw new NotImplementedException();
        }

        public BaseResult Report(Hashtable param = null)
        {
            throw new NotImplementedException();
        }

        private static void Post(string uri, HttpContent postContent)
        {
            httpClient
               .PostAsync(uri, postContent)
               .ContinueWith((postTask) => {
                   try
                   {
                       Debug.Print("响应地址:" + uri);
                       HttpResponseMessage response = postTask.Result;
                       response.EnsureSuccessStatusCode();

                       Debug.AutoFlush = true;
                       //// 异步读取响应为字符串  
                       response.Content.ReadAsStringAsync().ContinueWith(
                           (readTask) => {
                               Debug.Print("响应网页内容：" + readTask.Result);
                           });
                       Debug.Print("响应是否成功：" + response.IsSuccessStatusCode);

                       Debug.Print("响应头信息如下：\n");
                       var headers = response.Headers;
                       foreach (var header in headers)
                       {
                           Debug.Print("{0}: {1}", header.Key, string.Join("", header.Value.ToList()));
                       }

                   }
                   catch (HttpRequestException e)
                   {
                       throw e;
                   }
               }
           );
        }

    }
}

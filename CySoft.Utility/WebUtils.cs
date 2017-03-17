using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace CySoft.Utility
{
    public sealed class WebUtils
    {
        public string BuildGetUrl(string url, IDictionary<string, string> parameters)
        {
            if ((parameters != null) && (parameters.Count > 0))
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters);
                    return url;
                }
                url = url + "?" + BuildQuery(parameters);
            }
            return url;
        }

        /// <summary>
        /// 排序后生成参数字符串
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string BuildQuery(IDictionary<string, string> parameters)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> sort_params = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder builder = new StringBuilder();
            while (sort_params.MoveNext())
            {
                string key = sort_params.Current.Key;
                string value = sort_params.Current.Value;
                if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
                    builder.AppendFormat("{0}={1}&", key, value);
            }

            string content = builder.Remove(builder.Length - 1, 1).ToString();
            return content;
        }

        public static string BuildQuery2(IDictionary<string, string> parameters)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();

            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;

                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");
                    postData.Append(value);
                    hasParam = true;
                }
            }
            return postData.ToString();
        }

        /// <summary>
        /// 排序后生成参数字符串并加入签名
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string BuildQuery_Sign(IDictionary<string, string> parameters, string key)
        {
            string str_content = BuildQuery(parameters);

            //签名
            string str_sign = Utility.DEncrypt.Md5Encrypt.Encode(Encoding.UTF8, string.Format("{0}{1}", str_content, key));

            str_content = string.Format("{0}&sign={1}", str_content, str_sign);

            return str_content;
        }

        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        public string DoGet(string url, IDictionary<string, string> parameters)
        {
            if ((parameters != null) && (parameters.Count > 0))
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters);
                }
                else
                {
                    url = url + "?" + BuildQuery(parameters);
                }
            }

            HttpWebRequest webRequest = this.GetWebRequest(url, "GET");
            webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
            return this.GetResponseAsString(rsp, Encoding.UTF8);
        }

        public string DoPost(string url, IDictionary<string, string> parameters)
        {
            HttpWebRequest webRequest = this.GetWebRequest(url, "POST");
            webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            byte[] bytes = Encoding.UTF8.GetBytes(BuildQuery(parameters));
            Stream requestStream = webRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
            return this.GetResponseAsString(rsp, Encoding.UTF8);
        }


        public string DoPost(string url, IDictionary<string, string> parameters, int timeOut)
        {
            HttpWebRequest req = GetWebRequest(url, "POST");
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            req.Timeout = timeOut;
            byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(parameters));
            System.IO.Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, encoding);

        }


        public string DoPost(string url, string value)
        {
            HttpWebRequest webRequest = this.GetWebRequest(url, "POST");
            webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            Stream requestStream = webRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
            return this.GetResponseAsString(rsp, Encoding.UTF8);
        }

        public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream responseStream = null;
            StreamReader reader = null;
            string str;
            try
            {
                responseStream = rsp.GetResponseStream();
                reader = new StreamReader(responseStream, encoding);
                str = reader.ReadToEnd();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                if (rsp != null)
                {
                    rsp.Close();
                }
            }
            return str;
        }

        public HttpWebRequest GetWebRequest(string url, string method)
        {
            HttpWebRequest request = null;
            if (url.Contains("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
                request = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                request = (HttpWebRequest)WebRequest.Create(url);
            }
            request.ServicePoint.Expect100Continue = false;
            request.Method = method;
            request.KeepAlive = true;
            request.UserAgent = "Cysoft";
            return request;
        }
    }
}


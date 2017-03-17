using System;
using CySoft.Frame.Common;
using CySoft.Utility.Weixin.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CySoft.Utility.Weixin.Api
{
    public sealed class TokenSnsApi
    {
        /// <summary>
        /// 获取网页授权access_token、openid
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appsecret"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Domain.TokenSns GetToken(string appid, string appsecret, string code)
        {
            Domain.TokenSns token_sns = null;
            string strIsDebug = Utility.AppConfig.GetValue("isDebug");

            if (!string.IsNullOrWhiteSpace(strIsDebug) && strIsDebug.Equals("1", StringComparison.OrdinalIgnoreCase))
            {
                //TODO: YZQ TEST Return Token & openid
                token_sns = new Utility.Weixin.Domain.TokenSns
                {
                    access_token = "TgBi2hhQWCGph-Q4b9ah81ejRoYSp-uBBbfLb6yLHpZES9d5gELY7Ma_meWNtwm4MPoA-3acet7Q728RUJFPI3vCvwECd2KH-AIecag-gqw",
                    expires_in = 7200,
                    refresh_token = "5XxVyBZPEG9qiHwQZdGcDiO8t55epRLzHdMVvkE1Xrbo4nDRd4LLebEbSc2M_V5MEzAPN9Rg7ovpCVmVueOK6QCUPPRBRD8u5pmzSKtn9nw",
                    openid = "554393109",
                    scope = "snsapi_base"
                };

                return token_sns;
            }
            else
            {
                try
                {
                    string url = string.Format(ApiUrl.GetToken_SNS, appid, appsecret, code);
                    Utility.LogHelper.WeixinInfo(string.Format("【网页授权code获取token】, request:{0}", url));
                    string str_result = new WebUtils().DoGet(url, null);
                    Utility.LogHelper.WeixinInfo(string.Format("【网页授权code获取token】, response:{0}", str_result));

                    if (string.IsNullOrWhiteSpace(str_result)
                        || str_result.IndexOf("errcode", StringComparison.OrdinalIgnoreCase) != -1
                        || str_result.IndexOf("errmsg", StringComparison.OrdinalIgnoreCase) != -1)
                    {

                    }
                    else
                    {
                        JObject jobj_token = JsonConvert.DeserializeObject<JObject>(str_result);
                        token_sns = new Domain.TokenSns()
                        {
                            access_token = (string)jobj_token["access_token"],
                            expires_in = (int)jobj_token["expires_in"],
                            refresh_token = (string)jobj_token["refresh_token"],
                            openid = (string)jobj_token["openid"],
                            scope = (string)jobj_token["scope"],
                        };
                    }
                }
                catch (Exception ex)
                {
                    TextLogHelper.WriterExceptionLog(ex);
                }

                return token_sns;
            }
        }

        /// <summary>
        /// 网页授权用户获取用户信息(需scope为 snsapi_userinfo)
        /// </summary>
        /// <param name="token"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static Domain.UserInfo GetUserInfo(string token, string openid)
        {
            string strIsDebug = Utility.AppConfig.GetValue("isDebug");
            if (!string.IsNullOrWhiteSpace(strIsDebug) && strIsDebug.Equals("1", StringComparison.OrdinalIgnoreCase))
            {
                //TODO: YZQ TEST Return UserInfo
                Domain.UserInfo userinfo = new Utility.Weixin.Domain.UserInfo()
                {
                    openid = openid,
                    nickname = "SP",
                    sex = 1,
                    language = "zh_CN",
                    city = "Guangzhou",
                    province = "Guangdong",
                    country = "CN",
                    headimgurl = "http://wx.qlogo.cn/mmopen/Jiavz9UrH80moUmmetxLgfMOOicGvzYHq3oKia9lgXbXECIm1VoLMvHBeVibCic2zFNU7V6ulGUydPqWuOhneIKGhcU0rdj6iawvnS/0",
                    unionid = null
                };
                userinfo.privilege.Add("chinaunicom");

                return userinfo;
            }
            else
            {
                Domain.UserInfo user = null;

                try
                {
                    string url = string.Format(ApiUrl.GetUserInfo, token, openid);
                    Utility.LogHelper.WeixinInfo(string.Format("【获取用户基本信息】, request:{0}", url));
                    string str_result = new WebUtils().DoGet(url, null);
                    Utility.LogHelper.WeixinInfo(string.Format("【获取用户基本信息】, response:{0}", str_result));

                    if (string.IsNullOrWhiteSpace(str_result)
                        || str_result.IndexOf("errcode", StringComparison.OrdinalIgnoreCase) != -1
                        || str_result.IndexOf("errmsg", StringComparison.OrdinalIgnoreCase) != -1)
                    {

                    }
                    else
                    {
                        user = JsonConvert.DeserializeObject<Domain.UserInfo>(str_result);
                    }
                }
                catch (Exception ex)
                {
                    TextLogHelper.WriterExceptionLog(ex);
                }

                return user;
            }
        }
    }
}


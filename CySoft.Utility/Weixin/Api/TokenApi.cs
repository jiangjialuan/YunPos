using System;
using CySoft.Frame.Common;
using CySoft.Utility.Weixin.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CySoft.Utility.Weixin.Api
{
    public sealed class TokenApi
    {
        /// <summary>
        /// 商户获取自身Token并缓存
        /// </summary>
        /// <param name="appid">商户AppID</param>
        /// <param name="secret">商户AppSecret</param>
        /// <returns></returns>
        public static string GetToken(string appid, string secret)
        {
            //缓存KeyName: Weixin_APP_Token_{appid}
            string str_cache_key = string.Format(CacheKey.Token, appid);
            //取Cache中Token
            string str_result = CacheHelper.Get<string>(str_cache_key);

            if (string.IsNullOrWhiteSpace(str_result))
            {
                string url = string.Format(ApiUrl.GetToken, appid, secret);

                try
                {
                    str_result = new WebUtils().DoGet(url, null);
                    if (!string.IsNullOrWhiteSpace(str_result)
                    && str_result.IndexOf("access_token", StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        JObject jobj_token = JsonConvert.DeserializeObject<JObject>(str_result);
                        str_result = (string)jobj_token["access_token"];
                        if (!string.IsNullOrWhiteSpace(str_result))
                        {
                            //将access_token绝对过期缓存
                            CacheHelper.Insert(str_cache_key, str_result, Config.Token_Expires);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TextLogHelper.WriterExceptionLog(ex);
                }
            }

            return str_result;
        }
    }
}


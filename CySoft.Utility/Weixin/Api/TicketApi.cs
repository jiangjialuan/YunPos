using System;
using CySoft.Frame.Common;
using CySoft.Utility.Weixin.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CySoft.Utility.Weixin.Api
{
    public class TicketApi
    {
        /// <summary>
        /// 商户获取Ticket
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static string GetTicket(string accessToken)
        {
            //缓存KeyName: Weixin_Jsapi_Ticket_{accessToken}
            string str_cache_key = string.Format(CacheKey.Jsapi_Ticket, accessToken);
            //取Cache中Ticket
            string str_result = CacheHelper.Get<string>(str_cache_key);

            if (string.IsNullOrWhiteSpace(str_result))
            {
                string url = string.Format(ApiUrl.GetTicket, accessToken);

                try
                {
                    str_result = new WebUtils().DoGet(url, null);
                    if (!string.IsNullOrWhiteSpace(str_result)
                    && str_result.IndexOf("ticket", StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        JObject jobj_ticket = JsonConvert.DeserializeObject<JObject>(str_result);
                        int? errcode = (int?)jobj_ticket["errcode"];
                        string errmsg = (string)jobj_ticket["errmsg"];
                        string str_ticket = (string)jobj_ticket["ticket"];

                        if (!errcode.HasValue
                            || errcode != 0
                            || string.IsNullOrWhiteSpace(errmsg)
                            || !errmsg.Equals("ok", StringComparison.OrdinalIgnoreCase)
                            || string.IsNullOrWhiteSpace(str_ticket))
                        {
                            return string.Empty;
                        }

                        str_result = str_ticket;
                        //将Ticket绝对过期缓存
                        CacheHelper.Insert(str_cache_key, str_result, Config.Ticket_Expires);
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


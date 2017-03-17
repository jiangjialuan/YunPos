using CySoft.Frame.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Utility.Pay.YeePay
{
    public class YeePayResult<T>
    {
        public static BaseResult GetResult(string url, Dictionary<string, string> dir)
        {
            string info_json = JSON.Serialize(dir);
            string data = AESUtil.Encrypt(info_json, Config.AescKey);
            string postParams = "data=" + data + "&customernumber=" + Config.merchantAccount;
            BaseResult ybResult = HttpHelp.HttpPost(url, postParams);
            dynamic result = null;
            if (ybResult.Success)
            {
                RespondJson respJson = JSON.Deserialize<RespondJson>(ybResult.Data.ToString());
                if (respJson.data != null)
                {
                    result = JSON.Deserialize<T>(AESUtil.Decrypt(respJson.data, Config.AescKey));
                }
                else
                {
                    result = JSON.Deserialize<FailJson>(ybResult.Data.ToString());
                }
                ybResult.Success = true;
                ybResult.Data = result;
                return ybResult;
            }
            return ybResult;
        }
        public static dynamic GetResult(string ybResult)
        {
            RespondJson respJson = JSON.Deserialize<RespondJson>(ybResult);
            dynamic result = null;
            if (respJson.data != null)
            {
                result = JSON.Deserialize<T>(AESUtil.Decrypt(respJson.data, Config.AescKey));
            }
            else
            {
                result = JSON.Deserialize<FailJson>(ybResult);
            }
            return result;
        }
        
    }
}

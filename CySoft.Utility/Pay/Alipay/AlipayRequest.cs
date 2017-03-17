using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.Globalization;

#region 支付宝支付
#endregion
namespace CySoft.Utility.Pay.Alipay
{
    public class AlipayRequest:PaymentRequest
    {
        

        public AlipayRequest(PaymentParms parm)
        {
            this.subject = parm.subject;
            this.body = parm.body;
            this.outTradeNo = parm.orderId;
            this.totalFee = parm.amount.ToString("F", CultureInfo.InvariantCulture);
            this.returnUrl = parm.returnUrl;
            this.notifyUrl = parm.notifyUrl;
            this.showUrl = parm.showUrl;
            this.SellerEmail=parm.sellerEmail;
            this.Partner = parm.partner;
            this.Key = parm.key;
        }

        public override void SendRequest()
        {
            RedirectToGateway(CreatDirectUrl(Gateway,Service,Partner,SignType,outTradeNo,subject,body,PaymentType,totalFee,showUrl,SellerEmail,Key,returnUrl,InputCharset,notifyUrl,Agent,extend_param));
        }

        #region 常量
        private const string Gateway = "https://mapi.alipay.com/gateway.do?";
        private const string Service = "create_direct_pay_by_user";
        private const string SignType = "MD5";
        private const string PaymentType = "1";                  //支付类型 （商品购买）
        private const string InputCharset = "utf-8";
        private const string Agent = "C4335302345904805116";
        private const string extend_param = "isv^yf31"; //公共业务扩展参数
        #endregion
        public string SellerEmail { get; set; } //卖家email

        public string Partner { get; set; }

        private readonly string subject;	//商品名称
        private readonly string body;		//商品描述
        private readonly string totalFee;                      //总金额	0.01～50000.00
        private readonly string showUrl;

        public string Key { get; set; }              //账户的支付宝安全校验码
        private readonly string returnUrl;
        private readonly string notifyUrl;
        private readonly string outTradeNo;//订单号

        internal static string GetMD5(string s, string _input_charset)
        {
            /// <summary>
            /// 与ASP兼容的MD5加密算法
            /// </summary>

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(s));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        internal static string[] BubbleSort(string[] r)
        {
            /// <summary>
            /// 冒泡排序法
            /// </summary>

            int i, j; //交换标志 
            string temp;

            bool exchange;

            for (i = 0; i < r.Length; i++) //最多做R.Length-1趟排序 
            {
                exchange = false; //本趟排序开始前，交换标志应为假

                for (j = r.Length - 2; j >= i; j--)
                {
                    if (System.String.CompareOrdinal(r[j + 1], r[j]) < 0)　//交换条件
                    {
                        temp = r[j + 1];
                        r[j + 1] = r[j];
                        r[j] = temp;

                        exchange = true; //发生了交换，故将交换标志置为真 
                    }
                }

                if (!exchange) //本趟排序未发生交换，提前终止算法 
                {
                    break;
                }

            }
            return r;
        }

        internal static string CreatDirectUrl(
            string gateway,
            string service,
            string partner,
            string sign_type,
            string out_trade_no,
            string subject,
            string body,
            string payment_type,
            string total_fee,
            string show_url,
            string seller_email,
            string key,
            string return_url,
            string _input_charset,
            string notify_url,
            string agent,
            string extend_param
            )
        {
            
            int i;

            //构造数组；
            string[] Oristr ={ 
                "service="+service, 
                "partner=" + partner, 
                "agent=" + agent,
                "extend_param=" + extend_param,
                "subject=" + subject, 
                "body=" + body, 
                "out_trade_no=" + out_trade_no, 
                "total_fee=" + total_fee, 
                "show_url=" + show_url,  
                "payment_type=" + payment_type, 
                "seller_email=" + seller_email, 
                "notify_url=" + notify_url,
                "_input_charset="+_input_charset,          
                "return_url=" + return_url
              };

            //进行排序；
            string[] Sortedstr = BubbleSort(Oristr);

            //构造待md5摘要字符串 ；
            StringBuilder prestr = new StringBuilder();

            for (i = 0; i < Sortedstr.Length; i++)
            {
                if (i == Sortedstr.Length - 1)
                {
                    prestr.Append(Sortedstr[i]);
                }
                else
                {
                    prestr.Append(Sortedstr[i] + "&");
                }
            }

            prestr.Append(key);

            //生成Md5摘要；
            string sign =  GetMD5(prestr.ToString(), _input_charset);

            //构造支付Url；
            char[] delimiterChars = { '=' };
            StringBuilder parameter = new StringBuilder();
            parameter.Append(gateway);
            for (i = 0; i < Sortedstr.Length; i++)
            {
                parameter.Append(Sortedstr[i].Split(delimiterChars)[0] + "=" + HttpUtility.UrlEncode(Sortedstr[i].Split(delimiterChars)[1]) + "&");
            }

            parameter.Append("sign=" + sign + "&sign_type=" + sign_type);

            //返回支付Url；
            return parameter.ToString();
        }
    }
}

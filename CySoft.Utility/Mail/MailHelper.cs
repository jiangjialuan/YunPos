using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace CySoft.Utility.Mail
{
    public class MailHelper
    {
        private readonly static string _frommail = "tim@chaoying.com.cn";
        private readonly static string _host = "smtp.exmail.qq.com";
        private readonly static int _port = 465;
        private readonly static string _username = "tim@chaoying.com.cn";
        private readonly static string _password = "tim123";
        private static string _email = "tim@chaoying.com.cn";

        public MailHelper()
        { }

        private static MailMessage Message
        {
            get {
                MailMessage mailObj = new MailMessage();
                mailObj.From = new MailAddress("tim@chaoying.com.cn"); //发送人邮箱地址
                mailObj.To.Add(_email);   //收件人邮箱地址
                mailObj.Subject = "订货易邮箱验证";
                mailObj.IsBodyHtml = true;
                mailObj.BodyEncoding = Encoding.UTF8;
                mailObj.SubjectEncoding = Encoding.UTF8;
                mailObj.IsBodyHtml = true;
                mailObj.Priority = MailPriority.Normal;
                mailObj.Body = string.Format("这是订货易邮箱验证邮件，请<a href=\"http://www.dhy.hk/Account/VaildEmail?key={0}&email={1}&check=false&type=v_mail\">点击确认</a>", _email, _email);
                return mailObj;
            }
        }

        private static SmtpClient Smtp
        {
            get
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Host = _host;
                smtp.Port = _port;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(_username, _password);  //发送人的登录名和密码
                return smtp;
            }
        }


        public static void Send()
        {
            Smtp.SendAsync(Message, Message.To);
        }
    }
}

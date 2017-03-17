using System;
using System.Diagnostics;

namespace CySoft.Model.Other
{
    /// <summary>
    /// 找回密码
    /// </summary>
    [Serializable]
    [DebuggerDisplay("phone = {phone}, new_password = {new_password}, img_code = {img_code}, phonevaildcode = {phonevaildcode}")]
    public class UserReset
    {
        public string id_user { get; set; }

        public string phone { get; set; }

        public string new_password { get; set; }

        public string img_code { get; set; }

        public string phonevaildcode { get; set; }
    }
}

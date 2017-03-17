using System;
using System.Diagnostics;
using CySoft.Model.Flags;

namespace CySoft.Model.Other
{
    [Serializable]
    [DebuggerDisplay("flag_lx = {flag_lx},username = {username}, password = {password},vaildcode = {vaildcode}")]
    public class UserLogin
    {
        private AccountFlag _flag_lx;
        private string _username;
        private string _password;
        private string _vaildcode;

        public AccountFlag flag_lx
        {
            get
            {
                return this._flag_lx;
            }
            set
            {
                this._flag_lx = value;
            }
        }

        public string username
        {
            get
            {
                if (this._username == null)
                {
                    return String.Empty;
                }
                return this._username;
            }
            set
            {
                this._username = value;
            }
        }

        public string password
        {
            get
            {
                if (this._password == null)
                {
                    return String.Empty;
                }
                return this._password;
            }
            set
            {
                this._password = value;
            }
        }

        public string vaildcode
        {
            get
            {
                if (this._vaildcode == null)
                {
                    return String.Empty;
                }
                return this._vaildcode;
            }
            set
            {
                this._vaildcode = value;
            }
        }
        public string wxOpenid { get; set; }//微信openid
        public bool isWxOpenidBind { get; set; }//是否已绑定微信openid

        public long id_gys_gzh { get; set; }//公众号 供应商id

        public string loginType { get; set; }

        public bool yanshi { get; set; }
    }
}

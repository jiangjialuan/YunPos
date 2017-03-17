using System;
using CySoft.Model.Flags;
using System.Diagnostics;

namespace CySoft.Model.Other
{
    /// <summary>
    /// 用户注册
    /// </summary>
    [Serializable]
    [DebuggerDisplay("flag_supplier = {flag_supplier},phone = {phone}, phonevaildcode = {phonevaildcode}, username = {username}, password = {password}")]
    public class UserRegister
    {
        private YesNoFlag _flag_supplier;
        private string _phone = String.Empty;
        private string _phonevaildcode = String.Empty;
        private string _username = String.Empty;
        private string _password = String.Empty;
        private string _companyname = String.Empty;
        private string _name = String.Empty;
        private int _id_hy = 0;
        private string _email = String.Empty;
        private string _id_user = String.Empty;
        public string dealer { get; set; }
        /// <summary>
        /// 是否注册为供应商
        /// </summary>
        public YesNoFlag flag_supplier
        {
            get { return _flag_supplier; }
            set { _flag_supplier = value; }
        }
        /// <summary>
        /// 手机号
        /// </summary>
        public string phone
        {
            get {
                if (this._phone == null)
                {
                    return String.Empty;
                }
                return _phone; 
            }
            set { _phone = value; }
        }
        public byte version { get; set; }
        /// <summary>
        /// 手机效验码
        /// </summary>
        public string phonevaildcode
        {
            get {
                if (this._phonevaildcode == null)
                {
                    return String.Empty;
                }
                return _phonevaildcode; 
            }
            set { _phonevaildcode = value; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string username
        {
            get {
                if (this._username == null)
                {
                    return String.Empty;
                }
                return _username; 
            }
            set { _username = value; }
        }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string password
        {
            get {
                if (this._password == null)
                {
                    return String.Empty;
                }
                return _password; 
            }
            set { _password = value; }
        }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string companyname
        {
            get {
                if (this._companyname == null)
                {
                    return String.Empty;
                }
                return _companyname; 
            }
            set { _companyname = value; }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string name
        {
            get
            {
                if (this._name == null)
                {
                    return String.Empty;
                }
                return _name;
            }
            set { _name = value; }
        }
        /// <summary>
        /// 所属行业
        /// </summary>
        public Nullable<int> id_hy
        {
            get { return _id_hy; }
            set {
                if (value.HasValue)
                {
                    _id_hy = value.Value;
                }
                else
                {
                    this._id_hy = 0;
                }
            }
        }
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string email
        {
            get {
                if (this._email == null)
                {
                    return String.Empty;
                }
                return _email;
            }
            set { _email = value; }
        }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string id_user
        {
            get { return _id_user; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _id_user = value;
                }
                else
                {
                    this._id_user =String.Empty;
                }
            }
        }

        public string flag_from { get; set; }
        /// <summary>
        /// 门店号
        /// </summary>
        public string shop_bm { get; set; }
        /// <summary>
        /// 门店名称
        /// </summary>
        public string shop_mc { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string shop_lxr { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string shop_tel { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string shop_bz { get; set; }
        /// <summary>
        /// 超赢用户id
        /// </summary>
        public string id_cyuser { get; set; }

        public byte? industry { get; set; }

        public bool isAgree { get; set; }
    }
}

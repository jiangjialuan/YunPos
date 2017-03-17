using System;
using System.Diagnostics;

namespace CySoft.Model.Other
{
    [Serializable]
    [DebuggerDisplay("phone = {phone},vaildcode = {vaildcode}")]
    public class PhoneVaild
    {
        private string _phone = String.Empty;
        private string _vaildcode = String.Empty;
        private const string _keyMark = "Service.PhoneVaildCode";

        /// <summary>
        /// 手机号
        /// </summary>
        public string phone
        {
            get
            {
                if (_phone == null)
                {
                    return String.Empty;
                }
                return _phone;
            }
            set { _phone = value; }
        }
        /// <summary>
        /// 效验码
        /// </summary>
        public string vaildcode
        {
            get
            {
                if (_vaildcode == null)
                {
                    return String.Empty;
                }
                return _vaildcode;
            }
            set { _vaildcode = value; }
        }

        public string SeviceKey
        {
            get { return string.Format("{0}.{1}.{2}", _keyMark, phone, vaildcode); }
        }
    }
}

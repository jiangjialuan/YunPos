using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Model.Mapping;
using System.Diagnostics;
using CySoft.Model.Flags;

namespace CySoft.Model.Tb
{
    /// <summary>
    /// 供应商
    /// </summary>
    [Serializable]
    [Table("Tb_gys", "Tb_Gys")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Gys_Query:Tb_Gys
    {
        #region public method

        public Tb_Gys Clone()
        {
            return (Tb_Gys)this.MemberwiseClone();
        }

        #endregion

        #region private field
        private string _companyname = String.Empty;
        private string _username = String.Empty;
        private string _name_province = String.Empty;
        private string _name_city = String.Empty;
        private string _name_county = String.Empty;
        private string _name = String.Empty;
        private string _phone = String.Empty;
        private YesNoFlag _flag_actived = YesNoFlag.No;
        private string _name_flag_from = String.Empty;
        private string _flag_gcgx = string.Empty;//是否有采购关系
        #endregion

        #region public property
        /// <summary>
        /// 公司名称
        /// </summary>
        public string companyname
        {
            get
            {
                return _companyname;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _companyname = value;
                }
                else
                {
                    _companyname = String.Empty;
                }
            }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string username
        {
            get
            {
                return _username;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _username = value;
                }
                else
                {
                    _username = String.Empty;
                }
            }
        }
        /// <summary>
        /// 省份
        /// </summary>
        public string name_province
        {
            get
            {
                return _name_province;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name_province = value;
                }
                else
                {
                    _name_province = String.Empty;
                }
            }
        }
        /// <summary>
        /// 城市
        /// </summary>
        public string name_city
        {
            get
            {
                return _name_city;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name_city = value;
                }
                else
                {
                    _name_city = String.Empty;
                }
            }
        }
        /// <summary>
        /// 县(区)
        /// </summary>
        public string name_county
        {
            get
            {
                return _name_county;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name_county = value;
                }
                else
                {
                    _name_county = String.Empty;
                }
            }
        }
        /// <summary>
        /// 联系人
        /// </summary>
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name = value;
                }
                else
                {
                    _name = String.Empty;
                }
            }
        }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone
        {
            get
            {
                return _phone;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _phone = value;
                }
                else
                {
                    _phone = String.Empty;
                }
            }
        }
        /// <summary>
        /// 是否已激活
        /// </summary>
        public YesNoFlag flag_actived
        {
            get
            {
                return _flag_actived;
            }
            set
            {
                _flag_actived = value;
            }
        }
        /// <summary>
        /// 来源
        /// </summary>
        public string name_flag_from
        {
            get
            {
                return _name_flag_from;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name_flag_from = value;
                }
                else
                {
                    _name_flag_from = String.Empty;
                }
            }
        }
        /// <summary>
        /// 关注关系状态
        /// </summary>
        public string flag_gcgx
        {
            get
            {
                return _flag_gcgx;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _flag_gcgx = value;
                }
                else
                {
                    _flag_gcgx = String.Empty;
                }
            }
        }
        #endregion
    }
}

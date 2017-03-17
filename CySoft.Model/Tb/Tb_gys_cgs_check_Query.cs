#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using CySoft.Model.Flags;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    /// <summary>
    /// 供应商采购商关注审核
    /// </summary>
    [Serializable]
  
 
    public class Tb_Gys_Cgs_Check_Query:Tb_Gys_Cgs_Check
    {
        #region public method

        public Tb_Gys_Cgs_Check Clone()
        {
            return (Tb_Gys_Cgs_Check)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _companyname = String.Empty;
        private string _username = String.Empty;
        private string _name_province = String.Empty;
        private string _name_city = String.Empty;
        private string _name_county = String.Empty;
        private string _name_cgs_level = String.Empty;
        private string _name = String.Empty;
        private string _phone = String.Empty;
        private YesNoFlag _flag_actived = YesNoFlag.No;
        private string _name_flag_from = String.Empty;
        private string _bm_Interface = String.Empty;
        private YesNoFlag _flag_stop = YesNoFlag.No;

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
        /// 客户级别
        /// </summary>
        public string name_cgs_level
        {
            get
            {
                return _name_cgs_level;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name_cgs_level = value;
                }
                else
                {
                    _name_cgs_level = String.Empty;
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
        /// 接口编码
        /// </summary>
        public string bm_Interface
        {
            get
            {
                return _bm_Interface;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _bm_Interface = value;
                }
                else
                {
                    _bm_Interface = String.Empty;
                }
            }
        }
        /// <summary>
        /// 是否停用
        /// </summary>
        public YesNoFlag flag_stop
        {
            get
            {
                return _flag_stop;
            }
            set
            {
                _flag_stop = value;
            }
        }

        #endregion

    }
}
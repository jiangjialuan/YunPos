using System;
using CySoft.Model.Flags;

namespace CySoft.Model.Tb
{
    [Serializable]
    public class Tb_Gys_Cgs_Query
    {
        private long _id = 0;
        private long _id_user_master_cgs = 0;
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
        private string _remark = String.Empty;
        private string _refuse = String.Empty;
        private string _name_bank = String.Empty;
        private string _account_bank = String.Empty;
        private string _khr = String.Empty;
        private string _no_tax = String.Empty;
        private string _title_invoice = String.Empty;

        /// <summary>
        /// 采购商
        /// </summary>
        public Nullable<long> id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value.HasValue)
                {
                    _id = value.Value;
                }
                else
                {
                    _id = 0;
                }
            }
        }
        /// <summary>
        /// 采购商主用户Id
        /// </summary>
        public Nullable<long> id_user_master_cgs
        {
            get
            {
                return _id_user_master_cgs;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_user_master_cgs = value.Value;
                }
                else
                {
                    _id_user_master_cgs = 0;
                }
            }
        }
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


        /// <summary>
        /// 备注
        /// </summary>
        public string remark
        {
            get
            {
                return _remark;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _remark = value;
                }
                else
                {
                    _remark = String.Empty;
                }
            }
        }

        /// <summary>
        /// 拒绝理由
        /// </summary>
        public string refuse
        {
            get
            {
                return _refuse;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _refuse = value;
                }
                else
                {
                    _refuse = String.Empty;
                }
            }
        }

        /// <summary>
        /// 开户银行
        /// </summary>
        public string name_bank
        {
            get
            {
                return _name_bank;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name_bank = value;
                }
                else
                {
                    _name_bank = String.Empty;
                }
            }
        }

        /// <summary>
        /// 银行账号
        /// </summary>
        public string account_bank
        {
            get
            {
                return _account_bank;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _account_bank = value;
                }
                else
                {
                    _account_bank = String.Empty;
                }
            }
        }

        /// <summary>
        /// 开户人
        /// </summary>
        public string khr
        {
            get
            {
                return _khr;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _khr = value;
                }
                else
                {
                    _khr = String.Empty;
                }
            }
        }

        /// <summary>
        /// 纳税号
        /// </summary>
        public string no_tax
        {
            get
            {
                return _no_tax;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _no_tax = value;
                }
                else
                {
                    _no_tax = String.Empty;
                }
            }
        }

        /// <summary>
        /// 发票抬头
        /// </summary>
        public string title_invoice
        {
            get
            {
                return _title_invoice;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _title_invoice = value;
                }
                else
                {
                    _title_invoice = String.Empty;
                }
            }
        }

    }
}

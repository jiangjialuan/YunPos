using System;
using System.Diagnostics;
using CySoft.Model.Mapping;

namespace CySoft.Model.Tb
{
    /// <summary>
    /// 编辑采购商
    /// </summary>
    [Serializable]
    [Table("Tb_gys", "Tb_Gys")]
    [DebuggerDisplay("id = {id}, id_user_master = {id_user_master}, companyname = {companyname}, bm_gys_Interface = {bm_gys_Interface}")]
    public class Tb_Gys_Edit : Tb_Gys
    {
        private string _name = String.Empty;
        private string _companyname = String.Empty;
        private string _job = String.Empty;
        private string _qq = String.Empty;
        private string _email = String.Empty;
        private string _phone = String.Empty;
        private string _tel = String.Empty;
        private string _fax = String.Empty;
        private string _zipcode = String.Empty;
        private string _address = String.Empty;
        private DateTime _rq_treaty_start = new DateTime(1900, 1, 1);
        private DateTime _rq_treaty_end = new DateTime(1900, 1, 1);
        private string _bm_cgs_Interface = String.Empty;
        private string _name_hy = String.Empty;
        private string _name_province = String.Empty;
        private string _name_city = String.Empty;
        private string _name_county = String.Empty;

        private string _order_DeliveryDate_flag = string.Empty;
        private string _order_special_price_flag = string.Empty;
        private string _order_invoice_flag = string.Empty;
        private string _order_invoice_additional_flag=string.Empty;
        private string _order_invoice_fax_flag = string.Empty;
        /// <summary>
        /// 姓名
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
        /// 职务
        /// </summary>
        public string job
        {
            get
            {
                return _job;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _job = value;
                }
                else
                {
                    _job = String.Empty;
                }
            }
        }

        /// <summary>
        /// 腾讯QQ
        /// </summary>
        public string qq
        {
            get
            {
                return _qq;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _qq = value;
                }
                else
                {
                    _qq = String.Empty;
                }
            }
        }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string email
        {
            get
            {
                return _email;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _email = value;
                }
                else
                {
                    _email = String.Empty;
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
        /// 电话号码
        /// </summary>
        public string tel
        {
            get
            {
                return _tel;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _tel = value;
                }
                else
                {
                    _tel = String.Empty;
                }
            }
        }

        /// <summary>
        /// 传真
        /// </summary>
        public string fax
        {
            get
            {
                return _fax;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _fax = value;
                }
                else
                {
                    _fax = String.Empty;
                }
            }
        }

        /// <summary>
        /// 邮编
        /// </summary>
        public string zipcode
        {
            get
            {
                return _zipcode;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _zipcode = value;
                }
                else
                {
                    _zipcode = String.Empty;
                }
            }
        }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string address
        {
            get
            {
                return _address;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _address = value;
                }
                else
                {
                    _address = String.Empty;
                }
            }
        }

        /// <summary>
        /// 合约有效期起
        /// </summary>
        public Nullable<DateTime> rq_treaty_start
        {
            get
            {
                return _rq_treaty_start;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_treaty_start = value.Value;
                }
                else
                {
                    _rq_treaty_start = new DateTime(1900, 1, 1);
                }
            }
        }

        /// <summary>
        /// 合约有效期止
        /// </summary>
        public Nullable<DateTime> rq_treaty_end
        {
            get
            {
                return _rq_treaty_end;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_treaty_end = value.Value;
                }
                else
                {
                    _rq_treaty_end = new DateTime(1900, 1, 1);
                }
            }
        }

        /// <summary>
        /// 所属行业
        /// </summary>
        public string name_hy
        {
            get
            {
                return _name_hy;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name_hy = value;
                }
                else
                {
                    _name_hy = String.Empty;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string bm_cgs_Interface
        {
            get
            {
                return _bm_cgs_Interface;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _bm_cgs_Interface = value;
                }
                else
                {
                    _bm_cgs_Interface = String.Empty;
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
        /// 发货日期必填
        /// </summary>
        public string order_DeliveryDate_flag {
            get {
                return _order_DeliveryDate_flag;
            }
            set {
                _order_DeliveryDate_flag = value;
            }
        }
        /// <summary>
        /// 开启特价
        /// </summary>
        public string order_special_price_flag {
            get {
                return _order_special_price_flag;
            }
            set {
                _order_special_price_flag = value;
            }
        }

        /// <summary>
        /// 是否启用发票
        /// </summary>
        public string order_invoice_flag
        {
            get {
                return _order_invoice_flag;
            }
            set {
                _order_invoice_flag = value;
            }
        }

        /// <summary>
        /// 开具增值税
        /// </summary>
        public string order_invoice_additional_flag {
            get {
                return _order_invoice_additional_flag;
            }
            set {
                _order_invoice_additional_flag = value;
            }
        }
        /// <summary>
        /// 是否开启发票税率
        /// </summary>
        public string order_invoice_fax_flag {
            get {
                return _order_invoice_fax_flag;
            }
            set {
                _order_invoice_fax_flag = value;
            }
        }
    }
}

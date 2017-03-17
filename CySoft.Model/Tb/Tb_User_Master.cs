using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Tb
{
    public class Tb_User_Master : Tb_User
    {
        private string _name_hy = String.Empty;
        private string _name_province = String.Empty;
        private string _name_city = String.Empty;
        private string _name_county = String.Empty;
        private long _flag_role = 0;
        private long _id_master = 0;
        /// <summary>
        /// 
        /// </summary>
        public Nullable<long> id_master
        {
            get
            {
                return _id_master;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_master = value.Value;
                }
                else
                {
                    _id_master = 0;
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
        /// 商品总数
        /// </summary>
        public int sp_count { get; set; }
        /// <summary>
        /// 采购订单数
        /// </summary>
        public int order_cgs_count { get; set; }
        /// <summary>
        /// 销售订单数
        /// </summary>
        public int order_gys_count { get; set; }
        /// <summary>
        /// 客户（采购商）数
        /// </summary>
        public int cgs_count { get; set; }
        /// <summary>
        /// 供应商数
        /// </summary>
        public int gys_count { get; set; }
        /// <summary>
        /// 员工帐号数
        /// </summary>
        public int staff_count { get; set; }
        /// <summary>
        /// 行数
        /// </summary>
        public int rownumber { get; set; }
    }
}

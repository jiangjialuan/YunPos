using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Tb
{
    public class Tb_User_Query : Tb_User
    {
       
        private string _name_hy = String.Empty;
        private string _name_province = String.Empty;
        private string _name_city = String.Empty;
        private string _name_county = String.Empty;
        private long _flag_role = 0;
        private long _id_master = 0;
        public IList<Tb_User_Checkin> checkLog { get; set; }//签到记录结果集（目前只获取前10条）
        public IList<Tb_User_Pic> picList { get; set; }//图册结果集
        public IList<string> picUrl { get; set; }//上传图册路径结果集
        /// <summary>
        /// 角色标示
        /// 目前主要作用用于在员工账户内使用，作为员工是否属于业务员的标示
        /// </summary>
        public Nullable<long> flag_role
        {
            get
            {
                return _flag_role;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_role = value.Value;
                }
                else
                {
                    _flag_role = 0;
                }
            }
        }

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


        public int rownumber { get; set; }

        public string valid_email { get; set; }
        public string valid_phone { get; set; }

        public string name_shop { get; set; }

        
    }
}

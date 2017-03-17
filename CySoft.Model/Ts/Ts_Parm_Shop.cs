using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Mapping;

namespace CySoft.Model.Ts
{
    [Serializable]
    [Table("ts_parm_shop","Ts_Parm_Shop")]
	[DebuggerDisplay("id = {id}")]
	public class Ts_Parm_Shop
	{	
		#region public method
		
		public Ts_Parm_Shop Clone()
		{
			return (Ts_Parm_Shop)this.MemberwiseClone();
		}
		
		#endregion
		
		#region private field
		
		private string _id_masteruser  = String.Empty;
		private string _id  = String.Empty;
		private string _id_shop  = String.Empty;
		private string _parmcode  = String.Empty;
		private string _parmname  = String.Empty;
		private string _parmvalue  = String.Empty;
        private byte[] _nlast = null;
        private string _regex = String.Empty;
        private string _version = String.Empty;
        private string _parmdescribe = String.Empty;
        private int _sort_id = 0;
        private byte _flag_editstyle = 0;
        private int _flag_type = 0;
		#endregion
		
		#region public property
		
		public string id_masteruser
        {
            get
            {
                return _id_masteruser;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _id_masteruser = value;
                }
				else
				{
					_id_masteruser = String.Empty;
				}
            }
        }
		
		public string id
        {
            get
            {
                return _id;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _id = value;
                }
				else
				{
					_id = String.Empty;
				}
            }
        }
        public string regex
        {
            get
            {
                return _regex;
            }
            set { _regex = value; }
        }
		public string id_shop
        {
            get
            {
                return _id_shop;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _id_shop = value;
                }
				else
				{
					_id_shop = String.Empty;
				}
            }
        }
		
		public string parmcode
        {
            get
            {
                return _parmcode;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _parmcode = value;
                }
				else
				{
					_parmcode = String.Empty;
				}
            }
        }
		
		public string parmname
        {
            get
            {
                return _parmname;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _parmname = value;
                }
				else
				{
					_parmname = String.Empty;
				}
            }
        }
		
		public string parmvalue
        {
            get
            {
                return _parmvalue;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _parmvalue = value;
                }
				else
				{
					_parmvalue = String.Empty;
				}
            }
        }
		
		public Byte[] nlast
        {
            get
            {
                return _nlast;
            }
            set { _nlast = value; }
        }
        public string version
        {
            get
            {
                return _version;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _version = value;
                }
                else
                {
                    _version = String.Empty;
                }
            }
        }

        public string parmdescribe
        {
            get
            {
                return _parmdescribe;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _parmdescribe = value;
                }
                else
                {
                    _parmdescribe = String.Empty;
                }
            }
        }

        public Nullable<int> sort_id
        {
            get
            {
                return _sort_id;
            }
            set
            {
                if (value.HasValue)
                {
                    _sort_id = value.Value;
                }
                else
                {
                    _sort_id = 0;
                }
            }
        }

        public Nullable<byte> flag_editstyle
        {
            get
            {
                return _flag_editstyle;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_editstyle = value.Value;
                }
                else
                {
                    _flag_editstyle = 0;
                }
            }
        }

        public Nullable<int> flag_type
        {
            get
            {
                return _flag_type;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_type = value.Value;
                }
                else
                {
                    _flag_type = 0;
                }
            }
        }
		#endregion
		
	}
    [Serializable]
    [Table("ts_parm_shop", "Ts_Parm_Shop")]
    [DebuggerDisplay("id = {id}")]
    public class Ts_Parm_ShopWithShopMc : Ts_Parm_Shop
    {
        public string mc { get; set; }
    }

    public class UpdateParmShop : Ts_Parm_Shop
    {
        public string id_shop_master { get; set; }
    }


    public class Ts_HykDbjf 
    {
        public string id_masteruser { get; set; }
        public string id_shop { get; set; }

        public string hy_jfsz_hysr_nbjf { get; set; }//阳历生日--几倍
        public string hy_jfsz_hysr_lx { get; set; }//阳历生日--时间内
        public string hy_jfsz_week_nbjf { get; set; }//按星期--几倍
        public string hy_jfsz_week_val { get; set; }//按星期--星期
        public string hy_jfsz_day_nbjf { get; set; }//按天--几倍
        public string hy_jfsz_day_val { get; set; }//按天--天
        public string hy_jfsz_xs_nbjf { get; set; }//按金额--几倍
        public string hy_jfsz_xs_rq_b { get; set; }//按金额--开始时间
        public string hy_jfsz_xs_rq_e { get; set; }//按金额--结束时间
        public string hy_jfsz_xs_je { get; set; }//按金额--满多少元
        public string hy_czje_min_onec { get; set; }//每日最小充值金额:
        public string hy_czje_max_onec { get; set; }//每日最大充值金额:
        public string hy_czje_max_month { get; set; }//每月最大充值金额:


    }




}

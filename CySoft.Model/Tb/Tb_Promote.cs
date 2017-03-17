using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Mapping;

namespace CySoft.Model.Tb
{
     [Serializable]
    [Table("tb_promote","Tb_Promote")]
	[DebuggerDisplay("id = {id}")]
	public class Tb_Promote
	{	
		#region public method
		
		public Tb_Promote Clone()
		{
			return (Tb_Promote)this.MemberwiseClone();
		}
		
		#endregion
		
		#region private field
		
		private string _id_masteruser  = String.Empty;
		private string _id  = String.Empty;
		private string _id_bill  = String.Empty;
		private string _id_billmx  = String.Empty;
		private int _sort_id  = 0;
		private DateTime _day_b  = new DateTime(1900,1,1);
		private DateTime _day_e  = new DateTime(1900,1,1);
		private string _time_b  = String.Empty;
		private string _time_e  = String.Empty;
		private byte _yxj  = 0;
		private string _weeks  = String.Empty;
		private string _days  = String.Empty;
		private string _rule_name  = String.Empty;
		private string _id_object  = String.Empty;
		private string _style  = String.Empty;
		private string _examine  = String.Empty;
		private string _preferential  = String.Empty;
		private string _strategy  = String.Empty;
		private decimal _sl_largess_1  = 0m;
		private decimal _sl_largess_2  = 0m;
		private decimal _sl_largess_3  = 0m;
		private string _condition_1  = String.Empty;
		private string _condition_2  = String.Empty;
		private string _condition_3  = String.Empty;
		private string _result_1  = String.Empty;
		private string _result_2  = String.Empty;
		private string _result_3  = String.Empty;
		private string _zh_group  = String.Empty;
		private decimal _sl_billxl  = 0m;
		private byte _flag_zsz  = 0;
		private byte _flag_moling  = 0;
		private byte _flag_stop  = 0;
		private string _id_hyfl_list  = String.Empty;
		private string _id_create  = String.Empty;
		private DateTime _rq_create  = new DateTime(1900,1,1);
		private byte _flag_delete  = 0;
		private byte[] _nlast  = null;
		private string _bm_djlx  = String.Empty;
		
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
		
		public string id_bill
        {
            get
            {
                return _id_bill;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _id_bill = value;
                }
				else
				{
					_id_bill = String.Empty;
				}
            }
        }
		
		public string id_billmx
        {
            get
            {
                return _id_billmx;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _id_billmx = value;
                }
				else
				{
					_id_billmx = String.Empty;
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
		
		public Nullable<DateTime> day_b
        {
            get
            {
                return _day_b;
            }
            set
            {
                if (value.HasValue)
                {
                    _day_b = value.Value;
                }
				else
				{
					_day_b = new DateTime(1900,1,1);
				}
            }
        }
		
		public Nullable<DateTime> day_e
        {
            get
            {
                return _day_e;
            }
            set
            {
                if (value.HasValue)
                {
                    _day_e = value.Value;
                }
				else
				{
					_day_e = new DateTime(1900,1,1);
				}
            }
        }
		
		public string time_b
        {
            get
            {
                return _time_b;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _time_b = value;
                }
				else
				{
					_time_b = String.Empty;
				}
            }
        }
		
		public string time_e
        {
            get
            {
                return _time_e;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _time_e = value;
                }
				else
				{
					_time_e = String.Empty;
				}
            }
        }
		
		public Nullable<byte> yxj
        {
            get
            {
                return _yxj;
            }
            set
            {
                if (value.HasValue)
                {
                    _yxj = value.Value;
                }
				else
				{
					_yxj = 0;
				}
            }
        }
		
		public string weeks
        {
            get
            {
                return _weeks;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _weeks = value;
                }
				else
				{
					_weeks = String.Empty;
				}
            }
        }
		
		public string days
        {
            get
            {
                return _days;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _days = value;
                }
				else
				{
					_days = String.Empty;
				}
            }
        }
		
		public string rule_name
        {
            get
            {
                return _rule_name;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _rule_name = value;
                }
				else
				{
					_rule_name = String.Empty;
				}
            }
        }
		
		public string id_object
        {
            get
            {
                return _id_object;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _id_object = value;
                }
				else
				{
					_id_object = String.Empty;
				}
            }
        }
		
		public string style
        {
            get
            {
                return _style;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _style = value;
                }
				else
				{
					_style = String.Empty;
				}
            }
        }
		
		public string examine
        {
            get
            {
                return _examine;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _examine = value;
                }
				else
				{
					_examine = String.Empty;
				}
            }
        }
		
		public string preferential
        {
            get
            {
                return _preferential;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _preferential = value;
                }
				else
				{
					_preferential = String.Empty;
				}
            }
        }
		
		public string strategy
        {
            get
            {
                return _strategy;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _strategy = value;
                }
				else
				{
					_strategy = String.Empty;
				}
            }
        }
		
		public Nullable<decimal> sl_largess_1
        {
            get
            {
                return _sl_largess_1;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_largess_1 = value.Value;
                }
				else
				{
					_sl_largess_1 = 0m;
				}
            }
        }
		
		public Nullable<decimal> sl_largess_2
        {
            get
            {
                return _sl_largess_2;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_largess_2 = value.Value;
                }
				else
				{
					_sl_largess_2 = 0m;
				}
            }
        }
		
		public Nullable<decimal> sl_largess_3
        {
            get
            {
                return _sl_largess_3;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_largess_3 = value.Value;
                }
				else
				{
					_sl_largess_3 = 0m;
				}
            }
        }
		
		public string condition_1
        {
            get
            {
                return _condition_1;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _condition_1 = value;
                }
				else
				{
					_condition_1 = String.Empty;
				}
            }
        }
		
		public string condition_2
        {
            get
            {
                return _condition_2;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _condition_2 = value;
                }
				else
				{
					_condition_2 = String.Empty;
				}
            }
        }
		
		public string condition_3
        {
            get
            {
                return _condition_3;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _condition_3 = value;
                }
				else
				{
					_condition_3 = String.Empty;
				}
            }
        }
		
		public string result_1
        {
            get
            {
                return _result_1;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _result_1 = value;
                }
				else
				{
					_result_1 = String.Empty;
				}
            }
        }
		
		public string result_2
        {
            get
            {
                return _result_2;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _result_2 = value;
                }
				else
				{
					_result_2 = String.Empty;
				}
            }
        }
		
		public string result_3
        {
            get
            {
                return _result_3;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _result_3 = value;
                }
				else
				{
					_result_3 = String.Empty;
				}
            }
        }
		
		public string zh_group
        {
            get
            {
                return _zh_group;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _zh_group = value;
                }
				else
				{
					_zh_group = String.Empty;
				}
            }
        }
		
		public Nullable<decimal> sl_billxl
        {
            get
            {
                return _sl_billxl;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_billxl = value.Value;
                }
				else
				{
					_sl_billxl = 0m;
				}
            }
        }
		
		public Nullable<byte> flag_zsz
        {
            get
            {
                return _flag_zsz;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_zsz = value.Value;
                }
				else
				{
					_flag_zsz = 0;
				}
            }
        }
		
		public Nullable<byte> flag_moling
        {
            get
            {
                return _flag_moling;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_moling = value.Value;
                }
				else
				{
					_flag_moling = 0;
				}
            }
        }
		
        [Column(Update = false)]
		public Nullable<byte> flag_stop
        {
            get
            {
                return _flag_stop;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_stop = value.Value;
                }
				else
				{
					_flag_stop = 0;
				}
            }
        }
		
		public string id_hyfl_list
        {
            get
            {
                return _id_hyfl_list;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _id_hyfl_list = value;
                }
				else
				{
					_id_hyfl_list = String.Empty;
				}
            }
        }
		
        [Column(Update = false)]
		public string id_create
        {
            get
            {
                return _id_create;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _id_create = value;
                }
				else
				{
					_id_create = String.Empty;
				}
            }
        }
		
        [Column(Update = false, Insert = false)]
		public Nullable<DateTime> rq_create
        {
            get
            {
                return _rq_create;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_create = value.Value;
                }
				else
				{
					_rq_create = new DateTime(1900,1,1);
				}
            }
        }
		
		public Nullable<byte> flag_delete
        {
            get
            {
                return _flag_delete;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_delete = value.Value;
                }
				else
				{
					_flag_delete = 0;
				}
            }
        }
		
		public Byte[] nlast { get; set; }
		
		public string bm_djlx
        {
            get
            {
                return _bm_djlx;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _bm_djlx = value;
                }
				else
				{
					_bm_djlx = String.Empty;
				}
            }
        }
		
		#endregion
		
	}

    public class Tb_Promote_Query : Tb_Promote
    {
        /// <summary>
        ///  商品名称
        /// </summary>
        public string sp_mc { get; set; }
        /// <summary>
        /// 条件
        /// </summary>
        public string barcode { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string dw { get; set; }

        public string shop_mc { get; set; }

        public string bm { get; set; }

        public string dh { get; set; }
    }

}

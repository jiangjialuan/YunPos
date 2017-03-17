#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("tb_shopsp_qc_md","Tb_Shopsp_Qc_Md")]
	[DebuggerDisplay("id = {id}")]
	public class Tb_Shopsp_Qc_Md
	{	
		#region public method
		
		public Tb_Shopsp_Qc_Md Clone()
		{
			return (Tb_Shopsp_Qc_Md)this.MemberwiseClone();
		}
		
		#endregion
		
		#region private field

        private string _id_masteruser = String.Empty;
		private string _id  = String.Empty;
		private string _id_shop  = String.Empty;
		//private string _id_sp  = String.Empty;
		private int _dw_xh  = 0;
		private string _dw  = String.Empty;
		private string _id_kcsp  = String.Empty;
		private string _bm  = String.Empty;
		private string _mc  = String.Empty;
		private string _id_spfl  = String.Empty;
		private string _barcode  = String.Empty;
		private string _zjm  = String.Empty;
		private decimal _zhl  = 0m;
		private string _cd  = String.Empty;
		private byte _flag_state  = 0;
		private byte _flag_czfs  = 0;
		private decimal _dj_ls  = 0m;
		private decimal _dj_jh  = 0m;
		private decimal _sl_kc_min  = 0m;
		private decimal _sl_kc_max  = 0m;
		private string _bz  = String.Empty;
		private string _pic_path  = String.Empty;
		private string _id_create  = String.Empty;
		private DateTime _rq_create  = new DateTime(1900,1,1);
		private string _id_edit  = String.Empty;
		private DateTime _rq_edit  = new DateTime(1900,1,1);
		private byte _flag_delete  = 0;
		private string _id_qc  = String.Empty;
		private decimal _sl_qc  = 0m;
		private decimal _dj_cb  = 0m;
		private decimal _je_qc  = 0m;
		private string _md_mc  = String.Empty;
		private string _md_bm  = String.Empty;
		private string _md_lxr  = String.Empty;
		private string _md_tel  = String.Empty;
		
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
		
        //public string id_sp
        //{
        //    get
        //    {
        //        return _id_sp;
        //    }
        //    set
        //    {
        //        if (!String.IsNullOrEmpty(value))
        //        {
        //            _id_sp = value;
        //        }
        //        else
        //        {
        //            _id_sp = String.Empty;
        //        }
        //    }
        //}
		
		public Nullable<int> dw_xh
        {
            get
            {
                return _dw_xh;
            }
            set
            {
                if (value.HasValue)
                {
                    _dw_xh = value.Value;
                }
				else
				{
					_dw_xh = 0;
				}
            }
        }
		
		public string dw
        {
            get
            {
                return _dw;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _dw = value;
                }
				else
				{
					_dw = String.Empty;
				}
            }
        }
		
		public string id_kcsp
        {
            get
            {
                return _id_kcsp;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _id_kcsp = value;
                }
				else
				{
					_id_kcsp = String.Empty;
				}
            }
        }
		
		public string bm
        {
            get
            {
                return _bm;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _bm = value;
                }
				else
				{
					_bm = String.Empty;
				}
            }
        }
		
		public string mc
        {
            get
            {
                return _mc;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _mc = value;
                }
				else
				{
					_mc = String.Empty;
				}
            }
        }
		
		public string id_spfl
        {
            get
            {
                return _id_spfl;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _id_spfl = value;
                }
				else
				{
					_id_spfl = String.Empty;
				}
            }
        }
		
		public string barcode
        {
            get
            {
                return _barcode;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _barcode = value;
                }
				else
				{
					_barcode = String.Empty;
				}
            }
        }
		
		public string zjm
        {
            get
            {
                return _zjm;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _zjm = value;
                }
				else
				{
					_zjm = String.Empty;
				}
            }
        }
		
		public Nullable<decimal> zhl
        {
            get
            {
                return _zhl;
            }
            set
            {
                if (value.HasValue)
                {
                    _zhl = value.Value;
                }
				else
				{
					_zhl = 0m;
				}
            }
        }
		
		public string cd
        {
            get
            {
                return _cd;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _cd = value;
                }
				else
				{
					_cd = String.Empty;
				}
            }
        }
		
		public Nullable<byte> flag_state
        {
            get
            {
                return _flag_state;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_state = value.Value;
                }
				else
				{
					_flag_state = 0;
				}
            }
        }
		
		/// <summary>
        /// 3个方式:普通\称重\计数
   //普通商品不传送数据到电子称
        /// </summary>
		public Nullable<byte> flag_czfs
        {
            get
            {
                return _flag_czfs;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_czfs = value.Value;
                }
				else
				{
					_flag_czfs = 0;
				}
            }
        }
		
		public Nullable<decimal> dj_ls
        {
            get
            {
                return _dj_ls;
            }
            set
            {
                if (value.HasValue)
                {
                    _dj_ls = value.Value;
                }
				else
				{
					_dj_ls = 0m;
				}
            }
        }
		
		public Nullable<decimal> dj_jh
        {
            get
            {
                return _dj_jh;
            }
            set
            {
                if (value.HasValue)
                {
                    _dj_jh = value.Value;
                }
				else
				{
					_dj_jh = 0m;
				}
            }
        }
		
		public Nullable<decimal> sl_kc_min
        {
            get
            {
                return _sl_kc_min;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_kc_min = value.Value;
                }
				else
				{
					_sl_kc_min = 0m;
				}
            }
        }
		
		public Nullable<decimal> sl_kc_max
        {
            get
            {
                return _sl_kc_max;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_kc_max = value.Value;
                }
				else
				{
					_sl_kc_max = 0m;
				}
            }
        }
		
		public string bz
        {
            get
            {
                return _bz;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _bz = value;
                }
				else
				{
					_bz = String.Empty;
				}
            }
        }
		
		public string pic_path
        {
            get
            {
                return _pic_path;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _pic_path = value;
                }
				else
				{
					_pic_path = String.Empty;
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
		
		public string id_edit
        {
            get
            {
                return _id_edit;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _id_edit = value;
                }
				else
				{
					_id_edit = String.Empty;
				}
            }
        }
		
        [Column(Insert = false)]
		public Nullable<DateTime> rq_edit
        {
            get
            {
                return _rq_edit;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_edit = value.Value;
                }
				else
				{
					_rq_edit = new DateTime(1900,1,1);
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
		
		public string id_qc
        {
            get
            {
                return _id_qc;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _id_qc = value;
                }
				else
				{
					_id_qc = String.Empty;
				}
            }
        }
		
		public Nullable<decimal> sl_qc
        {
            get
            {
                return _sl_qc;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_qc = value.Value;
                }
				else
				{
					_sl_qc = 0m;
				}
            }
        }
		
		public Nullable<decimal> dj_cb
        {
            get
            {
                return _dj_cb;
            }
            set
            {
                if (value.HasValue)
                {
                    _dj_cb = value.Value;
                }
				else
				{
					_dj_cb = 0m;
				}
            }
        }
		
		public Nullable<decimal> je_qc
        {
            get
            {
                return _je_qc;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_qc = value.Value;
                }
				else
				{
					_je_qc = 0m;
				}
            }
        }
		
		public string md_mc
        {
            get
            {
                return _md_mc;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _md_mc = value;
                }
				else
				{
					_md_mc = String.Empty;
				}
            }
        }
		
		public string md_bm
        {
            get
            {
                return _md_bm;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _md_bm = value;
                }
				else
				{
					_md_bm = String.Empty;
				}
            }
        }
		
		public string md_lxr
        {
            get
            {
                return _md_lxr;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _md_lxr = value;
                }
				else
				{
					_md_lxr = String.Empty;
				}
            }
        }
		
		public string md_tel
        {
            get
            {
                return _md_tel;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _md_tel = value;
                }
				else
				{
					_md_tel = String.Empty;
				}
            }
        }
		
		#endregion
		
	}
}




#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using System.Collections.Generic;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("tb_gys","Tb_Gys")]
	[DebuggerDisplay("id = {id}")]
	public class Tb_Gys
	{	
		#region public method
		
		public Tb_Gys Clone()
		{
			return (Tb_Gys)this.MemberwiseClone();
		}
		
		#endregion
		
		#region private field
		
		private string _id_masteruser  = String.Empty;
		private string _id  = String.Empty;
		private string _bm  = String.Empty;
		private string _mc  = String.Empty;
		private string _companytel  = String.Empty;
		private string _zjm  = String.Empty;
		private string _tel  = String.Empty;
		private string _lxr  = String.Empty;
		private string _email  = String.Empty;
		private string _zipcode  = String.Empty;
		private string _address  = String.Empty;
		private byte _flag_state  = 0;
		private string _bz  = String.Empty;
		private string _id_create  = String.Empty;
		private DateTime _rq_create  = new DateTime(1900,1,1);
		private string _id_edit  = String.Empty;
		private DateTime _rq_edit  = new DateTime(1900,1,1);
		private byte _flag_delete  = 0;
        private byte[] _nlast;
        private string _id_gysfl = String.Empty;
		
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
		
		public string companytel
        {
            get
            {
                return _companytel;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _companytel = value;
                }
				else
				{
					_companytel = String.Empty;
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
		
		public string lxr
        {
            get
            {
                return _lxr;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _lxr = value;
                }
				else
				{
					_lxr = String.Empty;
				}
            }
        }
		
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

        public Byte[] nlast { get; set; }


        public string id_gysfl
        {
            get
            {
                return _id_gysfl;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _id_gysfl = value;
                }
				else
				{
                    _id_gysfl = String.Empty;
				}
            }
        }

		#endregion



     


		
	}

    public class Tb_Gys_User_QueryModel : Tb_Gys
    {

    }

    public class Tb_Gys_Import
    {
        public string id_gysfl { set; get; }
        public string mc { set; get; }
        public string bm { set; get; }
        public string flag_state { set; get; }
        public string lxr { set; get; }
        public string tel { set; get; }
        public string companytel { set; get; }
        public string email { set; get; }
        public string zipcode { set; get; }
        public string address { set; get; }
        public string bz { set; get; }
        public string bz_sys { set; get; }
    }

    public class Tb_Gys_Import_All
    {
        public List<Tb_Gys_Import> SuccessList { set; get; }
        public List<Tb_Gys_Import> FailList { set; get; }

    }



}
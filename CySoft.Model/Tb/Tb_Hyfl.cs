using CySoft.Model.Mapping;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("tb_hyfl","Tb_Hyfl")]
	[DebuggerDisplay("id = {id}")]
	public class Tb_Hyfl
	{	
		#region public method
		
		public Tb_Hyfl Clone()
		{
			return (Tb_Hyfl)this.MemberwiseClone();
		}
		
		#endregion
		
		#region private field
		
		private string _id_masteruser  = String.Empty;
		private string _id  = String.Empty;
		private string _bm  = String.Empty;
		private string _mc  = String.Empty;
		private int _sort_id  = 0;
		private string _id_create  = String.Empty;
		private DateTime _rq_create  = new DateTime(1900,1,1);
		private string _id_edit  = String.Empty;
		private DateTime _rq_edit  = new DateTime(1900,1,1);
		private byte _flag_delete  = 0;
		private byte[] _nlast  ;

        private byte _flag_yhlx = 0;
        private decimal _zk = 0m;
		
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


        public Nullable<byte> flag_yhlx
        {
            get
            {
                return _flag_yhlx;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_yhlx = value.Value;
                }
                else
                {
                    _flag_yhlx = 0;
                }
            }
        }

        public Nullable<decimal> zk
        {
            get
            {
                return _zk;
            }
            set
            {
                if (value.HasValue)
                {
                    _zk = value.Value;
                }
                else
                {
                    _zk = 0m;
                }
            }
        }
		
		#endregion
		
	}
}

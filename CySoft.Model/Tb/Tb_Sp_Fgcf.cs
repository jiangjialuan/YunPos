using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Mapping;

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("tb_sp_fgcf","Tb_Sp_Fgcf")]
	public class Tb_Sp_Fgcf
	{	
		#region public method
		
		public Tb_Sp_Fgcf Clone()
		{
			return (Tb_Sp_Fgcf)this.MemberwiseClone();
		}
		
		#endregion
		
		#region private field
		
		private string _id_masteruser  = String.Empty;
		private string _id  = String.Empty;
		private string _id_sp_cp  = String.Empty;
		private string _id_sp_yl  = String.Empty;
		private decimal _sl  = 0m;
		private byte _flag_stop  = 0;
		private byte _flag_rjauto  = 0;
		private string _id_create  = String.Empty;
		private DateTime _rq_create  = new DateTime(1900,1,1);
		private string _id_edit  = String.Empty;
		private DateTime _rq_edit  = new DateTime(1900,1,1);
		private byte _flag_delete  = 0;
		private byte[] _nlast  = null;
		
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
		
		public string id_sp_cp
        {
            get
            {
                return _id_sp_cp;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _id_sp_cp = value;
                }
				else
				{
					_id_sp_cp = String.Empty;
				}
            }
        }
		
		public string id_sp_yl
        {
            get
            {
                return _id_sp_yl;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _id_sp_yl = value;
                }
				else
				{
					_id_sp_yl = String.Empty;
				}
            }
        }
		
		public Nullable<decimal> sl
        {
            get
            {
                return _sl;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl = value.Value;
                }
				else
				{
					_sl = 0m;
				}
            }
        }
		 
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
		
		public Nullable<byte> flag_rjauto
        {
            get
            {
                return _flag_rjauto;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_rjauto = value.Value;
                }
				else
				{
					_flag_rjauto = 0;
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
		
        [Column(Insert = false)]
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
		[Column(Insert = false)]
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
		
		#endregion
		
	}
}

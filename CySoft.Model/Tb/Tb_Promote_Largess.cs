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
    [Table("tb_promote_largess","Tb_Promote_Largess")]
	[DebuggerDisplay("id = {id}")]
	public class Tb_Promote_Largess
	{	
		#region public method
		
		public Tb_Promote_Largess Clone()
		{
			return (Tb_Promote_Largess)this.MemberwiseClone();
		}
		
		#endregion
		
		#region private field
		
		private string _id_masteruser  = String.Empty;
		private string _id  = String.Empty;
		private byte[] _id_bill  ;
		private string _zs_group  = String.Empty;
		private string _id_sp  = String.Empty;
		private string _id_create  = String.Empty;
		private DateTime _rq_create  = new DateTime(1900,1,1);
		private byte _flag_delete  = 0;
		private byte[] _nlast  ;
		
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
		
		public Byte[] id_bill { get; set; }
		
		public string zs_group
        {
            get
            {
                return _zs_group;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _zs_group = value;
                }
				else
				{
					_zs_group = String.Empty;
				}
            }
        }
		
		public string id_sp
        {
            get
            {
                return _id_sp;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _id_sp = value;
                }
				else
				{
					_id_sp = String.Empty;
				}
            }
        }
		
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
		
		#endregion
		
	}
}

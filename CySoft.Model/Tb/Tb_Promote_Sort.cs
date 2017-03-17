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
    [Table("tb_promote_sort","Tb_Promote_Sort")]
	[DebuggerDisplay("id = {id}")]
	public class Tb_Promote_Sort
	{	
		#region public method
		
		public Tb_Promote_Sort Clone()
		{
			return (Tb_Promote_Sort)this.MemberwiseClone();
		}
		
		#endregion
		
		#region private field
		
		private string _id_masteruser  = String.Empty;
		private string _id  = String.Empty;
		private string _style  = String.Empty;
		private int _sort_id  = 0;
		private string _id_create  = String.Empty;
		private DateTime _rq_create  = new DateTime(1900,1,1);
		private byte[] _nlast;
		
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
		
		public Byte[] nlast { get; set; }
		
		#endregion
		
	}
}

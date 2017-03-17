#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Td
{
    [Serializable]
    [Table("td_saler_out_relation","Td_Saler_Out_Relation")]
	[DebuggerDisplay("id = {id}")]
	public class Td_Saler_Out_Relation
	{	
		#region public method
		
		public Td_Saler_Out_Relation Clone()
		{
			return (Td_Saler_Out_Relation)this.MemberwiseClone();
		}
		
		#endregion
		
		#region private field

        private System.Guid _id = Guid.NewGuid();
		private string _dh  = String.Empty;
		private string _dh_order  = String.Empty;
		private DateTime _rq_create  = new DateTime(1900,1,1);
		
		#endregion
		
		#region public property

        public Nullable<System.Guid> id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value.HasValue)
                {
                    _id = value.Value;
                }
				else
				{
                    _id = Guid.NewGuid();
            }
        }
        }
		
		public string dh
        {
            get
            {
                return _dh;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _dh = value;
                }
				else
				{
					_dh = String.Empty;
				}
            }
        }
		
		public string dh_order
        {
            get
            {
                return _dh_order;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _dh_order = value;
                }
				else
				{
					_dh_order = String.Empty;
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
		
		#endregion
		
	}
}

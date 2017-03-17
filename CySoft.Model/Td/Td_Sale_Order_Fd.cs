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
    [Table("td_sale_order_fd","Td_Sale_Order_Fd")]
	[DebuggerDisplay("id = {id}")]
	public class Td_Sale_Order_Fd
	{	
		#region public method
		
		public Td_Sale_Order_Fd Clone()
		{
			return (Td_Sale_Order_Fd)this.MemberwiseClone();
		}
		
		#endregion
		
		#region private field

        private System.Guid _id = Guid.NewGuid();
		private string _dh  = String.Empty;
		private string _dh_father  = String.Empty;
		private string _path  = String.Empty;
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
		
		public string dh_father
        {
            get
            {
                return _dh_father;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _dh_father = value;
                }
				else
				{
					_dh_father = String.Empty;
				}
            }
        }
		
		public string path
        {
            get
            {
                return _path;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _path = value;
                }
				else
				{
					_path = String.Empty;
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

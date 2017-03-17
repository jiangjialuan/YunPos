#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Ts
{
    [Serializable]
    [Table("ts_flag","Ts_Flag")]
	[DebuggerDisplay("id = {id}")]
	public class Ts_Flag
	{	
		#region public method
		
		public Ts_Flag Clone()
		{
			return (Ts_Flag)this.MemberwiseClone();
		}
		
		#endregion
		
		#region private field
		
		private string _id  = String.Empty;
		private string _listcode  = String.Empty;
		private string _listname  = String.Empty;
		private int _listdata  = 0;
		private string _listdisplay  = String.Empty;
		private int _listsort  = 0;
		private string _id_father  = String.Empty;
		private byte[] _nlast  = null;
		
		#endregion
		
		#region public property
		
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
		
		public string listcode
        {
            get
            {
                return _listcode;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _listcode = value;
                }
				else
				{
					_listcode = String.Empty;
				}
            }
        }
		
		public string listname
        {
            get
            {
                return _listname;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _listname = value;
                }
				else
				{
					_listname = String.Empty;
				}
            }
        }
		
		public Nullable<int> listdata
        {
            get
            {
                return _listdata;
            }
            set
            {
                if (value.HasValue)
                {
                    _listdata = value.Value;
                }
				else
				{
					_listdata = 0;
				}
            }
        }
		
		public string listdisplay
        {
            get
            {
                return _listdisplay;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _listdisplay = value;
                }
				else
				{
					_listdisplay = String.Empty;
				}
            }
        }
		
		public Nullable<int> listsort
        {
            get
            {
                return _listsort;
            }
            set
            {
                if (value.HasValue)
                {
                    _listsort = value.Value;
                }
				else
				{
					_listsort = 0;
				}
            }
        }
		
		public string id_father
        {
            get
            {
                return _id_father;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _id_father = value;
                }
				else
				{
					_id_father = String.Empty;
				}
            }
        }

        public Byte[] nlast { get; set; }
		
		#endregion
		
	}
}
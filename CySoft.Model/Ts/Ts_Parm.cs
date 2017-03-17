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
    [Table("ts_parm","Ts_Parm")]
	[DebuggerDisplay("id = {id}")]
	public class Ts_Parm
	{	
		#region public method
		
		public Ts_Parm Clone()
		{
			return (Ts_Parm)this.MemberwiseClone();
		}
		
		#endregion
		
		#region private field
		
		private string _id_masteruser  = String.Empty;
		private string _id  = String.Empty;
		private string _parmcode  = String.Empty;
		private string _parmname  = String.Empty;
		private string _parmvalue  = String.Empty;
		private byte[] _nlast  = null;
        private string _regex = String.Empty;
        private string _version = String.Empty;
        private string _parmdescribe = String.Empty;
        private int _sort_id = 0;
        private byte _flag_editstyle = 0;
		#endregion
		
		#region public property
        public string regex
        {
            get
            {
                return _regex;
            }
            set { _regex = value; }
        }
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
		
		public string parmcode
        {
            get
            {
                return _parmcode;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _parmcode = value;
                }
				else
				{
					_parmcode = String.Empty;
				}
            }
        }
		
		public string parmname
        {
            get
            {
                return _parmname;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _parmname = value;
                }
				else
				{
					_parmname = String.Empty;
				}
            }
        }
		
		public string parmvalue
        {
            get
            {
                return _parmvalue;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _parmvalue = value;
                }
				else
				{
					_parmvalue = String.Empty;
				}
            }
        }

        public Byte[] nlast { get; set; }
        public string version
        {
            get
            {
                return _version;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _version = value;
                }
                else
                {
                    _version = String.Empty;
                }
            }
        }
        public string parmdescribe
        {
            get
            {
                return _parmdescribe;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _parmdescribe = value;
                }
                else
                {
                    _parmdescribe = String.Empty;
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

        public Nullable<byte> flag_editstyle
        {
            get
            {
                return _flag_editstyle;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_editstyle = value.Value;
                }
                else
                {
                    _flag_editstyle = 0;
                }
            }
        }
		#endregion
		
	}
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Mapping;

namespace CySoft.Model.Ts
{
    [Serializable]
    [Table("ts_parm_terrace","Ts_Parm_Terrace")]
	[DebuggerDisplay("id = {id}")]
	public class Ts_Parm_Terrace
	{	
		#region public method
		
		public Ts_Parm_Terrace Clone()
		{
			return (Ts_Parm_Terrace)this.MemberwiseClone();
		}
		
		#endregion
		
		#region private field
		
		private string _id  = String.Empty;
		private string _parmcode  = String.Empty;
		private string _parmname  = String.Empty;
		private string _parmvalue  = String.Empty;
		private byte[] _nlast;
		
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
        
		
		#endregion
		
	}
}

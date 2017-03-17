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
    [Table("tb_pay_config","Tb_Pay_Config")]
	[DebuggerDisplay("id = {id}")]
	public class Tb_Pay_Config
	{	
		#region public method
		
		public Tb_Pay_Config Clone()
		{
			return (Tb_Pay_Config)this.MemberwiseClone();
		}
		
		#endregion
		
		#region private field
		
		private string _id_masteruser  = String.Empty;
		private string _id  = String.Empty;
		private string _id_shop  = String.Empty;
		private byte _flag_type  = 0;
		private string _parmcode  = String.Empty;
		private string _parmname  = String.Empty;
		private string _parmvalue  = String.Empty;
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
		
		public string id_shop
        {
            get
            {
                return _id_shop;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _id_shop = value;
                }
				else
				{
					_id_shop = String.Empty;
				}
            }
        }
		
		public Nullable<byte> flag_type
        {
            get
            {
                return _flag_type;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_type = value.Value;
                }
				else
				{
					_flag_type = 0;
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
    [Serializable]
    [Table("tb_pay_config", "Tb_Pay_Config")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Pay_ConfigWithMc : Tb_Pay_Config
    {
        public string shop_mc { get; set; }
    }
}

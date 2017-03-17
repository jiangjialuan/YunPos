#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("faq","Faq")]
	[DebuggerDisplay("id = {id}")]
	public class Faq
	{	
		#region public method
		
		public Faq Clone()
		{
			return (Faq)this.MemberwiseClone();
		}
		
		#endregion
		
		#region private field
		
		private long _id  = 0;
		private long _id_user  = 0;
		private long _id_user_master  = 0;
		private DateTime _rq_create  = new DateTime(1900,1,1);
		private string _contents  = String.Empty;
		private byte _flag_type  = 0;
		private byte _flag_state  = 0;
		private byte _flag_delete  = 0;
		private long _id_user_receive  = 0;
		private long _fatherId  = 0;
		private string _flag_from  = String.Empty;
		
		#endregion
		
		#region public property
		
		public Nullable<long> id
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
					_id = 0;
				}
            }
        }
		
		public Nullable<long> id_user
        {
            get
            {
                return _id_user;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_user = value.Value;
                }
				else
				{
					_id_user = 0;
				}
            }
        }
		
		public Nullable<long> id_user_master
        {
            get
            {
                return _id_user_master;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_user_master = value.Value;
                }
				else
				{
					_id_user_master = 0;
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
		
		public string contents
        {
            get
            {
                return _contents;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _contents = value;
                }
				else
				{
					_contents = String.Empty;
				}
            }
        }
		
		/// <summary>
        /// 0问题内容、1回复内容
        /// </summary>
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
		
		/// <summary>
        /// 0未回复、1已回复、2已解决
        /// </summary>
		public Nullable<byte> flag_state
        {
            get
            {
                return _flag_state;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_state = value.Value;
                }
				else
				{
					_flag_state = 0;
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
		
		public Nullable<long> id_user_receive
        {
            get
            {
                return _id_user_receive;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_user_receive = value.Value;
                }
				else
				{
					_id_user_receive = 0;
				}
            }
        }
		
		public Nullable<long> fatherId
        {
            get
            {
                return _fatherId;
            }
            set
            {
                if (value.HasValue)
                {
                    _fatherId = value.Value;
                }
				else
				{
					_fatherId = 0;
				}
            }
        }
		
		public string flag_from
        {
            get
            {
                return _flag_from;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _flag_from = value;
                }
				else
				{
					_flag_from = String.Empty;
				}
            }
        }
		
		#endregion
		
	}
}
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
namespace CySoft.Model.Tb
{
    /// <summary>
    /// 通知公告类
    /// </summary>

    [Serializable]
    [Table("info","Info")]
	[DebuggerDisplay("id = {id}")]
	public class Info
	{	
		#region public method
		
		public Info Clone()
		{
			return (Info)this.MemberwiseClone();
		}
		
		#endregion
		
		#region private field
		
		private long _id  = 0;
		private string _Title  = String.Empty;
		private string _content  = String.Empty;
		private int _id_info_type  = 0;
		private long _id_create  = 0;
		private DateTime _rq_create  = new DateTime(1900,1,1);
		private long _id_master  = 0;
		private string _filename  = String.Empty;
		private int _sl_send  = 0;
		private int _sl_read  = 0;
		private string _flag_from  = String.Empty;
		private string _fileSize  = String.Empty;
		
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
		
		public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _Title = value;
                }
				else
				{
					_Title = String.Empty;
				}
            }
        }
		
		public string content
        {
            get
            {
                return _content;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _content = value;
                }
				else
				{
					_content = String.Empty;
				}
            }
        }
		
		public Nullable<int> id_info_type
        {
            get
            {
                return _id_info_type;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_info_type = value.Value;
                }
				else
				{
					_id_info_type = 0;
				}
            }
        }
		
        [Column(Update = false)]
		public Nullable<long> id_create
        {
            get
            {
                return _id_create;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_create = value.Value;
                }
				else
				{
					_id_create = 0;
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
		
		public Nullable<long> id_master
        {
            get
            {
                return _id_master;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_master = value.Value;
                }
				else
				{
					_id_master = 0;
				}
            }
        }
		
		public string filename
        {
            get
            {
                return _filename;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _filename = value;
                }
				else
				{
					_filename = String.Empty;
				}
            }
        }
		
		public Nullable<int> sl_send
        {
            get
            {
                return _sl_send;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_send = value.Value;
                }
				else
				{
					_sl_send = 0;
				}
            }
        }
		
		public Nullable<int> sl_read
        {
            get
            {
                return _sl_read;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_read = value.Value;
                }
				else
				{
					_sl_read = 0;
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
		
		public string fileSize
        {
            get
            {
                return _fileSize;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _fileSize = value;
                }
				else
				{
					_fileSize = String.Empty;
				}
            }
        }
		
		#endregion
		
	}
}

    


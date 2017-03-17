#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
	/// <summary>
    /// 广告信息表
    /// </summary>
    [Serializable]
    [Table("Tb_Advertis","Tb_Advertis")]
	[DebuggerDisplay("id = {id}")]
	public class Tb_Advertis
	{	
		#region public method
		
		public Tb_Advertis Clone()
		{
			return (Tb_Advertis)this.MemberwiseClone();
		}
		
		#endregion
		
		#region private field
		
		private int _id  = 0;
		private string _title  = String.Empty;
		private string _flag_type  = String.Empty;
		private long _id_user_master  = 0;
		private string _info  = String.Empty;
		private string _url  = String.Empty;
		private long _id_edit  = 0;
		private DateTime _rq_edit  = new DateTime(1900,1,1);
		private string _filename  = String.Empty;
		private int _sort  = 0;
		private int _click  = 0;
		private int _isuse  = 0;
		
		#endregion
		
		#region public property
		
		/// <summary>
        /// 主键
        /// </summary>
		public Nullable<int> id
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
		
		/// <summary>
        /// 标题
        /// </summary>
		public string title
        {
            get
            {
                return _title;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _title = value;
                }
				else
				{
					_title = String.Empty;
				}
            }
        }
		
		/// <summary>
        /// 广告类型(001.图片,002.文本)
        /// </summary>
		public string flag_type
        {
            get
            {
                return _flag_type;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _flag_type = value;
                }
				else
				{
					_flag_type = String.Empty;
				}
            }
        }
		
		/// <summary>
        /// 所属主用户
        /// </summary>
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
		
		/// <summary>
        /// 广告信息
        /// </summary>
		public string info
        {
            get
            {
                return _info;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _info = value;
                }
				else
				{
					_info = String.Empty;
				}
            }
        }
		
		/// <summary>
        /// 链接地址
        /// </summary>
		public string url
        {
            get
            {
                return _url;
            }
            set
            {
				if (!String.IsNullOrEmpty(value))
                {
                    _url = value;
                }
				else
				{
					_url = String.Empty;
				}
            }
        }
		
		/// <summary>
        /// 编辑用户id
        /// </summary>
		public Nullable<long> id_edit
        {
            get
            {
                return _id_edit;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_edit = value.Value;
                }
				else
				{
					_id_edit = 0;
				}
            }
        }
		
		/// <summary>
        /// 编辑时间
        /// </summary>
        [Column(Insert = false)]
		public Nullable<DateTime> rq_edit
        {
            get
            {
                return _rq_edit;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_edit = value.Value;
                }
				else
				{
					_rq_edit = new DateTime(1900,1,1);
				}
            }
        }
		
		/// <summary>
        ///  文件名称
        /// </summary>
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
		
		/// <summary>
        /// 排序
        /// </summary>
		public Nullable<int> sort
        {
            get
            {
                return _sort;
            }
            set
            {
                if (value.HasValue)
                {
                    _sort = value.Value;
                }
				else
				{
					_sort = 0;
				}
            }
        }
		
		/// <summary>
        /// 点击数
        /// </summary>
		public Nullable<int> click
        {
            get
            {
                return _click;
            }
            set
            {
                if (value.HasValue)
                {
                    _click = value.Value;
                }
				else
				{
					_click = 0;
				}
            }
        }
		
		/// <summary>
        /// 是否使用
        /// </summary>
		public Nullable<int> isuse
        {
            get
            {
                return _isuse;
            }
            set
            {
                if (value.HasValue)
                {
                    _isuse = value.Value;
                }
				else
				{
					_isuse = 0;
				}
            }
        }
		
		#endregion
		
	}
}

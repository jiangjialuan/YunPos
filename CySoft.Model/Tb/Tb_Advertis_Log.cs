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
    /// 点击广告记录表
    /// </summary>
    [Serializable]
    [Table("Tb_Advertis_Log","Tb_Advertis_Log")]
	[DebuggerDisplay("id = {id}")]
	public class Tb_Advertis_Log
	{	
		#region public method
		
		public Tb_Advertis_Log Clone()
		{
			return (Tb_Advertis_Log)this.MemberwiseClone();
		}
		
		#endregion
		
		#region private field
		
		private int _id  = 0;
		private int _id_user  = 0;
		private long _id_adv  = 0;
		private DateTime _rq_create  = new DateTime(1900,1,1);
		
		#endregion
		
		#region public property
		
		/// <summary>
        /// 主键id
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
        /// 用户id
        /// </summary>
		public Nullable<int> id_user
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
		
		/// <summary>
        /// 广告id
        /// </summary>
		public Nullable<long> id_adv
        {
            get
            {
                return _id_adv;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_adv = value.Value;
                }
				else
				{
					_id_adv = 0;
				}
            }
        }
		
		/// <summary>
        /// 创建时间
        /// </summary>
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

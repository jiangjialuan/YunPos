using System;
using System.Diagnostics;
using CySoft.Model.Mapping;

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("tb_sp_sku_fd", "Tb_Sp_Sku_Fd")]
    [DebuggerDisplay("id = {_id} , id_sku = {_id_sku} , id_gys_fd = {_id_gys_fd} , id_gys = {_id_gys} , rq_create = {_rq_create}")]
    public class Tb_sp_sku_fd
    {
        #region public method

        public Tb_sp_sku_fd Clone()
        {
            return (Tb_sp_sku_fd) this.MemberwiseClone();
        }

        #endregion

        #region private field

        private System.Guid _id = Guid.NewGuid();
        private long _id_sku = 0;
        private long _id_gys_fd = 0;
        private long _id_gys = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);

        #endregion

        #region public property
        
        /// <summary>
        /// 主键
        /// </summary>
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
        
        /// <summary>
        /// sku
        /// </summary>
        public Nullable<long> id_sku
        {
            get
            {
                return _id_sku;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_sku = value.Value;
                }
                else
                {
                    _id_sku = 0;
                }
            }
        }
        /// <summary>
        /// 分单者供应商id
        /// </summary>
        public Nullable<long> id_gys_fd
        {
            get
            {
                return _id_gys_fd;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_gys_fd = value.Value;
                }
                else
                {
                    _id_gys_fd = 0;
                }
            }
        }
        /// <summary>
        /// 供应商id
        /// </summary>
        public Nullable<long> id_gys
        {
            get
            {
                return _id_gys;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_gys = value.Value;
                }
                else
                {
                    _id_gys = 0;
                }
            }
        }

        /// <summary>
        /// 创建日期
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
                    _rq_create = new DateTime(1900, 1, 1);
                }
            }
        }

        #endregion
    }
}

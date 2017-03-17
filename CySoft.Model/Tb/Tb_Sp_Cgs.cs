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
    [Table("Tb_sp_cgs", "Tb_Sp_Cgs")]
    [DebuggerDisplay("id_gys = {id_gys},id_cgs = {id_cgs},id_sku = {id_sku}")]
    public class Tb_Sp_Cgs
    {
        #region public method

        public Tb_Sp_Cgs Clone()
        {
            return (Tb_Sp_Cgs)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id_gys = 0;
        private long _id_cgs = 0;
        private long _id_sku = 0;
        private long _id_sp = 0;
        private decimal _sl_dh_min = 0m;
        private decimal _dj_dh = 0m;
        private long _id_create = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private long _id_edit = 0;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);

        #endregion

        #region public property

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

        public Nullable<long> id_cgs
        {
            get
            {
                return _id_cgs;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_cgs = value.Value;
                }
                else
                {
                    _id_cgs = 0;
                }
            }
        }

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

        public Nullable<long> id_sp
        {
            get
            {
                return _id_sp;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_sp = value.Value;
                }
                else
                {
                    _id_sp = 0;
                }
            }
        }

        public Nullable<decimal> sl_dh_min
        {
            get
            {
                return _sl_dh_min;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_dh_min = value.Value;
                }
                else
                {
                    _sl_dh_min = 0m;
                }
            }
        }

        public Nullable<decimal> dj_dh
        {
            get
            {
                return _dj_dh;
            }
            set
            {
                if (value.HasValue)
                {
                    _dj_dh = value.Value;
                }
                else
                {
                    _dj_dh = 0m;
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
                    _rq_create = new DateTime(1900, 1, 1);
                }
            }
        }

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
                    _rq_edit = new DateTime(1900, 1, 1);
                }
            }
        }

        #endregion

    }
}

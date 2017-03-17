#region Imports
using System;
using System.Collections.Generic;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("tb_sp_sku_push", "Tb_Sp_Sku_Push")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Sp_Sku_Push
    {
        #region public method

        public Tb_Sp_Sku_Push Clone()
        {
            return (Tb_Sp_Sku_Push)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private System.Guid _id = Guid.Empty;
        private long _id_gys_push = 0;
        private long _id_gys = 0;
        private long _id_sku = 0;
        private string _pushreason = String.Empty;
        private string _refusereason = String.Empty;
        private string _bm_Interface = String.Empty;
        private long _id_spfl = 0;
        private decimal _sl_dh_min = 0m;
        private decimal _dj_dh = 0m;
        private long _id_sh = 0;
        private DateTime _rq_sh = new DateTime(1900, 1, 1);
        private short _falg_state = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);

        #endregion

        #region public property

        public Nullable<Guid> id
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
                    _id = Guid.Empty;
                }
            }
        }

        public Nullable<long> id_gys_push
        {
            get
            {
                return _id_gys_push;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_gys_push = value.Value;
                }
                else
                {
                    _id_gys_push = 0;
                }
            }
        }

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

        public string pushreason
        {
            get
            {
                return _pushreason;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _pushreason = value;
                }
                else
                {
                    _pushreason = String.Empty;
                }
            }
        }

        public string refusereason
        {
            get
            {
                return _refusereason;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _refusereason = value;
                }
                else
                {
                    _refusereason = String.Empty;
                }
            }
        }

        public string bm_Interface
        {
            get
            {
                return _bm_Interface;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _bm_Interface = value;
                }
                else
                {
                    _bm_Interface = String.Empty;
                }
            }
        }

        public Nullable<long> id_spfl
        {
            get
            {
                return _id_spfl;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_spfl = value.Value;
                }
                else
                {
                    _id_spfl = 0;
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

        public Nullable<long> id_sh
        {
            get
            {
                return _id_sh;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_sh = value.Value;
                }
                else
                {
                    _id_sh = 0;
                }
            }
        }

        public Nullable<DateTime> rq_sh
        {
            get
            {
                return _rq_sh;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_sh = value.Value;
                }
                else
                {
                    _rq_sh = new DateTime(1900, 1, 1);
                }
            }
        }

        public Nullable<short> falg_state
        {
            get
            {
                return _falg_state;
            }
            set
            {
                if (value.HasValue)
                {
                    _falg_state = value.Value;
                }
                else
                {
                    _falg_state = 0;
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

        #endregion

    }
}

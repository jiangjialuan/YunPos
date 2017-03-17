#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Td
{
    [Serializable]
    [Table("Td_sale_out_body", "Td_Sale_Out_Body")]
    public class Td_Sale_Out_Body
    {
        #region public method

        public Td_Sale_Out_Body Clone()
        {
            return (Td_Sale_Out_Body)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _dh = String.Empty;
        private short _xh = 0;
        private string _dh_order = String.Empty;
        private short _xh_order = 0;
        private long _id_sku = 0;
        private string _unit = String.Empty;
        private decimal _zhl = 0m;
        private decimal _sl = 0m;
        private decimal _sl_ck = 0m;
        private decimal _sl_fh = 0m;
        private string _remark = String.Empty;

        #endregion

        #region public property

        public string dh
        {
            get
            {
                return _dh;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _dh = value;
                }
                else
                {
                    _dh = String.Empty;
                }
            }
        }

        public Nullable<short> xh
        {
            get
            {
                return _xh;
            }
            set
            {
                if (value.HasValue)
                {
                    _xh = value.Value;
                }
                else
                {
                    _xh = 0;
                }
            }
        }

        public string dh_order
        {
            get
            {
                return _dh_order;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _dh_order = value;
                }
                else
                {
                    _dh_order = String.Empty;
                }
            }
        }

        public Nullable<short> xh_order
        {
            get
            {
                return _xh_order;
            }
            set
            {
                if (value.HasValue)
                {
                    _xh_order = value.Value;
                }
                else
                {
                    _xh_order = 0;
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

        public string unit
        {
            get
            {
                return _unit;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _unit = value;
                }
                else
                {
                    _unit = String.Empty;
                }
            }
        }

        public Nullable<decimal> zhl
        {
            get
            {
                return _zhl;
            }
            set
            {
                if (value.HasValue)
                {
                    _zhl = value.Value;
                }
                else
                {
                    _zhl = 0m;
                }
            }
        }

        public Nullable<decimal> sl
        {
            get
            {
                return _sl;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl = value.Value;
                }
                else
                {
                    _sl = 0m;
                }
            }
        }

        public Nullable<decimal> sl_ck
        {
            get
            {
                return _sl_ck;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_ck = value.Value;
                }
                else
                {
                    _sl_ck = 0m;
                }
            }
        }

        public Nullable<decimal> sl_fh
        {
            get
            {
                return _sl_fh;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_fh = value.Value;
                }
                else
                {
                    _sl_fh = 0m;
                }
            }
        }

        public string remark
        {
            get
            {
                return _remark;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _remark = value;
                }
                else
                {
                    _remark = String.Empty;
                }
            }
        }

        #endregion

    }
}
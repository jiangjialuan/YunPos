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
    [Table("tb_barcode_pic", "Tb_Barcode_Pic")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Barcode_Pic
    {
        #region public method

        public Tb_Barcode_Pic Clone()
        {
            return (Tb_Barcode_Pic)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private long _id = 0;
        private string _barcode = String.Empty;
        private string _photo = String.Empty;
        private string _photo_min = String.Empty;
        private string _photo_min1 = String.Empty;
        private string _photo_min2 = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);

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

        public string barcode
        {
            get
            {
                return _barcode;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _barcode = value;
                }
                else
                {
                    _barcode = String.Empty;
                }
            }
        }

        public string photo
        {
            get
            {
                return _photo;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _photo = value;
                }
                else
                {
                    _photo = String.Empty;
                }
            }
        }

        public string photo_min
        {
            get
            {
                return _photo_min;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _photo_min = value;
                }
                else
                {
                    _photo_min = String.Empty;
                }
            }
        }

        public string photo_min1
        {
            get
            {
                return _photo_min1;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _photo_min1 = value;
                }
                else
                {
                    _photo_min1 = String.Empty;
                }
            }
        }

        public string photo_min2
        {
            get
            {
                return _photo_min2;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _photo_min2 = value;
                }
                else
                {
                    _photo_min2 = String.Empty;
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

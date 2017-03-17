using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Mapping;

namespace CySoft.Model.Td
{
    [Serializable]
    [Table("td_ls_jb_2", "Td_Ls_Jb_2")]
    [DebuggerDisplay("id = {id}")]
    public class Td_Ls_Jb_2
    {
        #region public method

        public Td_Ls_Jb_2 Clone()
        {
            return (Td_Ls_Jb_2)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_bill = String.Empty;
        private int _xh = 0;
        private string _bz = String.Empty;
        private int _bs = 0;
        private decimal _je = 0m;
        private string _id_pay = String.Empty;
        private byte _flag_type = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _id_shop = String.Empty;

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

        public string id_bill
        {
            get
            {
                return _id_bill;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_bill = value;
                }
                else
                {
                    _id_bill = String.Empty;
                }
            }
        }

        public Nullable<int> xh
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

        public string bz
        {
            get
            {
                return _bz;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _bz = value;
                }
                else
                {
                    _bz = String.Empty;
                }
            }
        }

        public Nullable<int> bs
        {
            get
            {
                return _bs;
            }
            set
            {
                if (value.HasValue)
                {
                    _bs = value.Value;
                }
                else
                {
                    _bs = 0;
                }
            }
        }

        public Nullable<decimal> je
        {
            get
            {
                return _je;
            }
            set
            {
                if (value.HasValue)
                {
                    _je = value.Value;
                }
                else
                {
                    _je = 0m;
                }
            }
        }

        public string id_pay
        {
            get
            {
                return _id_pay;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_pay = value;
                }
                else
                {
                    _id_pay = String.Empty;
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

        #endregion

    }
}

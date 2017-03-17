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
    [Table("td_ls_jb_1", "Td_Ls_Jb_1")]
    [DebuggerDisplay("id = {id}")]
    public class Td_Ls_Jb_1
    {
        #region public method

        public Td_Ls_Jb_1 Clone()
        {
            return (Td_Ls_Jb_1)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private DateTime _rq_jb = new DateTime(1900, 1, 1);
        private string _bz = String.Empty;
        private DateTime _rq_firstbill = new DateTime(1900, 1, 1);
        private DateTime _rq_lastbill = new DateTime(1900, 1, 1);
        private decimal _je_xs = 0m;
        private decimal _je_xsth = 0m;
        private int _bs_xs = 0;
        private int _bs_xsth = 0;
        private decimal _je_backup = 0m;
        private int _flag_jb = 0;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _id_edit = String.Empty;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private byte _flag_delete = 0;
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

        public Nullable<DateTime> rq_jb
        {
            get
            {
                return _rq_jb;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_jb = value.Value;
                }
                else
                {
                    _rq_jb = new DateTime(1900, 1, 1);
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

        public Nullable<DateTime> rq_firstbill
        {
            get
            {
                return _rq_firstbill;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_firstbill = value.Value;
                }
                else
                {
                    _rq_firstbill = new DateTime(1900, 1, 1);
                }
            }
        }

        public Nullable<DateTime> rq_lastbill
        {
            get
            {
                return _rq_lastbill;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_lastbill = value.Value;
                }
                else
                {
                    _rq_lastbill = new DateTime(1900, 1, 1);
                }
            }
        }

        public Nullable<decimal> je_xs
        {
            get
            {
                return _je_xs;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_xs = value.Value;
                }
                else
                {
                    _je_xs = 0m;
                }
            }
        }

        public Nullable<decimal> je_xsth
        {
            get
            {
                return _je_xsth;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_xsth = value.Value;
                }
                else
                {
                    _je_xsth = 0m;
                }
            }
        }

        public Nullable<int> bs_xs
        {
            get
            {
                return _bs_xs;
            }
            set
            {
                if (value.HasValue)
                {
                    _bs_xs = value.Value;
                }
                else
                {
                    _bs_xs = 0;
                }
            }
        }

        public Nullable<int> bs_xsth
        {
            get
            {
                return _bs_xsth;
            }
            set
            {
                if (value.HasValue)
                {
                    _bs_xsth = value.Value;
                }
                else
                {
                    _bs_xsth = 0;
                }
            }
        }

        public Nullable<decimal> je_backup
        {
            get
            {
                return _je_backup;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_backup = value.Value;
                }
                else
                {
                    _je_backup = 0m;
                }
            }
        }

        public Nullable<int> flag_jb
        {
            get
            {
                return _flag_jb;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_jb = value.Value;
                }
                else
                {
                    _flag_jb = 0;
                }
            }
        }

        public string id_create
        {
            get
            {
                return _id_create;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_create = value;
                }
                else
                {
                    _id_create = String.Empty;
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

        public string id_edit
        {
            get
            {
                return _id_edit;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_edit = value;
                }
                else
                {
                    _id_edit = String.Empty;
                }
            }
        }

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

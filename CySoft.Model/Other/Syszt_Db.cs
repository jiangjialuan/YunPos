using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Mapping;

namespace CySoft.Model.Other
{
    [Serializable]
    [Table("syszt_db", "Syszt_Db")]
    [DebuggerDisplay("id = {id}")]
    public class Syszt_Db
    {
        #region public method

        public Syszt_Db Clone()
        {
            return (Syszt_Db)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id = String.Empty;
        private string _bm = String.Empty;
        private string _db_mc = String.Empty;
        private string _db_yhm = String.Empty;
        private string _db_mm = String.Empty;
        private int _db_port = 0;
        private DateTime _rq = new DateTime(1900, 1, 1);
        private int _xh = 0;
        private string _db_from = String.Empty;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private string _db_title = String.Empty;
        private string _upgrade_url = String.Empty;
        private string _upgrade_allowkey = String.Empty;
        private string _db_flag = String.Empty;

        #endregion

        #region public property

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

        public string bm
        {
            get
            {
                return _bm;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _bm = value;
                }
                else
                {
                    _bm = String.Empty;
                }
            }
        }

        public string db_mc
        {
            get
            {
                return _db_mc;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _db_mc = value;
                }
                else
                {
                    _db_mc = String.Empty;
                }
            }
        }

        public string db_yhm
        {
            get
            {
                return _db_yhm;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _db_yhm = value;
                }
                else
                {
                    _db_yhm = String.Empty;
                }
            }
        }

        public string db_mm
        {
            get
            {
                return _db_mm;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _db_mm = value;
                }
                else
                {
                    _db_mm = String.Empty;
                }
            }
        }

        public Nullable<int> db_port
        {
            get
            {
                return _db_port;
            }
            set
            {
                if (value.HasValue)
                {
                    _db_port = value.Value;
                }
                else
                {
                    _db_port = 0;
                }
            }
        }

        public Nullable<DateTime> rq
        {
            get
            {
                return _rq;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq = value.Value;
                }
                else
                {
                    _rq = new DateTime(1900, 1, 1);
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

        public string db_from
        {
            get
            {
                return _db_from;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _db_from = value;
                }
                else
                {
                    _db_from = String.Empty;
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

        public string db_title
        {
            get
            {
                return _db_title;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _db_title = value;
                }
                else
                {
                    _db_title = String.Empty;
                }
            }
        }

        public string upgrade_url
        {
            get
            {
                return _upgrade_url;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _upgrade_url = value;
                }
                else
                {
                    _upgrade_url = String.Empty;
                }
            }
        }

        public string upgrade_allowkey
        {
            get
            {
                return _upgrade_allowkey;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _upgrade_allowkey = value;
                }
                else
                {
                    _upgrade_allowkey = String.Empty;
                }
            }
        }

        public string db_flag
        {
            get
            {
                return _db_flag;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _db_flag = value;
                }
                else
                {
                    _db_flag = String.Empty;
                }
            }
        }

        #endregion

    }
}

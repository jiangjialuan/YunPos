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
    [Table("syszt_pos", "Syszt_Pos")]
    [DebuggerDisplay("bm = {bm}")]
    public class Syszt_Pos
    {
        #region public method

        public Syszt_Pos Clone()
        {
            return (Syszt_Pos)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _bm = String.Empty;
        private string _id_gsjg = String.Empty;
        private string _id_db = String.Empty;
        private string _xym = String.Empty;
        private DateTime _rq = new DateTime(1900, 1, 1);
        private string _mc_gsjg = String.Empty;

        #endregion

        #region public property

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

        public string id_gsjg
        {
            get
            {
                return _id_gsjg;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_gsjg = value;
                }
                else
                {
                    _id_gsjg = String.Empty;
                }
            }
        }

        public string id_db
        {
            get
            {
                return _id_db;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_db = value;
                }
                else
                {
                    _id_db = String.Empty;
                }
            }
        }

        public string xym
        {
            get
            {
                return _xym;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _xym = value;
                }
                else
                {
                    _xym = String.Empty;
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

        public string mc_gsjg
        {
            get
            {
                return _mc_gsjg;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _mc_gsjg = value;
                }
                else
                {
                    _mc_gsjg = String.Empty;
                }
            }
        }

        #endregion

    }
}

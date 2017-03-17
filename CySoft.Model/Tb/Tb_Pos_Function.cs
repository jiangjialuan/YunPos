using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Mapping;

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("tb_pos_function", "Tb_Pos_Function")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Pos_Function
    {
        #region public method

        public Tb_Pos_Function Clone()
        {
            return (Tb_Pos_Function)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id = String.Empty;
        private string _mc = String.Empty;
        private string _bm = String.Empty;
        private string _version = String.Empty;
        private byte _flag_system = 0;
        private string _functiondescribe = String.Empty;
        private byte _flag_type =0;
        private byte _flag_stop = 0;
        private int _sort_id = 0;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private byte[] _nlast ;
        private string _regex = String.Empty;

        #endregion

        #region public property

        public string id
        {
            get
            {
                return _id;
            }
            set { _id = value; }
        }

        public string mc
        {
            get
            {
                return _mc;
            }
            set { _mc = value; }
        }

        public string version
        {
            get
            {
                return _version;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _version = value;
                }
                else
                {
                    _version = String.Empty;
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
        public Nullable<byte> flag_system
        {
            get
            {
                return _flag_system;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_system = value.Value;
                }
                else
                {
                    _flag_system = 0;
                }
            }
        }
        public string regex
        {
            get
            {
                return _regex;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _regex = value;
                }
                else
                {
                    _regex = String.Empty;
                }
            }
        }
        public string functiondescribe
        {
            get
            {
                return _functiondescribe;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _functiondescribe = value;
                }
                else
                {
                    _functiondescribe = String.Empty;
                }
            }
        }

        public byte flag_type
        {
            get
            {
                return _flag_type;
            }
            set
            {
                _flag_type = value;
            }
        }

        

        public Nullable<byte> flag_stop
        {
            get
            {
                return _flag_stop;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_stop = value.Value;
                }
                else
                {
                    _flag_stop = 0;
                }
            }
        }

        public Nullable<int> sort_id
        {
            get
            {
                return _sort_id;
            }
            set
            {
                if (value.HasValue)
                {
                    _sort_id = value.Value;
                }
                else
                {
                    _sort_id = 0;
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

        public byte[] nlast
        {
            get
            {
                return _nlast;
            }
            set { _nlast = value; }
        }

        #endregion

    }
}

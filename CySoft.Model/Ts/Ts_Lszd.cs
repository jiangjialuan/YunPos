using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Mapping;

namespace CySoft.Model.Ts
{
    [Serializable]
    [Table("ts_lszd", "Ts_Lszd")]
    [DebuggerDisplay("id = {id}")]
    public class Ts_Lszd
    {
        #region public method

        public Ts_Lszd Clone()
        {
            return (Ts_Lszd)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_shop = String.Empty;
        private string _os = String.Empty;
        private string _mac = String.Empty;
        private string _osversion = String.Empty;
        private string _screen = String.Empty;
        private string _hardware = String.Empty;
        private byte _flag_stop = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private byte _flag_delete = 0;
        private string _computer = String.Empty;
        private string _ip = String.Empty;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private int _discern_no = 0;
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

        public string os
        {
            get
            {
                return _os;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _os = value;
                }
                else
                {
                    _os = String.Empty;
                }
            }
        }

        public string mac
        {
            get
            {
                return _mac;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _mac = value;
                }
                else
                {
                    _mac = String.Empty;
                }
            }
        }

        public string osversion
        {
            get
            {
                return _osversion;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _osversion = value;
                }
                else
                {
                    _osversion = String.Empty;
                }
            }
        }

        public string screen
        {
            get
            {
                return _screen;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _screen = value;
                }
                else
                {
                    _screen = String.Empty;
                }
            }
        }

        public string hardware
        {
            get
            {
                return _hardware;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _hardware = value;
                }
                else
                {
                    _hardware = String.Empty;
                }
            }
        }

        [Column(Update = false)]
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

        public string computer
        {
            get
            {
                return _computer;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _computer = value;
                }
                else
                {
                    _computer = String.Empty;
                }
            }
        }

        public string ip
        {
            get
            {
                return _ip;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _ip = value;
                }
                else
                {
                    _ip = String.Empty;
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
        public Nullable<int> discern_no
        {
            get
            {
                return _discern_no;
            }
            set
            {
                if (value.HasValue)
                {
                    _discern_no = value.Value;
                }
                else
                {
                    _discern_no = 0;
                }
            }
        }
        #endregion
        
    }
    
}


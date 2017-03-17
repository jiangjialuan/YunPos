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
    [Table("tb_ticket", "Tb_Ticket")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Ticket
    {
        #region public method

        public Tb_Ticket Clone()
        {
            return (Tb_Ticket)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _ticket = String.Empty;
        private string _key_y = String.Empty;
        private string _id = String.Empty;
        private string _mac = "";
        private string _ip = "";

        #endregion

        #region public property

        public string ticket
        {
            get
            {
                return _ticket;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _ticket = value;
                }
                else
                {
                    _ticket = String.Empty;
                }
            }
        }

        public string key_y
        {
            get
            {
                return _key_y;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _key_y = value;
                }
                else
                {
                    _key_y = String.Empty;
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

        #endregion

    }
}

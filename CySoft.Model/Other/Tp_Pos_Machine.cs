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
    [Table("Tp_pos_machine", "Tp_Pos_Machine")]
    [DebuggerDisplay("id = {id}")]
    public class Tp_Pos_Machine
    {
        #region public method

        public Tp_Pos_Machine Clone()
        {
            return (Tp_Pos_Machine)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id = String.Empty;
        private string _com_name = String.Empty;
        private string _checkcode = String.Empty;
        private string _flag_state = String.Empty;
        private string _id_gsjg = String.Empty;
        private string _mac = String.Empty;

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

        public string com_name
        {
            get
            {
                return _com_name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _com_name = value;
                }
                else
                {
                    _com_name = String.Empty;
                }
            }
        }

        public string checkcode
        {
            get
            {
                return _checkcode;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _checkcode = value;
                }
                else
                {
                    _checkcode = String.Empty;
                }
            }
        }

        public string flag_state
        {
            get
            {
                return _flag_state;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _flag_state = value;
                }
                else
                {
                    _flag_state = String.Empty;
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

        #endregion

    }
}

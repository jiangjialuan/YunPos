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
    [Table("tb_role_pos_function", "Tb_Role_Pos_Function")]
    [DebuggerDisplay("id_role = {id_role},id_pos_function = {id_pos_function}")]
    public class Tb_Role_Pos_Function
    {
        #region public method

        public Tb_Role_Pos_Function Clone()
        {
            return (Tb_Role_Pos_Function)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_role = String.Empty;
        private string _id_pos_function = String.Empty;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _maxvalue = String.Empty;
        private string _minvalue = String.Empty;
        private byte[] _nlast;
        private string _id_masteruser = String.Empty;
        private byte _flag_use = 0;
        #endregion

        #region public property

        public string id_role
        {
            get
            {
                return _id_role;
            }
            set { _id_role = value; }
        }
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
        public string id_pos_function
        {
            get
            {
                return _id_pos_function;
            }
            set { _id_pos_function = value; }
        }

        public string maxvalue
        {
            get
            {
                return _maxvalue;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _maxvalue = value;
                }
                else
                {
                    _maxvalue = String.Empty;
                }
            }
        }

        public string minvalue
        {
            get
            {
                return _minvalue;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _minvalue = value;
                }
                else
                {
                    _minvalue = String.Empty;
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

        public Nullable<byte> flag_use
        {
            get
            {
                return _flag_use;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_use = value.Value;
                }
                else
                {
                    _flag_use = 0;
                }
            }
        }
        #endregion

    }

    public class Tb_Role_Pos_FunctionWithName : Tb_Role_Pos_Function
    {
        public string func_name { get; set; }
    }
}

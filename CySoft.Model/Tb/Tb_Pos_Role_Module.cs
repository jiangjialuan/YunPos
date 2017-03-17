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
    [Table("tb_pos_role_module", "Tb_Pos_Role_Module")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Pos_Role_Module
    {
        #region public method

        public Tb_Pos_Role_Module Clone()
        {
            return (Tb_Pos_Role_Module)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private short _id_platform_role = 0;
        private int _id_pos_function = 0;
        private int _id_module = 0;
        private int _id_module_fatherid = 0;
        private string _name = String.Empty;
        private int _sort_id = 0;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _id_edit = String.Empty;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private long _id = 0;

        #endregion

        #region public property

        public Nullable<short> id_platform_role
        {
            get
            {
                return _id_platform_role;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_platform_role = value.Value;
                }
                else
                {
                    _id_platform_role = 0;
                }
            }
        }

        public Nullable<int> id_pos_function
        {
            get
            {
                return _id_pos_function;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_pos_function = value.Value;
                }
                else
                {
                    _id_pos_function = 0;
                }
            }
        }

        public Nullable<int> id_module
        {
            get
            {
                return _id_module;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_module = value.Value;
                }
                else
                {
                    _id_module = 0;
                }
            }
        }

        public Nullable<int> id_module_fatherid
        {
            get
            {
                return _id_module_fatherid;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_module_fatherid = value.Value;
                }
                else
                {
                    _id_module_fatherid = 0;
                }
            }
        }

        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name = value;
                }
                else
                {
                    _name = String.Empty;
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

        #endregion

    }
}

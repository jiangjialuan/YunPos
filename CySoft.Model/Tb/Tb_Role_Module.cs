#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("tb_role_module", "Tb_Role_Module")]
    [DebuggerDisplay("id_platform_role = {id_platform_role},id_function = {id_function}")]
    public class Tb_Role_Module
    {
        #region public method

        public Tb_Role_Module Clone()
        {
            return (Tb_Role_Module)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id = String.Empty;
        private string _id_platform_role = String.Empty;
        private string _id_function = String.Empty;
        private string _id_module = String.Empty;
        private string _id_module_father = String.Empty;
        private string _name = String.Empty;
        private int _sort_id = 0;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _id_edit = String.Empty;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);

        #endregion

        #region public property

        public string id_platform_role
        {
            get
            {
                return _id_platform_role;
            }
            set
            {
                _id_platform_role = value;
            }
        }

        public string id_function
        {
            get
            {
                return _id_function;
            }
            set
            {
                _id_function = value;
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
                _id = value;
            }
        }
        public string id_module
        {
            get
            {
                return _id_module;
            }
            set
            {
                _id_module = value;
            }
        }

        public string id_module_father
        {
            get
            {
                return _id_module_father;
            }
            set { _id_module_father = value; }
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

        [Column(Update = false)]
        public string id_create
        {
            get
            {
                return _id_create;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _id_create = value;
                }
                else
                {
                    _id_create = String.Empty;
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

        public string id_edit
        {
            get
            {
                return _id_edit;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _id_edit = value;
                }
                else
                {
                    _id_edit = String.Empty;
                }
            }
        }

        [Column(Insert = false)]
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

        #endregion

    }
}

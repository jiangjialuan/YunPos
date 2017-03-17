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
    [Table("Tb_Districts", "Tb_Districts")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Districts
    {
        #region public method

        public Tb_Districts Clone()
        {
            return (Tb_Districts)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private int _id = 0;
        private int _fatherId = 0;
        private string _name = String.Empty;
        private string _path = String.Empty;
        private int _level = 0;
        private int _sort = 0;
        private string _local = String.Empty;

        #endregion

        #region public property

        public Nullable<int> id
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

        public Nullable<int> fatherId
        {
            get
            {
                return _fatherId;
            }
            set
            {
                if (value.HasValue)
                {
                    _fatherId = value.Value;
                }
                else
                {
                    _fatherId = 0;
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

        public string path
        {
            get
            {
                return _path;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _path = value;
                }
                else
                {
                    _path = String.Empty;
                }
            }
        }

        public Nullable<int> level
        {
            get
            {
                return _level;
            }
            set
            {
                if (value.HasValue)
                {
                    _level = value.Value;
                }
                else
                {
                    _level = 0;
                }
            }
        }

        public Nullable<int> sort
        {
            get
            {
                return _sort;
            }
            set
            {
                if (value.HasValue)
                {
                    _sort = value.Value;
                }
                else
                {
                    _sort = 0;
                }
            }
        }

        public string local
        {
            get
            {
                return _local;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _local = value;
                }
                else
                {
                    _local = String.Empty;
                }
            }
        }

        #endregion

    }
}

#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Ts
{
    [Serializable]
    [Table("ts_parm_gsjg", "Ts_Parm_Gsjg")]
    [DebuggerDisplay("id_gsjg = {id_gsjg},bm = {bm}")]
    public class Ts_Parm_Gsjg
    {
        #region public method

        public Ts_Parm_Gsjg Clone()
        {
            return (Ts_Parm_Gsjg)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private int _id_gsjg = 0;
        private string _bm = String.Empty;
        private string _mc = String.Empty;
        private string _val = String.Empty;
        private string _flag = String.Empty;
        private int _isvisible = 0;

        #endregion

        #region public property

        public Nullable<int> id_gsjg
        {
            get
            {
                return _id_gsjg;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_gsjg = value.Value;
                }
                else
                {
                    _id_gsjg = 0;
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

        public string mc
        {
            get
            {
                return _mc;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _mc = value;
                }
                else
                {
                    _mc = String.Empty;
                }
            }
        }

        public string val
        {
            get
            {
                return _val;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _val = value;
                }
                else
                {
                    _val = String.Empty;
                }
            }
        }

        public string flag
        {
            get
            {
                return _flag;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _flag = value;
                }
                else
                {
                    _flag = String.Empty;
                }
            }
        }

        public Nullable<int> isvisible
        {
            get
            {
                return _isvisible;
            }
            set
            {
                if (value.HasValue)
                {
                    _isvisible = value.Value;
                }
                else
                {
                    _isvisible = 0;
                }
            }
        }

        #endregion

    }
}

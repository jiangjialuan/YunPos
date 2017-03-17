#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region 省
#endregion

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("Tb_province", "Tb_Province")]
    [DebuggerDisplay("id = {id}, mc = {mc}")]
    public class Tb_Province
    {
        #region public method

        public Tb_Province Clone()
        {
            return (Tb_Province)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private int _id = 0;
        private string _name = String.Empty;

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

        #endregion

    }
}
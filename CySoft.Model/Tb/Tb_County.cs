#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region 县（区）
#endregion

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("Tb_county", "Tb_County")]
    [DebuggerDisplay("id = {id},mc = {mc}, id_city = {id_city}")]
    public class Tb_County
    {
        #region public method

        public Tb_County Clone()
        {
            return (Tb_County)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private int _id = 0;
        private string _name = String.Empty;
        private int _id_city = 0;

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

        public Nullable<int> id_city
        {
            get
            {
                return _id_city;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_city = value.Value;
                }
                else
                {
                    _id_city = 0;
                }
            }
        }

        #endregion

    }
}
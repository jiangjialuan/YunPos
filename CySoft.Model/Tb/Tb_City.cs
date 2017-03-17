#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region 市
#endregion

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("Tb_city", "Tb_City")]
    [DebuggerDisplay("id = {id}, mc = {mc}, zipcode = {zipcode}, id_province = {id_province}")]
    public class Tb_City
    {
        #region public method

        public Tb_City Clone()
        {
            return (Tb_City)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private int _id = 0;
        private string _name = String.Empty;
        private string _zipcode = String.Empty;
        private int _id_province = 0;

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

        public string zipcode
        {
            get
            {
                return _zipcode;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _zipcode = value;
                }
                else
                {
                    _zipcode = String.Empty;
                }
            }
        }

        public Nullable<int> id_province
        {
            get
            {
                return _id_province;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_province = value.Value;
                }
                else
                {
                    _id_province = 0;
                }
            }
        }

        #endregion

    }
}
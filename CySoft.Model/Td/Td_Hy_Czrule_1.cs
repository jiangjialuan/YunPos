#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using System.Collections.Generic;
#endregion

#region
#endregion

namespace CySoft.Model.Td
{
    [Serializable]
    [Table("td_hy_czrule_1", "Td_Hy_Czrule_1")]
    [DebuggerDisplay("id = {id}")]
    public class Td_Hy_Czrule_1
    {
        #region public method

        public Td_Hy_Czrule_1 Clone()
        {
            return (Td_Hy_Czrule_1)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_shop = String.Empty;
        private string _id_shop_cz = String.Empty;
        private DateTime _day_b = new DateTime(1900, 1, 1);
        private DateTime _day_e = new DateTime(1900, 1, 1);
        private string _id_hyfl = String.Empty;
        private decimal _je_cz = 0m;
        private decimal _je_cz_zs = 0m;
        private string _bz = String.Empty;
        private byte _flag_cancel = 0;
        private byte _flag_delete = 0;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private byte[] _nlast;
        private string _id_rule = String.Empty;
        
        #endregion

        #region public property

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

        public string id_shop
        {
            get
            {
                return _id_shop;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_shop = value;
                }
                else
                {
                    _id_shop = String.Empty;
                }
            }
        }

        public string id_shop_cz
        {
            get
            {
                return _id_shop_cz;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_shop_cz = value;
                }
                else
                {
                    _id_shop_cz = String.Empty;
                }
            }
        }

        public Nullable<DateTime> day_b
        {
            get
            {
                return _day_b;
            }
            set
            {
                if (value.HasValue)
                {
                    _day_b = value.Value;
                }
                else
                {
                    _day_b = new DateTime(1900, 1, 1);
                }
            }
        }

        public Nullable<DateTime> day_e
        {
            get
            {
                return _day_e;
            }
            set
            {
                if (value.HasValue)
                {
                    _day_e = value.Value;
                }
                else
                {
                    _day_e = new DateTime(1900, 1, 1);
                }
            }
        }

        public string id_hyfl
        {
            get
            {
                return _id_hyfl;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_hyfl = value;
                }
                else
                {
                    _id_hyfl = String.Empty;
                }
            }
        }

        public Nullable<decimal> je_cz
        {
            get
            {
                return _je_cz;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_cz = value.Value;
                }
                else
                {
                    _je_cz = 0m;
                }
            }
        }

        public Nullable<decimal> je_cz_zs
        {
            get
            {
                return _je_cz_zs;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_cz_zs = value.Value;
                }
                else
                {
                    _je_cz_zs = 0m;
                }
            }
        }

        public string bz
        {
            get
            {
                return _bz;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _bz = value;
                }
                else
                {
                    _bz = String.Empty;
                }
            }
        }

        public Nullable<byte> flag_cancel
        {
            get
            {
                return _flag_cancel;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_cancel = value.Value;
                }
                else
                {
                    _flag_cancel = 0;
                }
            }
        }

        public Nullable<byte> flag_delete
        {
            get
            {
                return _flag_delete;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_delete = value.Value;
                }
                else
                {
                    _flag_delete = 0;
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

        public Byte[] nlast { get; set; }


        public string id_rule
        {
            get
            {
                return _id_rule;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_rule = value;
                }
                else
                {
                    _id_rule = String.Empty;
                }
            }
        }

        #endregion

    }


    public class Td_Hy_Czrule_1_ReqModel : Td_Hy_Czrule_1
    {
        public string sp_list { set; get; }

        public List<Td_Hy_Czrule_2> czrule_2_list { set; get; }
    }


}
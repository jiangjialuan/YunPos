#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using System.Collections.Generic;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("tb_hy_czrule", "Tb_Hy_Czrule")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Hy_Czrule
    {
        #region public method

        public Tb_Hy_Czrule Clone()
        {
            return (Tb_Hy_Czrule)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_shop = String.Empty;
        private string _id_bill = String.Empty;
        private int _sort_id = 0;
        private DateTime _day_b = new DateTime(1900, 1, 1);
        private DateTime _day_e = new DateTime(1900, 1, 1);
        private string _id_hyfl = String.Empty;
        private decimal _je_cz = 0m;
        private decimal _je_cz_zs = 0m;
        private byte _flag_stop = 0;
        private byte _flag_delete = 0;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private byte[] _nlast;

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

        public string id_bill
        {
            get
            {
                return _id_bill;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_bill = value;
                }
                else
                {
                    _id_bill = String.Empty;
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

        [Column(Update = false)]
        public Nullable<byte> flag_stop
        {
            get
            {
                return _flag_stop;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_stop = value.Value;
                }
                else
                {
                    _flag_stop = 0;
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

        #endregion

    }


    public class Tb_Hy_Czrule_Query : Tb_Hy_Czrule
    {
        public string day_b_str { set; get; }
        public string day_e_str { set; get; }
        public string spmc { set; get; }
        public string shopmc { set; get; }
        public string hyflmc { set; get; }
        public List<Tb_Hy_Czrule_Zssp_Query> body_list{ set; get; }

}



    public class Tb_Hy_Czrule_Query_DetailModel
    {
        public Tb_Hy_Czrule_Query head { set; get; }
        public List<Tb_Hy_Czrule_Zssp_Query> body { set; get; }

    }



}

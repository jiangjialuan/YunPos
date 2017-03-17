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
    [Table("tb_hy_czrule_zssp", "Tb_Hy_Czrule_Zssp")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Hy_Czrule_Zssp
    {
        #region public method

        public Tb_Hy_Czrule_Zssp Clone()
        {
            return (Tb_Hy_Czrule_Zssp)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_bill = String.Empty;
        private string _id_sp = String.Empty;
        private decimal _sl = 0m;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private byte _flag_delete = 0;
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

        public string id_sp
        {
            get
            {
                return _id_sp;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_sp = value;
                }
                else
                {
                    _id_sp = String.Empty;
                }
            }
        }

        public Nullable<decimal> sl
        {
            get
            {
                return _sl;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl = value.Value;
                }
                else
                {
                    _sl = 0m;
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

        public Byte[] nlast { get; set; }

        #endregion

    }


    public class Tb_Hy_Czrule_Zssp_Query : Tb_Hy_Czrule_Zssp
    {
        public string mc { get; set; }
        public string bm { get; set; }
        public string dw { get; set; }
    }

    public class Tb_Hy_Czrule_Zssp_Api_Query 
    {
        public string id_sp { get; set; }
        public decimal sl { get; set; }
    }

}

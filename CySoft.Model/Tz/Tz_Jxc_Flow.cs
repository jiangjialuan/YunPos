using CySoft.Model.Mapping;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Tz
{
    [Serializable]
    [Table("tz_jxc_flow", "Tz_Jxc_Flow")]
    [DebuggerDisplay("id = {id}")]
    public class Tz_Jxc_Flow
    {
        #region public method

        public Tz_Jxc_Flow Clone()
        {
            return (Tz_Jxc_Flow)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_billmx = String.Empty;
        private string _bm_djlx = String.Empty;
        private string _id_shop = String.Empty;
        private string _id_shopsp = String.Empty;
        private decimal _sl = 0m;
        private decimal _je = 0m;
        private DateTime _rq = new DateTime(1900, 1, 1);
        private string _id_kcsp = String.Empty;

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

        public string id_billmx
        {
            get
            {
                return _id_billmx;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_billmx = value;
                }
                else
                {
                    _id_billmx = String.Empty;
                }
            }
        }

        public string bm_djlx
        {
            get
            {
                return _bm_djlx;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _bm_djlx = value;
                }
                else
                {
                    _bm_djlx = String.Empty;
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

        public string id_shopsp
        {
            get
            {
                return _id_shopsp;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_shopsp = value;
                }
                else
                {
                    _id_shopsp = String.Empty;
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

        public Nullable<decimal> je
        {
            get
            {
                return _je;
            }
            set
            {
                if (value.HasValue)
                {
                    _je = value.Value;
                }
                else
                {
                    _je = 0m;
                }
            }
        }

        public Nullable<DateTime> rq
        {
            get
            {
                return _rq;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq = value.Value;
                }
                else
                {
                    _rq = new DateTime(1900, 1, 1);
                }
            }
        }


        

            public string id_kcsp
        {
            get
            {
                return _id_kcsp;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_kcsp = value;
                }
                else
                {
                    _id_kcsp = String.Empty;
                }
            }
        }

        #endregion

    }
}

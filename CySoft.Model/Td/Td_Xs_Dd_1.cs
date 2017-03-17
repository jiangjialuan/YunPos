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
    [Table("td_xs_dd_1", "Td_Xs_Dd_1")]
    [DebuggerDisplay("id = {id}")]
    public class Td_Xs_Dd_1
    {
        #region public method

        public Td_Xs_Dd_1 Clone()
        {
            return (Td_Xs_Dd_1)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _dh = String.Empty;
        private DateTime _rq = new DateTime(1900, 1, 1);
        private string _id_shop = String.Empty;
        private DateTime _rq_jh = new DateTime(1900, 1, 1);
        private byte _flag_ddlx = 0;
        private byte _flag_dhtype = 0;
        private string _bm_djlx = String.Empty;
        private string _id_kh = String.Empty;
        private string _id_jbr = String.Empty;
        private decimal _je_mxtotal = 0m;
        private DateTime _rq_sh = new DateTime(1900, 1, 1);
        private byte _flag_sh = 0;
        private string _id_user_sh = String.Empty;
        private byte _flag_cancel = 0;
        private string _bz = String.Empty;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _id_edit = String.Empty;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private byte _flag_delete = 0;
        

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

        public string dh
        {
            get
            {
                return _dh;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _dh = value;
                }
                else
                {
                    _dh = String.Empty;
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

        public Nullable<DateTime> rq_jh
        {
            get
            {
                return _rq_jh;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_jh = value.Value;
                }
                else
                {
                    _rq_jh = new DateTime(1900, 1, 1);
                }
            }
        }

        public Nullable<byte> flag_ddlx
        {
            get
            {
                return _flag_ddlx;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_ddlx = value.Value;
                }
                else
                {
                    _flag_ddlx = 0;
                }
            }
        }

        public Nullable<byte> flag_dhtype
        {
            get
            {
                return _flag_dhtype;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_dhtype = value.Value;
                }
                else
                {
                    _flag_dhtype = 0;
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

        public string id_kh
        {
            get
            {
                return _id_kh;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_kh = value;
                }
                else
                {
                    _id_kh = String.Empty;
                }
            }
        }

        public string id_jbr
        {
            get
            {
                return _id_jbr;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_jbr = value;
                }
                else
                {
                    _id_jbr = String.Empty;
                }
            }
        }

        public Nullable<decimal> je_mxtotal
        {
            get
            {
                return _je_mxtotal;
            }
            set
            {
                if (value.HasValue)
                {
                    _je_mxtotal = value.Value;
                }
                else
                {
                    _je_mxtotal = 0m;
                }
            }
        }

        public Nullable<DateTime> rq_sh
        {
            get
            {
                return _rq_sh;
            }
            set
            {
                if (value.HasValue)
                {
                    _rq_sh = value.Value;
                }
                else
                {
                    _rq_sh = new DateTime(1900, 1, 1);
                }
            }
        }

        public Nullable<byte> flag_sh
        {
            get
            {
                return _flag_sh;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_sh = value.Value;
                }
                else
                {
                    _flag_sh = 0;
                }
            }
        }

        public string id_user_sh
        {
            get
            {
                return _id_user_sh;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_user_sh = value;
                }
                else
                {
                    _id_user_sh = String.Empty;
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

        public string id_edit
        {
            get
            {
                return _id_edit;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
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

        public byte[] nlast { set; get; }
        

        #endregion

    }

    public class Td_Xs_Dd_1_QueryModel : Td_Xs_Dd_1
    {
        #region public method

        public Td_Xs_Dd_1_ApiModel CloneApiModel()
        {
            var model = (Td_Xs_Dd_1_QueryModel)this.MemberwiseClone();
            Td_Xs_Dd_1_ApiModel rModel = new Td_Xs_Dd_1_ApiModel();
            rModel.id = model.id;
            rModel.rq = model.rq;
            rModel.dh = model.dh;
            rModel.id_kh = model.id_kh;
            rModel.je_mxtotal = model.je_mxtotal;
            rModel.bz = model.bz;
            return rModel;
        }

        #endregion

        public string shop_sh_name { set; get; }
        public string shop_name { set; get; }
        public string kh_name { set; get; }
        public string jbr_name { set; get; }
        public string bm_djlx_name { set; get; }
        public decimal? je_yf { set; get; }
        public int finish_ck { set; get; }

        public string create_name { set; get; }
        public string sh_name { set; get; }

    }


    public class Td_Xs_Dd_1_Query_DetailModel
    {
        public Td_Xs_Dd_1_QueryModel head { set; get; }
        public List<Td_Xs_Dd_2_QueryModel> body { set; get; }
    }

    public class Td_Xs_Dd_1_ApiModel
    {
        public string id { set; get; }
        public DateTime? rq { set; get; }
        public string dh { set; get; }
        public string id_kh { set; get; }
        public decimal? je_mxtotal { set; get; }
        public string bz { set; get; }
    }

    public class Td_Xs_Dd_2_ApiModel
    {
        public string id_bill { set; get; }
        public int? sort_id { set; get; }
        public string id_shopsp { set; get; }
        public decimal? dj { set; get; }
        public decimal? je { set; get; }
        public decimal? sl { set; get; }

    }



}

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
    [Table("tb_shopsp_exportdata", "Tb_Shopsp_Exportdata")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Shopsp_Exportdata
    {
        #region public method

        public Tb_Shopsp_Exportdata Clone()
        {
            return (Tb_Shopsp_Exportdata)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_cyuser = String.Empty;
        private string _id = String.Empty;
        private string _id_shop = String.Empty;
        private string _id_sp = String.Empty;
        private int _dw_xh = 0;
        private string _dw = String.Empty;
        private string _id_kcsp = String.Empty;
        private string _bm = String.Empty;
        private string _mc = String.Empty;
        private string _id_spfl = String.Empty;
        private string _barcode = String.Empty;
        private string _zjm = String.Empty;
        private decimal _zhl = 0m;
        private string _cd = String.Empty;
        private byte _flag_state = 0;
        private byte _flag_czfs = 0;
        private decimal _dj_ls = 0m;
        private decimal _dj_jh = 0m;
        private decimal _sl_kc_min = 0m;
        private decimal _sl_kc_max = 0m;
        private string _bz = String.Empty;
        private string _pic_path = String.Empty;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _id_edit = String.Empty;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private byte _flag_delete = 0;
        private string _spfl_mc = String.Empty;
        private string _state_mc = String.Empty;
        private string _czfs_mc = String.Empty;
        private decimal _qc_sl = 0m;
        private decimal _qc_je = 0m;
        private decimal _kc_sl = 0m;
        private decimal _kc_je = 0m;
        private decimal _dj_hy = 0m;


        #endregion

        #region public property

        public string id_cyuser
        {
            get
            {
                return _id_cyuser;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_cyuser = value;
                }
                else
                {
                    _id_cyuser = String.Empty;
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

        public Nullable<int> dw_xh
        {
            get
            {
                return _dw_xh;
            }
            set
            {
                if (value.HasValue)
                {
                    _dw_xh = value.Value;
                }
                else
                {
                    _dw_xh = 0;
                }
            }
        }

        public string dw
        {
            get
            {
                return _dw;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _dw = value;
                }
                else
                {
                    _dw = String.Empty;
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

        public string id_spfl
        {
            get
            {
                return _id_spfl;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_spfl = value;
                }
                else
                {
                    _id_spfl = String.Empty;
                }
            }
        }

        public string barcode
        {
            get
            {
                return _barcode;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _barcode = value;
                }
                else
                {
                    _barcode = String.Empty;
                }
            }
        }

        public string zjm
        {
            get
            {
                return _zjm;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _zjm = value;
                }
                else
                {
                    _zjm = String.Empty;
                }
            }
        }

        public Nullable<decimal> zhl
        {
            get
            {
                return _zhl;
            }
            set
            {
                if (value.HasValue)
                {
                    _zhl = value.Value;
                }
                else
                {
                    _zhl = 0m;
                }
            }
        }

        public string cd
        {
            get
            {
                return _cd;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _cd = value;
                }
                else
                {
                    _cd = String.Empty;
                }
            }
        }

        public Nullable<byte> flag_state
        {
            get
            {
                return _flag_state;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_state = value.Value;
                }
                else
                {
                    _flag_state = 0;
                }
            }
        }

        public Nullable<byte> flag_czfs
        {
            get
            {
                return _flag_czfs;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_czfs = value.Value;
                }
                else
                {
                    _flag_czfs = 0;
                }
            }
        }

        public Nullable<decimal> dj_ls
        {
            get
            {
                return _dj_ls;
            }
            set
            {
                if (value.HasValue)
                {
                    _dj_ls = value.Value;
                }
                else
                {
                    _dj_ls = 0m;
                }
            }
        }

        public Nullable<decimal> dj_jh
        {
            get
            {
                return _dj_jh;
            }
            set
            {
                if (value.HasValue)
                {
                    _dj_jh = value.Value;
                }
                else
                {
                    _dj_jh = 0m;
                }
            }
        }

        public Nullable<decimal> sl_kc_min
        {
            get
            {
                return _sl_kc_min;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_kc_min = value.Value;
                }
                else
                {
                    _sl_kc_min = 0m;
                }
            }
        }

        public Nullable<decimal> sl_kc_max
        {
            get
            {
                return _sl_kc_max;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_kc_max = value.Value;
                }
                else
                {
                    _sl_kc_max = 0m;
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

        public string pic_path
        {
            get
            {
                return _pic_path;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _pic_path = value;
                }
                else
                {
                    _pic_path = String.Empty;
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

        /// <summary>
        /// 商品分类名
        /// </summary>
        public string spfl_mc
        {
            get
            {
                return _spfl_mc;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _spfl_mc = value;
                }
                else
                {
                    _spfl_mc = String.Empty;
                }
            }
        }

        /// <summary>
        /// 生命周期名
        /// </summary>
        public string state_mc
        {
            get
            {
                return _state_mc;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _state_mc = value;
                }
                else
                {
                    _state_mc = String.Empty;
                }
            }
        }

        public string czfs_mc
        {
            get
            {
                return _czfs_mc;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _czfs_mc = value;
                }
                else
                {
                    _czfs_mc = String.Empty;
                }
            }
        }

        public Nullable<decimal> qc_sl
        {
            get
            {
                return _qc_sl;
            }
            set
            {
                if (value.HasValue)
                {
                    _qc_sl = value.Value;
                }
                else
                {
                    _qc_sl = 0m;
                }
            }
        }

        public Nullable<decimal> qc_je
        {
            get
            {
                return _qc_je;
            }
            set
            {
                if (value.HasValue)
                {
                    _qc_je = value.Value;
                }
                else
                {
                    _qc_je = 0m;
                }
            }
        }

        public Nullable<decimal> kc_sl
        {
            get
            {
                return _kc_sl;
            }
            set
            {
                if (value.HasValue)
                {
                    _kc_sl = value.Value;
                }
                else
                {
                    _kc_sl = 0m;
                }
            }
        }

        public Nullable<decimal> kc_je
        {
            get
            {
                return _kc_je;
            }
            set
            {
                if (value.HasValue)
                {
                    _kc_je = value.Value;
                }
                else
                {
                    _kc_je = 0m;
                }
            }
        }



        public Nullable<decimal> dj_hy
        {
            get
            {
                return _dj_hy;
            }
            set
            {
                if (value.HasValue)
                {
                    _dj_hy = value.Value;
                }
                else
                {
                    _dj_hy = 0m;
                }
            }
        }


        #endregion

        public string gys_mc { set; get; }
        public int yxq { set; get; }
        public decimal? dj_ps { set; get; }
        public decimal? dj_pf { set; get; }
        

    }
}
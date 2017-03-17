using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Mapping;

namespace CySoft.Model.Td
{
    [Serializable]
    [Table("td_promote_1", "Td_Promote_1")]
    [DebuggerDisplay("id = {id}")]
    public class Td_Promote_1
    {
        #region public method

        public Td_Promote_1 Clone()
        {
            return (Td_Promote_1)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _dh = String.Empty;
        private DateTime _rq = new DateTime(1900, 1, 1);
        private string _id_shop = String.Empty;
        private string _id_jbr = String.Empty;
        private byte _yxj_id = 0;
        private DateTime _day_b = new DateTime(1900, 1, 1);
        private DateTime _day_e = new DateTime(1900, 1, 1);
        private string _time_b = String.Empty;
        private string _time_e = String.Empty;
        private byte _flag_rqfw = 0;
        private string _weeks = String.Empty;
        private string _days = String.Empty;
        private string _rule_name = String.Empty;
        private string _style = String.Empty;
        private string _examine = String.Empty;
        private string _preferential = String.Empty;
        private string _strategy = String.Empty;
        private decimal _sl_largess_1 = 0m;
        private decimal _sl_largess_2 = 0m;
        private decimal _sl_largess_3 = 0m;
        private string _sd1 = String.Empty;
        private string _sd2 = String.Empty;
        private string _sd3 = String.Empty;
        private decimal _condition_1 = 0m;
        private decimal _condition_2 = 0m;
        private decimal _condition_3 = 0m;
        private decimal _result_1 = 0m;
        private decimal _result_2 = 0m;
        private decimal _result_3 = 0m;
        private string _zh_condition_1 = String.Empty;
        private byte _flag_zsz = 0;
        private byte _flag_moling = 0;
        private string _bz = String.Empty;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _id_edit = String.Empty;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private byte _flag_delete = 0;
        private byte _flag_sh = 0;
        private string _id_user_sh = String.Empty;
        private DateTime _rq_sh = new DateTime(1900, 1, 1);
        private string _bm_djlx = String.Empty;
        private string _id_hyfl_list = String.Empty;
        private byte _flag_cancel = 0;
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

        public Nullable<byte> yxj_id
        {
            get
            {
                return _yxj_id;
            }
            set
            {
                if (value.HasValue)
                {
                    _yxj_id = value.Value;
                }
                else
                {
                    _yxj_id = 0;
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

        public string time_b
        {
            get
            {
                return _time_b;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _time_b = value;
                }
                else
                {
                    _time_b = String.Empty;
                }
            }
        }

        public string time_e
        {
            get
            {
                return _time_e;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _time_e = value;
                }
                else
                {
                    _time_e = String.Empty;
                }
            }
        }

        public Nullable<byte> flag_rqfw
        {
            get
            {
                return _flag_rqfw;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_rqfw = value.Value;
                }
                else
                {
                    _flag_rqfw = 0;
                }
            }
        }

        public string weeks
        {
            get
            {
                return _weeks;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _weeks = value;
                }
                else
                {
                    _weeks = String.Empty;
                }
            }
        }

        public string days
        {
            get
            {
                return _days;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _days = value;
                }
                else
                {
                    _days = String.Empty;
                }
            }
        }

        public string rule_name
        {
            get
            {
                return _rule_name;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _rule_name = value;
                }
                else
                {
                    _rule_name = String.Empty;
                }
            }
        }

        public string style
        {
            get
            {
                return _style;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _style = value;
                }
                else
                {
                    _style = String.Empty;
                }
            }
        }

        public string examine
        {
            get
            {
                return _examine;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _examine = value;
                }
                else
                {
                    _examine = String.Empty;
                }
            }
        }

        public string preferential
        {
            get
            {
                return _preferential;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _preferential = value;
                }
                else
                {
                    _preferential = String.Empty;
                }
            }
        }

        public string strategy
        {
            get
            {
                return _strategy;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _strategy = value;
                }
                else
                {
                    _strategy = String.Empty;
                }
            }
        }

        public Nullable<decimal> sl_largess_1
        {
            get
            {
                return _sl_largess_1;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_largess_1 = value.Value;
                }
                else
                {
                    _sl_largess_1 = 0m;
                }
            }
        }

        public Nullable<decimal> sl_largess_2
        {
            get
            {
                return _sl_largess_2;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_largess_2 = value.Value;
                }
                else
                {
                    _sl_largess_2 = 0m;
                }
            }
        }

        public Nullable<decimal> sl_largess_3
        {
            get
            {
                return _sl_largess_3;
            }
            set
            {
                if (value.HasValue)
                {
                    _sl_largess_3 = value.Value;
                }
                else
                {
                    _sl_largess_3 = 0m;
                }
            }
        }

        public string sd1
        {
            get
            {
                return _sd1;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _sd1 = value;
                }
                else
                {
                    _sd1 = String.Empty;
                }
            }
        }

        public string sd2
        {
            get
            {
                return _sd2;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _sd2 = value;
                }
                else
                {
                    _sd2 = String.Empty;
                }
            }
        }

        public string sd3
        {
            get
            {
                return _sd3;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _sd3 = value;
                }
                else
                {
                    _sd3 = String.Empty;
                }
            }
        }

        public Nullable<decimal> condition_1
        {
            get
            {
                return _condition_1;
            }
            set
            {
                if (value.HasValue)
                {
                    _condition_1 = value.Value;
                }
                else
                {
                    _condition_1 = 0m;
                }
            }
        }

        public Nullable<decimal> condition_2
        {
            get
            {
                return _condition_2;
            }
            set
            {
                if (value.HasValue)
                {
                    _condition_2 = value.Value;
                }
                else
                {
                    _condition_2 = 0m;
                }
            }
        }

        public Nullable<decimal> condition_3
        {
            get
            {
                return _condition_3;
            }
            set
            {
                if (value.HasValue)
                {
                    _condition_3 = value.Value;
                }
                else
                {
                    _condition_3 = 0m;
                }
            }
        }

        public Nullable<decimal> result_1
        {
            get
            {
                return _result_1;
            }
            set
            {
                if (value.HasValue)
                {
                    _result_1 = value.Value;
                }
                else
                {
                    _result_1 = 0m;
                }
            }
        }

        public Nullable<decimal> result_2
        {
            get
            {
                return _result_2;
            }
            set
            {
                if (value.HasValue)
                {
                    _result_2 = value.Value;
                }
                else
                {
                    _result_2 = 0m;
                }
            }
        }

        public Nullable<decimal> result_3
        {
            get
            {
                return _result_3;
            }
            set
            {
                if (value.HasValue)
                {
                    _result_3 = value.Value;
                }
                else
                {
                    _result_3 = 0m;
                }
            }
        }

        public string zh_condition_1
        {
            get
            {
                return _zh_condition_1;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _zh_condition_1 = value;
                }
                else
                {
                    _zh_condition_1 = String.Empty;
                }
            }
        }

        public Nullable<byte> flag_zsz
        {
            get
            {
                return _flag_zsz;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_zsz = value.Value;
                }
                else
                {
                    _flag_zsz = 0;
                }
            }
        }

        public Nullable<byte> flag_moling
        {
            get
            {
                return _flag_moling;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_moling = value.Value;
                }
                else
                {
                    _flag_moling = 0;
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

        public string id_hyfl_list
        {
            get
            {
                return _id_hyfl_list;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_hyfl_list = value;
                }
                else
                {
                    _id_hyfl_list = String.Empty;
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
        #endregion

    }

    public class Td_Promote_1WithUserName : Td_Promote_1
    {
        /// <summary>
        /// 制单人
        /// </summary>
        public string zdr { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string shr { get; set; }
    }
}

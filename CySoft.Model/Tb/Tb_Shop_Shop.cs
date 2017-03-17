using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Mapping;

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("tb_shop_shop", "Tb_Shop_Shop")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Shop_Shop
    {
        #region public method

        public Tb_Shop_Shop Clone()
        {
            return (Tb_Shop_Shop)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _id_shop_child = String.Empty;
        private string _id_shop_father = String.Empty;
        private string _path = String.Empty;
        private byte _flag_state = 0;
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

        public string id_shop_child
        {
            get
            {
                return _id_shop_child;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_shop_child = value;
                }
                else
                {
                    _id_shop_child = String.Empty;
                }
            }
        }

        public string id_shop_father
        {
            get
            {
                return _id_shop_father;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_shop_father = value;
                }
                else
                {
                    _id_shop_father = String.Empty;
                }
            }
        }

        public string path
        {
            get
            {
                return _path;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _path = value;
                }
                else
                {
                    _path = String.Empty;
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

        #endregion

    }

    [Serializable]
    [Table("tb_shop_shop", "Tb_Shop_Shop")]
    [DebuggerDisplay("id = {id}")]

    public class Tb_Shop_ShopWithMc : Tb_Shop_Shop
    {
        public string mc { get; set; }
        public string bm { get; set; }
        public byte shop_flag_state { get; set; }
        public byte shop_flag_delete { get; set; }

        public int flag_type { get; set; }
    }
}

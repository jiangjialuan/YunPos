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
    [Table("tb_hy", "Tb_Hy")]
    [DebuggerDisplay("id = {id}")]
    public class Tb_Hy
    {
        #region public method

        public Tb_Hy Clone()
        {
            return (Tb_Hy)this.MemberwiseClone();
        }

        #endregion

        #region private field

        private string _id_masteruser = String.Empty;
        private string _id = String.Empty;
        private string _name = String.Empty;
        private string _qq = String.Empty;
        private string _email = String.Empty;
        private string _phone = String.Empty;
        private string _tel = String.Empty;
        private string _address = String.Empty;
        private string _MMno = String.Empty;
        private string _zipcode = String.Empty;
        private DateTime _birthday = new DateTime(1900, 1, 1);
        private byte _flag_nl = 0;
        private string _id_shop_create = String.Empty;
        private string _id_create = String.Empty;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private string _id_edit = String.Empty;
        private DateTime _rq_edit = new DateTime(1900, 1, 1);
        private byte _flag_delete = 0;
        private byte[] _nlast;
        private string _hysr = String.Empty;
        private byte _flag_sex = 1;
        private string _password = String.Empty;


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

        //public string membercard
        //{
        //    get
        //    {
        //        return _membercard;
        //    }
        //    set
        //    {
        //        if (!String.IsNullOrEmpty(value))
        //        {
        //            _membercard = value;
        //        }
        //        else
        //        {
        //            _membercard = String.Empty;
        //        }
        //    }
        //}

        public string qq
        {
            get
            {
                return _qq;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _qq = value;
                }
                else
                {
                    _qq = String.Empty;
                }
            }
        }

        public string email
        {
            get
            {
                return _email;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _email = value;
                }
                else
                {
                    _email = String.Empty;
                }
            }
        }

        public string phone
        {
            get
            {
                return _phone;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _phone = value;
                }
                else
                {
                    _phone = String.Empty;
                }
            }
        }

        public string tel
        {
            get
            {
                return _tel;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _tel = value;
                }
                else
                {
                    _tel = String.Empty;
                }
            }
        }

        public string address
        {
            get
            {
                return _address;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _address = value;
                }
                else
                {
                    _address = String.Empty;
                }
            }
        }

        public string MMno
        {
            get
            {
                return _MMno;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _MMno = value;
                }
                else
                {
                    _MMno = String.Empty;
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

        public Nullable<DateTime> birthday
        {
            get
            {
                return _birthday;
            }
            set
            {
                if (value.HasValue)
                {
                    _birthday = value.Value;
                }
                else
                {
                    _birthday = new DateTime(1900, 1, 1);
                }
            }
        }



        public Nullable<byte> flag_nl
        {
            get
            {
                return _flag_nl;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_nl = value.Value;
                }
                else
                {
                    _flag_nl = 0;
                }
            }
        }

        public string id_shop_create
        {
            get
            {
                return _id_shop_create;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_shop_create = value;
                }
                else
                {
                    _id_shop_create = String.Empty;
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

        public Byte[] nlast { get; set; }


        public string hysr
        {
            get
            {
                return _hysr;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _hysr = value;
                }
                else
                {
                    _hysr = String.Empty;
                }
            }
        }




        public Nullable<byte> flag_sex
        {
            get
            {
                return _flag_sex;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_sex = value.Value;
                }
                else
                {
                    _flag_sex = 0;
                }
            }
        }


        public string password
        {
            get
            {
                return _password;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _password = value;
                }
                else
                {
                    _password = String.Empty;
                }
            }
        }


        #endregion

    }


    public class Tb_Hy_Detail
    {
        public Tb_Hy Tb_Hy { set; get; }
        public Tb_Hy_Shop_Query Tb_Hy_Shop { set; get; }
    }



}
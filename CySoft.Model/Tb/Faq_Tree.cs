using System;
using CySoft.Model.Base;
using CySoft.Model.Mapping;
using System.Diagnostics;

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("faq_Tree", "Faq_Tree")]
    public class Faq_Tree:BaseTree<Faq_Tree>
    {
        #region public method
        public Faq_Tree Clone()
        {
            return (Faq_Tree)this.MemberwiseClone();
        }
        #endregion
        
        #region private field

        private long _id_user = 0;
        private long _id_user_master = 0;
        private DateTime _rq_create = new DateTime(1900, 1, 1);
        private byte _flag_type = 0;
        private byte _flag_state = 0;
        private byte _flag_delete = 0;
        private long _id_user_receive = 0;
        private string _flag_from = String.Empty;
        private string name = string.Empty;
        private string companyname = string.Empty;
        private string reName = string.Empty;
        private string reCompanyname = string.Empty;
        private string phone = string.Empty;
        private string rePhone = string.Empty;
        private string email = string.Empty;

        #endregion

        public Nullable<long> id_user
        {
            get
            {
                return _id_user;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_user = value.Value;
                }
                else
                {
                    _id_user = 0;
                }
            }
        }

        public Nullable<long> id_user_master
        {
            get
            {
                return _id_user_master;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_user_master = value.Value;
                }
                else
                {
                    _id_user_master = 0;
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

        /// <summary>
        /// 0问题内容、1回复内容
        /// </summary>
        public Nullable<byte> flag_type
        {
            get
            {
                return _flag_type;
            }
            set
            {
                if (value.HasValue)
                {
                    _flag_type = value.Value;
                }
                else
                {
                    _flag_type = 0;
                }
            }
        }

        /// <summary>
        /// 0未回复、1已回复、2已解决
        /// </summary>
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

        public Nullable<long> id_user_receive
        {
            get
            {
                return _id_user_receive;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_user_receive = value.Value;
                }
                else
                {
                    _id_user_receive = 0;
                }
            }
        }


        public string flag_from
        {
            get
            {
                return _flag_from;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _flag_from = value;
                }
                else
                {
                    _flag_from = String.Empty;
                }
            }
        }

        /// <summary>
        /// 提问人昵称
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (!String.IsNullOrEmpty(name))
                {
                    name = value;
                }
                else
                {
                    name = string.Empty;
                }
            }
        }
        
        /// <summary>
        /// 提问人公司名称
        /// </summary>
        public string Companyname
        {
            get { return companyname; }
            set 
            {
                if (!String.IsNullOrEmpty(companyname))
                {
                    companyname = value;
                }
                else
                {
                    companyname = string.Empty;
                }
            }
        }

        /// <summary>
        /// 回答人昵称
        /// </summary>
        public string ReName
        {
            get { return reName; }
            set
            {
                if (!String.IsNullOrEmpty(reName))
                {
                    reName = value;
                }
                else
                {
                    reName = string.Empty;
                }
            }
        }

        /// <summary>
        /// 回答人公司名称
        /// </summary>
        public string ReCompanyname
        {
            get { return reCompanyname; }
            set
            {
                if (!String.IsNullOrEmpty(reCompanyname))
                {
                    reCompanyname = value;
                }
                else
                {
                    reCompanyname = string.Empty;
                }
            }
        }

       /// <summary>
       /// 提问人联系电话
       /// </summary>
        public string Phone
        {
            get { return phone; }
            set
            {
                if (!String.IsNullOrEmpty(phone))
                {
                    phone = value;
                }
                else
                {
                    phone = string.Empty;
                }
            }
        }

        /// <summary>
        /// 回答人联系电话
        /// </summary>
        public string RePhone
        {
            get { return rePhone; }
            set
            {
                if (!String.IsNullOrEmpty(rePhone))
                {
                    rePhone = value;
                }
                else
                {
                    rePhone = string.Empty;
                }
            }
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email
        {
            get { return email; }
            set {
                if (!String.IsNullOrEmpty(email))
                {
                    email = value;
                }
                else
                {
                    email = string.Empty;
                }
            }
        }
    }
}

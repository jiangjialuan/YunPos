using System;
using System.Diagnostics;
using CySoft.Model.Mapping;

namespace CySoft.Model.Tb
{
    [Serializable]
    [Table("Tb_user", "Tb_User")]
    [DebuggerDisplay("id = {id}, username = {username}, password = {password}")]
    public class Tb_User_Edit : Tb_User
    {
        private string _email_old = String.Empty;
        private string _id_roles = String.Empty;
        public string id_shops { get; set; }
        /// <summary>
        /// 原电子邮箱
        /// </summary>
        public string email_old
        {
            get
            {
                return _email_old;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _email_old = value;
                }
                else
                {
                    _email_old = String.Empty;
                }
            }
        }
        /// <summary>
        /// 角色
        /// </summary>
        public string id_roles
        {
            get
            {
                return _id_roles;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _id_roles = value;
                }
                else
                {
                    _id_roles = String.Empty;
                }
            }
        }


    }
}

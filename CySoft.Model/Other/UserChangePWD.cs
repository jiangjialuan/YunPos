using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CySoft.Model.Other
{
   [Serializable]
   [DebuggerDisplay("id_user = {id_user}, oldPassword = {oldPassword}, newPassword = {newPassword}")]
   public class UserChangePWD
    {
       private string _id_user = String.Empty;
       private string _oldPassword;
       private string _newPassword;
       private string _id_edit = String.Empty;
       private string _req_newPassword;

       /// <summary>
       /// 用户Id
       /// </summary>
       public string id_user
       {
           get { return _id_user; }
           set
           {
               if (!string.IsNullOrEmpty(value))
               {
                   _id_user = value ;
               }
               else
               {
                   this._id_user =String.Empty;
               }
           }
       }

       public string oldPassword
       {
           get
           {
               if (this._oldPassword == null)
               {
                   return String.Empty;
               }
               return this._oldPassword;
           }
           set
           {
               this._oldPassword = value;
           }
       }

       public string newPassword
       {
           get
           {
               if (this._newPassword == null)
               {
                   return String.Empty;
               }
               return this._newPassword;
           }
           set
           {
               this._newPassword = value;
           }
       }

       public string req_newPassword
       {
           get
           {
               if (this._req_newPassword == null)
               {
                   return String.Empty;
               }
               return this._req_newPassword;
           }
           set
           {
               this._req_newPassword = value;
           }
       }

       public string id_edit
       {
           get { return _id_edit; }
           set
           {
               if (!string.IsNullOrEmpty(value))
               {
                   _id_edit = value;
               }
               else
               {
                   this._id_edit = String.Empty;
               }
           }
       }

       public string flag_from { get; set; }

       public string version { get; set; }

       public bool IsMaster { get; set; }

    }
}

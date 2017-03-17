using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using CySoft.Model.Enums;

namespace CySoft.BLL.SystemBLL.SettingBLL
{
    public class UserRoleBLL : BaseBLL
    {
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            if (param != null && !param.ContainsKey("flag_delete"))
                param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            br.Data = DAL.QueryList<Tb_User_Role>(typeof(Tb_User_Role), param);
            br.Success = true;
            return br;
        }
    }
}

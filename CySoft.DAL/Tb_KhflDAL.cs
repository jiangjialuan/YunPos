#region Imports
using Apache.Ibatis.DataMapper.Exceptions;
using CySoft.DAL.Base;
using CySoft.IDAL;
using CySoft.Model.Tb;
using System.Collections;
using System.Collections.Generic;
#endregion

namespace CySoft.DAL.Tb
{
    public class Tb_KhflDAL : BaseDAL, ITb_KhflDAL
    {
        public IList<Tb_Khfl> QuerySubListByPath(string id_masteruser, string fatherPath)
        {
            if (string.IsNullOrEmpty(id_masteruser) || string.IsNullOrEmpty(fatherPath)) return null;
            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("fartherPath", fatherPath);
            return dataMapper.QueryForList<Tb_Khfl>("Tb_Khfl.QuerySubListByPath", ht);
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.DAL.Base;
using CySoft.IDAL;
using CySoft.Model;

namespace CySoft.DAL 
{
    public class Tb_SpflDAL : BaseDAL, ITb_SpflDAL
    {

        public IList<Tb_Spfl> QuerySubListByPath(string id_masteruser, string fatherPath)
        {
            if (string.IsNullOrEmpty(id_masteruser) || string.IsNullOrEmpty(fatherPath)) return null;
            Hashtable ht=new Hashtable();
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("fartherPath", fatherPath);
            return dataMapper.QueryForList<Tb_Spfl>("Tb_Spfl.QuerySubListByPath", ht);
        }

    }

}

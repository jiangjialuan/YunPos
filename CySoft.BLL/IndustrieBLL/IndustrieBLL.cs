using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using System.Collections;
using CySoft.Model.Flags;

#region 行业
#endregion

namespace CySoft.BLL.IndustrieBLL
{
    public class IndustrieBLL : TreeBLL
    {
        /// <summary>
        /// 获得树结构数据
        /// lxt
        /// 2015-03-03
        /// </summary>
        public override BaseResult GetTree(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            var list = DAL.QueryTree<Tb_Hy_Tree>(typeof(Tb_Hy), param);
            br.Data = CreateTree(list);
            br.Success = true;
            return br;
        }
    }
}

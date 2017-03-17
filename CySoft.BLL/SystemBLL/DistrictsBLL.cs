using System;
using CySoft.Model.Tb;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.BLL.Base;


#region 区域
#endregion

namespace CySoft.BLL.SystemBLL
{
    public class DistrictsBLL : TreeBLL
    {
        protected static readonly Type _Type = typeof(Tb_Districts);
        protected static readonly string areaAbsolutePath = "/Scripts/config/cyDistricts.js";

        /// <summary>
        /// 不分页查询
        /// cxb
        /// 2015-02-11
        /// </summary>
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Tb_Districts>(_Type, param);
            br.Success = true;
            return br;
        }


        /// <summary>
        /// 获得树结构数据
        /// znt
        /// 2015-03-05
        /// </summary>
        public override BaseResult GetTree(Hashtable param = null)
        {
            BaseResult br = new BaseResult();         
            if (param!=null&&param.ContainsKey("childId") && param["childId"].ToString().Equals("0"))
            {
                param.Remove("childId");         
            }
            var list = DAL.QueryTree<Tb_Districts_Tree>(_Type, param);
            br.Data = CreateTree(list);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 获得单个完整对象
        /// lxt
        /// 2015-03-05
        /// </summary>
        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.GetItem<Tb_Districts>(_Type, param);
            if (br.Data == null)
            {
                br.Success = false;
                br.Message.Add("未找到该区域！");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            br.Success = true;
            return br;
        }    
    }
}

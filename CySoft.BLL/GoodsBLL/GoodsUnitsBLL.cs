#region Imports
using System;
using System.Collections;
using System.Collections.Generic;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using CySoft.BLL.Tools.CodingRule;
#endregion

#region 商品分类
#endregion

namespace CySoft.BLL.GoodsBLL
{
    public class GoodsUnitsBLL : BaseBLL
    {

        protected static readonly Type goodUnits = typeof(Tb_Unit);

        /// <summary>
        /// 添加节点
        /// cxb
        /// 2015-2-12 14:09:33
        /// </summary>
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Tb_Unit model = (Tb_Unit)entity;
            Hashtable param = new Hashtable();
            param.Add("id_gys", model.id_gys);
            param.Add("name", model.name);
            if (DAL.GetCount(typeof(Tb_Unit), param) > 0)
            {
                br.Success = false;
                br.Message.Add("该名称已被占用");
                br.Level = ErrorLevel.Warning;
                br.Data = "name";
                return br;
            }
            DAL.Add(model);
            br.Data = model;
            br.Message.Add(String.Format("新增计量单位成功。流水号{0}，名称:{1}", model.id, model.name));
            br.Success = true;
            return br;
        }
        /// <summary>
        /// 删除
        /// cxb
        /// 2015-2-27 
        /// </summary>
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult br = new BaseResult();
            long id = Convert.ToInt64(param["id"]);
            param.Clear();
            param.Add("id", id);
            Tb_Unit model = DAL.GetItem<Tb_Unit>(goodUnits, param);
            DAL.Delete(goodUnits, param);
            if (model != null)
            {
                br.Message.Add(String.Format("删除计量单位。流水号{0}，名称:{1}", model.id, model.name));
            }
            br.Success = true;
            return br;
        }
        /// <summary>
        /// 不分页查询
        /// cxb
        /// 2015-02-11
        /// </summary>
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Tb_Unit>(goodUnits, param);
            br.Success = true;
            return br;
        }

    }
}

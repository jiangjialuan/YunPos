using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using CySoft.Model.Other;
using System.Collections;
using CySoft.Model.Flags;
using CySoft.Utility;
using CySoft.Frame.Attributes;

#region 客户分类
#endregion

namespace CySoft.BLL.SupplierBLL.CustomerBLL
{
    public class CustomerTypeBLL : BaseBLL
    {
        protected static readonly Type cgsLevelType = typeof(Tb_Cgs_Level);

        /// <summary>
        /// 新增
        /// lxt
        /// 2015-02-11
        /// </summary>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Tb_Cgs_Level model = (Tb_Cgs_Level)entity;
            Hashtable param = new Hashtable();
            param.Add("id",model.id_gys);
            var gys = DAL.GetItem<Tb_Gys>(typeof(Tb_Gys),param);
            if (gys == null || !gys.id.HasValue || !(gys.id.Value > 0))
            {
                br.Success = false;
                br.Message.Add(string.Format("该供应商不存在。供应商ID{0}.",model.id_gys));
                br.Level = ErrorLevel.Warning;
                return br;
            }

            param.Clear();
            param.Add("name", model.name);
            param.Add("id_gys", model.id_gys);
            if (DAL.GetCount(cgsLevelType, param) > 0)
            {
                br.Success = false;
                br.Message.Add("该客户级别名称已存在。");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            model.flag_sys = YesNoFlag.No;
            model.rq_create = DateTime.Now;
            model.rq_edit = DateTime.Now;
            model.id_edit = model.id_create;
            DAL.Add(model);

            param.Clear();
            param.Add("id_gys", model.id_gys);
            var sp_list = DAL.QueryList<SkuData>(typeof(Tb_Gys_Sp),param);
            List<Tb_Sp_Dj> dj_list = new List<Tb_Sp_Dj>();
            Tb_Sp_Dj dj;
            foreach (var sp in sp_list)
            {
                dj = new Tb_Sp_Dj();
                dj.id_cgs_level = model.id;
                dj.id_gys = model.id_gys;
                dj.id_sku = sp.id;
                dj.id_sp = sp.id_sp;
                dj.sl_dh_min = 0;
                dj.dj_dh = (sp.dj_base * model.agio / 100).Digit(DigitConfig.dj);
                dj.id_create = model.id_create;
                dj.rq_create = model.rq_create;
                dj.id_edit = dj.id_create;
                dj.rq_edit = dj.rq_create;
                dj_list.Add(dj);
            }

            if (dj_list.Count > 0)
            {
                DAL.AddRange<Tb_Sp_Dj>(dj_list);
            }

            br.Message.Add(String.Format("新增客户级别。名称:{0}", model.name));
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 删除
        /// lxt
        /// 2015-02-12
        /// </summary>
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult br = new BaseResult();
            long id = Convert.ToInt64(param["id"]);
            param.Clear();
            param.Add("id_cgs_level", id);
            if (DAL.GetCount(typeof(Tb_Gys_Cgs), param) > 0)
            {
                br.Success = false;
                br.Message.Add("该客户级别已使用，不能删除，如要删除，请先修改此级别下的客户为其他级别。");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            param.Clear();
            param.Add("id", id);
            Tb_Cgs_Level model = DAL.GetItem<Tb_Cgs_Level>(cgsLevelType, param);
            if (model == null || !model.id.HasValue || !(model.id.Value > 0))
            {
                br.Success = false;
                br.Message.Add("该客户级别已不存在。");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            DAL.Delete(cgsLevelType, param);
            if (model != null)
            {
                br.Message.Add(String.Format("删除客户级别。流水号{0}，名称:{1}", model.id, model.name));
            }
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 修改
        /// lxt
        /// 2015-02-11
        /// </summary>
        public override BaseResult Update(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Tb_Cgs_Level model = (Tb_Cgs_Level)entity;
            Hashtable param = new Hashtable();
            param.Add("not_id", model.id);
            param.Add("name", model.name);
            param.Add("id_gys", model.id_gys);
            if (DAL.GetCount(cgsLevelType, param) > 0)
            {
                br.Success = false;
                br.Message.Add("该客户级别名称已存在");
                br.Level = ErrorLevel.Warning;
                br.Data = "name";
                return br;
            }
            model.rq_edit = DateTime.Now;
            DAL.Update(model);
            br.Message.Add(String.Format("修改客户级别。名称:{0}", model.name));
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 不分页查询
        /// lxt
        /// 2015-02-11
        /// </summary>
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Tb_Cgs_Level>(cgsLevelType, param);
            if (br.Data == null) br.Success = false;
            else  br.Success = true;
            return br;
        }

        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.GetItem<Tb_Cgs_Level>(cgsLevelType, param);
            if (br.Data == null)
            {
                br.Success = false;
                br.Message.Add("该客户级别不存在，请刷新后再试！");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            br.Success = true;
            return br;
        }
    }
}

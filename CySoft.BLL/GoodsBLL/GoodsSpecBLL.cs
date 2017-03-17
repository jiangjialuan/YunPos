using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Model.Tb;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Utility;
using CySoft.Model.Flags;

#region 商品规格模板
#endregion

namespace CySoft.BLL.GoodsBLL
{
    public class GoodsSpecBLL : BaseBLL
    {
         protected static readonly Type tb_Sp_Expand_TemplateType = typeof(Tb_Sp_Expand_Template);

         /// <summary>
         /// 新增
         /// znt
         /// 2015-02-28
         /// 2015-03-26 lxt 修改
         /// </summary>
         public override BaseResult Add(dynamic entity)
         {
             BaseResult br = new BaseResult();
             List<Tb_Sp_Expand_Template> list = (List<Tb_Sp_Expand_Template>)entity;
             var father = list.First(m => m.fatherId == 0);
             Hashtable param = new Hashtable();
             param.Add("mc", father.mc);
             param.Add("id_gys", father.id_gys);
             param.Add("fatherId", 0);
             if (DAL.GetCount(tb_Sp_Expand_TemplateType, param) > 0)
             {
                 br.Success = false;
                 br.Message.Add(String.Format("新增失败！模版名称【{0}】已被占用。", father.mc));
                 br.Level = ErrorLevel.Warning;
                 return br;
             }
             DAL.AddRange(list);

             br.Message.Add(String.Format("新增商品规格模板。信息：流水号:{0}，名称：{1}", father.id, father.mc));
             br.Success = true;
             return br;
         }

         /// <summary>
         /// 删除
         /// znt
         /// 2015-03-02
         /// 2015-03-27 lxt 修改
         /// </summary>
         [Transaction]
         public override BaseResult Delete(Hashtable param)
         {
             BaseResult br = new BaseResult();
             Hashtable param_son =new Hashtable();
             long id = Convert.ToInt64(param["id"]);
             param.Clear();
             param.Add("fatherId_sp_expand_template", id);
             if (DAL.GetCount(typeof(Tb_Sp_Expand), param) > 0)
             {
                 br.Success = false;
                 br.Message.Add("该规格模板正在被使用");
                 br.Level = ErrorLevel.Warning;
                 return br;
             }
             param.Clear();
             param.Add("id", id);
             Tb_Sp_Expand_Template model = DAL.GetItem<Tb_Sp_Expand_Template>(tb_Sp_Expand_TemplateType, param);
             if (model == null)
             {
                 br.Success = true;
                 return br;
             }

             param.Clear();
             param.Add("id", id);
             DAL.Delete(tb_Sp_Expand_TemplateType, param);
             param.Clear();
             param.Add("fatherId", id);
             DAL.Delete(tb_Sp_Expand_TemplateType, param);

             br.Message.Add(String.Format("删除商品规格模板。流水号{0}，名称:{1}", model.id, model.mc));
             br.Success = true;
             return br;
         }

         /// <summary>
         /// 修改
         /// znt
         /// 2015-02-28
         /// </summary>
         [Transaction]
         public override BaseResult Save(dynamic entity)
         {
             BaseResult br = new BaseResult();
             Hashtable param = (Hashtable)entity;
             var list = (List<Tb_Sp_Expand_Template>)param["list"];
             long id_edit = Convert.ToInt64(param["id_edit"]);
             long id_supplier = Convert.ToInt64(param["id_supplier"]);
             DateTime dbDateTime = DAL.GetDbDateTime();
             var deleteList = (from m in list where m.id > 0 && m.mc.IsEmpty() select m.id).ToList();
             if (deleteList.Count > 0)
             {
                 param.Clear();
                 param.Add("idList", deleteList);
                 param.Add("id_gys", id_supplier);
                 param.Add("flag_used", 1);
                 var deleteItem = DAL.GetItem<Tb_Sp_Expand_Template>(tb_Sp_Expand_TemplateType, param);
                 if (deleteItem != null)
                 {
                     param.Clear();
                     param.Add("id", deleteItem.fatherId);
                     var father = DAL.GetItem<Tb_Sp_Expand_Template>(tb_Sp_Expand_TemplateType, param);
                     br.Success = false;
                     br.Message.Add(String.Format("【{0}】>【{1}】正在被使用，不能删除！", father.mc, deleteItem.mc));
                     br.Level = ErrorLevel.Warning;
                     return br;
                 }
                 param.Clear();
                 param.Add("idList", deleteList);
                 param.Add("id_gys", id_supplier);
                 DAL.Delete(tb_Sp_Expand_TemplateType, param);
             }
             param.Clear();
             param.Add("id_gys", id_supplier);
             var oldList = DAL.QueryList<Tb_Sp_Expand_Template>(tb_Sp_Expand_TemplateType, param);
             List<Tb_Sp_Expand_Template> newList = new List<Tb_Sp_Expand_Template>();
             var query = from m in list where m.id > 0 && !m.mc.IsEmpty() select m;
             foreach (var item in query)
             {
                 if (oldList.Count(m => m.id == item.id) > 0)
                 {
                     if (oldList.Count(m => m.id == item.id && m.mc != item.mc) > 0)
                     {
                         param.Clear();
                         param.Add("id", item.id);
                         param.Add("id_gys", id_supplier);
                         param.Add("new_mc", item.mc);
                         param.Add("new_id_edit", id_edit);
                         param.Add("new_rq_edit", dbDateTime);
                         DAL.UpdatePart(tb_Sp_Expand_TemplateType, param);
                     }
                 }
                 else
                 {
                     item.id_create = id_edit;
                     item.id_edit = id_edit;
                     item.id_gys = id_supplier;
                     newList.Add(item);
                 }
             }
             DAL.AddRange(newList);

             br.Message.Add("保存商品规格模板。");
             br.Success = true;
             return br;
         }

         /// <summary>
         /// 查询所有
         /// znt
         /// 2015-2-28
         /// </summary>
         public override BaseResult GetAll(Hashtable param=null)
         {
             BaseResult br = new BaseResult();
             br.Data = DAL.QueryList<Tb_Sp_Expand_Template>(tb_Sp_Expand_TemplateType, param);
             br.Success = true;
             return br;
         }

         public override BaseResult Get(Hashtable param = null)
         {
             BaseResult br = new BaseResult();
             br.Data = DAL.GetItem<Tb_Sp_Expand_Template>(tb_Sp_Expand_TemplateType, param);
             br.Success = true;
             return br;
         }   
    }
}

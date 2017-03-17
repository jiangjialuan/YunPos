#region Imports
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Frame.Attributes;
using CySoft.Model.Tb;
using CySoft.BLL.Tools.CodingRule;
using CySoft.Model;

#endregion

#region 商品类别
#endregion

namespace CySoft.BLL.GoodsBLL
{
    public class GoodsTypeBLL : TreeBLL
    {

        protected static readonly Type goodsType = typeof(Tb_Spfl);

        /// <summary>
        /// 获得树结构数据
        /// cxb
        /// 2015-2-12 14:09:33
        /// 修改为批量保存
        /// hjb
        /// 2016-5-19
        /// </summary>
        /// 
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            //IList<Tb_Spfl> list = (List<Tb_Spfl>)entity;
            //foreach (var model in list)
            //{
            //    Hashtable param = new Hashtable();
            //    param.Add("id_gys", model.id_gys);
            //    param.Add("name", model.name);
            //    if (DAL.GetCount(typeof(Tb_Spfl), param) > 0)
            //    {
            //        br.Success = false;
            //        br.Message.Add("商品类别名称重复，请重新命名.");
            //        br.Level = ErrorLevel.Warning;
            //        br.Data = "name";
            //        throw new CySoftException(br);
            //    }

            //    var father = new Tb_Spfl() { id = 0, path = "/0" };
            //    if (!model.fatherId.Equals(0L))
            //    {
            //        param.Clear();
            //        param.Add("id", model.fatherId);
            //        father = DAL.GetItem<Tb_Spfl>(typeof(Tb_Spfl), param);

            //        if (father == null)
            //        {
            //            br.Success = false;
            //            br.Message.Add("商品类别父节点已丢失，请重新选择父节点.");
            //            br.Level = ErrorLevel.Warning;
            //            br.Data = "name";
            //            throw new CySoftException(br);
            //        }
            //    }

            //    model.path = String.Format("{0}/{1}", father.path, model.id);

            //    br = CodingRule.SetBaseCoding(model);
            //    if (!br.Success)
            //    {
            //        throw new CySoftException(br);
            //    }
            //    DAL.Add(model);

            //    br.Message.Add(String.Format("新增商品类别成功。流水号{0}，名称:{1}", model.id, model.name));

            //}
            //br.Data = list.Select(d => new { d.name });
            //br.Success = true;
            return br;
        }

        /// <summary>
        /// 删除
        /// cxb
        /// 2015-2-27 
        /// </summary>
        /// 
        [Transaction]
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult br = new BaseResult();
            long id = Convert.ToInt64(param["id"]);
            long backupId = Convert.ToInt64(param["backupId"]);
            long id_gys = Convert.ToInt64(param["id_gys"]);
            long id_user = Convert.ToInt64(param["id_user"]);

            param.Clear();
            param.Add("fatherId", id);
            if (DAL.QueryList<Tb_Spfl>(goodsType, param).Count > 0)
            {
                br.Success = false;
                br.Message.Add("该商品类别下有子类不可删除,请先删除子类");
                br.Level = ErrorLevel.Warning;
                return br;
            }
            param.Clear();
            param.Add("id_spfl", id);
            if (DAL.GetCount(typeof(Tb_Gys_Sp), param) > 0)
            {
                if (backupId == -1)
                {
                    br.Success = false;
                    br.Message.Add("该类别下已有商品，请重新删除，选择商品转移的类别！");
                    br.Level = ErrorLevel.Warning;
                    return br;
                }

                param.Clear();
                param.Add("id", backupId);
                var node = DAL.GetItem<Tb_Spfl>(typeof(Tb_Spfl), param);

                if (node == null)
                {
                    br.Success = false;
                    br.Message.Add("转移商品的类别不存在");
                    br.Level = ErrorLevel.Warning;
                    return br;
                }

                param.Clear();
                param.Add("fatherId", backupId);
                if (DAL.QueryList<Tb_Spfl>(goodsType, param).Count > 0)
                {
                    br.Success = false;
                    br.Message.Add("转移商品的类别下还有子类，商品不能转移到该类别下，请重新选择");
                    br.Level = ErrorLevel.Warning;
                    return br;
                }

                param.Clear();
                param.Add("id_spfl", id);
                param.Add("id_gys", id_gys);
                param.Add("new_id_spfl", backupId);
                param.Add("new_id_edit", id_user);
                param.Add("new_rq_edit", DateTime.Now);
                DAL.UpdatePart(typeof(Tb_Gys_Sp), param);
            }
            param.Clear();
            param.Add("id", id);
            Tb_Spfl model = DAL.GetItem<Tb_Spfl>(goodsType, param);
            DAL.Delete(goodsType, param);
            br.Message.Add("删除商品类别成功.");
            if (model != null)
            {
                br.Message.Clear();
                //br.Message.Add(String.Format("删除商品类别成功。编号{0}，名称:{1}", model.bm, model.name));
            }
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 修改
        /// cxb
        /// 2015-2-27 
        /// </summary>
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult br = new BaseResult();
            //Tb_Spfl model = (Tb_Spfl)entity;
            //Hashtable param = new Hashtable();

            //param.Add("id", model.id);
            //var node = DAL.GetItem<Tb_Spfl>(typeof(Tb_Spfl), param);
            //if (node == null)
            //{
            //    br.Success = false;
            //    br.Message.Add("所修改商品类别不存在.");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = "id";
            //    return br;
            //}

            //param.Clear();
            //param.Add("not_id", model.id);
            //param.Add("name", model.name);
            //param.Add("id_gys", model.id_gys);
            //if (DAL.GetCount(goodsType, param) > 0)
            //{
            //    br.Success = false;
            //    br.Message.Add("商品类别名称重复，请重新命名.");
            //    br.Level = ErrorLevel.Warning;
            //    br.Data = "name";
            //    return br;
            //}
            //Tb_Spfl fathernode;
            //if (!model.fatherId.Equals(node.fatherId))
            //{
            //    if (model.fatherId.Equals(0L))
            //    {
            //        fathernode = new Tb_Spfl() { id = 0, path = "/0" };
            //    }
            //    else
            //    {
            //        param.Clear();
            //        param.Add("id", model.fatherId);
            //        fathernode = DAL.GetItem<Tb_Spfl>(typeof(Tb_Spfl), param);
            //        if (fathernode.path.IndexOf(node.path) > -1)
            //        {
            //            br.Success = false;
            //            br.Message.Add("商品类别不允许转移到它自己或它的子类别下.");
            //            br.Level = ErrorLevel.Warning;
            //            br.Data = "name";
            //            return br;
            //        }

            //        param.Clear();
            //        param.Add("id_spfl", model.fatherId);
            //        param.Add("id_gys", model.id_gys);
            //        if (DAL.GetCount(typeof(Tb_Gys_Sp), param) > 0)
            //        {
            //            br.Success = false;
            //            br.Message.Add("您选择的上级类别中存在商品资料，请先将该类别中商品转移到其他分类下，再执行上级类别修改.");
            //            br.Level = ErrorLevel.Warning;
            //            br.Data = "name";
            //            return br;
            //        }
            //    }
            //    model.path = String.Format("{0}/{1}", fathernode.path, model.id);
            //    //更新子类路径
            //    param.Clear();
            //    param.Add("id_gys", model.id_gys);
            //    param.Add("path", node.path);
            //    param.Add("old_path_all", node.path + "/");
            //    param.Add("new_path_all", String.Format("{0}/", model.path));
            //    param.Add("new_rq_edit", DateTime.Now);
            //    param.Add("new_id_edit", model.id_edit);
            //    DAL.UpdatePart(typeof(Tb_Spfl), param);
            //}
            //else
            //{
            //    model.path = node.path;
            //    model.fatherId = node.fatherId;
            //}

            //node.path = model.path;
            //node.name = model.name;
            //node.fatherId = model.fatherId;
            //node.id_edit = model.id_edit;
            //node.rq_edit = DateTime.Now;
            //node.id_sp_expand_template = model.id_sp_expand_template;
            //DAL.Update(node);

            //br.Message.Add(String.Format("修改商品类别成功。流水号{0},名称:{1}", model.id, model.name));
            //br.Success = true;
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
            br.Data = DAL.QueryList<Tb_Spfl_Query>(goodsType, param);
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
            bool root = false;
            if (param.ContainsKey("childId") && param["childId"].ToString().Equals("0"))
            {
                param.Remove("childId");
                root = true;
            }
            var list = DAL.QueryTree<Tb_Spfl_Tree>(typeof(Tb_Spfl), param);
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
            //br.Data = DAL.GetItem<Tb_Spfl>(typeof(Tb_Spfl), param);
            //if (br.Data == null)
            //{
            //    br.Success = false;
            //    br.Message.Add("未找到该类别！");
            //    br.Level = ErrorLevel.Warning;
            //    return br;
            //}
            //br.Success = true;
            return br;
        }
    }
}

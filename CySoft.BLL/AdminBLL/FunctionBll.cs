using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.IBLL;
using CySoft.Model.Tb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Model.Enums;

namespace CySoft.BLL.AdminBLL
{
    public class FunctionBll : BaseBLL, IFunctionBll
    {
        /// <summary>
        /// 获取功能树
        /// </summary>
        /// <returns></returns>
        public IList<Tb_Function_Tree> GetFunctionTree(Hashtable param)
        {
            param.Add("flag_stop", 0);
            param.Add("sort", "sort_id");
            param.Add("dir", "asc");

            var list = DAL.QueryList<Tb_Function>(typeof(Tb_Function), param);

            var sourceList = list.Select(d => new Tb_Function_Tree
            {
                id = d.id,
                id_father = d.id_father,
                flag_stop = d.flag_stop,
                flag_type = d.flag_type,
                id_create = d.id_create,
                id_edit = d.id_edit,
                name = d.name,
                path = d.path,
                rq_create = d.rq_create,
                rq_edit = d.rq_edit,
                sort_id = d.sort_id,
                version = d.version,
                isclose = true,
                controller_name = d.controller_name,
                action_name = d.action_name,
                icon = d.icon,
                tag_name = d.tag_name
            }).ToList();

            var targetList = sourceList.Where(d => d.id_father == "0").OrderBy(a=>a.sort_id).ToList();

            var root = new Tb_Function_Tree
            {
                id = "0",
                flag_type = "module",
                path = "0",
                name = "根目录",
                children = targetList,
                isclose=false
            };

            foreach (var item in targetList)
            {
                BuildTree(item, sourceList);
            }

            var rootList = new List<Tb_Function_Tree>();
            rootList.Add(root);

            return rootList;
        }



        private void BuildTree(Tb_Function_Tree root, IList<Tb_Function_Tree> list)
        {
            if (root == null)
            {
                return;
            }

            var source = list;
            if (source == null)
            {
                return;
            }

            var children = source.Where(d => d.id_father == root.id);

            if (children == null)
            {
                return;
            }

            root.children = children.ToList();

            foreach (var item in children)
            {
                BuildTree(item, source);
            }
        }

        /// <summary>
        /// 新增功能
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            var br = new BaseResult();

            var model = (Tb_Function)entity;

            if (string.IsNullOrEmpty(model.id_father))
            {
                br.Message.Add("父级id不能为空！");
                br.Success = false;
                return br;
            }

            if (string.IsNullOrEmpty(model.name))
            {
                br.Message.Add("功能名称不能为空！");
                br.Success = false;
                return br;
            }

            model.id = Guid.NewGuid().ToString();

            if (model.id_father == "0")
            {
                model.path = string.Format("/0/{0}", model.id);
            }
            else
            {
                var param = new Hashtable();
                param.Add("flag_stop", 0);
                var list = DAL.QueryList<Tb_Function>(typeof(Tb_Function), param);
                model.path = string.Format("{0}/{1}", BuildPath(model.id_father, list), model.id);
            }

            model.flag_stop = 0;

            DAL.Add(model);
            br.Success = true;
            br.Message.Add("添加成功！");
            return br;
        }

        /// <summary>
        /// 获取路径
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fatherId"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private string BuildPath(string fatherId, IList<Tb_Function> source)
        {
            if (string.IsNullOrEmpty(fatherId))
            {
                return string.Format("/{0}", fatherId);
            }

            var item = source.FirstOrDefault(d => d.id == fatherId);
            if (item != null)
            {
                return string.Format("{0}/{1}", BuildPath(item.id_father, source), fatherId);
            }
            else
            {
                return string.Format("/{0}", fatherId);
            }
        }

        /// <summary>
        /// 删除该级别及子级节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult Delete(Hashtable param)
        {
            var br = new BaseResult();

            string deleteType = "soft";
            string id = "0";

            if (!param.ContainsKey("id"))
            {
                br.Message.Add("删除的节点id不能为空！");
                br.Success = false;
                return br;
            }

            id = param["id"].ToString();

            if (id == "0")
            {
                br.Message.Add("根节点不能直接删除！");
                br.Success = false;
                return br;
            }

            var idList = new List<string>();
            idList.Add(id);
            //获取要删除的 id 的子节点 集合
            var list = DAL.QueryList<Tb_Function>(typeof(Tb_Function), null);

            BuildIdList(id, idList, list);

            if (!(idList.Count > 0))
            {
                br.Message.Add("删除的节点id不能为空！");
                br.Success = false;
                return br;
            }

            if (param.ContainsKey("deleteType"))
            {
                deleteType = param["deleteType"].ToString();
            }

            param.Clear();
            param.Add("idList", idList);

            if (deleteType == "hard")
            {
                DAL.Delete(typeof(Tb_Function), param);

                //删除tb_role_function相关功能数据
                param.Clear();
                param.Add("id_functionList", idList);
                DAL.Delete(typeof(Tb_Role_Function), param);
            }
            else
            {
                param.Add("new_flag_stop", 1);
                DAL.UpdatePart(typeof(Tb_Function), param);
            }

            br.Message.Add("删除成功！");
            br.Success = true;
            return br;
        }

        private void BuildIdList(string rootId, IList<string> target, IList<Tb_Function> source)
        {
            var childList = source.Where(d => d.id_father == rootId).ToList();

            if (childList == null || childList.Count <= 0)
            {
                return;
            }

            foreach (var item in childList)
            {
                target.Add(item.id);
                BuildIdList(item.id, target, source);
            }
        }

        public override BaseResult Update(dynamic entity)
        {
            var model = (Tb_Function)entity;

            var br = new BaseResult();
            var param = new Hashtable();

            if (model.id == "0" || string.IsNullOrEmpty(model.id))
            {
                br.Message.Add("不能修改根节点！");
                br.Success = false;
                return br;
            }

            param.Add("id",model.id);
            param.Add("new_name",model.name);
            param.Add("new_sort_id", model.sort_id);
            param.Add("new_controller_name", model.controller_name);
            param.Add("new_action_name", model.action_name);
            param.Add("new_id_edit", model.id_edit);
            param.Add("new_flag_type", model.flag_type);
            param.Add("new_rq_edit", DateTime.Now);
            param.Add("new_icon", model.icon);
            param.Add("new_tag_name", model.tag_name);
            param.Add("new_version", model.version);
            DAL.UpdatePart(typeof(Tb_Function), param);
            br.Message.Add("修改成功！");
            br.Success = true;
            return br;
        }

        public override BaseResult Get(Hashtable param)
        {
            var res = new BaseResult(){Success = true};
            res.Data= DAL.QueryItem(typeof(Tb_Function), param);
            return res;
        }

        public override BaseResult GetAll(Hashtable param)
        {
            BaseResult res=new BaseResult(){Success = true};
            if (param==null)
            {
                return res;
            }
            res.Data= DAL.QueryList<Tb_Function>(typeof(Tb_Function), param).ToList();
            return res;
        }

        [Transaction]
        public BaseResult MoveNode(Hashtable param)
        {
            BaseResult res=new BaseResult(){Success = true};
            if (param == null || !param.Contains("id") || !param.Contains("id_father"))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            var id = string.Format("{0}", param["id"]);
            var id_father = string.Format("{0}", param["id_father"]);
            var id_like = param["id_like"];
            if (string.IsNullOrEmpty(id)||string.IsNullOrEmpty(id_father))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            if (id_father!="0")
            {
                param.Clear();
                param.Add("like_path", id_like);
                param.Add("flag_stop", (int)Enums.FlagStop.Start);
                var ListNodes = DAL.QueryList<Tb_Function>(typeof(Tb_Function), param).ToList();
                if (ListNodes.Any())
                {
                    if (!ListNodes.All(a => a.path.Contains(id)))
                    {
                        res.Success = false;
                        res.Message.Add("数据异常!");
                        return res;
                    }
                    #region 
                    param.Clear();
                    param.Add("id",id_father);
                    param.Add("flag_stop", (int)Enums.FlagStop.Start);
                    var fatherModel = DAL.QueryItem<Tb_Function>(typeof(Tb_Function), param);
                    var currentModel = ListNodes.FirstOrDefault(d => d.id == id);
                    if (fatherModel == null)
                    {
                        res.Success = false;
                        res.Message.Add("上级节点已不存在!");
                        return res;
                    }
                    if (currentModel == null)
                    {
                        res.Success = false;
                        res.Message.Add("选中节点已不存在!");
                        return res;
                    }
                    if (
                           (currentModel.flag_type.ToLower() == "action" && fatherModel.flag_type.ToLower() != "controller")
                        || (currentModel.flag_type.ToLower() == "controller" && fatherModel.flag_type.ToLower() != "module")
                        || (currentModel.flag_type.ToLower() == "module" && fatherModel.flag_type.ToLower() != "module")
                        )
                    {
                        res.Success = false;
                        res.Message.Add("不能移动到当前父节点下!");
                        return res;
                    }

                    currentModel.id_father = id_father;
                    var oldPath = currentModel.path;
                    currentModel.path = string.Format("{0}/{1}", fatherModel.path, currentModel.id);
                    ListNodes.Remove(currentModel);
                    ListNodes.Remove(fatherModel);
                    if (ListNodes.Any())
                    {
                        ListNodes.ForEach(d =>
                        {
                            d.path = d.path.Replace(oldPath, currentModel.path);
                        });
                    }
                    param.Clear();
                    param.Add("like_path", id_like);
                    DAL.Delete(typeof(Tb_Function), param);
                    ListNodes.Add(currentModel);
                    DAL.AddRange(ListNodes); 
                    #endregion
                }
            }
            else if (id_father == "0")
            {
                #region

                param.Clear();
                param.Add("like_path", id_like);
                param.Add("flag_stop", (int)Enums.FlagStop.Start);
                var ListNodes = DAL.QueryList<Tb_Function>(typeof(Tb_Function), param).ToList();
                if (ListNodes.Any())
                {
                    if (!ListNodes.All(a => a.path.Contains(id)))
                    {
                        res.Success = false;
                        res.Message.Add("数据异常!");
                        return res;
                    }
                }
                var currentModel = ListNodes.FirstOrDefault(d => d.id == id);
                if (currentModel == null)
                {
                    res.Success = false;
                    res.Message.Add("选中节点已不存在!");
                    return res;
                }
                if (currentModel.flag_type.ToLower() != "module")
                {
                    res.Success = false;
                    res.Message.Add("不能移动到当前父节点下!");
                    return res;
                }
                currentModel.id_father = id_father;
                var oldPath = currentModel.path;
                currentModel.path = string.Format("{0}/{1}", "/0", currentModel.id);
                ListNodes.Remove(currentModel);
                if (ListNodes.Any())
                {
                    ListNodes.ForEach(d =>
                    {
                        d.path = d.path.Replace(oldPath, currentModel.path);
                    });
                }
                param.Clear();
                param.Add("like_path", id_like);
                DAL.Delete(typeof(Tb_Function), param);
                ListNodes.Add(currentModel);
                DAL.AddRange(ListNodes);

                #endregion
            }
            else
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            return res;
        }
    }
}

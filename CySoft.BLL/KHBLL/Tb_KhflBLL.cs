using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Common;
using CySoft.Frame.Core;
using CySoft.IBLL;
using CySoft.IDAL;
using CySoft.Model;
using CySoft.Model.Enums;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Utility;

namespace CySoft.BLL.KHBLL
{
    public class Tb_KhflBLL  : TreeBLL, ITb_KhflBLL
    {
        public ITb_KhflDAL Tb_KhflDAL { get; set; }


        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult result = new BaseResult() { Success = true };
            try
            {
                var id_masteruser = param["id_masteruser"].ToString();
                if (string.IsNullOrEmpty(id_masteruser))
                {
                    result.Success = false;
                    result.Message.Add("参数有误!");
                    return result;
                }
                
                if (!param.ContainsKey("flag_delete"))
                    param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);

                result.Data = DAL.QueryList<Tb_Khfl>(typeof(Tb_Khfl), param);
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message.Add("查询操作异常!");
            }
            return result;
        }

        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            try
            {
                param["flag_delete"] = (int)Enums.FlagDelete.NoDelete;
                br.Data = DAL.GetItem<Tb_Khfl>(typeof(Tb_Khfl), param);
                br.Success = true;
            }
            catch (Exception ex)
            {
                br.Message.Add("操作异常!");
            }
            return br;
        }

        public override BaseResult Add(dynamic entity)
        {
            BaseResult result = new BaseResult() { Success = true };
            var model = entity as Tb_Khfl;
            if (model == null)
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            Hashtable ht = new Hashtable();
            ht.Add("id_masteruser", model.id_masteruser);
            ht.Add("id_farther", model.id_farther);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            ht.Add("mc", model.mc);
            if (CheckMC(ht))
            {
                result.Success = false;
                result.Message.Add("客户分类名称已存在!");
                return result;
            }
            try
            {
                model.id = Guid.NewGuid().ToString();
                model.flag_delete = (int)Enums.FlagDelete.NoDelete;
                if (string.IsNullOrEmpty(model.id_farther) || model.id_farther == "0")
                {
                    model.id_farther = "0";
                    model.path = string.Format("/0/{0}", model.id);
                }
                else
                {
                    ht.Clear();
                    ht.Add("id_masteruser", model.id_masteruser);
                    ht.Add("id", model.id_farther);
                    var item = DAL.GetItem<Tb_Khfl>(typeof(Tb_Khfl), ht);
                    if (item == null)
                    {
                        result.Success = false;
                        result.Message.Add("选中的父节点已不存在!");
                        return result;
                    }
                    model.path = item.path + "/" + model.id;
                }
                model.rq_create = model.rq_edit = DateTime.Now;
                DAL.Add<Tb_Khfl>(model);
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);
                result.Success = false;
                result.Message.Add(ex.Message);
            }
            return result;
        }

        public override BaseResult Update(dynamic entity)
        {
            BaseResult result = new BaseResult() { Success = true };
            var model = entity as Tb_Khfl;
            if (model == null)
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            Hashtable htcm = new Hashtable();
            htcm.Add("id_masteruser", model.id_masteruser);
            htcm.Add("id", model.id);
            var changeModel = DAL.GetItem<Tb_Khfl>(typeof(Tb_Khfl), htcm);
            if (changeModel == null)
            {
                result.Success = false;
                result.Message.Add("此分类已不存在!");
                return result;
            }
            htcm.Clear();
            htcm.Add("id_masteruser", changeModel.id_masteruser);
            htcm.Add("id_farther", changeModel.id_farther);
            htcm.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            htcm.Add("mc", model.mc);
            htcm.Add("not_id", model.id);
            if (CheckMC(htcm))
            {
                result.Success = false;
                result.Message.Add("客户分类名称已存在!");
                return result;
            }
            try
            {
                htcm.Clear();
                htcm.Add("new_rq_edit", DateTime.Now);
                htcm.Add("id_masteruser", model.id_masteruser);
                htcm.Add("new_id_edit", model.id_edit);
                htcm.Add("new_bm", model.bm);
                htcm.Add("new_mc", model.mc);
                htcm.Add("new_sort_id", model.sort_id);
                htcm.Add("id", model.id);
                var count = DAL.UpdatePart(typeof(Tb_Khfl), htcm);
                if (count <= 0)
                {
                    result.Success = false;
                    result.Message.Add("更新操作失败!");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message.Add("更新操作异常!");
            }
            return result;
        }

        public override BaseResult Delete(Hashtable param)
        {
            BaseResult result = new BaseResult() { Success = true };
            if (param == null || param.Keys.Count < 2)
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            var id = param["id"].ToString();
            var id_masteruser = param["id_masteruser"].ToString();
            Hashtable ht = new Hashtable();

            #region 验证此客户分类是否存在客户

            ht.Clear();
            ht.Add("id_gysfl", id);
            ht.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            int gys_count = DAL.GetCount(typeof(Tb_Gys), ht);

            if (gys_count > 0)
            {
                result.Success = false;
                result.Message.Add("此客户分类存在客户,不允许删除!");
                return result;
            }

            #endregion



            ht.Clear();
            ht.Add("id_farther", id);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            if (DAL.GetCount(typeof(Tb_Khfl), ht) <= 0)
            {
                
                ht.Clear();
                ht.Add("new_rq_edit", DateTime.Now);
                ht.Add("id_masteruser", id_masteruser);
                ht.Add("new_id_edit", id_masteruser);
                ht.Add("new_flag_delete", (int)Enums.FlagDelete.Deleted);
                ht.Add("id", id);
                var count = DAL.UpdatePart(typeof(Tb_Khfl), ht);
                if (count <= 0)
                {
                    result.Success = false;
                    result.Message.Add("删除操作失败!");
                }
                
            }
            else
            {
                result.Success = false;
                result.Message.Add("请先删除子分类!");
            }
            return result;
        }
        /// <summary>
        /// 检查同级分类名称
        /// </summary>
        /// <param name="ht">id_cyuser：当前用户ID id_farther所属父节点ID flag_delete：未删除标识 mc:分类名称</param>
        /// <returns></returns>
        private bool CheckMC(Hashtable ht)
        {
            return DAL.GetCount(typeof(Tb_Khfl), ht) > 0;
        }
        /// <summary>
        /// 移动节点
        /// </summary>
        /// <param name="param">id 节点ID id_cyuser：当前用户ID id_farther：新的父节点ID</param>
        /// <returns></returns>
        [Transaction]
        public BaseResult MoveNode(Hashtable param)
        {
            BaseResult result = new BaseResult() { Success = true };
            if (param == null || param.Keys.Count <= 0)
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            var id = param["id"].ToString();
            var id_cyuser = param["id_cyuser"].ToString();
            var id_farther = param["id_farther"].ToString();
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(id_cyuser) || string.IsNullOrWhiteSpace(id_farther))
            {
                result.Success = false;
                result.Message.Add("参数有误!");
                return result;
            }
            Hashtable ht = new Hashtable();
            ht.Add("id", id);
            ht.Add("id_cyuser", id_cyuser);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            try
            {
                var nodeModel = DAL.GetItem<Tb_Khfl>(typeof(Tb_Khfl), ht);
                if (nodeModel != null)
                {
                    ht["id"] = id_farther;
                    Hashtable checkMc = new Hashtable();
                    checkMc.Add("id_masteruser", nodeModel.id_masteruser);
                    checkMc.Add("id_farther", id_farther);
                    checkMc.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                    checkMc.Add("mc", nodeModel.mc);
                    if (CheckMC(checkMc))
                    {
                        result.Success = false;
                        result.Message.Add("同级中有相同商品分类名称，不能移动!");
                        return result;
                    }
                    var oldSubList = Tb_KhflDAL.QuerySubListByPath(id_cyuser, nodeModel.path);
                    var newFartherNode = DAL.GetItem<Tb_Khfl>(typeof(Tb_Khfl), ht);
                    var oldNodeModelPath = nodeModel.path;
                    var nd = DateTime.Now;
                    if (id_farther == "0")
                    {
                        nodeModel.path = nodeModel.id;
                        ChangeNodePath(oldSubList, nodeModel, oldNodeModelPath);
                    }
                    else
                    {
                        if (newFartherNode != null)
                        {
                            nodeModel.path = newFartherNode.path + "/" + nodeModel.id;
                            ChangeNodePath(oldSubList, nodeModel, oldNodeModelPath);
                        }
                    }
                    Hashtable nodeHt = new Hashtable();
                    nodeHt.Add("id", nodeModel.id);
                    nodeHt.Add("id_masteruser", nodeModel.id_masteruser);
                    nodeHt.Add("new_path", nodeModel.path);
                    nodeHt.Add("new_id_edit", nodeModel.id_edit);
                    nodeHt.Add("new_rq_edit", nd);
                    nodeHt.Add("new_id_farther", id_farther);
                    DAL.UpdatePart(typeof(Tb_Khfl), nodeHt);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message.Add("称动节点异常");
            }
            return result;
        }
        /// <summary>
        /// 修改节点Path方法
        /// </summary>
        /// <param name="oldSubList">原子节点列表</param>
        /// <param name="nodeModel">节点</param>
        /// <param name="oldNodeModelPath">节点原Path</param>
        private void ChangeNodePath(IList<Tb_Khfl> oldSubList, Tb_Khfl nodeModel, string oldNodeModelPath)
        {
            var nd = DateTime.Now;
            if (oldSubList.Any())
            {
                oldSubList.ToList().ForEach(n =>
                {
                    if (n.id != nodeModel.id)
                    {
                        #region
                        n.path = n.path.Replace(oldNodeModelPath, nodeModel.path);
                        Hashtable subHt = new Hashtable();
                        subHt.Add("id", n.id);
                        subHt.Add("id_masteruser", n.id_masteruser);
                        subHt.Add("new_path", n.path);
                        subHt.Add("new_id_edit", n.id_edit);
                        subHt.Add("new_rq_edit", nd);
                        DAL.UpdatePart(typeof(Tb_Khfl), subHt);
                        #endregion
                    }
                });
            }
        }

        /// <summary>
        /// 根据节点ID获取树
        /// </summary>
        /// <param name="param">id_cyuser：用户ID childId：当前节点ID </param>
        /// <returns></returns>
        public override BaseResult GetTree(Hashtable param)
        {
            BaseResult result = new BaseResult() { Success = true };
            string str_id_farther = "0";
            if (!param.ContainsKey("id_masteruser") || string.IsNullOrWhiteSpace(param["id_masteruser"].ToString()))
            {
                result.Success = false;
                result.Message.Add("请先登录用户!");
                return result;
            }
            if (param.ContainsKey("childId") && param["childId"].ToString().Equals("0"))
            {
                param.Remove("childId");
            }
            else
            {
                str_id_farther = param["childId"].ToString();
                Hashtable ht = new Hashtable();
                ht.Add("id", str_id_farther);
                ht.Add("id_masteruser", param["id_masteruser"].ToString());
                ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                var item = DAL.GetItem<Tb_Khfl>(typeof(Tb_Khfl), ht);
                if (item != null)
                {
                    str_id_farther = item.id_farther;
                }
            }
            param.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            var list = DAL.QueryTree<Tb_Khfl_Tree>(typeof(Tb_Khfl), param);
            result.Data = base.CreateTree(list, str_id_farther);
            result.Success = true;
            return result;
        }


        public BaseResult CheckCanDeleteNode(Hashtable param)
        {
            BaseResult res = new BaseResult() { Success = true };
            var id = param["id"].ToString();
            var id_masteruser = param["id_masteruser"].ToString();
            Hashtable ht = new Hashtable();
            ht.Add("id_farther", id);
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            if (DAL.GetCount(typeof(Tb_Khfl), ht) > 0)
            {
                res.Success = false;
                res.Message.Add("此商品分类有子分类存在，不能删除!");
                return res;
            }
            ht.Clear();
            ht.Add("id_masteruser", id_masteruser);
            ht.Add("id_spfl", id);
            ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
            if (DAL.GetCount(typeof(Tb_Shopsp), ht) > 0)
            {
                res.Success = false;
                res.Message.Add("有商品引用此分类，不能删除!");
                return res;
            }
            return res;
        }

        [Transaction]
        public BaseResult UpdateTree(dynamic entity, string id_masteruser)
        {
            BaseResult res = new BaseResult() { Success = true };
            var nodeTree = entity as List<SpflUpdateTreeModel>;
            if (string.IsNullOrEmpty(id_masteruser))
            {
                res.Success = false;
                res.Message.Add("登录超时，请重新登录!");
                return res;
            }
            if (nodeTree != null && nodeTree.Any())
            {
                Hashtable param = new Hashtable();
                param.Add("id_masteruser", id_masteruser);
                List<Tb_Khfl> gysflList = DAL.QueryList<Tb_Khfl>(typeof(Tb_Khfl), param).ToList();
                List<Tb_Khfl> newList = new List<Tb_Khfl>();
                for (int i = 0; i < nodeTree.Count; i++)
                {
                    var n = nodeTree[i];
                    n.sortNum = i + 10;
                    if (n != null)
                    {
                        n.fartherPath = "/0";
                        n.id_fahter = "0";
                        HandleTreeNode(n, gysflList, newList, res);
                    }
                }
                CheckName(nodeTree, gysflList, res);
                {
                    if (!res.Success)
                        return res;
                }
                DAL.Delete(typeof(Tb_Khfl), param);
                DAL.AddRange(newList);
            }
            return res;
        }

        private void HandleTreeNode(SpflUpdateTreeModel node, List<Tb_Khfl> gysflList, List<Tb_Khfl> newList, BaseResult res)
        {
            if (node != null)
            {
                Tb_Khfl gysflModel = gysflList.FirstOrDefault(s => s.id == node.id);
                gysflModel = gysflModel ?? new Tb_Khfl();
                gysflModel.id = node.id;
                gysflModel.id_farther = node.id_fahter;
                gysflModel.sort_id = node.sortNum;
                gysflModel.path = node.fartherPath + "/" + node.id;
                newList.Add(gysflModel);
                if (node.children != null && node.children.Any())
                {
                    for (int i = 0; i < node.children.Count; i++)
                    {
                        var sn = node.children[i];
                        sn.fartherPath = gysflModel.path;
                        sn.sortNum = i + 10;
                        sn.id_fahter = gysflModel.id;
                        HandleTreeNode(sn, gysflList, newList, res);
                    }
                    CheckName(node.children, gysflList, res);
                }
            }
        }

        private bool CheckName(List<SpflUpdateTreeModel> nodes, List<Tb_Khfl> gysflList, BaseResult res)
        {
            if (nodes != null && nodes.Any() && gysflList != null && gysflList.Any())
            {
                var mcList = from s in gysflList where nodes.Any(n => n.id == s.id) select s.mc;
                var enumerable = mcList as string[] ?? mcList.ToArray();
                var count = enumerable.Count();
                var newCount = enumerable.Distinct().Count();
                if (count > newCount)
                {
                    res.Success = false;
                    res.Message.Add(string.Join(",", enumerable) + " 这此节点同级中有同名节点，请处理!");
                    return false;
                }
            }
            return true;
        }



        #region 客户分类
        /// <summary>
        /// 客户分类导入
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public BaseResult ImportIn(Hashtable param)
        {
            #region 获取数据
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            string FilePath = param["filePath"].ToString();
            string id_masteruser = param["id_masteruser"].ToString();
            string id_user = param["id_user"].ToString();
            List<Tb_Khfl_Import> list = (List<Tb_Khfl_Import>)param["list"];
            List<Tb_Khfl_Import> successList = new List<Tb_Khfl_Import>();
            List<Tb_Khfl_Import> failList = new List<Tb_Khfl_Import>();

            List<Tb_Khfl> khflList = new List<Tb_Khfl>();

            #endregion

            if (list.Any())
            {
                GetSpflImportTree(list, khflList, failList, id_user, id_masteruser);
            }

            if (khflList.Any())
            {
                DAL.AddRange(khflList);
            }

            foreach (var item in khflList)
            {
                var suc = list.Where(d => d.mc == item.mc).FirstOrDefault();
                successList.Add(suc);
            }

            br.Message.Add("导入分类成功!");
            br.Success = true;
            br.Data = new Tb_Khfl_Import_All() { SuccessList = successList, FailList = failList };
            return br;
        }
        public BaseResult CheckImportInfo(Tb_Khfl_Import model, List<Tb_Khfl> dbList)
        {
            BaseResult br = new BaseResult();
            if (string.IsNullOrEmpty(model.mc))
            {
                br.Success = false;
                br.Data = "mc";
                br.Message.Add("名称不能为空");
                return br;
            }
            //if (string.IsNullOrEmpty(model.bm))
            //{
            //    br.Success = false;
            //    br.Data = "bm";
            //    br.Message.Add("编码不能为空");
            //    return br;
            //}

            if (dbList != null)
            {
                if (dbList.Where(d => d.mc == model.mc && d.id_farther == "0").Count() > 0)
                {
                    br.Success = false;
                    br.Data = "mc";
                    br.Message.Add("名称重复");
                    return br;
                }
                //else if (dbList.Where(d => d.bm == model.bm && d.id_farther == "0").Count() > 0)
                //{
                //    br.Success = false;
                //    br.Data = "bm";
                //    br.Message.Add("编码重复");
                //    return br;
                //}
                else
                {
                    var st = new Tb_Khfl();
                    st.bm = model.bm;
                    st.mc = model.mc;
                    st.id = Guid.NewGuid().ToString();
                    st.path = "/0/" + st.id;
                    st.id_farther = "0";
                    st.flag_delete = (byte)Enums.FlagDelete.NoDelete;
                    st.rq_edit = st.rq_create = DateTime.Now;
                    st.id_edit = st.id_create = "";
                    st.id_masteruser = "";
                    dbList.Add(st);
                }
            }


            br.Success = true;
            return br;
        }


        public void GetSpflImportTree(List<Tb_Khfl_Import> list, List<Tb_Khfl> khflList, List<Tb_Khfl_Import> fiallist, string id_user, string id_masteruser)
        {
            if (list.Any())
            {

                Hashtable ht = new Hashtable();
                ht.Add("id_masteruser", id_masteruser);
                ht.Add("id_farther", "0");
                ht.Add("flag_delete", (int)Enums.FlagDelete.NoDelete);
                var dbList = DAL.QueryList<Tb_Khfl>(typeof(Tb_Khfl), ht);

                var firstList = list.Where(l => string.IsNullOrWhiteSpace(l.father)).ToList();
                firstList.ForEach(fl =>
                {
                    var res = CheckImportInfo(fl, dbList.ToList());
                    if (!res.Success)
                    {
                        fl.bz = res.Message[0] ?? "数据不合要求!";
                        fiallist.Add(fl);
                    }
                    else
                    {
                        if (firstList.Where(d => d.mc == fl.mc).Count() > 1)
                        {
                            fl.bz = "操作失败 导入数据存在同名分类!";
                            fiallist.Add(fl);
                        }
                        else
                        {
                            Tb_Khfl st = new Tb_Khfl();
                            st.bm = fl.bm;
                            st.mc = fl.mc;
                            st.id = Guid.NewGuid().ToString();
                            st.path = "/0/" + st.id;
                            st.id_farther = "0";
                            st.flag_delete = (byte)Enums.FlagDelete.NoDelete;
                            st.rq_edit = st.rq_create = DateTime.Now;
                            st.id_edit = st.id_create = id_user;
                            st.id_masteruser = id_masteruser;
                            CreatSubTree(st, list, khflList, fiallist, id_user, id_masteruser, dbList.ToList());
                            var model = khflList.FirstOrDefault(sl => sl.mc == st.mc);
                            if (model != null && model.id_farther == st.id_farther)
                            {
                                fl.bz = "同一级中已有同名分类";
                                fiallist.Add(fl);
                            }
                            else
                            {
                                khflList.Add(st);
                            }
                        }
                    }
                });

                foreach (var item in list.Where(d => !(from s in khflList select s.mc).Contains(d.mc) && string.IsNullOrEmpty(d.bz)))
                {
                    item.bz = "导入文件中无此 上级名称 的根目录数据 或上级名称 根目录不符合要求";
                    fiallist.Add(item);
                }

            }
        }

        public void CreatSubTree(Tb_Khfl st, List<Tb_Khfl_Import> list, List<Tb_Khfl> khflList, List<Tb_Khfl_Import> fiallist, string id_user, string id_masteruser, List<Tb_Khfl> dbList)
        {
            if (st != null)
            {
                var sublist = FindSubSpflByMC(st.mc, list);
                if (sublist.Any())
                {
                    sublist.ForEach(fl =>
                    {
                        var res = CheckImportInfo(fl, dbList);
                        if (!res.Success)
                        {
                            fl.bz = res.Message[0] ?? "数据不合要求!";
                            fiallist.Add(fl);
                        }
                        else
                        {
                            Tb_Khfl sub = new Tb_Khfl();
                            sub.bm = fl.bm;
                            sub.mc = fl.mc;
                            sub.id = Guid.NewGuid().ToString();
                            sub.path = st.path + "/" + st.id;
                            sub.id_farther = st.id;
                            sub.flag_delete = (byte)Enums.FlagDelete.NoDelete;
                            sub.rq_edit = st.rq_create = DateTime.Now;
                            sub.id_edit = st.id_create = id_user;
                            sub.id_masteruser = id_masteruser;
                            CreatSubTree(sub, list, khflList, fiallist, id_user, id_masteruser, dbList);
                            var model = khflList.FirstOrDefault(sl => sl.mc == sub.mc);
                            if (model != null && model.id_farther == sub.id_farther)
                            {
                                fl.bz = "同一级中已有同名分类";
                                fiallist.Add(fl);
                            }
                            else
                            {
                                khflList.Add(sub);
                            }
                        }
                    });
                }
            }
        }
        public List<Tb_Khfl_Import> FindSubSpflByMC(string mc, List<Tb_Khfl_Import> list)
        {
            if (string.IsNullOrEmpty(mc))
            {
                return null;
            }
            return list.Where(l => l.father == mc).ToList();
        }
        #endregion




    }
}

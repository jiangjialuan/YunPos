﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.IBLL;
using CySoft.Model.Tb;

namespace CySoft.BLL.AdminBLL
{
    public class Tb_Pos_Role_ModuleBLL : BaseBLL, ITb_Pos_Role_ModuleBLL
    {
        public IList<Tb_Pos_Role_Module_Tree> GetRoleModuleTree(Hashtable param)
        {
            if (!param.ContainsKey("id_platform_role"))
            {
                //默认为供应商
                param.Add("id_platform_role", 0);
            }

            param.Add("sort", "sort_id");
            param.Add("dir", "asc");

            var sourceList = DAL.QueryList<Tb_Pos_Role_Module_Tree>(typeof(Tb_Pos_Role_Module), param);

            //var targetList = sourceList.Where(d => d.id_module_fatherid == 0).ToList();

            var root = new Tb_Pos_Role_Module_Tree
            {
                id = 0,
                id_module = 0,
                name = "根目录",
                children = sourceList,
                isclose = false
            };

            //foreach (var item in sourceList)
            //{
            //    BuildTree(item, sourceList);
            //}

            var rootList = new List<Tb_Pos_Role_Module_Tree>();
            rootList.Add(root);

            return rootList;
        }


        private void BuildTree(Tb_Pos_Role_Module_Tree root, IList<Tb_Pos_Role_Module_Tree> list)
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

            var children = source.Where(d => d.id_module_fatherid == root.id_module);

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

            var model = (Tb_Pos_Role_Module)entity;
            var param = new Hashtable();

            //获取最大id_module值+1
            model.id_module = DAL.GetNextKey<int>(typeof(Tb_Pos_Role_Module));

            if (string.IsNullOrEmpty(model.name))
            {
                br.Message.Add("模块名称不能为空！");
                br.Success = false;
                return br;
            }

            DAL.Add(model);
            br.Success = true;
            br.Message.Add("添加成功！");
            return br;
        }

        /// <summary>
        /// 删除该级别及子级节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult Delete(Hashtable param)
        {
            var br = new BaseResult();

            int id = 0;

            if (!param.ContainsKey("id"))
            {
                br.Message.Add("删除的节点id不能为空！");
                br.Success = false;
                return br;
            }

            id = Convert.ToInt32(param["id"]);

            if (id == 0)
            {
                br.Message.Add("根节点不能直接删除！");
                br.Success = false;
                return br;
            }

            param.Clear();
            param.Add("id", id);
            //获取对应的模块信息
            var info = DAL.GetItem<Tb_Pos_Role_Module>(typeof(Tb_Pos_Role_Module), param);

            var idList = new List<long?>();
            idList.Add(id);
            //获取要删除的 id 的子节点 集合
            param.Clear();
            param.Add("id_platform_role", info.id_platform_role);
            var list = DAL.QueryList<Tb_Pos_Role_Module>(typeof(Tb_Pos_Role_Module), param);

            BuildIdList(info.id_module, idList, list);

            if (!(idList.Count > 0))
            {
                br.Message.Add("删除的节点id不能为空！");
                br.Success = false;
                return br;
            }

            param.Clear();
            param.Add("idList", idList);
            DAL.Delete(typeof(Tb_Pos_Role_Module), param);

            br.Message.Add("删除成功！");
            br.Success = true;
            return br;
        }

        private void BuildIdList(int? rootId, IList<long?> target, IList<Tb_Pos_Role_Module> source)
        {
            var childList = source.Where(d => d.id_module_fatherid == rootId).ToList();

            if (childList == null || childList.Count <= 0)
            {
                return;
            }

            foreach (var item in childList)
            {
                target.Add(item.id);
                BuildIdList(item.id_module, target, source);
            }
        }

        public override BaseResult Update(dynamic entity)
        {
            var model = (Tb_Pos_Role_Module)entity;

            var br = new BaseResult();
            var param = new Hashtable();

            if (model.id == 0)
            {
                br.Message.Add("不能修改根节点！");
                br.Success = false;
                return br;
            }

            param.Add("id", model.id);
            param.Add("new_name", model.name);
            param.Add("new_sort_id", model.sort_id);
            param.Add("new_id_pos_function", model.id_pos_function);

            DAL.UpdatePart(typeof(Tb_Pos_Role_Module), param);
            br.Message.Add("修改成功！");
            br.Success = true;
            return br;
        }


    }
}

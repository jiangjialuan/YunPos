using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Model.Enums;
using CySoft.Model.Tb;

namespace CySoft.Controllers.AdminCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class RoleModuleController : BaseController
    {
        [ActionPurview(true)]
        public ActionResult List(int? id)
        {
            try
            {
                var _param = base.GetParameters();
                var param = new Hashtable();

                if (id == null)
                {
                    id = -1;
                }
                IList<Tb_Role_Module_Tree> list = new List<Tb_Role_Module_Tree>();

                //if (!Enum.IsDefined(typeof(RoleFlag), id))
                //{
                //    return View(list);
                //}

                param.Add("id_platform_role", id);
                list = BusinessFactory.RoleModule.GetRoleModuleTree(param);
                ViewData["list"] = list;
                ViewData["id_platform_role"] = id;
                param.Clear();
                ViewData["FunList"] = BusinessFactory.Function.GetFunctionTree(param);
                if (_param["_search_"] != null && _param["_search_"].ToString() == "1")
                {
                    return PartialView("_List");
                }
                return View();
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ActionPurview(true)]
        public ActionResult AddChild(Tb_Role_Module info)
        {
            try
            {
                var result = new BaseResult();

                var id_user = GetLoginInfo<string>("id_user");
                info.id_create = id_user;
                info.id_edit = id_user;
                info.rq_create = DateTime.Now;
                info.rq_edit = DateTime.Now;

                result = BusinessFactory.RoleModule.Add(info);

                return Json(result);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ActionPurview(true)]
        public ActionResult Delete(string id)
        {
            try
            {
                var br = new BaseResult();
                var param = new Hashtable();
                param.Add("id", id);
                if (id == "0"||string.IsNullOrEmpty(id))
                {
                    br.Message.Add("根节点不能直接删除！");
                    br.Success = false;
                    return JsonString(br,1);
                }

                br = BusinessFactory.RoleModule.Delete(param);

                return JsonString(br,1);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ActionPurview(true)]
        public ActionResult saveEdit(Tb_Role_Module info)
        {
            var br = new BaseResult();
            try
            {
                br = BusinessFactory.RoleModule.Update(info);
            }
            catch (CySoftException ex)
            {
                br.Message.Add(ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return JsonString(br,1);
        }
        [ActionPurview(true)]
        public ActionResult saveAdd(Tb_Role_Module info)
        {
            var br = new BaseResult();
            try
            {
                br = BusinessFactory.RoleModule.Add(info);
            }
            catch (CySoftException ex)
            {
                br.Message.Add(ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return JsonString(br,1);
        }
        [ActionPurview(true)]
        public ActionResult Add()
        {

            Hashtable param=base.GetParameters();
            ParamVessel pv = new ParamVessel();
            pv.Add("id_module", String.Empty, HandleType.ReturnMsg);
            pv.Add("id_platform_role", String.Empty, HandleType.ReturnMsg);
            param = param.Trim(pv);
            ViewData["id_module_father"] = param["id_module"];
            ViewData["id_platform_role"] = param["id_platform_role"];
            param.Clear();
            //param.Add("flag_type", "module");
            ViewData["list"] = BusinessFactory.Function.GetFunctionTree(param);
            ViewData["option"] = "add";
            return View("_Edit");
        }
        [ActionPurview(true)]
        public ActionResult Edit()
        {
            var param = base.GetParameters();
            ParamVessel pv=new ParamVessel();
            pv.Add("id",String.Empty,HandleType.ReturnMsg);
            param = param.Trim(pv);
            ViewData["id"] = param["id"];
            ViewData["item_edit"] = BusinessFactory.RoleModule.Get(param).Data;
            param.Clear();
            ViewData["list"]=  BusinessFactory.Function.GetFunctionTree(param);
            ViewData["option"] = "edit";
            return View("_Edit");
        }

        [ActionPurview(true)]
        [ActionAlias("rolemodule", "list")]
        public ActionResult QueryTree(int id_platform_role=-1)
        {
            var res = HandleResult(() =>
            {
                BaseResult br = new BaseResult() { Success = true };
                Hashtable param = new Hashtable();
                param.Add("id_platform_role", id_platform_role);
                var list = BusinessFactory.RoleModule.GetAll(param).Data as List<Tb_Role_Module_Tree>;
                if (list != null && list.Any())
                {
                    list.Insert(0, new Tb_Role_Module_Tree() { id = "0", id_module_father = "#", name = "全部" });
                    var tree = (from node in list
                                select new
                                {
                                    id = node.id,
                                    parent = node.id_module_father,
                                    text = node.name
                                });
                    br.Data = tree;
                }
                return br;
            });
            return JsonString(res.Data??"");
        }

        [ActionPurview(true)]
        [ActionAlias("rolemodule", "list")]
        public ActionResult QuerySubList(string id, int id_platform_role=-1)
        {
            var res = HandleResult(() =>
            {
                Hashtable param = new Hashtable();
                param.Add("id_module_father", id);
                param.Add("id_platform_role", id_platform_role);
                return BusinessFactory.RoleModule.GetAll(param);
            });
            return JsonString(res);
        }

    }
}

using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Model.Enums;
using CySoft.Model.Other;
using CySoft.Model.Ts;
using Spring.Collections;

namespace CySoft.Controllers.AdminCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class FunctionManageController : BaseController
    {
        [ActionPurview(true)]
        public ActionResult Index()
        {
            try
            {

                return View(BusinessFactory.Function.GetFunctionTree(new Hashtable()));
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
        public ActionResult AddChildFunction(Tb_Function info)
        {
            try
            {
                var result = new BaseResult();

                var id_user = GetLoginInfo<string>("id_user");
                info.id = Guid.NewGuid().ToString();//(int)BusinessFactory.Utilety.GetNextKey(typeof(Tb_Function));
                info.id_create = id_user;
                info.id_edit = id_user;
                info.rq_create = DateTime.Now;
                info.rq_edit = DateTime.Now;

                result = BusinessFactory.Function.Add(info);

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
        public ActionResult DeleteFunction(int id, string deleteType)
        {
            try
            {
                var br = new BaseResult();
                var param = new Hashtable();
                param.Add("id", id);
                param.Add("deleteType", deleteType);
                if (id == 0)
                {
                    br.Message.Add("根节点不能直接删除！");
                    br.Success = false;
                    return Json(br);
                }

                br = BusinessFactory.Function.Delete(param);

                return Json(br);
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
        public ActionResult EditFunction(Tb_Function info)
        {
            try
            {
                var result = new BaseResult();

                result = BusinessFactory.Function.Update(info);

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
        public ActionResult GetModulTree()
        {
            try
            {
                var param = new Hashtable();

                param.Add("flag_type", "module");

                return JsonString(new { Success = true, Data = BusinessFactory.Function.GetFunctionTree(param) });
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
        public ActionResult List()
        {
            ViewData["list"] = BusinessFactory.Function.GetFunctionTree(new Hashtable());
            var type = typeof(Enums.FuncVersion);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            List<Ts_Flag> versionList = new List<Ts_Flag>();
            if (fields.Any())
            {
                foreach (var fieldInfo in fields)
                {
                    var des = fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
                    var name = des.Description;
                    var value = (int)fieldInfo.GetValue(fieldInfo);
                    versionList.Add(new Ts_Flag()
                    {
                        listdisplay = name,
                        listdata = value
                    });
                }
            }
            ViewData["versionList"] = versionList;
            return View();
        }
        [ActionPurview(true)]
        public ActionResult Edit()
        {
            BaseResult br = new BaseResult();
            BaseResult br_model = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();

            pv.Add("id_masteruser", id_user_master, HandleType.ReturnMsg);
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            pv.Add("childId", "0", HandleType.DefaultValue);

            string id = string.Empty;
            try
            {
                param["id_masteruser"] = id_user_master;
                param_model = param.Trim(pv);
                id = param_model["id"].ToString();
                param_model.Remove("id");

                Hashtable param_query_item = new Hashtable();
                param_query_item.Add("id", id);
                br_model = BusinessFactory.Function.Get(param_query_item);
                if (!br_model.Success || !(br_model.Data is Tb_Function))
                    throw new Exception("没有相应功能模块");
                Hashtable ht = new Hashtable();
                br.Data = BusinessFactory.Function.GetFunctionTree(ht);
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
            ViewData["item_edit"] = br_model.Data;
            ViewData["category_id"] = id;
            ViewData["option"] = "edit";
            ViewData["list"] = br.Data;
            var listType = new List<SelectFunctionType>()
            {
                new SelectFunctionType(){DisplayName = "模块",Value="module"},
                new SelectFunctionType(){DisplayName = "控制器",Value="controller"},
                new SelectFunctionType(){DisplayName = "方法",Value="action"},
            };
            var funModel = br_model.Data as Tb_Function;
            //ViewData["typeName"]= listType.FirstOrDefault(a => a.Value == funModel.flag_type).DisplayName;
            SelectList selectListType = new SelectList(listType, "Value", "DisplayName",funModel.flag_type);
            ViewData["selectListType"] = selectListType;
            if (br.Success)
            {
                List<Tb_Function_Tree> list = br.Data as List<Tb_Function_Tree>;
                ViewData["list"] = list;
            }
            ViewData["versionList"] = GetVersionList();
            if (string.IsNullOrWhiteSpace(id) || id.Equals("0", StringComparison.OrdinalIgnoreCase))
            {
                return PartialView("_Edit");
            }
            else
            {
                return PartialView("_Edit");
            }
        }
        [ActionPurview(true)]
        public ActionResult saveEdit()
        {
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            pv.Add("name", string.Empty, HandleType.ReturnMsg);
            pv.Add("flag_type", string.Empty, HandleType.ReturnMsg);
            pv.Add("sort_id", 1, HandleType.DefaultValue);
            pv.Add("controller_name", "", HandleType.DefaultValue);
            pv.Add("action_name", "", HandleType.DefaultValue);
            pv.Add("icon", "", HandleType.DefaultValue);//
            pv.Add("tag_name", "", HandleType.DefaultValue);//
            pv.Add("version", "", HandleType.ReturnMsg);
            try
            {
                param_model = param.Trim(pv);
                br = BusinessFactory.Function.Update(new Tb_Function()
                {
                    id = param_model["id"].ToString(),
                    name = param_model["name"].ToString(),
                    controller_name = param_model["controller_name"].ToString(),
                    action_name = param_model["action_name"].ToString(),
                    sort_id = Convert.ToInt32(param_model["sort_id"]),
                    id_edit=id_user,
                    flag_type = param_model["flag_type"].ToString(),
                    icon = param_model["icon"].ToString(),
                    tag_name = param_model["tag_name"].ToString(),
                    version = param_model["version"].ToString()
                });
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }

            if (br.Success)
            {
                return base.JsonString(new
                {
                    status = "success",
                    message = "执行成功,正在载入页面...",
                });
            }
            else
            {
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message),
                });
            }
        }
        [ActionPurview(true)]
        public ActionResult Add()
        {
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();

            pv.Add("id_masteruser", id_user_master, HandleType.ReturnMsg);
            pv.Add("parent_id", "0", HandleType.DefaultValue);
            pv.Add("childId", "0", HandleType.DefaultValue);
            string pid = "0";

            try
            {
                param["id_masteruser"] = id_user_master;
                param_model = param.Trim(pv);
                pid = param_model["parent_id"].ToString();
                param_model.Remove("parent_id");
                Hashtable ht = new Hashtable();
                //ht.Add("flag_type", "module");
                br.Data = BusinessFactory.Function.GetFunctionTree(ht);      //完整分类树
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }

            ViewData["category_id"] = pid;                           //指定节点添加子分类
            ViewData["category_pid"] = string.Empty;
            ViewData["option"] = "add";
            IList<Tb_Function_Tree> list = br.Data as IList<Tb_Function_Tree>;
            var listType = new List<SelectFunctionType>()
            {
                new SelectFunctionType(){DisplayName = "模块",Value="module"},
                new SelectFunctionType(){DisplayName = "控制器",Value="controller"},
                new SelectFunctionType(){DisplayName = "方法",Value="action"},
            };
            SelectList selectListType = new SelectList(listType, "Value","DisplayName");
            ViewData["selectListType"] = selectListType;
            ViewData["list"] = list;
            ViewData["versionList"] = GetVersionList();
            return PartialView("_Edit");
        }

        private List<Ts_Flag> GetVersionList()
        {
            var type = typeof(Enums.FuncVersion);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            List<Ts_Flag> versionList = new List<Ts_Flag>();
            if (fields.Any())
            {
                foreach (var fieldInfo in fields)
                {
                    var des = fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
                    var name = des.Description;
                    var value = (int)fieldInfo.GetValue(fieldInfo);
                    versionList.Add(new Ts_Flag()
                    {
                        listdisplay = name,
                        listdata = value
                    });
                }
            }
            return versionList;
        }
        [ActionPurview(true)]
        public ActionResult saveAdd()
        {
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            pv.Add("name", string.Empty, HandleType.ReturnMsg);
            pv.Add("sort_id", 1, HandleType.DefaultValue);
            pv.Add("controller_name", "", HandleType.DefaultValue);
            pv.Add("action_name", "", HandleType.DefaultValue);
            pv.Add("icon", "", HandleType.DefaultValue);
            pv.Add("tag_name", "", HandleType.DefaultValue);
            pv.Add("flag_type", "", HandleType.ReturnMsg);
            pv.Add("version", "", HandleType.ReturnMsg);
            try
            {
                param_model = param.Trim(pv);
                br = BusinessFactory.Function.Add(new Tb_Function()
                {
                    name = param_model["name"].ToString(),
                    controller_name = param_model["controller_name"].ToString(),
                    action_name = param_model["action_name"].ToString(),
                    sort_id = Convert.ToInt32(param_model["sort_id"]),
                    id_father = param_model["id"].ToString(),
                    id_edit = id_user_master,
                    icon = param_model["icon"].ToString(),
                    flag_type = param_model["flag_type"].ToString(),
                    tag_name = param_model["tag_name"].ToString(),
                    version = param_model["version"].ToString()
                });
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }

            if (br.Success)
            {
                return base.JsonString(new
                {
                    status = "success",
                    message = "执行成功,正在载入页面...",
                });
            }
            else
            {
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message),
                });
            }
        }
        [ActionPurview(true)]
        public ActionResult Delete()
        {
            BaseResult br = new BaseResult();
            Hashtable param = base.GetParameters();
            param.Add("id_masteruser", id_user_master);
            Hashtable param_model = null;
            ParamVessel pv = new ParamVessel();
            pv.Add("id_masteruser", id_user_master, HandleType.ReturnMsg);
            pv.Add("id", string.Empty, HandleType.ReturnMsg);
            try
            {
                param_model = param.Trim(pv);
                br = BusinessFactory.Function.Delete(param);
            }
            catch (Exception ex)
            {
                br.Message.Add(ex.Message);
            }
            if (br.Success)
            {
                return base.JsonString(new
                {
                    status = "success",
                    message = "执行成功,正在载入页面..."
                });
            }
            else
            {
                return base.JsonString(new
                {
                    status = "error",
                    message = string.Join(";", br.Message)
                });
            }
        }


        [ActionPurview(true)]
        [ActionAlias("functionmanage", "list")]
        public ActionResult QuerySubList(string id)
        {
            var res = HandleResult(() =>
            {
                Hashtable param=new Hashtable();
                param.Add("id_father",id);
                param.Add("flag_stop",(int)Enums.FlagStop.Start);
                return BusinessFactory.Function.GetAll(param);
            });
            return JsonString(res);
        }

        [ActionPurview(true)]
        [ActionAlias("functionmanage", "list")]
        public ActionResult QueryTree()
        {
            var res = HandleResult(() =>
            {
                BaseResult br = new BaseResult() {Success = true};
                Hashtable param = new Hashtable();
                param.Add("flag_stop", (int)Enums.FlagStop.Start);
                var list= BusinessFactory.Function.GetAll(param).Data as List<Tb_Function>;
                if (list!=null&&list.Any())
                {
                   list.Insert(0, new Tb_Function() { id = "0", id_father = "#", name = "全部" });
                   var tree = (from node in list
                            select new
                            {
                                id = node.id,
                                parent = node.id_father,
                                text = node.name
                            });
                    br.Data = tree;
                }
                return br;
            });
            return JsonString(res.Data??"");
        }

        [ActionPurview(false)]
        public ActionResult MoveNode(string id, string id_father)
        {
            var res = HandleResult(() =>
            {
                Hashtable param = new Hashtable();
                ParamVessel pv=new ParamVessel();
                param.Add("id", id);
                param.Add("id_like",id);
                param.Add("id_father",id_father);
                pv.Add("id", "", HandleType.ReturnMsg);
                pv.Add("id_like", "", HandleType.ReturnMsg, true);
                pv.Add("id_father", "0", HandleType.ReturnMsg);
                param = param.Trim(pv);
                return BusinessFactory.Function.MoveNode(param);
            });
            return JsonString(res, 1);
        }

    }
}

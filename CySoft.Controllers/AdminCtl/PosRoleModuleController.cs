using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Tb;

namespace CySoft.Controllers.AdminCtl
{
    [LoginActionFilter]
    public  class PosRoleModuleController:BaseController
    {
        [ActionPurview(true)]
        public ActionResult List(int? id)
        {
            try
            {
                var param = new Hashtable();

                if (id == null)
                {
                    id = 0;
                }

                IList<Tb_Pos_Role_Module_Tree> list = new List<Tb_Pos_Role_Module_Tree>();

                if (!Enum.IsDefined(typeof(RoleFlag), id))
                {
                    return View(list);
                }

                param.Add("id_platform_role", id);
                list = BusinessFactory.Tb_Pos_Role_Module.GetRoleModuleTree(param);

                //if (Request.IsAjaxRequest())
                //{
                //    return PartialView("_RecursiveRoleModuleTree", list);
                //}
                ViewData["id_platform_role"] = id;
                return View(list);
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
        public ActionResult AddChild(Tb_Pos_Role_Module info)
        {
            try
            {
                var result = new BaseResult();

                var id_user = GetLoginInfo<string>("id_user");
                info.id_create = id_user;
                info.id_edit = id_user;
                info.rq_create = DateTime.Now;
                info.rq_edit = DateTime.Now;

                result = BusinessFactory.Tb_Pos_Role_Module.Add(info);

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
        public ActionResult Delete(int id)
        {
            try
            {
                var br = new BaseResult();
                var param = new Hashtable();
                param.Add("id", id);
                if (id == 0)
                {
                    br.Message.Add("根节点不能直接删除！");
                    br.Success = false;
                    return Json(br);
                }

                br = BusinessFactory.Tb_Pos_Role_Module.Delete(param);

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
        public ActionResult Edit(Tb_Pos_Role_Module info)
        {
            try
            {
                var result = new BaseResult();

                result = BusinessFactory.Tb_Pos_Role_Module.Update(info);

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
    }
}

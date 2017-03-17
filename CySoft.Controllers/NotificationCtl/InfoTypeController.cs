using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Model.Tb;
using CySoft.Utility.Mvc.Html;
using CySoft.Frame.Core;
using CySoft.Model.Enums;

#region 通知分类管理
#endregion
namespace CySoft.Controllers.InfoCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class InfoTypeController:BaseController
    {
        [ActionPurview(false)]
        public ActionResult List()
        {
            int limit = 10;
            Hashtable param = GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
            param = param.Trim(p);
            int pageIndex = Convert.ToInt32(param["pageIndex"]);

            param.Add("id_master", GetLoginInfo<long>("id_user_master"));
            param.Add("limit", limit);
            param.Add("start", (pageIndex - 1) * limit);
            param.Add("sort", "xh");
            param.Add("dir", "asc");
            PageList<Info_Type> lst = new PageList<Info_Type>(limit);
            try
            {
                PageNavigate pn = BusinessFactory.InfoType.GetPage(param);
                lst = new PageList<Info_Type>(pn, pageIndex, limit);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return View(lst);
        }

        /// <summary>
        /// 获取所有通知分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult GetAll()
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();
            param.Add("id_user", GetLoginInfo<long>("id_user"));
            param.Add("id_role", 1);
            if (param != null && !param.ContainsKey("flag_delete"))
                param.Add("flag_delete", (byte)Enums.FlagDelete.NoDelete);
            if (BusinessFactory.InfoType.isAdmin(param) > 0)
            {
                param.Add("bm1", "update");
                param.Add("bm2", "system");
            }
            param.Remove("id_user");
            param.Remove("id_role");
            param.Add("id_master", GetLoginInfo<long>("id_user_master"));
            param.Add("sort", "xh");
            param.Add("dir", "asc");
            try
            {
                br = BusinessFactory.InfoType.GetAll(param);
                br.Success = true;
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(br);
        }


        /// <summary>
        /// 添加通知分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(false)]        
        public ActionResult Add()
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("mc", string.Empty, HandleType.ReturnMsg);
            param = param.Trim(p);
            Info_Type type = new Info_Type();
            type.mc = param["mc"].ToString();
            param.Remove("mc");
            try
            {
                param.Add("id_master", GetLoginInfo<long>("id_user_master"));                
                br = BusinessFactory.InfoType.GetAll(param);
                //获取集合中最大的序号
                IList<Info_Type> lst = (IList<Info_Type>)br.Data;
                if (lst.Count > 0)
                {
                    type.xh = lst.Max(m => m.xh)+1;
                }
                else
                {
                    type.xh = 1;
                }
                param.Clear();
                //取最大的id
                type.id = BusinessFactory.Utilety.GetNextKey(typeof(Info_Type));
                type.id_create = GetLoginInfo<long>("id_user");
                type.id_master = GetLoginInfo<long>("id_user_master");
                br = BusinessFactory.InfoType.Add(type);
            }
            catch(CySoftException ex)
            {
                throw ex;
            }
            catch(Exception ex){
                throw ex;
            }
            return Json(br);
        }

        /// <summary>
        /// 更新通知分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult Update()
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("mc", string.Empty, HandleType.ReturnMsg);
            p.Add("id", 0, HandleType.ReturnMsg);
            param = param.Trim(p);
            param.Add("new_mc", param["mc"].ToString());
            param.Remove("mc");
            br = BusinessFactory.InfoType.Update(param);
            return Json(br);

        }

        /// <summary>
        /// 删除通知分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult Delete()
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();
            ParamVessel p = new ParamVessel();
            p.Add("id", 0, HandleType.ReturnMsg);
            p.Add("typeId",string.Empty,HandleType.Remove);
            param = param.Trim(p);
            try
            {
                br = BusinessFactory.InfoType.Delete(param);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(br);
        }

        /// <summary>
        /// 置顶
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPurview(false)]
        public ActionResult UpdateTop()
        {
            BaseResult br = new BaseResult();
            Hashtable param = GetParameters();
            param.Add("new_xh",1);
            param.Add("id_master", GetLoginInfo<long>("id_user_master"));
            br = BusinessFactory.InfoType.Active(param);
            return Json(br);
        }
    }
}

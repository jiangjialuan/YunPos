using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Controllers.Base;
using System.Web.Mvc;
using CySoft.Controllers.Filters;
using System.Web.UI;
using System.Collections;
using CySoft.Model.Tb;
using CySoft.Frame.Core;
using CySoft.Utility;
using CySoft.Model.Flags;

namespace CySoft.Controllers.GoodsCtl
{
    [LoginActionFilter]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class GoodsSpecController : BaseController
    {
        /// <summary>
        /// 商品规格列表
        /// znt
        /// 2015-2-28
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = new Hashtable();
                param.Add("id_gys", GetLoginInfo<string>("id_supplier"));
                param.Add("sort", "rq_create");
                param.Add("dir", "asc");
                br = BusinessFactory.GoodsSpec.GetAll(param);
            }

            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListControl", br.Data);
            }
            return View(br.Data);
        }

        /// <summary>
        ///  更新
        ///  znt
        ///  2015-03-02
        ///  2015-03-26 lxt 修改
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                List<Tb_Sp_Expand_Template> list = JSON.Deserialize<List<Tb_Sp_Expand_Template>>(obj);
                if (list.Count < 1)
                {
                    br.Success = false;
                    br.Message.Add("<h5>没有可更新的数据！</h5>");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                if (list.Count(m => m.id < 1 && m.mc.IsEmpty()) > 0)
                {
                    br.Success = false;
                    br.Message.Add("<h5>更新的规格数据有误，请检查后再试！</h5>");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }

                var query = from item in list where item.fatherId == 0 group item by item.mc into g select g;
                var query1 = (from item in query where item.Count() > 1 select item.Key).ToList();
                if (query1.Count() > 0)
                {
                    var item = query1[0];
                    br.Success = false;
                    br.Message.Add(String.Format("保存失败，{0} 有同名模版！", item));
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }

                var query4 = from item in list where item.fatherId == 0 select item;
                foreach (var item in query4)
                {
                    if (list.Count(m => m.fatherId == item.id && !m.mc.IsEmpty()) < 1)
                    {
                        br.Success = false;
                        br.Message.Add(String.Format("保存失败，模版【{0}】至少需要保留一个规格！", item.mc));
                        br.Level = ErrorLevel.Warning;
                        return Json(br);
                    }
                }

                var query2 = from item in list where item.fatherId > 0 && !item.mc.IsEmpty() group item by new { item.fatherId, item.mc } into g select g;
                var query3 =  (from item in query2 where item.Count() > 1 select item.Key).ToList();
                if (query3.Count() > 0)
                {
                    var item = query3[0];
                    var father = list.First(m => m.id == item.fatherId);
                    br.Success = false;
                    br.Message.Add(String.Format("保存失败，{0} > {1}有同名规格！", father.mc, item.mc));
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }

                foreach (var item in list)
                {
                    if (item.id < 1)
                    {
                        item.id = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Sp_Expand_Template));
                    }
                }
                Hashtable param = new Hashtable();
                param.Add("list", list);
                param.Add("id_edit", GetLoginInfo<long>("id_user"));
                param.Add("id_supplier", GetLoginInfo<long>("id_supplier"));
                br = BusinessFactory.GoodsSpec.Save(param);
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Base, br.Message);
                }
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
        /// 删除 规格模板
        /// znt
        /// 2015-03-02
        /// 2015-03-26 lxt 修改
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Delete(long id)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            param.Add("id", id);
            try
            {
                br = BusinessFactory.GoodsSpec.Delete(param);
                
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
        /// 新增UI
        /// znt
        /// 2015-03-02
        /// </summary>
        [HttpPost]
        [ActionPurview(false)]
        [ValidateInput(false)]
        public ActionResult AddView()
        {
            return PartialView("_AddControl");
        }


        /// <summary>
        /// 新增 
        /// znt
        /// 2015-03-02
        /// 2015-03-26 lxt 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                List<Tb_Sp_Expand_Template> list = JSON.Deserialize<List<Tb_Sp_Expand_Template>>(obj);
                if (list.Count(m => m.fatherId == 0) != 1)
                {
                    br.Success = false;
                    br.Message.Add("新增失败！请刷新后再试。");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                if (list.Count(m => m.fatherId != 0) < 1)
                {
                    br.Success = false;
                    br.Message.Add("新增失败！至少需要填写一个规格。");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }

                var query2 = from item in list where item.fatherId != 0 group item by item.mc into g select g;
                var query3 = (from item in query2 where item.Count() > 1 select item.Key).ToList();
                if (query3.Count() > 0)
                {
                    var item = query3[0];
                    br.Success = false;
                    br.Message.Add(String.Format("新增失败！{0}有同名规格。", item));
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                long id_supplier = GetLoginInfo<long>("id_supplier");
                long id_user = GetLoginInfo<long>("id_user");
                long id_next = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Sp_Expand_Template));
                foreach (var item in list)
                {
                    if (item.fatherId == 0)
                    {
                        item.id = id_next;
                    }
                    else
                    {
                        item.id = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Sp_Expand_Template));
                        item.fatherId = id_next;
                    }
                    item.id_create = id_user;
                    item.id_edit = id_user;
                    item.id_gys = id_supplier;
                }
                br = BusinessFactory.GoodsSpec.Add(list);
                if (br.Success)
                {
                    WriteDBLog(LogFlag.Base, br.Message);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Json(br);
        }

        /// <summary>
        /// 获取不分页json数据
        /// znt
        /// 2015-03-03
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        [ActionPurview(false)]
        public ActionResult JsonData()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = GetParameters();
                param.Add("id_gys", GetLoginInfo<string>("id_supplier"));
                // param.Add("fatherId", 0);
                br = BusinessFactory.GoodsSpec.GetAll(param);
                if (!br.Success)
                {
                    throw new CySoftException(br);
                }
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
    }
}

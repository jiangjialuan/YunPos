using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Service.Base;
using CySoft.Frame.Core;
using CySoft.Utility;
using CySoft.Model.Flags;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using CySoft.Model.Ts;

namespace CySoft.Controllers.ServiceCtl
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceGoodsController : ServiceBaseController
    {
        /// <summary>
        /// 分页查询
        /// lxt
        /// 2015-03-06
        /// </summary>
        [HttpPost]
        public ActionResult GetPage(string obj)
        {
            PageNavigate pn = new PageNavigate();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("id_spfl", (long)0, HandleType.Remove);//商品分类Id                
                p.Add("id_cgs", (long)0, HandleType.Remove);//客户Id
                p.Add("id_gys", GetLoginInfo<long>("id_supplier"), HandleType.DefaultValue);//客户Id
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("limit", 20, HandleType.DefaultValue);//当前页码
                p.Add("sort", "gsp.rq_create", HandleType.DefaultValue);
                p.Add("dir", "desc", HandleType.DefaultValue);
                param = param.Trim(p);
               // param.Add("id_gys", GetLoginInfo<long>("id_supplier"));//供应商Id
                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                if (pageIndex < 1)
                {
                    pageIndex = 1;
                }
                int limit = Convert.ToInt32(param["limit"]);
                if (limit < 1)
                {
                    limit = 20;
                }
                if (limit > 200)
                {
                    limit = 200;
                }
                param.Add("start", (pageIndex - 1) * limit);
                param.Add("flag_up", YesNoFlag.Yes);
                param.Add("baseurl", Request.Url.Scheme + "://" + Request.Url.Authority);
                pn = BusinessFactory.Goods.GetPageSkn(param);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(pn);
        }

        /// <summary>
        /// 获得单个完整对象
        /// lxt
        /// 2015-03-06
        /// </summary>
        [HttpPost]
        public ActionResult GetItem(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                ParamVessel p = new ParamVessel();
                p.Add("id", (long)0, HandleType.ReturnMsg);                
                p.Add("id_cgs", (long)0, HandleType.Remove);//客户Id
                p.Add("id_gys", GetLoginInfo<long>("id_supplier"), HandleType.DefaultValue);//客户Id
                param = param.Trim(p);
               // param.Add("id_gys", GetLoginInfo<long>("id_supplier"));//供应商Id
                param.Add("baseurl", Request.Url.Scheme + "://" + Request.Url.Authority);
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                br = BusinessFactory.Goods.Get(param);
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
        /// 供应商商品列表分页查询
        /// tim
        /// 2015-07-31
        /// </summary>
        [HttpPost]
        public ActionResult GetPageSupplierGoods(string obj)
        {
            PageNavigate pn = new PageNavigate();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("id_spfl", (long)0, HandleType.Remove);//商品分类Id
                p.Add("id_gys", (long)0, HandleType.Remove);//供应商Id               
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                p.Add("pageIndex", 1, HandleType.DefaultValue);//当前页码
                p.Add("limit", 20, HandleType.DefaultValue);//当前页码
                p.Add("sort", "gsp.rq_create", HandleType.DefaultValue);
                p.Add("dir", "desc", HandleType.DefaultValue);
                param = param.Trim(p);
                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));//采购商Id
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                int pageIndex = Convert.ToInt32(param["pageIndex"]);
                if (pageIndex < 1)
                {
                    pageIndex = 1;
                }
                int limit = Convert.ToInt32(param["limit"]);
                if (limit < 1)
                {
                    limit = 20;
                }
                if (limit > 200)
                {
                    limit = 200;
                }
                param.Add("start", (pageIndex - 1) * limit);
                param.Add("flag_up", YesNoFlag.Yes);
                param.Add("baseurl", Request.Url.Scheme + "://" + Request.Url.Authority);
                pn = BusinessFactory.Goods.GetPageSkn(param);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(pn);
        }

        /// <summary>
        /// 获得供应商商品详情
        /// tim
        /// 2015-07-31
        /// </summary>
        [HttpPost]
        public ActionResult GetSupplierGoodsItem(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                ParamVessel p = new ParamVessel();
                p.Add("id", (long)0, HandleType.ReturnMsg);
                p.Add("id_gys", (long)0, HandleType.ReturnMsg);//供应商Id
                param = param.Trim(p);
                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));//采购商Id
                param.Add("baseurl", Request.Url.Scheme + "://" + Request.Url.Authority);
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                br = BusinessFactory.Goods.Get(param);
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
        /// 获取商品单位
        /// tim
        /// 2015-7-30
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUnit(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj) ?? new Hashtable();
                ParamVessel p = new ParamVessel();
                p.Add("id", (long)0, HandleType.Remove);                
                param = param.Trim(p);

                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));
                br = BusinessFactory.GoodsUnits.GetAll(param);
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
        /// 新增商品
        /// tim
        /// 2015-7-28
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                GoodsData model = JSON.Deserialize<GoodsData>(obj);

                if (string.IsNullOrEmpty(model.mc))
                {
                    br.Success = false;
                    br.Data = "sp_mc";
                    br.Message.Add("商品名称不能为空");
                    return Json(br);
                }

                if (model.id_spfl == 0)
                {
                    br.Success = false;
                    br.Data = "id_spfl";
                    br.Message.Add("商品类别不能为空");
                    return Json(br);
                }

                if (string.IsNullOrEmpty(model.unit))
                {
                    br.Success = false;
                    br.Data = "unit";
                    br.Message.Add("商品单位不能为空");
                    return Json(br);
                }

                if (model.sku == null || model.sku.Count.Equals(0))
                {
                    br.Success = false;
                    br.Data = "sku";
                    br.Message.Add("必须至少有一条商品数据.");
                    return Json(br);
                }

                if (!string.IsNullOrWhiteSpace(model.description) && model.description.Length > 2000)
                {
                    br.Success = false;
                    br.Data = "sku";
                    br.Message.Add("商品介绍应在2000字以内.");
                    return Json(br);
                }

                var param = new Hashtable();
                var id_gys = GetLoginInfo<long>("id_supplier");

                param.Clear();
                param.Add("id_gys", GetLoginInfo<long>("id_supplier"));            
                br = BusinessFactory.CustomerType.GetAll(param);
                if (!br.Success || br.Data == null)
                {
                    br.Success = false;
                    br.Data = "cgs_level";
                    br.Message.Add("没有设置客户级别.");
                    return Json(br);
                }
                var cgs_level = (List<Tb_Cgs_Level>)br.Data;
                if (cgs_level.Count<1)
                {
                    br.Success = false;
                    br.Data = "cgs_level";
                    br.Message.Add("没有设置客户级别.");
                    return Json(br);
                }
                var default_level = new Tb_Cgs_Level() { flag_sys = YesNoFlag.Yes, agio = decimal.Parse("100"), id_gys = id_gys };
                default_level = cgs_level.Find(m => m.flag_sys.Equals(YesNoFlag.Yes));
                if (default_level == null) default_level = cgs_level[0];
               

                // 生成编码规则 
                param.Clear();
                param.Add("coding", typeof(Tb_Sp_Sku).Name.ToLower());
                br = BusinessFactory.CodingRule.Get(param);
                Ts_Codingrule Codingrule = br.Data as Ts_Codingrule;

                List<string> bmlist = new List<string>();
                foreach (var item in model.sku)
                {

                    if (bmlist.Contains(item.bm_Interface))
                    {
                        br.Success = false;
                        br.Data = "bm";
                        br.Message.Add("商品编码不能重复也不能为空");
                        return Json(br);
                    }
                    if (string.IsNullOrEmpty(item.bm))
                    {
                        long xh_max = BusinessFactory.Utilety.GetNextXh(typeof(Tb_Sp_Sku));
                        if (xh_max.ToString().Length < Codingrule.length - Codingrule.prefix.Length)
                        {
                            item.bm = Codingrule.prefix + xh_max.ToString().PadLeft(Codingrule.length.Value - Codingrule.prefix.Length, '0');
                        }
                        else
                        {
                            br.Success = false;
                            br.Message.Add("商品编码已超过有效范围！请联系管理员！");
                            return Json(br);
                        }
                    }
                    if (string.IsNullOrEmpty(item.bm_Interface)) item.bm_Interface = item.bm;
                    bmlist.Add(item.bm_Interface);

                    foreach (var p in item.sp_dj)
                    {
                        if (!p.id_cgs_level.HasValue || p.id_cgs_level.Value<1) p.id_cgs_level = default_level.id;
                    }
                    foreach (var p in cgs_level)
                    {
                        if (!item.sp_dj.Exists(m => m.id_cgs_level.Equals(p.id)))
                        {
                            item.sp_dj.Add(new Tb_Sp_Dj() { id_cgs_level = p.id, sl_dh_min = 0, dj_dh = (p.agio * item.dj_base/100).Digit(DigitConfig.dj) });
                        }
                    }
                }

                if (!model.id.HasValue || model.id.Value.Equals(0))
                    model.id = BusinessFactory.Utilety.GetNextKey(typeof(Tb_Sp));

                model.rq_create = DateTime.Now;
                model.rq_edit = DateTime.Now;
                model.id_gys_create = GetLoginInfo<long>("id_supplier");
                model.id_edit = GetLoginInfo<long>("id_user");
                model.id_create = GetLoginInfo<long>("id_user");


                if (!model.id_gys_create.HasValue || model.id_gys_create.Value < 1)
                {
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    br.Message.Add("该用户没有分配供应商角色！");
                    return Json(br);
                }
                br = BusinessFactory.Goods.Add(model);

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

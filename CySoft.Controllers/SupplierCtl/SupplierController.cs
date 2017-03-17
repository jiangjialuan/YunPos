//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using CySoft.Controllers.Base;
//using System.Web.Mvc;
//using CySoft.Frame.Core;
//using CySoft.Frame.Common;
//using System.Collections;
//using CySoft.Utility.Mvc.Html;
//using CySoft.Model.Tb;
//using CySoft.Model.Ts;
//using CySoft.Controllers.Filters;
//using System.Web.UI;
//using CySoft.Model.Flags;
//using CySoft.Utility;

//namespace CySoft.Controllers.SupplierCtl
//{
//    //[LoginActionFilter]
//    [ValidateInput(false)]
//    [OutputCache(Location = OutputCacheLocation.None)]
//    public class SupplierController : BaseController
//    {
//        /// <summary>
//        /// 供应商列表 cxb 2015-3-14
//        /// </summary>
//        public ActionResult List()
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = base.GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("orderby", 1, HandleType.DefaultValue);//排序
//                p.Add("keyword", string.Empty, HandleType.Remove, true);//搜素关键字
//                param = param.Trim(p);
//                switch (param["orderby"].ToString())
//                {
//                    case "2":
//                        param.Add("sort", "rq_create");
//                        param.Add("dir", "asc");
//                        break;
//                    default:
//                        param["orderby"] = 1;
//                        param.Add("sort", "rq_create");
//                        param.Add("dir", "desc");
//                        break;
//                }


//                ViewData["keyword"] = param["keyword"];
//                ViewData["orderby"] = param["orderby"];
//                ViewData["Discovery"] = IsHasSupplierDiscoveryFunc();
//                param.Add("flag_state", 1);
//                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
//                param.Add("flag_stop", 0);
//                br = BusinessFactory.BuyerAttention.Get(param);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return View(br.Data);
//        }

//        /// <summary>
//        /// 供应商关注列表
//        /// </summary>
//        public ActionResult Watchlist()
//        {
//            if (!IsHasSupplierDiscoveryFunc())
//            {
//                throw  new CySoftException("您没有使用此服务的权限！", ErrorLevel.Error);
//            }

//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = base.GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("status", string.Empty, HandleType.Remove);//是否开通
//                p.Add("orderby", 1, HandleType.DefaultValue);//排序
//                param = param.Trim(p);
//                switch (param["orderby"].ToString())
//                {
//                    case "2":
//                        param.Add("sort", "rq_sq");
//                        param.Add("dir", "asc");
//                        break;
//                    default:
//                        param["orderby"] = 1;
//                        param.Add("sort", "rq_sq");
//                        param.Add("dir", "desc");
//                        break;
//                }
//                if (param.ContainsKey("status"))
//                {
//                    ViewData["status"] = param["status"];
//                    param.Add("flag_state", param["status"]);
//                    param.Remove("status");
//                }
//                ViewData["orderby"] = param["orderby"];
//                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
//                br = BusinessFactory.BuyerAttention.GetAll(param);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return View(br.Data);
//        }

//        /// <summary>
//        /// 显示弹出框供应商列表
//        /// </summary>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult DialogList()
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = base.GetParameters();
//                if (param["companyname"].ToString() != "")
//                {
//                    ParamVessel p = new ParamVessel();
//                    p.Add("companyname", String.Empty, HandleType.DefaultValue);//搜素关键字
//                    param = param.Trim(p);
//                    param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
//                    param.Add("not_id", GetLoginInfo<long>("id_supplier"));
//                    br = BusinessFactory.Supplier.GetFindGys(param);
//                }
//                else
//                {
//                    IList<Tb_Gys_Query> list = new List<Tb_Gys_Query>();
//                    br.Data = list;
//                }

//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return PartialView("_AlertListControl", br.Data);
//        }

//        /// <summary>
//        /// 获取客户详细信息
//        /// cxb
//        /// 2015-4-20
//        /// </summary>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult DialogDetail()
//        {
//            BaseResult br = new BaseResult();
//            string flag = "";
//            try
//            {
//                Hashtable param = GetParameters();
//                if (param["flag"] != null)
//                {
//                    flag = param["flag"].ToString();
//                }
//                ParamVessel p = new ParamVessel();
//                //p.Add("flag_edit", "Add", HandleType.DefaultValue);
//                p.Add("id", (long)0, HandleType.ReturnMsg);
//                param = param.Trim(p);
//                br = BusinessFactory.Supplier.GetGys(param);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            if (flag == "detail")
//            {
//                return PartialView("_AlertDetailControl", br.Data);
//            }
//            else
//            {
//                return PartialView("_AlertAttentionControl", br.Data);
//            }
//        }

//        /// <summary>
//        /// 显示供应商信息
//        /// </summary>
//        public ActionResult Edit(long? id, string role)
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = GetParameters();
//                param["id"] = id;
//                ParamVessel p = new ParamVessel();
//                p.Add("flag_edit", "Add", HandleType.DefaultValue);
//                p.Add("id", (long)0, HandleType.ReturnMsg);
//                param = param.Trim(p);
//                ViewData["flag_edit"] = param["flag_edit"];
//                if (param["flag_edit"].ToString() == "Update")
//                {
//                    param.Remove("flag_edit");
//                    br = BusinessFactory.Supplier.Get(param);
//                }
//                else
//                {
//                    br.Data = new Tb_Gys_Edit();
//                }
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return View(br.Data);
//        }

//        /// <summary>
//        /// 撤销关注
//        /// cxb
//        /// 2015-4-13
//        /// </summary>
//        [HttpPost]
//        public ActionResult Revocation()
//        {
//            BaseResult br = new BaseResult();

//            try
//            {
//                Hashtable param = GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("id", (long)0, HandleType.ReturnMsg);
//                param = param.Trim(p);
//                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
//                param.Add("id_user", GetLoginInfo<long>("id_user"));
//                param.Add("flag_from", "pc");
//                br = BusinessFactory.SupplierAttention.Delete(param);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(br);
//        }

//        /// <summary>
//        /// 申请关注供应商
//        /// tim
//        /// 2015-05-21
//        /// </summary>
//        [HttpPost]
//        public ActionResult Attention()
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("id_gys", (long)0, HandleType.ReturnMsg);
//                p.Add("remark", string.Empty, HandleType.Remove);
//                param = param.Trim(p);

//                var id_gys = TypeConvert.ToInt64(param["id_gys"].ToString(), 0);

//                if (id_gys.Equals(0))
//                {
//                    br.Success = false;
//                    br.Level = ErrorLevel.Warning;
//                    br.Message.Add("请选择供应商.");
//                    return Json(br);
//                }
//                Tb_Gys_Cgs_Check model = new Tb_Gys_Cgs_Check()
//                {
//                    flag_form = "pc",
//                    flag_state = Gys_Cgs_Status.Apply,
//                    id_cgs = GetLoginInfo<long>("id_buyer"),
//                    id_gys = id_gys,
//                    remark = param.ContainsKey("remark") ? param["remark"].ToString() : string.Empty,
//                    rq_sq = DateTime.Now,
//                    id_user = GetLoginInfo<long>("id_user")
//                };

//                br = BusinessFactory.BuyerAttention.Add(model);

//                if (br.Success) WriteDBLog(LogFlag.Base, br.Message);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(br);
//        }

//        /// <summary>
//        /// 修改
//        /// lxt
//        /// 2015-02-27
//        /// </summary>
//        [HttpPost]
//        public ActionResult Update(Tb_Gys_Edit model)
//        {
//            BaseResult br = new BaseResult();
//            if (model.id < 1 || model.id_user_master < 1)
//            {
//                br.Success = false;
//                br.Message.Add("提交的数据不完整，请刷新后再试");
//                br.Level = ErrorLevel.Warning;
//                return Json(br);
//            }
//            if (model.companyname.IsEmpty())
//            {
//                br.Success = false;
//                br.Message.Add("供应商名称不能为空");
//                br.Level = ErrorLevel.Warning;
//                br.Data = "companyName";
//                return Json(br);
//            }
//            try
//            {
//                model.id_edit = GetLoginInfo<long>("id_user");
//                Hashtable param = new Hashtable();
//                param.Add("model", model);
//                param.Add("id_user_master_cgs", GetLoginInfo<long>("id_user_master"));
//                br = BusinessFactory.Supplier.Update(param);
//                if (br.Success)
//                {
//                    WriteDBLog(LogFlag.Base, br.Message);
//                }
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(br);
//        }

//        /// <summary>
//        /// 取消关注
//        /// lxt
//        /// 2015-03-23
//        /// </summary>
//        [HttpPost]
//        public ActionResult CancelAttention()
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = GetParameters();
//                ParamVessel p = new ParamVessel();
//                p.Add("id", (long)0, HandleType.ReturnMsg);
//                param = param.Trim(p);
//                param.Add("id_user_master_cgs", GetLoginInfo<long>("id_user_master"));
//                param.Add("id_user", GetLoginInfo<long>("id_user"));
//                param.Add("flag_from", "pc");
//                br = BusinessFactory.Supplier.Delete(param);
//                if (br.Success)
//                {
//                    WriteDBLog(LogFlag.Base, br.Message);
//                }
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Json(br);
//        }

//        /// <summary>
//        ///  已关注的供应商 
//        ///  znt 2015-04-07
//        /// </summary>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult JsonDataOfCgs()
//        {
//            BaseResult br = new BaseResult();
//            try
//            {
//                Hashtable param = new Hashtable();
//                param.Add("flag_stop", YesNoFlag.No);
//                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
//                br = BusinessFactory.Supplier.GetAll(param);
//                return Json(br);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        /// <summary>
//        /// 获取单体
//        /// znt 2015-04-10
//        /// cxb 2015-6-29 改
//        /// </summary>
//        [HttpPost]
//        [ActionPurview(false)]
//        public ActionResult Get()
//        {
//            BaseResult br = new BaseResult();
//            var bm_List = new List<string>();
//            Hashtable param = GetParameters();
//            if (string.IsNullOrEmpty(param["id"].ToString()))
//            {
//                br.Success = false;
//                br.Message.Add("参数校验失败");
//                br.Data = "none";
//                return Json(br);
//            }

//            try
//            {


//                br = BusinessFactory.Supplier.Get(param);
//                // 获取系统参数设置值
//                //param.Clear();
//                //param["id_user_master"] = ((Tb_Gys_Edit)br.Data).id_user_master;
//                //bm_List.AddRange(new List<string> { "order_special_price_flag", "order_DeliveryDate_flag", "order_invoice_fax_flag", "order_invoice_additional_fax", "order_invoice_fax" });
//                //param["bmLists"] = bm_List;
//                //br = BusinessFactory.System.GetAll(param);
//                //ViewData["order_settingList"] = br.Data;
//                return Json(br);
//            }
//            catch (CySoftException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        /// <summary>
//        /// 是否拥有发现功能（申请关注功能）
//        /// </summary>
//        /// <returns></returns>
//        private bool IsHasSupplierDiscoveryFunc()
//        {
//            try
//            {
//                Hashtable param = new Hashtable();
//                BaseResult br = new BaseResult();

//                param.Clear();
//                param["id_user_master"] = GetLoginInfo<long>("id_user_master");
//                br = BusinessFactory.Setting.GetAll(param);
//                var list = br.Data as IList<Ts_Param_Business>;

//                if (list == null || list.Count <= 0)
//                {
//                    return false;
//                }

//                var item = list.FirstOrDefault(d => d.bm == "supplier_discovery_flag");

//                if (item != null && item.val == "1")
//                {
//                    return true;
//                }

//                return false;
//            }
//            catch
//            {
//                return false;
//            }

//        }
//    }
//}

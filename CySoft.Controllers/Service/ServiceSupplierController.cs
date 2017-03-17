using System;
using System.Collections;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Service.Base;
using CySoft.Frame.Common;
using CySoft.Frame.Core;
using CySoft.Utility;
using CySoft.Model.Tb;
using CySoft.Model.Flags;
using System.Collections.Generic;

namespace CySoft.Controllers.ServiceCtl
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceSupplierController : ServiceBaseController
    {
        /// <summary>
        /// 分页查询供应商列表
        /// cxb
        /// 2015-3-16
        /// </summary>
        [HttpPost]
        public ActionResult GetPage(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("orderby", 1, HandleType.DefaultValue);//排序
                p.Add("keyword", String.Empty, HandleType.Remove, true);//搜素关键字
                param = param.Trim(p);
                switch (param["orderby"].ToString())
                {
                    case "2":
                        param.Add("sort", "rq_create");
                        param.Add("dir", "asc");
                        break;
                    default:
                        param["orderby"] = 1;
                        param.Add("sort", "rq_create");
                        param.Add("dir", "desc");
                        break;
                }
               
                param.Add("flag_state", 1);
                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                param.Add("flag_stop", 0);
                br = BusinessFactory.BuyerAttention.Get(param);
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
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("id", (long)0, HandleType.ReturnMsg);
                param = param.Trim(p);
                br = BusinessFactory.Supplier.Get(param);
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
        /// 供应商关注列表
        /// cxb
        /// 2015-4-16
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Watchlist(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("status", String.Empty, HandleType.Remove);//是否开通
                p.Add("orderby", 1, HandleType.DefaultValue);//排序
                param = param.Trim(p);
                switch (param["orderby"].ToString())
                {
                    case "2":
                        param.Add("sort", "rq_sq");
                        param.Add("dir", "asc");
                        break;
                    default:
                        param["orderby"] = 1;
                        param.Add("sort", "rq_sq");
                        param.Add("dir", "desc");
                        break;
                }
                if (param.ContainsKey("status"))
                {
                    param.Add("flag_state", param["status"]);
                    param.Remove("status");
                }
                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                br = BusinessFactory.BuyerAttention.GetAll(param);
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
        /// 获取客户详细信息
        /// cxb
        /// 2015-4-20
        /// </summary>
        [HttpPost]
        public ActionResult DialogDetail(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                //p.Add("flag_edit", "Add", HandleType.DefaultValue);
                p.Add("id", (long)0, HandleType.ReturnMsg);
                param = param.Trim(p);
                br = BusinessFactory.Supplier.GetGys(param);
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
        /// 申请关注供应商
        /// cxb
        /// 2015-4-13
        /// </summary>
        [HttpPost]
        public ActionResult Attention(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("id_gys", (long)0, HandleType.ReturnMsg);
                p.Add("remark", string.Empty, HandleType.Remove);
                p.Add("flag_from", string.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);

                var id_gys = TypeConvert.ToInt64(param["id_gys"].ToString(), 0);
                if (id_gys.Equals(0))
                {
                    br.Success = false;
                    br.Level = ErrorLevel.Warning;
                    br.Message.Add("请选择供应商.");
                    return Json(br);
                }
                Tb_Gys_Cgs_Check model = new Tb_Gys_Cgs_Check()
                {
                    flag_form = param["flag_from"].ToString(),
                    flag_state = Gys_Cgs_Status.Apply,
                    id_cgs = GetLoginInfo<long>("id_buyer"),
                    id_gys = id_gys,
                    remark = param.ContainsKey("remark") ? param["remark"].ToString() : string.Empty,
                    rq_sq = DateTime.Now,
                    id_user = GetLoginInfo<long>("id_user")
                };

                br = BusinessFactory.BuyerAttention.Add(model);
                if (br.Success) WriteDBLog(LogFlag.Base, br.Message);
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
        /// 取消关注
        /// cxb
        /// 2015-4-16
        /// </summary>
        [HttpPost]
        public ActionResult CancelAttention(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("id", (long)0, HandleType.ReturnMsg);
                p.Add("flag_from",string.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                param.Add("id_user_master_cgs", GetLoginInfo<long>("id_user_master"));
                param.Add("id_user", GetLoginInfo<long>("id_user"));                
                br = BusinessFactory.Supplier.Delete(param);
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
        /// 显示弹出框供应商列表
        /// </summary>
        [HttpPost]
        public ActionResult DialogList(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                if (param["companyname"].ToString() != "")
                {
                    ParamVessel p = new ParamVessel();
                    p.Add("companyname", String.Empty, HandleType.DefaultValue);//搜素关键字
                    param = param.Trim(p);
                    param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                    param.Add("not_id", GetLoginInfo<long>("id_supplier"));
                    br = BusinessFactory.Supplier.GetFindGys(param);
                }
                else
                {
                    IList<Tb_Gys_Query> list = new List<Tb_Gys_Query>();
                    br.Data = list;
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
        /// 撤销关注        
        /// cxb
        /// 2015-4-13
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Revocation(string obj)
        {
            BaseResult br = new BaseResult();           
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("id", (long)0, HandleType.ReturnMsg);
                p.Add("flag_from", string.Empty, HandleType.ReturnMsg);
                param = param.Trim(p);
                param.Add("id_cgs", GetLoginInfo<long>("id_buyer"));
                param.Add("id_user", GetLoginInfo<long>("id_user"));
                br = BusinessFactory.SupplierAttention.Delete(param);
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

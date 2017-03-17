using System;
using System.Collections;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Tb;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;
using CySoft.Model.Td;
using System.Linq;
using System.Collections.Generic;
/**/
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using CySoft.Frame.Attributes;
using CySoft.Model.Enums;
using CySoft.Model.Other;
using NPOI.HSSF.Util;
#region 客户管理
#endregion

namespace CySoft.Controllers.SupplierCtl.CustomerCtl
{
    [LoginActionFilter]
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ReportController : BaseController
    {
        #region 商品库存报表
        /// <summary>
        /// 商品库存报表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult SpkcReport(SpkcReportModel model)
        {
            ViewBag.DigitHashtable = GetParm();
            return View();

        }
        [ActionPurview(false)]
        public ActionResult SpkcReportApi(SpkcReportModel model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_spkc");
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}",
                id_user,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.id_shop ?? "0",
                model.s_id_kcsp ?? "0",
                model.barcode ?? "",
                model.mc_sp ?? ""));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }
        #endregion


        #region 商品销售汇总报表
        /// <summary>
        /// 商品销售汇总报表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult SpxshzReport(SpxshzReportModel model)
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }
        [ActionPurview(false)]
        //[HttpPost]
        public ActionResult SpxshzReportApi(SpxshzReportModel model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_spxs_hz");
            //条件 id_user|@id_shop_user|@beginrecode|@endrecode|@id_shop|@rq_begin|@rq_end|@id_shopsp|@id_spfl|@barcode|@mc_sp  
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}",
                id_user,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.id_shop ?? "",
                model.rq_begin.ToString("yyyy-MM-dd"),
                model.rq_end.ToString("yyyy-MM-dd"),
                "0",
                model.id_spfl ?? "0",
                model.barcode ?? "",
                model.mc_sp ?? ""));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }
        #endregion


        #region 商品销售明细报表
        /// <summary>
        /// 商品销售明细报表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult SpxsmxReport(SpxsmxReportModel model)
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }
        [ActionPurview(false)]
        [HttpPost]
        public ActionResult SpxsmxReportApi(SpxsmxReportModel model)
        {
            int limit = base.PageSizeFromCookie;
            //var www = base.GetParameters();
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_spxs_mx");
            if (model.id_spfl == "0")
            {
                model.id_spfl = "";
            }
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}",
                id_user,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.id_shop ?? "",
                model.rq_begin.ToString("yyyy-MM-dd"),
                model.rq_end.ToString("yyyy-MM-dd"),
                model.id_sp ?? "",
                model.id_spfl ?? "",
                model.dh ?? "",
                model.barcode ?? "",
                model.mc_sp));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }

        #endregion


        #region 
        [ActionPurview(true)]
        public ActionResult SkhzReport()
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }

        [ActionPurview(false)]
        public ActionResult SkhzReportApi(SkhzReport model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_user_sk_hz");
            //条件 id_user_work|id_masteruser|id_shop_user|@beginrecode|@endrecode|@rq_begin|@rq_end|@id_shop|@id_user
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
                id_user,
                id_user_master,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.rq_begin.ToString("yyyy-MM-dd"),
                model.rq_end.ToString("yyyy-MM-dd"),
                model.id_shop ?? "",
                model.id_user ?? ""));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }
        
        #endregion


        #region 商品进销存汇总

        [ActionPurview(true)]
        public ActionResult SpcrkhzReport()
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }

        [ActionPurview(false)]
        public ActionResult SpcrkhzReportApi(SpcrkhzReportModel model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_sp_crk_hz");
            //条件 id_user|id_shop_user|@beginrecode|@endrecode|@rq_begin|@rq_end|@id_shop|@id_spfl 
            if (model.id_spfl == "0")
            {
                model.id_spfl = "";
            }
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                id_user,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.rq_begin.ToString("yyyy-MM-dd"),
                model.rq_end.ToString("yyyy-MM-dd"),
                model.id_shop ?? "",
                model.id_spfl ?? "",
                model.barcode ?? "",
                model.mc_sp ?? ""));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }
        
        #endregion
        #region 商品进销存流水
        [ActionPurview(true)]
        public ActionResult SpcrklsReport()
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }

        [ActionPurview(false)]
        public ActionResult SpcrklsReportApi(SpcrklsReportModel model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_sp_crk_ls");
            //条件 id_user|id_shop_user|@beginrecode|@endrecode|@rq_begin|@rq_end|@id_shop|@barcode|@mc_sp  
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
                id_user,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.rq_begin.ToString("yyyy-MM-dd"),
                model.rq_end.ToString("yyyy-MM-dd"),
                model.id_shop ?? "", 
                model.barcode ?? "",
                model.mc_sp ?? ""));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }

        #endregion


        #region 应付款查询报表
        [ActionPurview(true)]
        public ActionResult GysfkReport()
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }
        [ActionPurview(false)]
        public ActionResult GysfkReportApi(GysfkReportModel model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_gys_yf");
            //条件 id_masteruser|@id_shop_user|@beginrecode|@endrecode|@id_shop|@id_gys
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                id_user,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.id_shop ?? "",
                model.id_gys ?? ""));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }
        #endregion


        #region 应收款查询报表
        [ActionPurview(true)]
        public ActionResult ShopysReport()
        {
            ViewBag.DigitHashtable = GetParm();
            return View();}
        [ActionPurview(false)]
        public ActionResult ShopysReportReportApi(ShopysReportModel model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_shop_ys");
            //条件 id_user|@id_shop_user|@beginrecode|@endrecode|@id_shop|@id_kh
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                id_user,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.id_shop ?? "",
                model.id_kh ?? ""));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }
        #endregion




        #region 商品进货统计报表

        /// <summary>
        /// 商品进货统计报表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult SpjhhzReport(SpjhHzReportModel model)
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }


        [ActionPurview(false)]
        public ActionResult SpjhhzReportApi(SpjhHzReportModel model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_spjh_hz");
            //条件 @id_user|@id_shop_user|@beginrecode|@endrecode|@rq_begin|@rq_end|@id_shop|@id_gys|@id_spfl|@barcode|@mc_sp  
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}",
                id_user,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.rq_begin.ToString("yyyy-MM-dd"),
                model.rq_end.ToString("yyyy-MM-dd"),
                model.id_shop ?? "",
                model.id_gys ?? "",
                model.id_spfl ?? "",
                model.barcode ?? "",
                model.mc_sp ?? ""
                ));

            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }


        #endregion

        #region 订单收货查询

        /// <summary>
        /// 订单收货查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionPurview(true)]
        public ActionResult JhddrkmxReport(JhddrkmxReport model)
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }


        [ActionPurview(false)]
        public ActionResult JhddrkmxReportApi(JhddrkmxReport model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_jh_ddrkmx");
            //条件  @id_user|@id_shop_user|@beginrecode|@endrecode|@rq_begin|@rq_end|@id_shop_sh|@id_gys|@dh_dd|@id_sp|@id_spfl|@barcode|@mc_sp
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}",
                id_user,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.rq_begin.ToString("yyyy-MM-dd"),
                model.rq_end.ToString("yyyy-MM-dd"),
                model.id_shop ?? "",
                model.id_gys ?? "",
                model.dh_dd ?? "",
                model.id_sp ?? "",
                model.id_spfl ?? "",
                model.barcode ?? "",
                model.mc_sp ?? ""
                ));

            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }


        #endregion

        #region 申请出入库报表
        /// <summary>
        /// 申请出入库报表View
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult PssqcrkmxReport()
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }
        /// <summary>
        /// 申请出入库报表数据接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult PssqcrkmxReportApi(PssqcrkReportModel model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_ps_sqcrkmx");
            //条件 id_masteruser|id_user|id_shop_user|@beginrecode|@endrecode|@dh_sq|@id_shop_sq|@rq_begin|@rq_end|@id_shopsp|@id_spfl 
            param.Add("str", string.Format(@"{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}",
                                                         id_user_master,
                                                         id_user,
                                                         id_shop,
                                                         model.beginIndex,
                                                         model.endIndex,
                                                         model.dh_sq ?? "",
                                                         model.id_shop_sq ?? "",
                                                         model.rq_begin.ToString("yyyy-MM-dd"),
                                                         model.rq_end.ToString("yyyy-MM-dd"),
                                                         "",
                                                         ""
                                                         ));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }
        #endregion

        #region 出入库明细报表
        /// <summary>
        /// 出入库明细报表View
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult PscrkmxReport()
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }
        [ActionPurview(false)]
        public ActionResult PscrkmxReportApi(PscrkmxReportModel model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_ps_crkmx");
            //条件  id_masteruser|@id_shop_user|@beginrecode|@endrecode|@id_shop_rk|@rq_begin|@rq_end|@dh_ck|@id_sp|@id_spfl   
            param.Add("str", string.Format(@"{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                                                         id_user,
                                                         id_shop,
                                                         model.beginIndex,
                                                         model.endIndex,
                                                         model.id_shop_rk ?? "",
                                                          model.rq_begin.ToString("yyyy-MM-dd"),
                                                         model.rq_end.ToString("yyyy-MM-dd"),
                                                         model.dh_ck ?? "",
                                                         "",
                                                         model.id_spfl??""
                                                         ));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }
        #endregion

        #region 调拨出入库明细报表
        /// <summary>
        /// 调拨出入库明细报表View
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult PsdbcrkmxReport()
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }
        [ActionPurview(false)]
        public ActionResult PsdbcrkmxReportApi(PsdbcrkmxReportModel model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_ps_dbcrkmx");
            //条件 id_masteruser|@id_shop_user|@beginrecode|@endrecode|@id_shop_ck|@id_shop_rk|@rq_begin|@rq_end|@dh_ck|@id_sp|@id_spfl  
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}",
                id_user,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.id_shop_ck ?? "",
                model.id_shop_rk ?? "",
                model.rq_begin.ToString("yyyy-MM-dd"),
                model.rq_end.ToString("yyyy-MM-dd"),
                model.dh_ck ?? "",
                 "",
                 ""));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }
        #endregion

        #region 历史进货查询
        /// <summary>
        /// 历史进货查询
        /// </summary>
        /// <returns></returns>
        public ActionResult SpjhmxReport(SpjhmxReport model)
        {
            ViewBag.DigitHashtable = GetParm();
            return View();

        }

        [ActionPurview(false)]
        public ActionResult SpjhmxReportApi(SpjhmxReport model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_spjh_mx");
            //条件 @id_user|@id_shop_user|@beginrecode|@endrecode|@rq_begin|@rq_end|@id_shop|@id_gys|@id_sp|@id_spfl|@barcode|@mc_sp|@dh
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}",
                id_user,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.rq_begin.ToString("yyyy-MM-dd"),
                model.rq_end.ToString("yyyy-MM-dd"),
                model.id_shop ?? "",
                model.id_gys ?? "",
                model.id_sp ?? "",
                model.id_spfl ?? "",
                model.barcode ?? "",
                model.mc_sp ?? "",
                model.dh ?? ""
                ));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }
        #endregion

        #region 会员储值流水
        /// <summary>
        /// 会员储值流水
        /// </summary>
        /// <returns></returns>
        public ActionResult HyczxflsReport(HyczxflsReport model)
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }

        [ActionPurview(false)]
        public ActionResult HyczxflsReportApi(HyczxflsReport model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_hy_czxfls");
            //条件 @id_user|@id_shop_user|@beginrecode|@endrecode|@rq_begin|@rq_end|@id_shop|@phone|@name|@dh
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                id_user,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.rq_begin.ToString("yyyy-MM-dd"),
                model.rq_end.ToString("yyyy-MM-dd"),
                model.id_shop ?? "",
                model.phone ?? "",
                model.name ?? "",
                model.dh ?? ""
                ));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }

        #endregion

        #region 会员充值明细
        /// <summary>
        /// 会员充值明细
        /// </summary>
        /// <returns></returns>
        public ActionResult HyczmxReport(HyczmxReport model)
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }

        [ActionPurview(false)]
        public ActionResult HyczmxReportApi(HyczmxReport model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_hy_czmx");
            //条件 @id_user|@id_shop_user|@beginrecode|@endrecode|@rq_begin|@rq_end|@id_shop|@phone|@name|@dh
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                id_user,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.rq_begin.ToString("yyyy-MM-dd"),
                model.rq_end.ToString("yyyy-MM-dd"),
                model.id_shop ?? "",
                model.phone ?? "",
                model.name ?? "",
                model.dh ?? ""
                ));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }

        #endregion









        #region 门店销售汇总

        /// <summary>
        /// 门店销售汇总
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult XshzshopReport()
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }
        [ActionPurview(false)]
        public ActionResult XshzshopReportApi(XshzshopReportModel model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_xshz_shop");
            //--条件 id_masteruser|@id_shop_user|@beginrecode|@endrecode|@id_shop|@rq_begin|@rq_end
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                id_user,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.id_shop ?? "",
                model.rq_begin.ToString("yyyy-MM-dd"),
                model.rq_end.ToString("yyyy-MM-dd")
                ));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }
        #endregion

        #region 门店销售日报
        /// <summary>
        /// 门店销售日报
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult XshzshopdayReport()
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }
        [ActionPurview(false)]
        public ActionResult XshzshopdayReportApi(XshzshopdayReportModel model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_xshz_shop_day");
            //--条件 id_masteruser|@id_shop_user|@beginrecode|@endrecode|@id_shop|@rq_begin|@rq_end
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                id_user,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.id_shop ?? "",
                model.rq_begin.ToString("yyyy-MM-dd"),
                model.rq_end.ToString("yyyy-MM-dd")
                ));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }
        #endregion
        /// <summary>
        /// 商品销售排行榜
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult SpxsranklistReport()
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }
        [ActionPurview(false)]
        public ActionResult SpxsranklistReportApi(SpxsranklistReportModel model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_spxs_ranklist");
            //--条件id_masteruser|@id_shop_user|@beginrecode|@endrecode|@id_shop|@rq_begin|@rq_end|@id_spfl|@flag_lx|@rankno
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                id_user_master,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.id_shop ?? "",
                model.rq_begin.ToString("yyyy-MM-dd"),
                model.rq_end.ToString("yyyy-MM-dd"),
                model.id_spfl ?? "",
                model.flag_lx ?? "sl_xs",
                model.rankno ?? ""
                ));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }


        #region 库存调整明细报表
        /// <summary>
        /// 库存调整明细查询
        /// </summary>
        /// <returns></returns>
        [ActionPurview(false)]
        public ActionResult KcsltzmxReport()
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }

        [ActionPurview(false)]
        public ActionResult KcsltzmxReportApi(KcsltzmxReportModel model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_kc_sltz_mx");
            //条件 id_masteruser|@id_shop_user|@beginrecode|@endrecode|@id_shop|@rq_begin|@rq_end|@dh|@id_sp|@id_spfl|@barcode|@mc_sp    
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}",
                id_user,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.id_shop ?? "",
                model.rq_begin.ToString("yyyy-MM-dd"),
                model.rq_end.ToString("yyyy-MM-dd"),
                model.dh ?? "",
                model.id_sp ?? "",
                model.id_spfl ?? "",
                model.barcode ?? "",
                model.mc_sp ?? ""
                ));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }
        #endregion

        #region 库存盘点明细报表
        /// <summary>
        /// 库存盘点明细查询
        /// </summary>
        /// <returns></returns>
        public ActionResult KckspdmxReport()
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }
        [ActionPurview(false)]
        public ActionResult KckspdmxReportApi(KckspdmxReportModel model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_kc_kspd_mx");
            //条件 id_masteruser|@id_shop_user|@beginrecode|@endrecode|@id_shop|@rq_begin|@rq_end|@dh|@id_sp|@id_spfl|@barcode|@mc_sp    
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}",
                id_user,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.id_shop ?? "",
                model.rq_begin.ToString("yyyy-MM-dd"),
                model.rq_end.ToString("yyyy-MM-dd"),
                model.dh ?? "",
                model.id_sp ?? "",
                model.id_spfl ?? "",
                model.barcode ?? "",
                model.mc_sp ?? ""
                ));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }
        #endregion


        #region 收银员收银流水
        /// <summary>
        /// 收银员收银流水
        /// </summary>
        /// <returns></returns>
        public ActionResult UsersklsReport(UsersklsReport model)
        {
            ViewBag.DigitHashtable = GetParm();
            return View();
        }

        [ActionPurview(false)]
        public ActionResult UsersklsReportApi(UsersklsReport model)
        {
            int limit = base.PageSizeFromCookie;
            if (model.pageSize <= 0)
            {
                model.pageSize = limit;
            }
            Hashtable param = new Hashtable();
            param.Clear();
            param.Add("proname", "p_rep_user_sk_ls");
            //条件 id_masteruser|id_shop_user|@beginrecode|@endrecode|@rq_begin|@rq_end|@id_shop|@id_user|@dh|@id_user_login
            param.Add("str", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                id_user_master,
                id_shop,
                model.beginIndex,
                model.endIndex,
                model.rq_begin.ToString("yyyy-MM-dd"),
                model.rq_end.ToString("yyyy-MM-dd"),
                model.id_shop ?? "",
                model.id_user ?? "",
                model.dh ?? "",
                id_user
                ));
            var res = BusinessFactory.Report.Get(param);
            return JsonString(res.Data);
        }
        #endregion







    }
}
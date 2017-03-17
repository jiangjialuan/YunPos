using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Td;
using CySoft.IDAL;
using CySoft.Model.Tb;
using System.Collections;
using CySoft.IBLL;

namespace CySoft.BLL.ReportBLL
{
    public class ReportBLL :BaseBLL,IReportBLL
    {
        private ITd_Sale_Order_HeadDAL Td_Sale_Order_HeadDAL { get; set; }

        /// <summary>
        /// 获取订单统计报表 cxb 2015-5-6
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PageNavigate GetPage(System.Collections.Hashtable param = null)
        {
             PageNavigate pn = new PageNavigate();
             if (param["checktype"]==null)
             {
                 
                 pn.TotalCount = Td_Sale_Order_HeadDAL.QueryReportbyDateCount(typeof(Td_Sale_Order_Head), param);
                 if (pn.TotalCount > 0)
                 {
                     List<Td_Report> list = Td_Sale_Order_HeadDAL.QueryReportbyDatePage(typeof(Td_Sale_Order_Head), param).ToList();
                     pn.Data = list;
                     pn.Success = true;
                     return pn;
                 }
             }
             else if (param["checktype"].ToString() == "1")
             {
                 pn.TotalCount = Td_Sale_Order_HeadDAL.QueryReportbyWeekCount(typeof(Td_Sale_Order_Head), param);
                 if (pn.TotalCount > 0)
                 {
                     List<Td_Report> list = Td_Sale_Order_HeadDAL.QueryReportbyWeekPage(typeof(Td_Sale_Order_Head), param).ToList();
                     pn.Data = list;
                     pn.Success = true;
                     return pn;
                 }
             }
             else if (param["checktype"].ToString() == "2")
             {
                 pn.TotalCount = Td_Sale_Order_HeadDAL.QueryReportbyMonthCount(typeof(Td_Sale_Order_Head), param);
                 if (pn.TotalCount > 0)
                 {
                     List<Td_Report> list = Td_Sale_Order_HeadDAL.QueryReportbyMonthPage(typeof(Td_Sale_Order_Head), param).ToList();
                     pn.Data = list;
                     pn.Success = true;
                     return pn;
                 }
             }
             else if (param["checktype"].ToString() == "5")
             {
                 param["Tb_province"] = "Tb_province";
                 pn.TotalCount = Td_Sale_Order_HeadDAL.QueryReportbyAreaCount(typeof(Td_Sale_Order_Head), param);
                 if (pn.TotalCount > 0)
                 {
                     List<Td_Report> list = Td_Sale_Order_HeadDAL.QueryReportbyAreaPage(typeof(Td_Sale_Order_Head), param).ToList();
                     pn.Data = list;
                     pn.Success = true;
                     return pn;
                 }
             }
             else if (param["checktype"].ToString() == "6") {
                 param["Tb_city"] = "Tb_city";
                 pn.TotalCount = Td_Sale_Order_HeadDAL.QueryReportbyAreaCount(typeof(Td_Sale_Order_Head), param);
                 if (pn.TotalCount > 0)
                 {
                     List<Td_Report> list = Td_Sale_Order_HeadDAL.QueryReportbyAreaPage(typeof(Td_Sale_Order_Head), param).ToList();
                     pn.Data = list;
                     pn.Success = true;
                     return pn;
                 }
             }
             else if (param["checktype"].ToString() == "7")
             {
                 param["Tb_county"] = "Tb_county";
                 pn.TotalCount = Td_Sale_Order_HeadDAL.QueryReportbyAreaCount(typeof(Td_Sale_Order_Head), param);
                 if (pn.TotalCount > 0)
                 {
                     List<Td_Report> list = Td_Sale_Order_HeadDAL.QueryReportbyAreaPage(typeof(Td_Sale_Order_Head), param).ToList();
                     pn.Data = list;
                     pn.Success = true;
                     return pn;
                 }
             }
             else if (param["checktype"].ToString() == "8")
             {
                 pn.TotalCount = Td_Sale_Order_HeadDAL.QueryReportbySaleCount(typeof(Td_Sale_Order_Head), param);
                 if (pn.TotalCount > 0)
                 {
                     List<Td_Report> list = Td_Sale_Order_HeadDAL.QueryReportbySalePage(typeof(Td_Sale_Order_Head), param).ToList();
                     pn.Data = list;
                     pn.Success = true;
                     return pn;
                 }
             }
             else if (param["checktype"].ToString() == "9")
             {
                 pn.TotalCount = Td_Sale_Order_HeadDAL.QueryReportbySaleTypeCount(typeof(Td_Sale_Order_Head), param);
                 if (pn.TotalCount > 0)
                 {
                     List<Td_Report> list = Td_Sale_Order_HeadDAL.QueryReportbySaleTypePage(typeof(Td_Sale_Order_Head), param).ToList();
                     pn.Data = list;
                     pn.Success = true;
                     return pn;
                 }
             }
             else if (param["checktype"].ToString() == "10")
             {
                 pn.TotalCount = Td_Sale_Order_HeadDAL.QueryReportbyCustomeCount(typeof(Td_Sale_Order_Head), param);
                 if (pn.TotalCount > 0)
                 {
                     List<Td_Report> list = Td_Sale_Order_HeadDAL.QueryReportbyCustomePage(typeof(Td_Sale_Order_Head), param).ToList();
                     pn.Data = list;
                     pn.Success = true;
                     return pn;
                 }
             }
             else if (param["checktype"].ToString() == "11")
             {
                 pn.TotalCount = Td_Sale_Order_HeadDAL.QueryReportbyCustomeTypeCount(typeof(Td_Sale_Order_Head), param);
                 if (pn.TotalCount > 0)
                 {
                     List<Td_Report> list = Td_Sale_Order_HeadDAL.QueryReportbyCustomeTypePage(typeof(Td_Sale_Order_Head), param).ToList();
                     pn.Data = list;
                     pn.Success = true;
                     return pn;
                 }
             }
            pn.Data = new List<Td_Report>();
            pn.Success = true;
            return pn;
        }
        /// <summary>
        /// 获取汇总数据 cxb 2015-5-20 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data=Td_Sale_Order_HeadDAL.QueryTotal(typeof(Td_Sale_Order_Head), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 获取日期报表数据给图显示 cxb 2015-5-21
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PageNavigate GetDataforImg(Hashtable param)
        {
            PageNavigate pn = new PageNavigate();
            if (param["checktype"] == null)
            {
                 pn.Data=Td_Sale_Order_HeadDAL.QueryReportbyDateImg(typeof(Td_Sale_Order_Head),param);
            }
            else if (param["checktype"].ToString() == "1")
            {
                pn.Data = Td_Sale_Order_HeadDAL.QueryReportbyWeekImg(typeof(Td_Sale_Order_Head), param);
            }
            else if (param["checktype"].ToString() == "2" || param["checktype"].ToString()=="3")
            {
                 pn.Data=Td_Sale_Order_HeadDAL.QueryReportbyMonthImg(typeof(Td_Sale_Order_Head),param);
            }
            pn.Success = true;
            return pn;
        }

        /// <summary>
        /// 获取客户订单数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseResult GetCgsCount(Hashtable param) {
            Hashtable param0 = new Hashtable();
            param0["id_gys"] = param["id_gys"];
            param0["start_rq_create"] = param["start_rq_create"];
            param0["end_rq_create"] = param["end_rq_create"];
            param0["not_flag_state"] = param["not_flag_state"];
            BaseResult br = new BaseResult();
            br.Data = Td_Sale_Order_HeadDAL.QuerycgsTotal(typeof(Td_Sale_Order_Head), param0);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 获取订单统计数据 cxb 2015-6-2
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseResult GetOrderStatistics(Hashtable param) {
            BaseResult br = new BaseResult();
            br.Data = Td_Sale_Order_HeadDAL.QueryOrderStatistics(typeof(Td_Sale_Order_Head), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 获取订单数 cxb 2015-5-15
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult Get(Hashtable param)
        {
            BaseResult res = new BaseResult(){Success = true};
            Hashtable ht = new Hashtable();
            ht.Add("proname", param["proname"]);
            ht.Add("str", param["str"]);
            ht.Add("outstr", param["outstr"]??"");

            var rList = DAL.ProcedureOutQuery(ht);
            var outstr = ht["outstr"].ToString();

            ProcedureOutQueryResult result = new ProcedureOutQueryResult() { rList = rList, outstr = outstr };
            //var data = DAL.ProcedureQuery(param);
            res.Data = result;
            return res;
        }

        
    }

}

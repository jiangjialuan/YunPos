using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IDAL.Base;
using System.Collections;
using CySoft.Model.Td;
using CySoft.Model.Tb;
using CySoft.Model.Other;

namespace CySoft.IDAL
{
    public interface ITd_Sale_Order_HeadDAL : IBaseDAL
    {
         IList<Td_Sale_Order_Head_Query> QueryPageOfService(Type type, IDictionary param, string database = null);
         int QueryCountOfService(Type type, IDictionary param, string database = null);
         IList<Td_Report> QueryReportbyDatePage(Type type, IDictionary param, string database = null);
         Query_Pay_Total QueryPayTotal(Type type, System.Collections.IDictionary param, string database = null);
         Query_Pay_Total GysHomeTotal(Type type, System.Collections.IDictionary param, string database = null);
         int QueryReportbyDateCount(Type type, IDictionary param, string database = null);
         IList<Td_Report> QueryReportbyWeekPage(Type type, System.Collections.IDictionary param, string database = null);
         int QueryReportbyWeekCount(Type type, IDictionary param, string database = null);
         int QueryReportbyMonthCount(Type type, IDictionary param, string database = null);
         IList<Td_Report> QueryReportbyMonthPage(Type type, System.Collections.IDictionary param, string database = null);
         IList<Td_Report> QueryReportbyAreaPage(Type type, System.Collections.IDictionary param, string database = null);
         int QueryReportbyAreaCount(Type type, IDictionary param, string database = null);
         int QueryReportbySaleCount(Type type, IDictionary param, string database = null);
         IList<Td_Report> QueryReportbySalePage(Type type, System.Collections.IDictionary param, string database = null);
         int QueryReportbySaleTypeCount(Type type, IDictionary param, string database = null);
         IList<Td_Report> QueryReportbySaleTypePage(Type type, System.Collections.IDictionary param, string database = null);
         int QueryReportbyCustomeCount(Type type, IDictionary param, string database = null);
         IList<Td_Report> QueryReportbyCustomePage(Type type, System.Collections.IDictionary param, string database = null);
         int QueryReportbyCustomeTypeCount(Type type, IDictionary param, string database = null);
         IList<Td_Report> QueryReportbyCustomeTypePage(Type type, System.Collections.IDictionary param, string database = null);
         int QueryReportbyOrderCount(Type type, IDictionary param, string database = null);
         Td_Report QueryTotal(Type type, System.Collections.IDictionary param, string database = null);
         IList<Td_Report> QueryReportbyDateImg(Type type, System.Collections.IDictionary param, string database = null);
         IList<Td_Report> QueryReportbyMonthImg(Type type, System.Collections.IDictionary param, string database = null);
         IList<Td_Report> QueryReportbyWeekImg(Type type, System.Collections.IDictionary param, string database = null);
         int QuerycgsTotal(Type type, System.Collections.IDictionary param, string database = null);
         IList<Td_Report> QueryOrderStatistics(Type type, System.Collections.IDictionary param, string database = null);
         Td_Report QueryOrderStatisticsInfo(Type type, System.Collections.IDictionary param, string database = null);
    }
}

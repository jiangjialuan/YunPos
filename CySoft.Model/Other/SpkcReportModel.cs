using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Other
{
    public class ReportBase
    {
        public int page { get; set; }

        public int pageSize { get; set; }

        public int beginIndex
        {
            get { return page * pageSize + 1; }
        }

        public int endIndex
        {
            get { return (page + 1) * pageSize; }
        }

        public string _search_ { get; set; }

        private DateTime _rq_begin;
        private DateTime _rq_end;
        public virtual DateTime rq_begin
        {
            get
            {
                if (_rq_begin < new DateTime(2000, 1, 1))
                {
                    var currendDate = DateTime.Now;
                    return new DateTime(currendDate.Year, currendDate.Month, 1);
                }
                return _rq_begin;

            }
            set { _rq_begin = value; }
        }

        public virtual DateTime rq_end
        {
            get
            {
                if (_rq_end < new DateTime(2000, 1, 1))
                {
                    var currendDate = DateTime.Now;
                    var date = new DateTime(currendDate.Year, currendDate.Month, 1);
                    return new DateTime(currendDate.Year, currendDate.Month, date.AddMonths(1).AddDays(-1).Day);
                }
                return _rq_end;

            }
            set { _rq_end = value; }
        }
    }

    public class ReportBaseRQ : ReportBase
    {
        private DateTime _rq_begin;
        private DateTime _rq_end;
        public override DateTime rq_begin
        {
            get
            {
                if (_rq_begin < new DateTime(2000, 1, 1))
                {
                    var currendDate = DateTime.Now;
                    return new DateTime(currendDate.Year, currendDate.Month, 1);
                }
                return _rq_begin;
            }
            set { _rq_begin = value; }
        }

        public override DateTime rq_end
        {
            get
            {
                if (_rq_end < new DateTime(2000, 1, 1))
                {
                    var currendDate = DateTime.Now;
                    return new DateTime(currendDate.Year, currendDate.Month, 1);
                }
                return _rq_end.AddDays(1);
            }
            set { _rq_end = value; }
        }
    }
    public class SpkcReportModel : ReportBaseRQ
    {

        public string id_shop { get; set; }

        public string s_shopsp_name { get; set; }

        public string s_id_kcsp { get; set; }

        public string barcode { get; set; }
        public string mc_sp { get; set; }

    }

    public class SpxshzReportModel : ReportBase
    {
       
        public string id_shop { get; set; }
        /// <summary>
        /// 商品分类
        /// </summary>
        public string id_spfl { get; set; }

        public string barcode { get; set; }

        public string mc_sp { get; set; }

    }

    public class SpcrkhzReportModel : ReportBaseRQ
    {
        public string id_shop { get; set; }
        public string id_spfl { get; set; }
        public string barcode { get; set; }

        public string mc_sp { get; set; }
        
        
    }
    public class SpcrklsReportModel : ReportBaseRQ
    {
        public string id_shop { get; set; } 
        public string barcode { get; set; }

        public string mc_sp { get; set; }


    }
    public class SpxsmxReportModel : ReportBaseRQ
    {
        public string id_sp { get; set; }
        public string barcode { get; set; }
        public string mc_sp { get; set; }

        public string id_spfl { get; set; }



        public string id_shop { get; set; }

        public string id_user { get; set; }

        public string sort { get; set; }

        public string dh { get; set; }
    }

    public class SkhzReport : ReportBaseRQ
    {
        public string id_user { get; set; }
        public string id_shop { get; set; }
    }

    public class GysfkReportModel : ReportBaseRQ
    {
        public string id_shop { get; set; }
        public string id_gys { get; set; }
    }

    public class ShopysReportModel : ReportBaseRQ
    {
        public string id_shop { get; set; }
        public string id_kh { get; set; }
    }

    public class SpjhHzReportModel : ReportBase
    {
        public string id_shop { get; set; }
        public string id_gys { get; set; }

        public string id_spfl { get; set; }
        public string barcode { get; set; }
        public string mc_sp { get; set; }

    }

    public class PsdbcrkmxReportModel : ReportBaseRQ
    {
        public string id_shop_ck { get; set; }
        public string id_shop_rk { get; set; }

        public string dh_ck { get; set; }

    }
    public class PssqcrkReportModel : ReportBaseRQ
    {
        public string id_shop_sq { get; set; }
        public string dh_sq { get; set; }

        public string id_sp { get; set; }

        public string id_spfl { get; set; }
    }

    public class KcsltzmxReportModel : ReportBaseRQ
    {
        public string id_shop { get; set; }

        public string dh { get; set; }

        public string id_sp { get; set; }

        public string id_spfl { get; set; }

        public string barcode { get; set; }

        public string mc_sp { get; set; }
    }

    public class KckspdmxReportModel : ReportBaseRQ
    {
        public string id_shop { get; set; }

        public string dh { get; set; }

        public string id_sp { get; set; }

        public string id_spfl { get; set; }

        public string barcode { get; set; }

        public string mc_sp { get; set; }
    }

    public class PscrkmxReportModel : ReportBaseRQ
    {
        public string id_shop_rk { get; set; }
        public string dh_ck { get; set; }

        public string id_sp { get; set; }

        public string id_spfl { get; set; }
    }


    public class JhddrkmxReport : ReportBase
    {
        public string id_shop { get; set; }
        public string id_gys { get; set; }
        public string dh_dd { get; set; }
        public string id_sp { get; set; }
        public string id_spfl { get; set; }
        public string barcode { get; set; }
        public string mc_sp { get; set; }
    }

    public class SpjhmxReport : ReportBase
    {
        public string id_shop { get; set; }
        public string id_gys { get; set; }
        public string dh { get; set; }
        public string id_sp { get; set; }
        public string id_spfl { get; set; }
        public string barcode { get; set; }
        public string mc_sp { get; set; }

    }

    public class XshzshopReportModel : ReportBaseRQ
    {
        public string id_shop { get; set; }
    }

    public class XshzshopdayReportModel : ReportBaseRQ
    {
        public string id_shop { get; set; }
    }

    public class SpxsranklistReportModel : ReportBaseRQ
    {
        public string id_shop { get; set; }
        public string id_spfl { get; set; }

        public string rankno { get; set; }

        public string flag_lx { get; set; }
    }
    

    public class UsersklsReport : ReportBase
    {
        public string id_shop { get; set; }
        public string id_user { get; set; }
        public string dh { get; set; }

    }

    public class HyczxflsReport : ReportBase
    {
        public string id_shop { get; set; }
        public string phone { get; set; }
        public string dh { get; set; }
        public string name { get; set; }
    }

    public class HyczmxReport : ReportBase
    {
        public string id_shop { get; set; }
        public string phone { get; set; }
        public string dh { get; set; }
        public string name { get; set; }

    }


}

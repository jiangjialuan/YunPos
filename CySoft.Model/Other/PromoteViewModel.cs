using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Other
{
    public class PromoteViewModel
    {
        /// <summary>
        /// id号
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 经办人
        /// </summary>
        public string id_jbr { get; set; }
        /// <summary>
        /// 促销主题
        /// </summary>
        public string cxzt { get; set; }
        /// <summary>
        /// 促销开始日期
        /// </summary>
        public DateTime day_b { get; set; }
        /// <summary>
        /// 促销结束日期
        /// </summary>
        public DateTime day_e { get; set; }
        /// <summary>
        /// 指定的日期方式 1:日期 2星期
        /// </summary>
        public byte flag_rqfw { get; set; }
        /// <summary>
        /// 选择的指定日期
        /// </summary>
        public string days { get; set; }
        /// <summary>
        /// 选择指定的星期
        /// </summary>
        public string weeks { get; set; }
        /// <summary>
        /// 促销开始时间
        /// </summary>
        public string time_b { get; set; }
        /// <summary>
        /// 促销结束时间
        /// </summary>
        public string time_e { get; set; }
        /// <summary>
        /// 商品选择方式//dp单品/dzsp单组/zh组合/bill整单/spfl整类
        /// </summary>
        public string spxz { get; set; }
        /// <summary>
        /// 结算方式 sl数量/je金额/xl限量
        /// </summary>
        public string jsfs { get; set; }
        /// <summary>
        /// 结算规则 :每\满
        /// </summary>
        public string jsgz { get; set; }
        /// <summary>
        /// 条件1
        /// </summary>
        public decimal condition_1 { get; set; }
        /// <summary>
        /// 条件2
        /// </summary>
        public decimal condition_2 { get; set; }
        /// <summary>
        /// 条件3
        /// </summary>
        public decimal condition_3 { get; set; }
        
        /// <summary>
        /// 赠送商品1
        /// </summary>
        public string zs_sp_1 { get; set; }
        /// <summary>
        /// 赠送商品2
        /// </summary>
        public string zs_sp_2 { get; set; }
        /// <summary>
        /// 赠送商品3
        /// </summary>
        public string zs_sp_3 { get; set; }
        /// <summary>
        /// 赠送数量1
        /// </summary>
        public decimal sl_largess_1 { get; set; }
        /// <summary>
        /// 赠送数量2
        /// </summary>
        public decimal sl_largess_2 { get; set; }
        /// <summary>
        /// 赠送数量3
        /// </summary>
        public decimal sl_largess_3 { get; set; }
        /// <summary>
        /// 结果1
        /// </summary>
        public decimal result_1 { get; set; }
        /// <summary>
        /// 结果2
        /// </summary>
        public decimal result_2 { get; set; }
        /// <summary>
        /// 结果3
        /// </summary>
        public decimal result_3 { get; set; }
        /// <summary>
        /// 例外商品
        /// </summary>
        public string lwsp { get; set; }
        /// <summary>
        /// 选中商品
        /// </summary>
        public string sp { get; set; }

        /// <summary>
        /// 会员类型
        /// </summary>
        public string hylx { get; set; }
        /// <summary>
        /// 门店ID
        /// </summary>
        public string id_shops { get; set; }

        public string id_shop { get; set; }
        /// <summary>
        /// 主用户
        /// </summary>
        public string id_masteruser { get; set; }
        /// <summary>
        /// 当前用户
        /// </summary>
        public string id_user { get; set; }
        /// <summary>
        /// 优惠方式:zk打折/tj特价/yh优惠/zs赠送/jjhg加价换购
        /// </summary>
        public string preferential { get; set; }

        public byte flag_stop { get; set; }

        public string bm_djlx { get; set; }

        public string zh_condition_1 { get; set; }

        public string zh_sp_a { get; set; }
        public string zh_sp_b { get; set; }
        public string zh_sp_c { get; set; }


        private int _sd1 = -1;
        private int _sd2 = -1;
        private int _sd3 = -1;
        /// <summary>
        /// 时点1
        /// </summary>
        public int sd1 { get { return _sd1; } set { _sd1 = value; }}
        /// <summary>
        /// 时点2
        /// </summary>
        public int sd2 { get { return _sd2; } set { _sd2 = value; } }
        /// <summary>
        /// 时点3
        /// </summary>
        public int sd3 { get { return _sd3; } set { _sd3= value; } }

        public bool AutoAudit { get; set; }

        public string op { get; set; }

        public byte flag_zsz { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Tb
{
    public class SetPayConfigModel
    {
        public string pay_wx_appid { get; set; }
        public string pay_wx_key { get; set; }
        public string pay_wx_mch_id { get; set; }

        public string pay_wx_is_use { get; set; }
        public string pay_wx_mch_id_child { get; set; }

        public byte? flag_type { get; set; }

        public string pay_alipay_partner { get; set; }
        public string pay_alipay_rsakey1 { get; set; }

        public string pay_alipay_is_use { get; set; }
        public string pay_alipay_product_code { get; set; }

        public string id_masteruser { get; set; }

        public string id_shop { get; set; }

        public string pay_alipay_rsafile { get; set; }
    }
}

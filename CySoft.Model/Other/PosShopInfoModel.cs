using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Other
{
    [Serializable]
    public class PosShopInfoModel
    {
        public string id_masteruser { get; set; }
        public string id_cyuser { get; set; }

        public string id_shop { get; set; }
        public string mc { get; set; }
        /// <summary>
        /// GUID
        /// </summary>
        public string ticket { get; set; }

        public string data_url { get; set; }

        public int discern_no { get; set; }

        public string bm { get; set; }

        public string id_shop_master { get; set; }

        public byte version { get; set; }
    }
}

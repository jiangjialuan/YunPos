using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Other
{
    public class UpgradeToPlatform
    {
        public long currole { get; set; }
        public string phone { get; set; }
        public string companyname { get; set; }
        public string linkman { get; set; }
        public int location_id_province { get; set; }
        public int location_id_city { get; set; }
        public int location_id_county { get; set; }
        public string detaillocation { get; set; }
        public string defaultman { get; set; }
        public int address_id_province { get; set; }
        public int address_id_city { get; set; }
        public int address_id_county { get; set; }
        public string detailaddress { get; set; }
        public string id_user { get; set; }
        public long id_cgs { get; set; }
        public string id_create { get; set; }

        public long id_shdz { get; set; }
    }
}

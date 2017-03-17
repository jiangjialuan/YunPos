using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Other
{
    public class SpflUpdateTreeModel
    {
        public string id { get; set; }
        public string fartherPath { get; set; }
        public int sortNum { get; set; }

        public string id_fahter { get; set; }
        public List<SpflUpdateTreeModel> children { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Model.Td;

namespace CySoft.Model.Other
{
    [Serializable]
    public class JqGrid
    {
        public int page { get; set; }
        public int total { get; set; }
        public int records { get; set; }
        public List<JqGrid_Body> rows { get; set; }

    }

}

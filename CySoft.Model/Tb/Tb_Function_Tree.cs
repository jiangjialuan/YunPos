using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Tb
{
    [Serializable]
    public class Tb_Function_Tree : Tb_Function
    {
        public IList<Tb_Function_Tree> children { get; set; }
        public bool isclose { get; set; }
    }
}

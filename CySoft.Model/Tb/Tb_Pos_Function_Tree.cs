using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Tb
{
    public class Tb_Pos_Function_Tree:Tb_Pos_Function
    {
        public IList<Tb_Pos_Function_Tree> children { get; set; }
        public bool isclose { get; set; }
    }
}

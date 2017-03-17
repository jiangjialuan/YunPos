using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Tb
{
    public class Tb_Pos_Role_Module_Tree:Tb_Pos_Role_Module
    {
        public IList<Tb_Pos_Role_Module_Tree> children { get; set; }
        public bool isclose { get; set; }
        public string name_function { get; set; }
    }
}

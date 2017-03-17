using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Tb
{
    public class Tb_Role_Module_Tree : Tb_Role_Module
    {
        public IList<Tb_Role_Module_Tree> children { get; set; }
        public bool isclose { get; set; }
        public string name_function { get; set; }
    }
}

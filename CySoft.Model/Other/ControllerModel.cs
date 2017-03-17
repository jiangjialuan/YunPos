using System.Collections.Generic;
using System.Diagnostics;

namespace CySoft.Model.Other
{
    [DebuggerDisplay("name = {name}, action.Count = {actions.Count}")]
    public class ControllerModel
    {
        public string name { get; set; }
        public List<string> actions { get; set; }
    }
}

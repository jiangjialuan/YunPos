using System;
using System.Diagnostics;

namespace CySoft.Model.Other
{
    [Serializable]
    [DebuggerDisplay("id = {id}, name = {name}")]
    public class ChangeRoleName
    {
        public long id { get; set; }
        public string name { get; set; }
    }
}

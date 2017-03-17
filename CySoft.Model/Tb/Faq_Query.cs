#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    [Serializable]
    public class Faq_Query : Faq
    {
        public string user_name { get; set; }
        public string receive_user_name { get; set; }
    }
}
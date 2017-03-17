using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Other
{
    [Serializable]
    public class TicketModel
    {

        public string ip { get; set; }

        public string mac { get; set; }

        public string id_user { get; set; }

        public string id_masteruser { get; set; }

        public string id_shop { get; set; }

    }
}

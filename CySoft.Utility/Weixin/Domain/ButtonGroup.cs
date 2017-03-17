using System.Collections.Generic;

namespace CySoft.Utility.Weixin.Domain
{
    public class ButtonGroup
    {
        public ButtonGroup()
        {
            this.button = new List<BaseButton>();
        }

        public List<BaseButton> button { get; set; }
    }
}


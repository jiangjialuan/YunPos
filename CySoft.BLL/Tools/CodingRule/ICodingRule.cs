using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Frame.Core;

namespace CySoft.BLL.Tools.CodingRule
{
    public interface ICodingRule
    {
        BaseResult SetCoding(object entity, Type type = null, string setPropertyName = "bm");
    }
}

using CySoft.IDAL.Base;
using Spring.Context.Support;

namespace CySoft.BLL.Base
{
    public abstract class AbstractBaseBLL
    {
        public IBaseDAL DAL { protected get; set; }

    }
}

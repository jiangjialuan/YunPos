using System.Collections;
using CySoft.Frame.Core;

namespace CySoft.IBLL.Base
{
    public interface ITreeBLL : IBaseBLL
    {
        /// <summary>
        /// 构建树形结构
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        BaseResult GetTree(Hashtable param = null);
    }
}

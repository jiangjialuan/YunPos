#region Imports
using CySoft.Frame.Core;
using CySoft.IBLL.Base;
using System.Collections;
#endregion

namespace CySoft.IBLL
{
    public interface ITb_KhflBLL : IBaseBLL, ITreeBLL
    {
        /// <summary>
        /// 移动节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        BaseResult MoveNode(Hashtable param);
        /// <summary>
        /// 检查节点是否可以删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        BaseResult CheckCanDeleteNode(Hashtable param);
        /// <summary>
        /// 更新节点数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        BaseResult UpdateTree(dynamic entity, string id_cyuser);

        BaseResult ImportIn(Hashtable param);
    }
}
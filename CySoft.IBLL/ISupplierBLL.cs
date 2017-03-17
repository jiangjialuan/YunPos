using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IBLL.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Model.Tb;

namespace CySoft.IBLL
{
    public interface ISupplierBLL : IBaseBLL
    {
        /// <summary>
        /// 获取供应商采购商关系
        /// </summary>
        /// <param name="param">采购单信息</param>
        /// <returns></returns>
       BaseResult GetGysCgsRelation(Hashtable param);
       /// <summary>
       /// 获取供应商列表
       /// </summary>
       /// <param name="param"></param>
       /// <returns></returns>
       BaseResult GetGysAll(Hashtable param);
       /// <summary>
       /// 获取单个供应商
       /// </summary>
       /// <param name="param">供应商id</param>
       /// <returns></returns>
       BaseResult GetGys(Hashtable param);
       /// <summary>
       /// 获取要关注的供应商列表
       /// </summary>
       /// <param name="param">供应商名字</param>
       /// <returns></returns>
       BaseResult GetFindGys(Hashtable param);

        /// <summary>
        /// 货单单个供应商资料
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
       // Tb_Gys GetGys2(Hashtable param);
    }
}

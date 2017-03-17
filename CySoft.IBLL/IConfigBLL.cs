using System;
using CySoft.Frame.Core;
using CySoft.IBLL.Base;
using CySoft.Model.Other;
using CySoft.Model.Tb;
using System.Collections.Generic;
using System.Collections;

namespace CySoft.IBLL
{
    public interface IConfigBLL
    {
        /// <summary>
        /// 生成区域城市详细列表_JS文件
        /// </summary>
        /// <returns>文件路径</returns>
        BaseResult InitArea();
    }
}

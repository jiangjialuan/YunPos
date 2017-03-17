using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IBLL.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Model.Tb;
using System.Data;
using CySoft.Model.Other;
namespace CySoft.IBLL
{
    public interface IGoodsBLL : IBaseBLL
    {
        PageNavigate GetPageSkn(Hashtable param = null);
        /// <summary>
        /// 上传图片接口
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="imgList"></param>
        /// <returns></returns>
        BaseResult UploadImg(dynamic entity,  Dictionary<string,string> imgList);
        /// <summary>
        /// 查询单个 供应商商品
        /// </summary>
        /// <param name="param">id_sp/id_gys</param>
        /// <returns></returns>
        BaseResult Get_Gys_sp(Hashtable param);
        /// <summary>
        /// 查 单个商品单价
        /// </summary>
        /// <param name="id_gys">货品所有者-供应商ID</param>
        /// <param name="level">下单者-供应商关系-等级</param>
        /// <param name="id_sku">商品sku</param>
        /// <returns></returns>
        Tb_Sp_Dj Get_Sp_dj(long? id_gys, long? level, long? id_sku);
        /// <summary>
        /// 订货商品列表（根据sku显示）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        PageNavigate GetOrderPage(Hashtable param);
        /// <summary>
        /// 检查商品是否存在
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        BaseResult CheckGood(Hashtable param);
        /// <summary>
        /// 获取所有商品介绍
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        BaseResult GetInfoAll(Hashtable param = null);
        /// <summary>
        /// 添加/更新商品介绍
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        BaseResult AddOfUpInfo(Tb_Sp_Info info);
        /// <summary>
        /// 导出所有商品库存
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        List<SkuData> GetExportAll(Hashtable param = null);
        /// <summary>
        /// 批量修改商品
        /// </summary>
        /// <param name="success"></param>
        /// <param name="fail"></param>
        /// <param name="table"></param>
        /// <param name="ht"></param>
        /// <param name="mTb_Cgs_Level"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        BaseResult UpSp(out int success, out int fail, out DataTable table, Hashtable ht, IList<Tb_Cgs_Level> mTb_Cgs_Level, string savePath);

        bool IsExistsGoods(Hashtable param = null);
        IList<Tb_Barcode_Pic> GetBarcodePic(string barCode);
        BaseResult ExportAllSp(Hashtable param);
        BaseResult GetPicList(List<SkuData> list, long id_gys);
    }
}

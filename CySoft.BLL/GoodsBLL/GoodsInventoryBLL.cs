using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using System.Collections;

#region 商品库存 
#endregion

namespace CySoft.BLL.GoodsBLL
{
    public class GoodsInventoryBLL : BaseBLL
    {
        /// <summary>
        ///  修改
        ///  znt 2015-03-13
        /// </summary>
        public override BaseResult Update(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = (Hashtable)entity;
            param.Add("new_sl_kc", param["sl_kc"]);
            param.Add("new_sl_kc_bj", param["sl_kc_bj"]);
            param.Remove("sl_kc");
            param.Remove("sl_kc_bj");
            DAL.UpdatePart(typeof(Tb_Gys_Sp), param);
            br.Message.Add(String.Format("修改库存成功"));
            br.Success = true;
            return br;
        }
        public override BaseResult GetCount(Hashtable param)
        {
            BaseResult br = new BaseResult();
            if (DAL.GetCount(typeof(Tb_Gys_Sp), param) > 0)//检查用户编码是否存在
            {
                br.Success = false;
            }
            else//不存在，则检查系统编码是否存在
            {
                if (param.Contains("bm_Interface"))
                {
                    var bm = param["bm_Interface"];
                    param.Remove("bm_Interface");
                    param.Add("bm", bm);
                    if (DAL.GetCount(typeof(Tb_Sp_Sku), param) > 0)
                    {
                        br.Success = false;
                    }
                    else
                        br.Success = true;
                }
            }
           
            return br;
        }
        /// <summary>
        /// 检查导入表格中的商品编码是否存在
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult CheckStock(Hashtable param)
        {
            BaseResult br = new BaseResult();
            var sp = DAL.GetItem<Tb_Sp_Sku>(typeof(Tb_Sp_Sku), param);
            if (sp == null)
            {
                br.Success = false;
                return br;
            }
            br.Success = true;
            br.Data = sp.id;
            return br;
        }
    }
}

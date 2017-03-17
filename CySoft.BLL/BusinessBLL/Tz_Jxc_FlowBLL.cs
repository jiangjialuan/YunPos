using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.Model.Tz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.BLL.BusinessBLL
{
     public class Tz_Jxc_FlowBLL : BaseBLL 
    {

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            #region 获取数据
            Hashtable param = (Hashtable)entity;
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();
            var tz_Jxc_Flow_Model = this.TurnTz_Jxc_FlowModel(param);
            #endregion

            #region 插入数据库
            DAL.Add(tz_Jxc_Flow_Model);
            #endregion

            br.Message.Add(String.Format("新增进销存流水表。流水号：{0}", tz_Jxc_Flow_Model.id));
            br.Success = true;
            br.Data = tz_Jxc_Flow_Model.id;
            return br;
        }
        #endregion




        #region TurnTz_Jxc_Flow
        /// <summary>
        /// 将Hashtable转换为Model
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private Tz_Jxc_Flow TurnTz_Jxc_FlowModel(Hashtable param)
        {
            Tz_Jxc_Flow model = new Tz_Jxc_Flow();
            model.id_masteruser = param["id_masteruser"].ToString();
            model.id = Guid.NewGuid().ToString();
            model.id_billmx = param["id_billmx"].ToString();
            model.bm_djlx = param["bm_djlx"].ToString();
            model.id_shop = param["id_shop"].ToString();
            model.id_shopsp = param["id_shopsp"].ToString();
            model.sl = decimal.Parse(param["sl"].ToString());
            model.je = decimal.Parse(param["je"].ToString());
            model.rq = DateTime.Now;
            model.id_kcsp = param["id_kcsp"].ToString();
            return model;
        }
        #endregion



    }
}

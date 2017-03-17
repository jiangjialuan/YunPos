using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Ts;
using System.Collections;

namespace CySoft.BLL.OrderBLL
{
    public class OrderLogBLL : BaseBLL
    {
        /// <summary>
        /// 新增
        /// znt 2015-03-31
        /// </summary>
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Ts_Sale_Order_Log model= (Ts_Sale_Order_Log)entity;

            if (string.IsNullOrEmpty(model.dh))
            {
                br.Data = "dh";
                br.Success = false;
                br.Message.Add(string.Format("单号不能为空"));
                return br;
            }

            DAL.Add(model);
            br.Success = true;
            return br;
        }

        /// <summary>
        ///  不分页获取数据
        ///  znt 2015-03-31
        /// </summary>
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Ts_Sale_Order_Log_Query>(typeof(Ts_Sale_Order_Log), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 分页获取数据
        /// znt 2015-03-31
        /// </summary>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = DAL.GetCount(typeof(Ts_Sale_Order_Log), param);
            if (pn.TotalCount > 0)
            {
                pn.Data = DAL.QueryPage<Ts_Sale_Order_Log>(typeof(Ts_Sale_Order_Log), param);
            }
            else
            {
                pn.Data = new List<Ts_Sale_Order_Log>();
            }
            pn.Success = true;
            return pn;
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using CySoft.Model.Td;

namespace CySoft.BLL.PromoteBLL
{
    public class Tb_Promote_SortBLL : BaseBLL 
    {

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Tb_Promote_Sort), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tb_Promote_Sort>(typeof(Tb_Promote_Sort), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }

        public override BaseResult Add(dynamic entity)
        {
            BaseResult res=new BaseResult(){Success = true};
            var model= entity as Tb_Promote_Sort;
            if (model==null||string.IsNullOrEmpty(model.id_masteruser))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            if (string.IsNullOrEmpty(model.style))
            {
                res.Success = false;
                res.Message.Add("请选择促销样式!");
                return res;
            }
            if (model.sort_id==null||model.sort_id<0)
            {
                res.Success = false;
                res.Message.Add("请设置排序!");
                return res;
            }
            model.id = GetGuid;
            DAL.Add(model);
            return res;
        }


        public override BaseResult Update(dynamic entity)
        {
            BaseResult res=new BaseResult(){Success = true};
            var model= entity as Tb_Promote_Sort;
            if (model==null
                ||string.IsNullOrEmpty(model.id_masteruser)
                ||string.IsNullOrEmpty(model.id))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            Hashtable param=new Hashtable();
            param.Add("id_masteruser", model.id_masteruser);
            param.Add("sort_id", model.sort_id);
            param.Add("not_id", model.id);
            if (DAL.GetCount(typeof(Tb_Promote_Sort), param)>0)
            {
                res.Success = false;
                res.Message.Add("排序号不能重复!");
                return res;
            }
            param.Clear();
            param.Add("id_masteruser",model.id_masteruser);
            param.Add("id", model.id);
            param.Add("new_sort_id", model.sort_id);
            DAL.UpdatePart(typeof(Tb_Promote_Sort), param);
            return res;
        }

        public override BaseResult Delete(Hashtable param)
        {
            BaseResult res=new BaseResult(){Success = true};
            if (param == null 
                || !param.ContainsKey("id_masteruser") 
                || !param.ContainsKey("id"))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            var id_masteruser = string.Format("{0}", param["id_masteruser"]);
            var id = string.Format("{0}", param["id"]);
            if (string.IsNullOrEmpty(id_masteruser)||string.IsNullOrEmpty(id))
            {
                res.Success = false;
                res.Message.Add("参数有误!");
                return res;
            }
            param.Clear();
            param.Add("id",id);
            param.Add("id_masteruser", id_masteruser);
            DAL.Delete(typeof(Tb_Promote_Sort), param);
            return res;
        }

    }
}

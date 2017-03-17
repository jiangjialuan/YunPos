using System.Collections.Generic;
using System.Linq;
using CySoft.Model.Tb;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.Frame.Attributes;
using CySoft.BLL.Base;


#region 商品收藏
#endregion
namespace CySoft.BLL.GoodsBLL
{
    public class GoodsFavoritesBLL : BaseBLL
    {
        /// <summary>
        /// 添加收藏商品
        /// tim
        /// 2015-05-12
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            var model = (List<Tb_Sp_Favorites>)entity;
            if (model == null||model.Count<1)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("收藏的商品提交错误.");
                return br;
            }

            if (model.Exists(m=>!m.id_gys.HasValue||m.id_gys.Value<1
                ||!m.id_sp.HasValue||m.id_sp.Value<1
                ||!m.id_user.HasValue||m.id_user.Value<1
                ))
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("收藏的商品提交错误.");
                return br;
            }
            Hashtable ht;
            var s = new List<string>(); 
            foreach (var p in model)
            {
                ht = new Hashtable();
                ht.Add("id_user", p.id_user);
                ht.Add("id_gys", p.id_gys);
                ht.Add("id_sp", p.id_sp);
                if (!(DAL.GetCount(typeof(Tb_Sp_Favorites), ht) > 0))
                {
                    DAL.Add<Tb_Sp_Favorites>(p);
                }
                s.Add(p.id_sp.ToString());
            }

          
            br.Success = true;
            br.Message.Add(string.Format("商品收藏成功。商品ID:{0}",string.Join(",",s)));            
            return br;
        }


        /// <summary>
        /// 取消商品收藏
        /// tim 2015-05-12
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Delete(Hashtable param)
        {

            BaseResult br = new BaseResult();
            if (param == null || param.Count == 0 || !param.ContainsKey("sp"))
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("取消收藏商品参数错误.");
                return br;
            }
            var model = (List<Tb_Sp_Favorites>)param["sp"];
            if (model == null || model.Count < 1)
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("收藏的商品提交错误.");
                return br;
            }

            if (model.Exists(m => !m.id_gys.HasValue || m.id_gys.Value < 1
                || !m.id_sp.HasValue || m.id_sp.Value < 1
                || !m.id_user.HasValue || m.id_user.Value < 1
                ))
            {
                br.Success = false;
                br.Level = ErrorLevel.Warning;
                br.Message.Add("收藏的商品提交错误.");
                return br;
            }
            Hashtable ht;
            var s = new List<string>();
            foreach (var p in model)
            {
                ht = new Hashtable();
                ht.Add("id_user", p.id_user);
                ht.Add("id_gys", p.id_gys);
                ht.Add("id_sp", p.id_sp);
                DAL.Delete(typeof(Tb_Sp_Favorites), ht);
                s.Add(p.id_sp.ToString());
            }

            br.Success = true;
            br.Message.Add(string.Format("取消商品收藏成功。商品ID:{0}", string.Join(",", s)));
            return br;          
        }


        /// <summary>
        /// 收藏商品列表
        /// tim
        /// 2015-05-12
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PageNavigate GetPage(Hashtable param)
        {
            PageNavigate pn = new PageNavigate() { TotalCount = DAL.GetCount(typeof(Tb_Sp_Favorites), param) };
            if (pn.TotalCount > 0)
            {
                var list = DAL.QueryPage<Tb_Sp_Query>(typeof(Tb_Sp_Favorites), param).ToList();                
                pn.Data = list;
            }
            else
            {
                pn.Data = new List<Tb_Sp_Query>();
            }

            pn.Success = true;
            return pn;
        }
    }
}

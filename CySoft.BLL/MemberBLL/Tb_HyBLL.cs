using CySoft.BLL.Base;
using CySoft.Frame.Attributes;
using CySoft.Frame.Core;
using CySoft.Model.Enums;
using CySoft.Model.Tb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.BLL.MemberBLL
{
    public class Tb_HyBLL : BaseBLL
    {
        #region 获取分页数据
        /// <summary>
        /// 获取分页数据
        /// lz
        /// 2016-09-18
        /// </summary>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate() { Success = true };
            var totalCount = DAL.GetCount(typeof(Tb_Hy), param);
            if (totalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tb_Hy>(typeof(Tb_Hy), param);
                pn.TotalCount = totalCount;
            }
            return pn;
        }
        #endregion



        #region 更新
        /// <summary>
        /// 更新
        /// lz
        /// 2016-09-20
        /// </summary>
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            #region 获取数据
            Hashtable param = (Hashtable)entity;
            BaseResult br = new BaseResult();
            Hashtable ht = new Hashtable();

            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("id", param["id"].ToString());

            var dbTb_Hy = DAL.GetItem<Tb_Hy>(typeof(Tb_Hy), ht);

            if (dbTb_Hy == null)
            {
                br.Success = false;
                br.Message.Add("会员信息不存在!");
                return br;
            }

            if (dbTb_Hy.password != param["old_pwd"].ToString())
            {
                br.Success = false;
                br.Message.Add("原密码不正确!");
                return br;
            }
            #endregion

            #region 更新Tb_Hy
            ht.Clear();
            ht.Add("id_masteruser", param["id_masteruser"].ToString());
            ht.Add("id", param["id"].ToString());
            ht.Add("new_password", param["new_pwd"].ToString());
            DAL.UpdatePart(typeof(Tb_Hy), ht);
            #endregion

            #region 返回
            br.Message.Add(String.Format("更新成功。"));
            br.Success = true;
            return br;
            #endregion
        }
        #endregion






    }
}

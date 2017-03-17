using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Model.Tb;
using CySoft.Frame.Attributes;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.IDAL;
using CySoft.Utility;

namespace CySoft.BLL.AdvertisBLL
{
    public class AdvertisBLL : BaseBLL
    {
        /// <summary>
        /// 添加广告内容
        /// wwc 2016-06-28
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = (Hashtable)entity;
            Tb_Advertis Advertis = new Tb_Advertis();
            Advertis.id = int.Parse(param["id"].ToString());

            if (Advertis.id == 0)
            {
                br.Data = "id";
                br.Success = false;
                br.Level = ErrorLevel.Error;
                br.Message.Add(string.Format("添加失败！标识列Id不能为空"));
                return br;
            }
            Advertis.title = param["Title"].ToString();
            Advertis.click = 0;
            Advertis.flag_type = param["flag_type"].ToString();
            Advertis.id_edit = long.Parse(param["id_edit"].ToString());
            Advertis.id_user_master = long.Parse(param["id_user_master"].ToString());
            //上传内容处理
            if (!String.IsNullOrEmpty(param["filename"].ToString()))
            {
                Advertis.filename = param["filename"].ToString();
                Advertis.filename = Advertis.filename.Replace("Temp", "Advertis");
                // 复制 原图 到 Advertis 文件夹下
                FileExtension.Copy(System.Web.HttpContext.Current.Server.MapPath(param["filename"].ToString()), System.Web.HttpContext.Current.Server.MapPath(Advertis.filename));
            }
            Advertis.info = param["info"].ToString();
            Advertis.sort = Advertis.id;
            Advertis.url = param["url"].ToString();
            Advertis.isuse = int.Parse(param["isuse"].ToString());
            Advertis.rq_edit = DateTime.Now;
            DAL.Add(Advertis);

            //获取发送数量
            //long AdvertisId = long.Parse(param["AdvertisId"].ToString());
            //param.Clear();
            //param.Add("id_Advertis", AdvertisId);
            //Advertis.sl_send = DAL.GetCount(typeof(Advertis_User), param);


            br.Success = true;
            return br;
        }

        /// <summary>
        /// 记录发送数量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Active(Hashtable param)
        {
            BaseResult br = new BaseResult();
            param.Add("new_sl_send", DAL.GetCount(typeof(Tb_Advertis), param));//获取发送数量
            param.Add("id", param["id_Advertis"]);
            param.Remove("id_Advertis");
            br.Data = DAL.UpdatePart(typeof(Tb_Advertis), param);

            br.Success = true;
            return br;
        }


        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            IList<Tb_Advertis> lst = new List<Tb_Advertis>();
            pn.TotalCount = DAL.GetCount(typeof(Tb_Advertis), param);
            if (pn.TotalCount > 0)
            {
                lst = DAL.QueryPage<Tb_Advertis>(typeof(Tb_Advertis), param);
            }
            pn.Data = lst;
            return pn;
        }

        /// <summary>
        ///  查询广告详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            Tb_Advertis Advertis = new Tb_Advertis();

            Advertis = (Tb_Advertis)DAL.GetItem(typeof(Tb_Advertis), param) ?? new Tb_Advertis();
            br.Data = Advertis;

            br.Success = true;
            return br;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = (Hashtable)entity;
            if (DAL.UpdatePart(typeof(Tb_Advertis), param) < 0)
            {
                br.Success = true;
                br.Message.Add(string.Format("更新广告信息成功"));
            }
            else
            {
                br.Success = false;
                br.Message.Add(string.Format("修改广告信息失败！"));
            }
            return br;
        }

        /// <summary>
        /// 删除广信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult br = new BaseResult();
            if (DAL.Delete(typeof(Tb_Advertis), param) == -1)
            {
                param.Add("id_adv", param["id"].ToString());
                param.Remove("id");
                if (DAL.Delete(typeof(Tb_Advertis_Log), param) == -1)
                {
                    br.Success = true;
                    br.Message.Add(string.Format("删除广告信息成功！"));
                }
                else
                {
                    br.Success = false;
                    br.Message.Add(string.Format("删除广告点击人群信息失败！"));
                }
            }
            else
            {
                br.Success = false;
                br.Message.Add(string.Format("删除广告信息失败！"));
            }
            return br;
        }

        /// <summary>
        ///  获取所有广告
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult GetAll(Hashtable param=null)
        {
            BaseResult br = new BaseResult();
            IList<Tb_Advertis> list =DAL.QueryList<Tb_Advertis>(typeof(Tb_Advertis), param)??new List<Tb_Advertis>();
            br.Success = true;
            br.Data = list;
            return br;
        }
    }
}

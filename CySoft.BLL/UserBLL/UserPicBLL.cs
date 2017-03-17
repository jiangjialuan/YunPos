using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using System.Collections;
using CySoft.IDAL;
using CySoft.Utility;
using CySoft.Model.Tb;
using CySoft.Frame.Attributes;
using System.IO;

namespace CySoft.BLL.UserBLL
{
    /// <summary>
    /// 用户图册BLL
    /// 2015-7-15
    /// wzp
    /// </summary>
    public class UserPicBLL : BaseBLL
    {
        /// <summary>
        /// 分页获取相册结果集
        /// 2015-7-16 wzp
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PageNavigate GetPage(Hashtable param = null)
        {
            PageNavigate pn = new PageNavigate();
            pn.TotalCount = DAL.GetCount(typeof(Tb_User_Pic), param);
            if (pn.TotalCount > 0)
            {
                pn.Data = DAL.QueryPage<Tb_User_Pic>(typeof(Tb_User_Pic), param) ?? new List<Tb_User_Pic>();
                pn.Success = true;
            }
            return pn;
        }

        /// <summary>
        /// 获取所有的册结果集
        /// 2015-7-16 wzp
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult GetAll(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.QueryList<Tb_User_Pic>(typeof(Tb_User_Pic), param) ?? new List<Tb_User_Pic>();
            br.Success = true;
            br.Message.Add("获取图片结果集成功！");
            return br;
        }

        /// <summary>
        /// 获取图片数量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override BaseResult GetCount(Hashtable param = null)
        {
            BaseResult br = new BaseResult();
            br.Data = DAL.GetCount(typeof(Tb_User_Pic), param);
            br.Success = true;
            return br;
        }

        /// <summary>
        /// 上传图册
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Add(dynamic entity)
        {
            Tb_User_Pic pic = (Tb_User_Pic)entity;
            Hashtable param = new Hashtable();
            param.Add("id_master", pic.id_master);
            IList<Tb_User_Pic> lst = DAL.QueryList<Tb_User_Pic>(typeof(Tb_User_Pic), param) ?? new List<Tb_User_Pic>();
            if (lst.Count > 0)
            {
                pic.xh = Convert.ToByte(lst.Max(m => m.xh) + 1);
            }
            else
            {
                pic.xh = 1;
            }
            BaseResult br = new BaseResult();
            if (pic != null)
            {
                DAL.Add(pic);
                br.Success = true;
                br.Message.Add("上传图片成功");
            }
            return br;
        }

        /// <summary>
        /// 删除图片
        /// 2015-7-16
        /// wzp
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Transaction]
        public override BaseResult Delete(Hashtable param)
        {
            BaseResult br = new BaseResult();
            //获取要删除的图片集
            IList<Tb_User_Pic> lst = DAL.QueryList<Tb_User_Pic>(typeof(Tb_User_Pic), param);
            if (lst != null && lst.Count > 0)
            {
                int flag = DAL.Delete(typeof(Tb_User_Pic), param);
                if (flag == -1)
                {
                    foreach (var pic in lst)
                    {
                        string hostUrl = System.Web.HttpContext.Current.Server.MapPath("~/");
                        FileInfo pMax;//原图
                        FileInfo pMin;//缩略图
                        //删除图片
                        try
                        {
                            pMax = new FileInfo(hostUrl + pic.photo);
                            pMin = new FileInfo(hostUrl + pic.photo_min);
                            if (pMax.Exists)
                            {
                                pMax.Delete();
                            }
                            if (pMin.Exists)
                            {
                                pMin.Delete();
                            }
                            br.Success = true;
                            br.Message.Add("删除图片成功！");
                        }
                        catch (Exception ex)
                        {
                            br.Success = false;
                            br.Message.Add("删除图片失败！");
                        }
                    }
                }
            }
            else
            {
                br.Success = false;
                br.Message.Add("要删除的图集不存在，请刷新后重试！");
            }
            return br;
        }
    }
}

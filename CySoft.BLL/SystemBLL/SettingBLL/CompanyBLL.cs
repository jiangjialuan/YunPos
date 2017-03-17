using System;
using System.Collections;
using System.Collections.Generic;
using CySoft.BLL.Base;
using CySoft.Frame.Core;
using CySoft.Model.Tb;
using CySoft.Utility;
using System.IO;
using CySoft.Frame.Attributes;

namespace CySoft.BLL.SystemBLL.SettingBLL
{
    /// <summary>
    /// 公司信息
    /// </summary>
    public class CompanyBLL : BaseBLL
    {
        /// <summary>
        /// 获得公司信息
        /// lxt
        /// 2015-03-24
        /// </summary>
        public override BaseResult Get(Hashtable param)
        {
            BaseResult br = new BaseResult();
            Tb_User_Query model = new Tb_User_Query();
            model = DAL.QueryItem<Tb_User_Query>(typeof(Tb_User), param)?? new Tb_User_Query();
            //判断是否是PC端请求数据
            if (param.ContainsKey("flag_pc"))
            {
                param["id_master"] = param["id"];
                param.Add("getTop", "");
                param.Remove("id");
                param.Remove("flag_pc");
                model.picList = DAL.QueryList<Tb_User_Pic>(typeof(Tb_User_Pic), param)??new List<Tb_User_Pic>();//获取图册结果集
            }
            br.Data = model;
            br.Success = true;
            return br;          
        }

        /// <summary>
        /// 更新
        /// lxt
        /// 2015-03-2
        /// 2015-7-15 wzp 修改
        /// </summary>
        [Transaction]
        public override BaseResult Update(dynamic entity)
        {
            BaseResult br = new BaseResult();
            Hashtable param = new Hashtable();
            Tb_User_Query model = (Tb_User_Query)entity;
            //if (!model.email.IsEmpty() && model.email != model.email_old)
            //{
            //    param.Clear();
            //    param.Add("not_id", model.id);
            //    param.Add("email", model.email);
            //    if (DAL.GetCount(typeof(Tb_User), param) > 0)
            //    {
            //        br.Success = true;
            //        br.Message.Add("此邮箱已被占用");
            //        br.Level = ErrorLevel.Warning;
            //        br.Data = "email";
            //        return br;
            //    }
            //}
            DateTime dbDateTime = DAL.GetDbDateTime();

            param.Clear();
            param.Add("id", model.id);
            param.Add("new_companyname", model.companyname);
            //param.Add("new_id_hy", model.id_hy);
            param.Add("new_id_province", model.id_province);
            param.Add("new_id_city", model.id_city);
            param.Add("new_id_county", model.id_county);
            param.Add("new_address", model.address);
            param.Add("new_zipcode", model.zipcode);
            param.Add("new_tel", model.tel);
            param.Add("new_fax", model.fax);
            //param.Add("new_location", model.location);
            //param.Add("new_pic_erwei", model.pic_erwei);
            //param.Add("new_name", model.name);
            //param.Add("new_job", model.job);
            //param.Add("new_phone", model.phone);
            //param.Add("new_qq", model.qq);
            //param.Add("new_email", model.email);
            param.Add("new_id_edit", model.id_edit);
            param.Add("new_rq_edit", dbDateTime);
            DAL.UpdatePart(typeof(Tb_User), param);
            //图片上传
            if (model.picUrl != null && model.picUrl.Count > 0)
            {
                Tb_User_Pic pic;
                int xh =0;
                //获取图册结果集
                param.Clear();
                param.Add("id_master", model.id_master);
                IList<Tb_User_Pic> lst = DAL.QueryList<Tb_User_Pic>(typeof(Tb_User_Pic), param) ?? new List<Tb_User_Pic>();
                if (lst.Count > 0)
                {
                    xh = Convert.ToInt32(lst[lst.Count - 1].xh);  //取最大的序号
                }
                foreach (var url in model.picUrl)
                {
                    //只添加新上传的图片
                    if (url.Contains("Temp"))
                    {
                        pic = new Tb_User_Pic();
                        string[] url_img = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        string guid = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(url_img[url_img.Length - 1]);//取图片后缀名
                        string fileName = guid + extension;//文件名

                        // 复制原图到 Master 文件夹下
                        FileExtension.Copy(System.Web.HttpContext.Current.Server.MapPath(url), System.Web.HttpContext.Current.Server.MapPath("/UpLoad/User/Master/" + fileName));
                        //生成缩略图至UpLoad/Master下
                        ImgExtension.MakeThumbnail(url, "/UpLoad/User/Master/_480x480_" + fileName, 480, 480, ImgCreateWay.Cut, false);
                        pic.photo = string.Format("/UpLoad/User/Master/{0}", fileName);
                        pic.photo_min = string.Format("/UpLoad/User/Master/_480x480_{0}", fileName);
                        //pic.id_create = model.id;
                        pic.id_master = model.id_master;
                        xh++;
                        pic.xh = Convert.ToByte(xh);
                        DAL.Add(pic);
                    }
                }
            }

            br.Success = true;
            br.Message.Add(String.Format("更新公司信息。信息：流水号:{0}，公司名称:{1}", model.id, model.companyname));
            return br;
        }
    }
}

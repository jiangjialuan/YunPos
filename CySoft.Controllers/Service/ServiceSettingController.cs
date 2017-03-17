using System;
using System.Collections;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Filters;
using CySoft.Frame.Core;
using CySoft.Model.Flags;
using CySoft.Model.Tb;
using CySoft.Model.Ts;
using CySoft.Utility;
using CySoft.Utility.Mvc.Html;
using CySoft.Controllers.Service.Base;
using System.Web;
using System.IO;
using CySoft.Frame.Utility;

namespace CySoft.Controllers.SystemCtl
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceSettingController : ServiceBaseController
    {

        /// <summary>
        /// 获取公司信息
        /// cxb
        /// 2015-4-2
        /// </summary>
        [HttpPost]
        public ActionResult Company()
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = new Hashtable();
                param.Add("id", GetLoginInfo<int>("id_user_master"));
                br = BusinessFactory.Company.Get(param);
            }
            catch (CySoftException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(br);
        }

        /// <summary>
        /// 根据用户id_master获取用户相册
        /// 2015-7-16
        /// wzp
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMasterPic(string obj)
        {
            BaseResult br = new BaseResult();
            Hashtable param = JSON.Deserialize<Hashtable>(obj);
            ParamVessel p = new ParamVessel();
            p.Add("id_master", (long)0,HandleType.ReturnMsg);
            param = param.Trim(p);            
            try
            {
                br = BusinessFactory.UserPic.GetAll(param);
            }
            catch (Exception)
            {

                br.Success = false;
                br.Level = ErrorLevel.Error;
                br.Message.Add("<h5>获取相册数据失败</h5>");
                br.Message.Add("");
                br.Message.Add("请重试或与管理员联系！");
            }
            return Json(br);
        }

        /// <summary>
        /// 图片上传
        /// wzp
        /// 2015-7-16
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadPic(string obj)
        {
            BaseResult br = new BaseResult();
            Hashtable param = JSON.Deserialize<Hashtable>(obj);
            ParamVessel p = new ParamVessel();
            p.Add("id_master", (long)0, HandleType.ReturnMsg);
            param = param.Trim(p);
            try
            {
                HttpPostedFileBase postedFile = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (postedFile == null || postedFile.ContentLength <= 0)
                {
                    br.Success = false;
                    br.Level = ErrorLevel.Error;
                    br.Message.Add("<h5>上传失败</h5>");
                    br.Message.Add("");
                    br.Message.Add("未发现上传的文件！");
                    return Json(br);
                }
                if (!CyVerify.IsImage(postedFile.InputStream))
                {
                    br.Success = false;
                    br.Message.Add("<h5>上传失败</h5>");
                    br.Message.Add("");
                    br.Message.Add("上传的文件格式不正确，必须图片文件!");
                    br.Level = ErrorLevel.Warning;
                    return Json(br);
                }
                Tb_User_Pic pic = new Tb_User_Pic();
                string guid = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(postedFile.FileName);
                string fileName = guid + extension;//文件名
                string url = "/UpLoad/User/Master/" + fileName;//存入数据库中的图片路径
                string fileFullName = ApplicationInfo.UserMasterPath + "\\" + fileName;
                if (!Directory.Exists(ApplicationInfo.UserMasterPath))
                {
                    Directory.CreateDirectory(ApplicationInfo.UserMasterPath);
                }
                //上传原图
                postedFile.SaveAs(fileFullName);
                //生成缩略图至UpLoad/Master下
                string minUrl = "/UpLoad/User/Master/_480x480_" + fileName;
                ImgExtension.MakeThumbnail(url, minUrl, 480, 480, ImgCreateWay.Cut, false);
                pic.id_master = long.Parse(param["id_master"].ToString());
                pic.id_create = GetLoginInfo<long>("id_user");
                pic.photo = url;
                pic.photo_min = minUrl;
                br = BusinessFactory.UserPic.Add(pic);
                br.Data = url;
                br.Success = true;
            }
            catch (CySoftException cex)
            {
                br = cex.Result;
            }
            catch (Exception)
            {
                br.Success = false;
                br.Level = ErrorLevel.Error;
                br.Message.Add("<h5>上传失败</h5>");
                br.Message.Add("");
                br.Message.Add("请重试或与管理员联系！");
            }
            return Json(br);
        }

        /// <summary>
        /// 删除图片
        /// 2015-7-16
        /// wzp
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeletePic(string obj)
        {
            BaseResult br = new BaseResult();
            Hashtable param = JSON.Deserialize<Hashtable>(obj);
            ParamVessel p = new ParamVessel();
            p.Add("xhList", null, HandleType.ReturnMsg);
            param = param.Trim(p);
            object[] o = (object[])param["xhList"];

            string[] lst = new string[o.Length];
            for (int i = 0; i < o.Length; i++)
            {
                lst[i] = o[i].ToString();
            }
            param.Add("id_master", GetLoginInfo<long>("id_user_master"));
            try
            {
                br=BusinessFactory.UserPic.Delete(param);
            }
            catch (CySoftException ex)
            {
                br.Data = ex.Result;
            }
            catch (Exception)
            {
                br.Success = false;
                br.Level = ErrorLevel.Error;
                br.Message.Add("<h5>删除失败</h5>");
                br.Message.Add("");
                br.Message.Add("请重试或与管理员联系！");
            }
            return Json(br);
        }
    }
}

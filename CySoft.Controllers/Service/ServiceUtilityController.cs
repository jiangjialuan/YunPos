using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CySoft.Controllers.Base;
using CySoft.Controllers.Service.Base;
using CySoft.Frame.Common;
using CySoft.Frame.Core;
using CySoft.Frame.Utility;
using CySoft.Utility;

namespace CySoft.Controllers.Service
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ServiceUtilityController : ServiceBaseController
    {
        /// <summary>
        /// 图片上传
        /// </summary>
        #region public ActionResult UpLoadImage()
     
        [HttpPost]
        public ActionResult UpLoadImage()
        {
            BaseResult br = new BaseResult();
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
                string extension = Path.GetExtension(postedFile.FileName);
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;
                string url = "/UpLoad/Temp/" + fileName;
                string fileFullName = ApplicationInfo.TempPath + "\\" + fileName;
                if (!Directory.Exists(ApplicationInfo.TempPath))
                {
                    Directory.CreateDirectory(ApplicationInfo.TempPath);
                }

                postedFile.SaveAs(fileFullName);
                br.Data = url;
                br.Message.Add("上传成功！");
                br.Success = true;

                //移除过期文件
                FileHelper.FilesClear(ApplicationInfo.TempPath, TimeSpan.FromDays(1));
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
            return Content(JSON.Serialize(br));
        }
        #endregion

        /// <summary>
        /// 上传文件
        /// </summary>
        #region public ActionResult UploadFile()
     
        public ActionResult UploadFile()
        {
            BaseResult br = new BaseResult();
            try
            {
                HttpPostedFileBase postedFile = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (postedFile == null)
                {
                    br.Success = false;
                    br.Level = ErrorLevel.Error;
                    br.Message.Add("<h5>上传失败</h5>");
                    br.Message.Add("");
                    br.Message.Add("未发现上传的文件！");
                    return Json(br);
                }
                string ranx = "|.html|.htm|.aspx|.php|.asp|.axd|.shtml|.shtm|.exe|.cmd|.asmx|.cs|.cshtml|.vb|.c|.dll|.sh|.config|.js|.css|";
                int pos = postedFile.FileName.LastIndexOf('.') + 1;
                string extension = Path.GetExtension(postedFile.FileName);
                if (ranx.Contains("|" + extension.ToLower() + "|"))
                {
                    br.Success = false;
                    br.Message.Add("<h5>上传失败</h5>");
                    br.Message.Add("");
                    br.Message.Add("该文件属于禁止上传类型！");
                    br.Level = ErrorLevel.Error;
                    return Content(JSON.Serialize(br));
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;
                string url = "/UpLoadt/Temp/" + fileName;
                string fileFullName = ApplicationInfo.TempPath + "\\" + fileName;
                if (!Directory.Exists(ApplicationInfo.TempPath))
                {
                    Directory.CreateDirectory(ApplicationInfo.TempPath);
                }
                postedFile.SaveAs(fileFullName);
                br.Data = url;
                br.Message.Add("上传成功！");
                br.Success = true;

                //移除过期文件
                FileHelper.FilesClear(ApplicationInfo.TempPath, TimeSpan.FromDays(1));
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
            return Content(JSON.Serialize(br));
        }
        #endregion

        /// <summary>
        /// 获取所在地
        /// tim
        /// 2015-7-7
        /// </summary>
        [HttpPost]
        public ActionResult Districts(string obj)
        {
            BaseResult br = new BaseResult();
            try
            {
                Hashtable param = JSON.Deserialize<Hashtable>(obj);
                ParamVessel p = new ParamVessel();
                p.Add("fatherId", 0, HandleType.DefaultValue);//客户端验证                
                param = param.Trim(p);
                br = BusinessFactory.Districts.GetAll(param);
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
    }
}

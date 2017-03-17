using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using CySoft.BLL.Base;
using CySoft.Frame.Common;
using CySoft.Frame.Core;
using CySoft.IBLL;
using CySoft.Model.Tb;
using CySoft.Utility;

#region 配置信息
#endregion

namespace CySoft.BLL.SystemBLL
{
    public class ConfigBLL : AbstractBaseBLL, IConfigBLL
    {
        protected static readonly string areaAbsolutePath = "/Scripts/config/cyArea.js";
        protected static readonly Type _Type = typeof(Tb_Districts);
        public BaseResult InitArea()
        {
            BaseResult result = new BaseResult();
            try
            {
                Hashtable param = new Hashtable();
                var list = DAL.QueryList<Tb_Districts>(_Type, param);
                var provinceList = (from item in list where item.fatherId==0 orderby item.sort select item).ToList();             
                string ProvinceStr="";
                string CityStr="";
                string CountyStr = "";
                StringBuilder builder = new StringBuilder();
                foreach (var item in provinceList)
                {
                    ProvinceStr = String.Format("{0}'{1}':'{2}',", ProvinceStr, StringToUnicode(item.name), item.id);
                }
                builder.AppendFormat("cy.area.province={0};", "{" + ProvinceStr.TrimEnd(',')+ "}");
                builder.AppendLine();
                builder.Append("cy.area.city={");
         
                foreach (var province in provinceList)
                {
                    var cityQuery = from item in list where item.fatherId == province.id orderby item.sort select item;
                    foreach (var item in cityQuery)
                    {
                        CityStr = CityStr + "'" + item.id + "':'" + StringToUnicode(item.name) + "',";
                    }
                    builder.Append("'" + province.id + "':{" + CityStr.TrimEnd(',') + "},");
                    CityStr = "";
                }
                builder.Remove(builder.Length - 1, 1);
                builder.AppendLine("};");

                var countyGroupQuery = from item in list where item.level == 2 select item;
                builder.Append("cy.area.county={");
                foreach (var city in countyGroupQuery)
                {
                    var countyQuery = from item in list where item.fatherId == city.id orderby item.sort select item;
                    foreach (var item in countyQuery)
                    {
                        CountyStr = CountyStr + "'" + item.id + "':'" + StringToUnicode(item.name) + "',";
                    }
                    builder.Append("'" + city.id + "'" + ":{" + CountyStr.TrimEnd(',') + "},");
                    CountyStr = "";
                }
                builder.Remove(builder.Length - 1, 1);
                builder.Append("};");
                this.Save(builder.ToString(), "~" + areaAbsolutePath);
                result.Data = areaAbsolutePath;
                result.Success = true;
            }
            catch (Exception exception)
            {
                TextLogHelper.WriterExceptionLog(exception);
                result.Success = false;
                result.Message.Clear();
                result.Message.Add("生成areaList.js配置文件出错,请联系管理员！");
                result.Level = ErrorLevel.Error;
            }
            return result;
        }

        protected void Save(string text, string fileName)
        {
            string path =  HttpContext.Current.Server.MapPath(fileName);
            string directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
            writer.WriteLine(text);
            writer.Flush();
            writer.Close();
        }

        protected string StringToUnicode(string srcText)
        {

            string dst = "";

            char[] src = srcText.ToCharArray();

            for (int i = 0; i < src.Length; i++)
            {

                byte[] bytes = Encoding.Unicode.GetBytes(src[i].ToString());

                string str = @"\u" + bytes[1].ToString("X2") + bytes[0].ToString("X2");

                dst += str;

            }

            return dst;

        }
    }
}

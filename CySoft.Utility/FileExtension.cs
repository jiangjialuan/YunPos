using System;
using System.IO;
using CySoft.Frame.Core;

namespace CySoft.Utility
{
    public class FileExtension
    {
        /// <summary>
        /// 移动文件(不会覆盖现有文件)
        /// </summary>
        /// <param name="sourceFileName">源文件全名</param>
        /// <param name="destFileName">目标文件全名</param>
        public static void Move(string sourceFileName, string destFileName)
        {
            Move(sourceFileName, destFileName, false);
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="sourceFileName">源文件全名</param>
        /// <param name="destFileName">目标文件全名</param>
        /// <param name="overwrite">是否覆盖现有文件</param>
        public static void Move(string sourceFileName, string destFileName, bool overwrite)
        {
            if (!File.Exists(sourceFileName))
            {
                throw new CySoftException("源文件" + sourceFileName + "不存在或没有足够的访问权限!");
            }
            string directoryName = Path.GetDirectoryName(destFileName);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            if (File.Exists(destFileName))
            {
                if (overwrite)
                {
                    File.Delete(destFileName);
                }
                else
                {
                    throw new CySoftException("目标文件" + destFileName + "已存在!");
                }
            }
            File.Move(sourceFileName, destFileName);
        }

        /// <summary>
        /// 复制文件(不会覆盖现有文件)
        /// </summary>
        /// <param name="sourceFileName">源文件全名</param>
        /// <param name="destFileName">目标文件全名</param>
        public static void Copy(string sourceFileName, string destFileName)
        {
            Copy(sourceFileName, destFileName, false);
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourceFileName">源文件全名</param>
        /// <param name="destFileName">目标文件全名</param>
        /// <param name="overwrite">是否覆盖现有文件</param>
        public static void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            if (!File.Exists(sourceFileName))
            {
                throw new CySoftException("源文件" + sourceFileName + "不存在或没有足够的访问权限!");
            }
            string directoryName = Path.GetDirectoryName(destFileName);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            if (!overwrite && File.Exists(destFileName))
            {
                throw new CySoftException("目标文件" + destFileName + "已存在!");
            }
            File.Copy(sourceFileName, destFileName, overwrite);
        }
        /// <summary>
        /// 删除指定目录下特定类型的文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="suffixName"></param>
        public static void DeleteSignedFile(string path, string suffixName)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            //遍历该路径下的所有文件
            foreach (FileInfo fi in di.GetFiles())
            {
                string exname = fi.Name.Substring(fi.Name.LastIndexOf(".") + 1);//得到后缀名
                //判断当前文件后缀名是否与给定后缀名一样
                if (exname == suffixName)
                {
                    File.Delete(path + "\\" + fi.Name);//删除当前文件
                }
            }
        } 
    }
}

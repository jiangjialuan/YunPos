using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml;

namespace CySoft.Utility
{
    public class ConfigHelper
    {
        private static readonly string configPath = HttpContext.Current.Server.MapPath("~/CyApp.config");

        public static bool IsExist
        {
            get
            {
                return File.Exists(configPath);
            }
        }

        private ConfigHelper() { }

        public static string GetValue(string key)
        {
            if (!IsExist)
            {
                return null;
            }

            XmlNode node = GetNode(key);
            if (node == null)
            {
                return null;
            }
            return node.Attributes["value"].Value;
        }

        public static XmlNode GetNode(string key)
        {
            if (!IsExist)
            {
                return null;
            }
            return XmlDoc.SelectSingleNode(String.Format("//settings/add[@key='{0}']", key));
        }

        public static bool ExistNode(string key, string fnode, string snode)
        {
            if (!IsExist)
            {
                return false;
            }
            XmlNode node = XmlDoc.SelectSingleNode(String.Format("//settings/add[@key='{0}']", key));
            return node == null ? false : true;
        }

        public static Dictionary<string, string> KeyValues
        {
            get
            {
                if (!IsExist)
                    return null;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                XmlNodeList nodeList = XmlDoc.GetElementsByTagName("add");
                foreach (XmlNode node in nodeList)
                {
                    dic.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                }
                return dic;
            }
        }

        public static XmlDocument XmlDoc
        {
            get
            {
                if (!IsExist)
                {
                    return null;
                }
                XmlDocument doc = new XmlDocument();
                doc.Load(configPath);
                return doc;
            }
        }
    }
}

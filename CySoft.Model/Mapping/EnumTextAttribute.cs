using System;

namespace CySoft.Model.Mapping
{
    /// <summary>
    /// 标记枚举字段，返回枚举所标记的文本
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class EnumTextAttribute : System.Attribute
    {
        private string _text;

        public EnumTextAttribute(string str_EnumText)
        {
            if (string.IsNullOrWhiteSpace(str_EnumText))
                str_EnumText = string.Empty;

            _text = str_EnumText;
        }

        public string Text
        {
            get { return _text; }
        }
    }
}

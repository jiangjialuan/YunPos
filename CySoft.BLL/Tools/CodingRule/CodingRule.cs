using System;
using CySoft.Frame.Common;
using CySoft.Frame.Core;
using CySoft.Frame.Utility;

namespace CySoft.BLL.Tools.CodingRule
{
    internal class CodingRule
    {
        private CodingRule() { }

        internal static BaseResult SetBaseCoding(object entity, Type type = null, string setPropertyName = "bm")
        {
            BaseResult br = new BaseResult();
            try
            {
                ICodingRule helper = null;
                //if (entity.HasProperty("fatherId"))
                //{
                //    helper = new TreeCodingRule();
                //}
                //else
                if (entity.HasProperty("dh"))
                {
                    helper = new BillCodingRule();
                    setPropertyName = "dh";
                }
                else
                {
                    helper = new BaseCodingRule();
                }
                br = helper.SetCoding(entity, type, setPropertyName);
            }
            catch (CySoftException ex)
            {
                br = ex.Result;
            }
            catch (Exception ex)
            {
                TextLogHelper.WriterExceptionLog(ex);

                br.Success = false;
                br.Message.Clear();
                br.Message.Add("生成编码失败，请联系管理员！");
                br.Level = ErrorLevel.Error;
            }
            return br;
        }
    }
}

#region Imports
using System;
using System.Data.SqlClient;
using Apache.Ibatis.DataMapper.Exceptions;
using CySoft.Frame.Common;
using CySoft.Frame.Core;
using Spring.Aop;
#endregion

namespace CySoft.BLL.Base.Proxy
{
    /// <summary>
    /// 异常拦截通知
    /// </summary>
    public sealed class ExceptionAdvice : IThrowsAdvice
    {
        public void AfterThrowing(CySoftException ex)
        {
            if (ex.Result != null && ex.Result.Level == ErrorLevel.Error)
            {
                TextLogHelper.WriterExceptionLog(ex);
            }
        }

        public void AfterThrowing(DataMapperException ex)
        {
            TextLogHelper.WriterExceptionLog(ex);
        }

        public void AfterThrowing(SqlException ex)
        {
            TextLogHelper.WriterExceptionLog(ex);
        }

        public void AfterThrowing(Exception ex)
        {
            TextLogHelper.WriterExceptionLog(ex);
        }
    }
}
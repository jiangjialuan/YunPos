using System;
using System.Data.SqlClient;
using AopAlliance.Intercept;
using Apache.Ibatis.DataMapper.Exceptions;
using CySoft.Frame.Core;
using CySoft.IDAL.Base;
using Spring.Context.Support;

namespace CySoft.BLL.Base.Proxy
{
    public sealed class TransactionAdvice : IMethodInterceptor
    {
        #region public IExpandDAL DAL
        private IBaseDAL _DAL;

        public IBaseDAL DAL
        {
            get
            {
                if (_DAL == null)
                {
                    _DAL = (IBaseDAL)ContextRegistry.GetContext().GetObject("BaseDAL");
                }
                return _DAL;
            }
        }
        #endregion

        public object Invoke(IMethodInvocation invocation)
        {
            object result = null;
            try
            {
                DAL.BeginTransaction();
                result = invocation.Proceed();
                DAL.CommitTransaction();
            }
            catch (CySoftException ex)
            {
                DAL.RollBackTransaction();
                throw ex;
            }
            catch (DataMapperException ex)
            {
                DAL.RollBackTransaction();
                throw ex;
            }
            catch (SqlException ex)
            {
                DAL.RollBackTransaction();
                throw ex;
            }
            catch (Exception ex)
            {
                DAL.RollBackTransaction();
                throw ex;
            }
            return result;
        }
    }
}

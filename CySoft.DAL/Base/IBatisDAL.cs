using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using Apache.Ibatis.Common.Resources;
using Apache.Ibatis.Common.Utilities;
using Apache.Ibatis.DataMapper;
using Apache.Ibatis.DataMapper.Configuration;
using Apache.Ibatis.DataMapper.Configuration.Interpreters.Config.Xml;
using Apache.Ibatis.DataMapper.Exceptions;
using Apache.Ibatis.DataMapper.Session;
using Apache.Ibatis.DataMapper.Session.Stores;

namespace CySoft.DAL.Base
{
    public abstract class IBatisDAL
    {
        protected static IDataMapper dataMapper;
        protected static ISessionFactory sessionFactory = null;
        protected ISessionStore sessionStore = null;
        private static readonly string ext = ".map.xml";
        private static readonly Regex regex = new Regex(@"(?<namespaceName>\S*)[\.]{1}(?<resourceName>\w+" + ext + ")", RegexOptions.Compiled);
        private static object mylock = new object();

        public string SessionId { get; private set; }

        public IBatisDAL()
        {
            if (dataMapper == null)
            {
                lock (mylock)
                {
                    if (dataMapper == null)
                    {
                        InitDataMapper();
                    }
                }
            }
            this.SessionId = HashCodeProvider.GetIdentityHashCode(this).ToString();
            sessionStore = SessionStoreFactory.GetSessionStore(this.SessionId);
        }

        protected void InitDataMapper()
        {
            Assembly assembly = this.GetType().Assembly;
            AssemblyName assemblyName = assembly.GetName();
            Stream stream = assembly.GetManifestResourceStream(assemblyName.Name + ".Configs.SqlMap.config");
            XmlDocument document = new XmlDocument();
            document.Load(stream);
            stream.Close();
            stream.Dispose();

            string[] manifestResourceNames = assembly.GetManifestResourceNames();
            
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            foreach (string manifestResourceName in manifestResourceNames)
            {
                if (manifestResourceName.ToLower().EndsWith(ext))
                {
                    Match m = regex.Match(manifestResourceName);

                    string namespaceName = m.Result("${namespaceName}");
                    string resourceName = m.Result("${resourceName}");
                    builder.Append(String.Format("<sqlMap uri=\"assembly://CySoft.DAL/{0}/{1}\" />", namespaceName, resourceName));
                }
            }

            document.GetElementsByTagName("sqlMaps")[0].InnerXml = builder.ToString();

            ConfigurationSetting configurationSetting = new ConfigurationSetting();
            IConfigurationEngine engine = new DefaultConfigurationEngine(configurationSetting);
            IResource resource = new StaticContentResource(document.InnerXml);
            engine.RegisterInterpreter(new XmlConfigurationInterpreter(resource));

            IMapperFactory mapperFactory = engine.BuildMapperFactory();
            sessionFactory = engine.ModelStore.SessionFactory;
            dataMapper = ((IDataMapperAccessor)mapperFactory).DataMapper;
        }

        public ISession BeginTransaction()
        {
            if (sessionStore.CurrentSession != null)
            {
                throw new DataMapperException("SqlMap could not invoke BeginTransaction(). A Transaction is already started. Call CommitTransaction() or RollbackTransaction first.");
            }
            ISession session = this.CreateSession();
            sessionStore.Store(session);
            session.BeginTransaction();
            return session;
        }

        public ISession BeginTransaction(IsolationLevel isolationLevel)
        {
            if (sessionStore.CurrentSession != null)
            {
                throw new DataMapperException("SqlMap could not invoke BeginTransaction(). A Transaction is already started. Call CommitTransaction() or RollbackTransaction first.");
            }
            ISession session = this.CreateSession();
            sessionStore.Store(session);
            session.BeginTransaction(isolationLevel);
            return session;
        }

        public void CommitTransaction()
        {
            if (this.sessionStore.CurrentSession == null)
            {
                throw new DataMapperException("SqlMap could not invoke CommitTransaction(). No Transaction was started. Call BeginTransaction() first.");
            }
            try
            {
                this.sessionStore.CurrentSession.Transaction.Commit();
                this.sessionStore.CurrentSession.Close();
            }
            finally
            {
                this.sessionStore.Dispose();
            }
        }

        public void CommitTransaction(bool autoClose)
        {
            if (this.sessionStore.CurrentSession == null)
            {
                throw new DataMapperException("SqlMap could not invoke CommitTransaction(). No Transaction was started. Call BeginTransaction() first.");
            }
            try
            {
                this.sessionStore.CurrentSession.Transaction.Commit();
                if (autoClose)
                {
                    this.sessionStore.CurrentSession.Close();
                }
            }
            finally
            {
                this.sessionStore.Dispose();
            }
        }

        public void RollBackTransaction()
        {
            if (this.sessionStore.CurrentSession == null)
            {
                throw new DataMapperException("SqlMap could not invoke RollBackTransaction(). No Transaction was started. Call BeginTransaction() first.");
            }
            try
            {
                this.sessionStore.CurrentSession.Transaction.Rollback();
                this.sessionStore.CurrentSession.Close();
            }
            finally
            {
                this.sessionStore.Dispose();
            }
        }

        internal object Insert(string statementName, object parameterObject)
        {
            return dataMapper.Insert(statementName, parameterObject);
        }

        internal int Delete(string statementName, object parameterObject)
        {
            int num = 0;
            try
            {
                num = dataMapper.Delete(statementName, parameterObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return num;
        }

        internal int Update(string statementName, object parameterObject)
        {
            int num = 0;
            try
            {
                num = dataMapper.Update(statementName, parameterObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return num;
        }

        internal IList QueryForList(string statementName, object parameterObject)
        {
            IList list;
            try
            {
                list = dataMapper.QueryForList(statementName, parameterObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        internal IList<T> QueryForList<T>(string statementName, object parameterObject)
        {
            IList<T> list;
            try
            {
                list = dataMapper.QueryForList<T>(statementName, parameterObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        internal T QueryForObject<T>(string statementName, object parameterObject)
        {
            T model;
            try
            {
                model = dataMapper.QueryForObject<T>(statementName, parameterObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        internal IDictionary QueryForDictionary(string statementName, object parameterObject, string keyProperty, string valueProperty)
        {
            IDictionary dic;
            try
            {
                dic = dataMapper.QueryForDictionary(statementName, parameterObject, keyProperty, valueProperty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dic;
        }

        internal IDictionary<TKey, TValue> QueryForDictionary<TKey, TValue>(string statementName, object parameterObject, string keyProperty, string valueProperty)
        {
            IDictionary<TKey, TValue> dic;
            try
            {
                dic = dataMapper.QueryForDictionary<TKey, TValue>(statementName, parameterObject, keyProperty, valueProperty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dic;
        }

        public ISession CreateSession()
        {
            ISession session = sessionFactory.OpenSession();
            return session;
        }
    }
}

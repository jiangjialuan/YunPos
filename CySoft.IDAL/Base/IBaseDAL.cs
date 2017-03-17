using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Apache.Ibatis.DataMapper.Session;
using CySoft.Model.Other;

namespace CySoft.IDAL.Base
{
    public interface IBaseDAL
    {
        string SessionId { get; }

        ISession CreateSession();
        ISession BeginTransaction();
        ISession BeginTransaction(IsolationLevel isolationLevel);
        void CommitTransaction();
        void CommitTransaction(bool autoClose);
        void RollBackTransaction();

        TResult GetNextKey<TResult>(Type type, IDictionary param = null, string database = null);
        int GetNextXh(Type type, IDictionary param = null, string database = null);
        IList<TResult> QueryList<TResult>(Type type, IDictionary param = null, string database = null);
        IList QueryList(Type type, IDictionary param = null, string database = null);
        IList<TResult> ExportAll<TResult>(Type type, IDictionary param = null, string database = null);
        IList<TResult> QueryPage<TResult>(Type type, IDictionary param, string database = null);
        IList QueryPage(Type type, IDictionary param, string database = null);
        IList<TResult> QueryTree<TResult>(Type type, IDictionary param = null, string database = null);
        TResult QueryItem<TResult>(Type type, IDictionary param, string database = null);
        object QueryItem(Type type, IDictionary param, string database = null);
        int GetCount(Type type, IDictionary param = null, string database = null);
        int QueryCount(Type type, IDictionary param = null, string database = null);
        TResult GetItem<TResult>(Type type, IDictionary param, string database = null);
        object GetItem(Type type, IDictionary param, string database = null);
        int UpdatePart(Type type, IDictionary param, string database = null);
        int Save(Type type, IDictionary param, string database = null);
        IList<TResult> GetTree<TResult>(Type type, IDictionary param = null, string database = null);
        TResult CopyItem<TResult>(Type type, IDictionary param, string database = null);
        IList<TResult> CopyList<TResult>(Type type, IDictionary param, string database = null);
        int Update<T>(T item, string database = null);
        int Delete(Type type, IDictionary param, string database = null);
        void Check(Type type, IDictionary param, string database = null);
        void Add<T>(T item, string database = null);
        void AddRange<T>(IList<T> list, string database = null);
        IList<T> QueryListByStatementName<T>(Type type,IDictionary param, string statementName="QueryList", string database =null);
        DateTime GetDbDateTime();
        int GetBlockedCount(string database);
        int Procedure(string procedureName, Type type, IDictionary param, string database = null);

        void ExecuteSql(string sql);
        decimal ExecuteSqlWithBack(Hashtable param);

        decimal ExecuteFunctionWithName(Hashtable param);
        

        void RunProcedure(Hashtable param);

        void RunProcedure2(Hashtable param);
        
        IList ProcedureQuery(Hashtable param);
        IList ProcedureOutQuery(Hashtable param);
    }
}

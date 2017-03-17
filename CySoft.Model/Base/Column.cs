using System;
using System.Diagnostics;
using System.Reflection;
using System.Data;

namespace CySoft.Model.Base
{
    [Serializable]
    [DebuggerDisplay("name = {name}, dbType = {dbType}, max_length = {max_length}, is_primary_key = {is_primary_key}, is_nullable = {is_nullable}, is_computed = {is_computed}")]
    public sealed class Column
    {
        private bool _update = true;
        private bool _insert = true;

        public int id { get; set; }
        public string name { get; set; }
        public PropertyInfo property { get; set; }
        public SqlDbType dbType { get; set; }
        public uint max_length { get; set; }
        public uint precision { get; set; }
        public uint scale { get; set; }
        public bool is_primary_key { get; set; }
        public bool is_nullable { get; set; }
        public bool is_computed { get; set; }
        public bool is_identity { get; set; }

        public bool update
        {
            get { return this._update; }
            set { this._update = value; }
        }

        public bool insert
        {
            get { return this._insert; }
            set { this._insert = value; }
        }
    }
}

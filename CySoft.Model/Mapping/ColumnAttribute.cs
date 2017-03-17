using System;

namespace CySoft.Model.Mapping
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ColumnAttribute : Attribute
    {
        public bool Insert { get; set; }
        public bool Update { get; set; }

        public ColumnAttribute()
        {
            this.Insert = true;
            this.Update = true;
        }

        public ColumnAttribute(bool insert, bool update)
        {
            this.Insert = insert;
            this.Update = update;
        }
    }
}

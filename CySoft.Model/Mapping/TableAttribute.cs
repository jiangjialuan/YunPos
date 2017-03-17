using System;

namespace CySoft.Model.Mapping
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TableAttribute : Attribute
    {
        private string _Name;
        private string _MapName;

        public TableAttribute(string name, string mapName)
        {
            this._Name = name;
            this._MapName = mapName;
        }

        public string Name
        {
            get
            {
                return this._Name;
            }
        }

        public string MapName
        {
            get
            {
                return this._MapName;
            }
        }
    }
}

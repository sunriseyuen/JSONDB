using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Volte.Data.Dapper
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ObjectAttribute : Attribute
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public DbType Type { get; set; } = DbType.Object;
        public bool PrimaryKey { get; set; } = false;
        public bool Indexes { get; set; } = false;
        public bool Ignore { get; set; } = false;
        public bool AutoIdentity { get; set; } = false;
        public bool Nullable { get; set; } = true;
        
        public ObjectAttribute()
        {

        }
    }
}

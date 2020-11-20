using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Volte.Data.Dapper
{
    internal class ObjectProperty
    {
        // Fields
        public List<ObjectAttribute> Property { get; set; }
        public string TableName { get; set; }
        public IDataObject DataObject { get; set; }
        // Methods
        public ObjectProperty()
        {
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Volte.Data.Dapper
{
    public abstract class DataObject : IDataObject, ICloneable
    {

        [Object(Indexes = true)]
        public string sKey { get; set; }
        [Object(Indexes = true)]
        public string sCorporation { get; set; }
        public int Version { get; set; } = 1;

        [Object(Ignore = true)]
        [JsonIgnore]
        public string Contents { get; set; }

        [Object(Ignore = true)]
        [JsonIgnore]
        public ObjectState State { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public object DeepClone()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, this);
            ms.Seek(0, SeekOrigin.Begin);
            return bf.Deserialize(ms);
        }
    }
}
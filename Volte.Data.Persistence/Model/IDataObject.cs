using System;
using System.Collections.Generic;
using System.Text;

namespace Volte.Data.Dapper
{
    public interface IDataObject
    {
        string sKey { get; set; }
        string sCorporation { get; set; }
        int Version { get; set; }
        string Contents { get; set; }
        ObjectState State { get; set; }
    }
}

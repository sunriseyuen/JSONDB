using System;
using System.Collections.Generic;
using System.Text;
using Volte.Data.Dapper;

namespace ConsoleApp2
{
    [Object(TableName = "person")]
    public class Person : DataObject
    {
        public Person()
        {
            Id = Guid.NewGuid().ToString("N");
            Age = 12;
            Name = "xxx-name";
            this.sKey = Guid.NewGuid().ToString("N");
            this.sCorporation = "corp-" + Guid.NewGuid().ToString("N");
            this.Details = new List<string>();
            this.Details.Add("111221");
            this.Details.Add("111222");
            this.Details.Add("111223");
            this.Details.Add("111224");
            this.Details.Add("111225");
            this.Phones = new List<Phone>();
            this.Phones.Add(new Phone());
            this.Phones.Add(new Phone());
            this.Phones.Add(new Phone());
            this.Phones.Add(new Phone());
            this.Phones.Add(new Phone());
            this.Phones.Add(new Phone());
            this.Phones.Add(new Phone());


        }
        public string x1x { get; set; }
        public string x2x { get; set; }
        public string x3x { get; set; }
        public List<string> Details { get; set; }
        public List<Phone> Phones { get; set; }

        public string Id { get; set; }
        public int Age { get; set; }
        [Object(Indexes = true)]
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp2
{
    public class Phone
    {
        public Phone()
        {
            this.No = "No." + Guid.NewGuid().ToString("N");
            this.Name = "Name." + Guid.NewGuid().ToString("N");
        }
        public string No { get; set; }
        public string Name { get; set; }
    }
}

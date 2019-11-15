using System;
using System.Collections.Generic;

namespace CompliXpertApp.Models
{
    public class ApplicationEntityGroup : List<Object>
    {
       public string Heading { get; set; }
        public List<Object> Entries { get; set; }
    }
}

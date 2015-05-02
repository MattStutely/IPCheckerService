using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    public class IPRange
    {
        public IPAddress StartAddress { get; set; }
        public IPAddress EndAddress { get; set; }
    }
}

using System.Net;

namespace IPChecker.Services.Business
{
    public class IPRange
    {
        public IPAddress StartAddress { get; set; }
        public IPAddress EndAddress { get; set; }
    }
}

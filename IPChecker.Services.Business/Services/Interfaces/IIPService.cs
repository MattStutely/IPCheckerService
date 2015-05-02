using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IPChecker.Services.Business.Services.Interfaces
{
    public interface IIPService
    {
        bool ValidateIP();
    }
}

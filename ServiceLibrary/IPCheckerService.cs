using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;

namespace ServiceLibrary
{
    public class IPCheckerService : IIPCheckerService
    {
        public bool ValidateIPXml()
        {
            return ValidateIP();
        }

        public bool ValidateIPJson()
        {
            return ValidateIP();
        }

        private bool ValidateIP()
        {
            try
            {
                var requestIP = IPAddress.Parse(HttpContext.Current.Request.UserHostAddress);
                NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("IPRanges");
                foreach (var entry in section)
                {
                    string ipEntry = entry.ToString();
                    int bits = Convert.ToInt32(section[ipEntry]);
                    var ip = IPAddress.Parse(ipEntry);
                    var range = GetIPRange(ip, bits);

                    if (ValidateIPAgainstRange(requestIP, range))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }   
        }

        public bool ValidateIPAgainstRange(IPAddress ipAddress, IPRange range)
        {
                List<int> adressInt = ipAddress.ToString().Split('.').Select(str => int.Parse(str)).ToList();
                List<int> lowerInt = range.StartAddress.ToString().Split('.').Select(str => int.Parse(str)).ToList();
                List<int> upperInt = range.EndAddress.ToString().Split('.').Select(str => int.Parse(str)).ToList();

                if (adressInt[0] >= lowerInt[0] && adressInt[0] < upperInt[0])
                {
                    return true;
                }
                else if (adressInt[0] >= lowerInt[0] && adressInt[0] == upperInt[0])
                {
                    if (adressInt[1] >= lowerInt[1] && adressInt[1] < upperInt[1])
                    {
                        return true;
                    }
                    else if (adressInt[1] >= lowerInt[1] && adressInt[1] == upperInt[1])
                    {
                        if (adressInt[2] >= lowerInt[2] && adressInt[2] < upperInt[2])
                        {
                            return true;
                        }
                        else if (adressInt[2] >= lowerInt[2] && adressInt[2] == upperInt[2])
                        {
                            if (adressInt[3] >= lowerInt[3] && adressInt[3] <= upperInt[3])
                            {
                                return true;
                            }
                        }

                    }
                }
           
            return false;
        }

        private IPRange GetIPRange(IPAddress ip, int bits)
        {
            uint mask = ~(uint.MaxValue >> bits);

            // Convert the IP address to bytes.
            byte[] ipBytes = ip.GetAddressBytes();

            // BitConverter gives bytes in opposite order to GetAddressBytes().
            byte[] maskBytes = BitConverter.GetBytes(mask).Reverse().ToArray();

            byte[] startIPBytes = new byte[ipBytes.Length];
            byte[] endIPBytes = new byte[ipBytes.Length];

            // Calculate the bytes of the start and end IP addresses.
            for (int i = 0; i < ipBytes.Length; i++)
            {
                startIPBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
                endIPBytes[i] = (byte)(ipBytes[i] | ~maskBytes[i]);
            }

            // Convert the bytes to IP addresses.
            return new IPRange
            {
                StartAddress = new IPAddress(startIPBytes),
                EndAddress = new IPAddress(endIPBytes)
            };
        }
    }
}

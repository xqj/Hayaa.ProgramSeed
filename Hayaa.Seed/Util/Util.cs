using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Hayaa.Seed.Util
{
   internal class Util
    {
       public static string GetMachineIps()
       {
           string hostName = Dns.GetHostName();
           System.Net.IPAddress[] addressList = Dns.GetHostAddresses(hostName);
           StringBuilder ips=new StringBuilder();
           if (addressList != null)
           {
               addressList.ToList().ForEach(a => ips.Append(a.ToString()));               
           }
           return ips.ToString();
       }
    }
}

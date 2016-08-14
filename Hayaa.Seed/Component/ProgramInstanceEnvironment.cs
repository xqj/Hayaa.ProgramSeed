using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Hayaa.Seed.Model;

namespace Hayaa.Seed.Component
{
    class ProgramInstanceEnvironment
    {
        internal InstanceEnvironmentInfo ScanEnvironment()
        {
            return new InstanceEnvironmentInfo() {
                  SystemIPs=GetSystemIP()
            };
        }

       

        private List<IPAddress> GetSystemIP()
        {
            IPHostEntry hostComputer = Dns.GetHostEntry(Dns.GetHostName());
            var list = hostComputer.AddressList;
            if (list != null)
                return list.ToList();
            else
                return null;
        }
    }
}
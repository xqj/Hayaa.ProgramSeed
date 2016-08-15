using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Hayaa.Seed
{
    public class SentinelService : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }
    }
}

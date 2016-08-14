using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Hayaa.Seed.Component
{
    class ProgramSentinel
    {
        internal void InitSentinelService()
        {
            if (HttpContext.Current==null) {
                CreateWindowsSentinel();
            } else {
                CreateWebSentinel();
            }
        }

        private void CreateWebSentinel()
        {
            throw new NotImplementedException();
        }

        private void CreateWindowsSentinel()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hayaa.Seed.Model;

namespace Hayaa.DistributedConfigService.Config
{
    class ConfigHelper:ConfigTool<DistributedConfig>
    {
        private static ConfigHelper _intance = new ConfigHelper();

        internal static ConfigHelper Intance
        {
            get
            {
                return _intance;
            }
        }
        private ConfigHelper() : base(DefineTable.ComponentID)
        {

        }
    }
}

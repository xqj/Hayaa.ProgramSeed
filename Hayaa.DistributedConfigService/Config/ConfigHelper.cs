using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hayaa.BaseComponent.DataAccess;
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
        public IDatabaseService CreateDataService(string conName)
        {
            var configDatabaseType = ConfigHelper.Intance.GetDatabaseType(conName, Hayaa.Seed.Model.EnumDatabaseType.SqlServer);
            return DataAccessService.Intance.CreateIntance((Hayaa.BaseComponent.DataAccess.Config.EnumDatabaseType)configDatabaseType);
        }
    }
}

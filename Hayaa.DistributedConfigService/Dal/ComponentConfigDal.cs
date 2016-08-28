using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hayaa.DistributedConfigService.Config;
using Hayaa.DistributedConfigService.Interface.Model;

namespace Hayaa.DistributedConfigService.Dal
{
    class ComponentConfigDal
    {
        internal static List<ComponentConfig> GetActiveComponentConfigs(Guid solutionID, int version)
        {
            var conStr = ConfigHelper.Intance.GetConnection("DistributedConfig_RW", "");
            var service = ConfigHelper.Intance.CreateDataService("DistributedConfig_RW");
            return service.GetList<ComponentConfig, object>(conStr, "select * from Rel_Solution_CompoentConfig where [SolutionID]=@SolutionID and [SolutionVersion]=@SolutionVersion and [IsActive]=1", new { SolutionID = solutionID, SolutionVersion= version });
        }
    }
}

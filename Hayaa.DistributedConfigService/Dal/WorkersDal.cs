using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hayaa.DistributedConfigService.Config;
using Hayaa.DistributedConfigService.Interface.Model;

namespace Hayaa.DistributedConfigService.Dal
{
    class WorkersDal
    {
        internal static List<ServiceWorker> GetActiveWorkers(Guid solutionID)
        {
            var conStr = ConfigHelper.Intance.GetConnection("DistributedConfig_RW", "");
            var service = ConfigHelper.Intance.CreateDataService("DistributedConfig_RW");
            return service.GetList<ServiceWorker, object>(conStr, "select * from [Rel_Solution_Doer_Component] where [SolutionID]=@SolutionID and [IsActive]=1", new { SolutionID = solutionID });
        }
    }
}

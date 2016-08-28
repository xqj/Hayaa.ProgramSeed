using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hayaa.DistributedConfigService.Dal;
using Hayaa.DistributedConfigService.Interface;
using Hayaa.DistributedConfigService.Interface.Model;

namespace Hayaa.DistributedConfigService
{
    public partial class DistributedConfigServer : IDistributedConfigService
    {
        public AppSolution GetRemoteConfig(Guid solutionID, int version)
        {
            AppSolution appsoulution = AppSolutionDal.Get(solutionID);
            if (appsoulution != null)
            {
                appsoulution.Components=ComponentConfigDal.GetActiveComponentConfigs(solutionID, version);
                appsoulution.Workers = WorkersDal.GetActiveWorkers(solutionID);
            }
            return appsoulution;
        }
    }
}

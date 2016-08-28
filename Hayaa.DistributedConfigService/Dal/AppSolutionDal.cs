﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hayaa.BaseComponent.DataAccess;
using Hayaa.BaseComponent.DataAccess.Config;
using Hayaa.DistributedConfigService.Config;
using Hayaa.DistributedConfigService.Interface.Model;

namespace Hayaa.DistributedConfigService.Dal
{
    class AppSolutionDal
    {

        internal static AppSolution Get(Guid solutionID)
        {
            var conStr = ConfigHelper.Intance.GetConnection("DistributedConfig_RW", "");
            var service = ConfigHelper.Intance.CreateDataService("DistributedConfig_RW");
            return service.GetData<AppSolution, object>(conStr, "select * from AppSolution", new { id = solutionID });
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hayaa.DistributedConfigService.Interface.Model;

namespace Hayaa.DistributedConfigService.Interface
{
   public interface IDistributedConfigService
    {
        AppSolution GetRemoteConfig(Guid solutionID, int version);
    }
}

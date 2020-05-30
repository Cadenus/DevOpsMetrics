﻿using System;

namespace DevOpsMetrics.Service.Models.Common
{
    //Inheirits from the build model, adding the DeploymentWasSuccessful flag
    public class ChangeFailureRateBuild : Build
    {
        public bool DeploymentWasSuccessful { get; set; }
    }
}

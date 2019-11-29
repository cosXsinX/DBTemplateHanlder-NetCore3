using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Studio.Deployment
{
    public class DeploymentConfiguration
    {
        public bool ForceReDeploy { get; set; }
        public string DeploymentHistoryFolderPath { get; set; }
        public string DeploymentTemplateFolderPath { get; set; }
        public string DatabaseModelsDeploymentPaths { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Studio.Deployment
{
    public class DeploymentManifestContent
    {
        public string DeploymentUser { get; set; }
        public DateTimeOffset DeploymentDate { get; set; }
    }
}

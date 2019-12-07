using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace DBTemplateHandler.Studio.Deployment
{
    public class Deployer
    {
        private string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public void Deploy(DeploymentConfiguration deploymentConfiguration)
        {
            if (deploymentConfiguration == null) throw new ArgumentNullException(nameof(deploymentConfiguration));

            if(!Directory.Exists(deploymentConfiguration.DeploymentHistoryFolderPath))
            {
                Directory.CreateDirectory(deploymentConfiguration.DeploymentHistoryFolderPath);
            }

            var deploymentManifestPath = Path.Combine(deploymentConfiguration.DeploymentHistoryFolderPath,
                    "deployment.manifest.json");

            var wasDeploymentAlreadyDone = File.Exists(deploymentManifestPath);
            if (wasDeploymentAlreadyDone && !deploymentConfiguration.ForceReDeploy) return;

            File.WriteAllText(deploymentManifestPath, JsonSerializer.Serialize(new DeploymentManifestContent()
            {
                DeploymentDate = DateTimeOffset.Now,
                DeploymentUser = Environment.UserName,
            }));


            if (!Directory.Exists(deploymentConfiguration.DeploymentTemplateFolderPath))
            {
                Directory.CreateDirectory(deploymentConfiguration.DeploymentTemplateFolderPath);
            }

            var deployedTemplateFiles = Path.Combine(AssemblyDirectory, "Deployed", "Templates");
            var deployedFiles = Directory.GetFiles(deployedTemplateFiles,"*.json");
            foreach(var deployedTemplateFile in deployedFiles)
            {
                File.Copy(deployedTemplateFile, Path.Combine(deploymentConfiguration.DeploymentTemplateFolderPath,Path.GetFileName(deployedTemplateFile)), deploymentConfiguration.ForceReDeploy);
            }

            var deployedDatabaseModelsFiles = Path.Combine(AssemblyDirectory, "Deployed", "DatabaseModels");
            var deployedModels = Directory.GetFiles(deployedDatabaseModelsFiles, "*.json");
            foreach(var deployedDatabaseModel in deployedModels)
            {
                File.Copy(deployedDatabaseModel,Path.Combine(deploymentConfiguration.DatabaseModelsDeploymentPaths,Path.GetFileName(deployedDatabaseModel)), deploymentConfiguration.ForceReDeploy);
            }

            var deployedTypeSetFiles = Path.Combine(AssemblyDirectory, "Deployed", "TypeSets");
            var deployedTypeSets = Directory.GetFiles(deployedTypeSetFiles, "*.json");
            foreach (var deployedTypeSet in deployedTypeSets)
            {
                File.Copy(deployedTypeSet, Path.Combine(deploymentConfiguration.TypeSetsDeploymentPaths, Path.GetFileName(deployedTypeSet)), deploymentConfiguration.ForceReDeploy);
            }

            var deployedTypeMappingsFiles = Path.Combine(AssemblyDirectory, "Deployed", "TypeMappings");
            var deployedTypeMappings = Directory.GetFiles(deployedTypeMappingsFiles, "*.json");
            foreach (var deployedTypeMapping in deployedTypeMappings)
            {
                File.Copy(deployedTypeMapping, Path.Combine(deploymentConfiguration.TypeMappingDeploymentPaths, Path.GetFileName(deployedTypeMapping)), deploymentConfiguration.ForceReDeploy);
            }
        }
    }
}

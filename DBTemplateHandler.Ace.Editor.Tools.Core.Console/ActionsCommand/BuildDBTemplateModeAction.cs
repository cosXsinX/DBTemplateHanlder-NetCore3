using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Ace.Editor.Tools.Core.Console.ActionsCommand
{
    public class BuildDBTemplateModeAction : IActionCommand
    {
        private const string actionName = "BuildDBTemplateMode";

        public string Description => "Create the rule set javascript file associated to the Ace editor rule set at the specified location";

        private const string DestinationArgCommandDeclaration = "-Destination";
        private const string ForceArgCommandDeclaration = "-Force";
        public string CallPattern => $"{actionName} {DestinationArgCommandDeclaration} {{DestinationFilePath}} {{optional:-Force}}";

        public string ActionName => actionName;

        private readonly HighLightRulesGenerator _highLightRulesGenerator;
        public BuildDBTemplateModeAction(HighLightRulesGenerator highLightRulesGenerator)
        {
            if (highLightRulesGenerator == null) throw new ArgumentNullException(nameof(highLightRulesGenerator));
            _highLightRulesGenerator = highLightRulesGenerator;
        }

        public void Run(string[] args)
        {
            var lowerCasedArgs = args.Select(arg => arg.ToLower()).ToList();
            string destinationPath = "";
            if(lowerCasedArgs.Contains(DestinationArgCommandDeclaration.ToLower()))
            {
                var destinationArgCommandDeclarationIndex = lowerCasedArgs.IndexOf(DestinationArgCommandDeclaration.ToLower());
                var destinationArgIndex = destinationArgCommandDeclarationIndex + 1;
                if (!(args.Length <= destinationArgIndex || ForceArgCommandDeclaration == args[destinationArgIndex]))
                {
                    destinationPath = args[destinationArgIndex];
                }
            }
            bool force = args.Contains(ForceArgCommandDeclaration);

            var fileModel = _highLightRulesGenerator.GetDbTemplateAceMode();

            string destinationFileFullName;
            if (String.IsNullOrWhiteSpace(destinationPath))
            {
                destinationFileFullName = Path.Combine(Directory.GetCurrentDirectory(),  fileModel.FileName);
            }
            else
            {
                destinationFileFullName = Path.Combine(Directory.GetCurrentDirectory(), destinationPath, fileModel.FileName);
            }
            if(File.Exists(destinationFileFullName) && force)
            {
                File.Delete(destinationFileFullName);
            }
            else if(!force)
            {
                System.Console.Error.WriteLine($"{destinationFileFullName} already exists, nothing has been done. You can use '-Force' arg in order to overwrite the existing file");
                return;
            }
            var directoryRoot = Path.GetDirectoryName(destinationFileFullName);
            if (!Directory.Exists(directoryRoot)) Directory.CreateDirectory(directoryRoot);
            File.WriteAllText(destinationFileFullName, fileModel.Content);
            System.Console.WriteLine($"{destinationFileFullName} successfully created");
        }
    }
}

using System;
using System.Collections.Generic;
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

        public void Run(string[] args)
        {
            var lowerCasedArgs = args.Select(arg => arg.ToLower()).ToList();
            string destinationPath = "mode-dbtemplate.js";
            if(lowerCasedArgs.Contains(DestinationArgCommandDeclaration.ToLower()))
            {
                var destinationArgCommandDeclarationIndex = lowerCasedArgs.IndexOf(DestinationArgCommandDeclaration.ToLower());
                var destinationArgIndex = destinationArgCommandDeclarationIndex + 1;
                if (!(args.Length <= destinationArgIndex || ForceArgCommandDeclaration == args[destinationArgIndex]))
                {
                    destinationPath = args[destinationArgIndex];
                }
            }
            
            
        }
    }
}

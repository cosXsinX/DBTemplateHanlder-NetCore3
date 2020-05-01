using DBTemplateHandler.Ace.Editor.Tools.Core.Console.ActionsCommand;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DBTemplateHandler.Ace.Editor.Tools.Core.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                System.Console.WriteLine($"Executing at path : {Directory.GetCurrentDirectory()}");
                System.Console.WriteLine($"Providden args : {string.Join(" ", args)}");
                if (args.Length < 1)
                {
                    System.Console.WriteLine("Input problem, command line request at least one argument : {action}");
                    return;
                }

                string action = args[0];
                Run(action, args);
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.ToString());
                File.WriteAllText("DBTemplateHandler.Ace.Editor.Tools.Core.Console.Error.txt", e.ToString());
                throw;
            }
        }

        private readonly static IDictionary<string, IActionCommand> commandByCommandKey = new List<IActionCommand>()
        {
            CreateBuildDBTemplateModeAction(),
        }.ToDictionary(m => m.ActionName, m => m);

        private static BuildDBTemplateModeAction CreateBuildDBTemplateModeAction()
        {
            var templateHandlerNew = TemplateHandlerBuilder.Build(null);
            TemplateContextHandlerRegister register = new TemplateContextHandlerRegister(templateHandlerNew, null);
            var highLightRulesGenerator = new HighLightRulesGenerator(register);
            return new BuildDBTemplateModeAction(highLightRulesGenerator);
        }

        private static void Run(string action, string[] args)
        {
            if(commandByCommandKey.TryGetValue(action, out var actionCommand))
            {
                actionCommand.Run(args);
                return;
            }
            System.Console.WriteLine($"The action {action} is not managed");
        }
    }
}

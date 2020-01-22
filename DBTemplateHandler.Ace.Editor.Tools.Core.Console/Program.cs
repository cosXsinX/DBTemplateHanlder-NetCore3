using DBTemplateHandler.Ace.Editor.Tools.Core.Console.ActionsCommand;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBTemplateHandler.Ace.Editor.Tools.Core.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                System.Console.WriteLine("Input problem, command line request at least one argument : {action}");
            }

            string action = args[1];
            if(commandByCommandKey.TryGetValue(action,out var actionCommand))
            {
                actionCommand.Run(args);
            };
        }

        private readonly static IDictionary<string, IActionCommand> commandByCommandKey = new List<IActionCommand>()
        {
            new BuildDBTemplateModeAction(),
        }.ToDictionary(m => m.ActionName, m => m);

        private static void ActionsCommandProxy(string action, string[] args)
        {

        }
    }
}

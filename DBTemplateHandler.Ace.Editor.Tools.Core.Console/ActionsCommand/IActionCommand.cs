using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Ace.Editor.Tools.Core.Console.ActionsCommand
{
    public interface IActionCommand
    {
        public string Description { get; }
        public string ActionName { get; }
        public string CallPattern { get; }
        public void Run(string[] args);
    }
}

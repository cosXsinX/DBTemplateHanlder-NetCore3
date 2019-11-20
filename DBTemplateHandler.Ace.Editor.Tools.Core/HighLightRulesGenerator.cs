using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Ace.Editor.Tools.Core
{
    public class HighLightRulesGenerator
    {
        private readonly InputModelHandler handler;

        public HighLightRulesGenerator(InputModelHandler handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            this.handler = handler;
        }

        public IList<FileModel> GenerateModes(
            IList<string> startContextes, 
            IList<string> endContextes,
            FileModel fileModel)
        {
            return null;
        }

        public IList<FileModel> GenerateHighlightRules(
            IList<string> startContextes,
            IList<string> endContextes,
            FileModel fileModel)
        {
            return null;
        }
    }
}

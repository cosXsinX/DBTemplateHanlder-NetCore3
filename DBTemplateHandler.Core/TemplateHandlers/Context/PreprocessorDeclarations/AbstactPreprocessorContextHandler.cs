using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.PreprocessorDeclarations
{
    public abstract class AbstactPreprocessorContextHandler : AbstractTemplateContextHandler, IPreprocessorContextHandler
    {
        
        public AbstactPreprocessorContextHandler(TemplateHandlerNew templateHandlerNew)
            : base(templateHandlerNew)
        {

        }

        public string HandleTrimedContext(string StringTrimedContext)
        {
            throw new NotImplementedException();
        }

        public string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            
        }

    }
}

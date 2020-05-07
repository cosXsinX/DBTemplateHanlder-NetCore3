using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.PreprocessorDeclarations
{
    public abstract class AbstactPreprocessorContextHandler : AbstractTemplateContextHandler, IPreprocessorContextHandler
    {
        
        public AbstactPreprocessorContextHandler(ITemplateHandler templateHandlerNew)
            : base(templateHandlerNew)
        {

        }

        public override string HandleTrimedContext(string StringTrimedContext)
        {
            var result = PrepareProcessor(StringTrimedContext);
            return result;
        }

        public override string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            return HandleTrimedContext(TrimedStringContext);
        }

        public abstract string PrepareProcessor(string TrimmedStringContext);


        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            return processContext(StringContext);
        }
    }
}

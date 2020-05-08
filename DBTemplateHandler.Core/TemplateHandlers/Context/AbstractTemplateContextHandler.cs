using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context
{
    public abstract class AbstractTemplateContextHandler : ITemplateContextHandler
    {
        
        public AbstractTemplateContextHandler(ITemplateHandler templateHandlerNew)
        {
            if (templateHandlerNew == null) throw new ArgumentNullException(nameof(templateHandlerNew));
            TemplateHandler = templateHandlerNew;
            DatabaseContextCopier = new DatabaseContextCopier(); //TODO bad architecture to be solved with IOC
        }

        protected ITemplateHandler TemplateHandler { get; }

        protected IDatabaseContextCopier DatabaseContextCopier { get; }

        public abstract string StartContext { get; }

        public abstract string EndContext { get; }

        public abstract bool isStartContextAndEndContextAnEntireWord { get; }

        public string Signature => string.Concat(StartContext, EndContext);

        public abstract string ContextActionDescription { get; }

        public string TrimContextFromContextWrapper(string stringContext)
        {

            if (!stringContext.StartsWith(StartContext, StringComparison.Ordinal))
            {
                throw new
                    Exception($"The provided {nameof(stringContext)} does not start with {StartContext}");
            }

            if (!stringContext.EndsWith(EndContext, StringComparison.Ordinal))
            {
                throw new
                    Exception($"The provided {nameof(stringContext)} does not end with {EndContext}");
            }

            string result = stringContext.Substring(
                StartContext.Length, stringContext.Length - EndContext.Length - StartContext.Length);

            return result;
        }

        public abstract string HandleTrimedContext(string StringTrimedContext, IDatabaseContext databaseContext);

        public abstract string ProcessContext(string StringContext, IDatabaseContext databaseContext);

        protected abstract void ControlContext(string StringContext, IDatabaseContext databaseContext);
    }
}
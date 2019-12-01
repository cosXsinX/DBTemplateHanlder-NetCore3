using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context
{
    public abstract class AbstractTemplateContextHandler : ITemplateContextHandler
    {
        public abstract string StartContext { get; }

        public abstract string EndContext { get; }

        public abstract bool isStartContextAndEndContextAnEntireWord { get; }

        public string Signature => string.Concat(StartContext, EndContext);

        public abstract string ContextActionDescription { get; }

        public String TrimContextFromContextWrapper(string stringContext)
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

        public abstract string processContext(string StringContext);

        public abstract string HandleTrimedContext(string StringTrimedContext);
    }
}
using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context
{
    public abstract class AbstractTemplateContextHandler : ITemplateContextHandler
    {
        public abstract string StartContext { get; }

        public abstract string EndContext { get; }

        public abstract bool isStartContextAndEndContextAnEntireWord();

        public string Signature()
        {
            return string.Concat(StartContext,EndContext);
        }

        public String TrimContextFromContextWrapper(String stringContext)
        {
		
	    if(!stringContext.StartsWith(StartContext, StringComparison.Ordinal))
	    {
		    throw new
                Exception("The provided stringContext does not start with " + StartContext);
        }
		
	    if(!stringContext.EndsWith(EndContext, StringComparison.Ordinal))
	    {
		    throw new
                Exception("The provided stringContext does not end with " + EndContext);
        }

        String result = stringContext.Substring(
            StartContext.Length, stringContext.Length - EndContext.Length - StartContext.Length);
		
	        return result;
        }
	
        public abstract String processContext(String StringContext);

        public abstract String HandleTrimedContext(String StringTrimedContext);
    }
}
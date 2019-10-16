using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context
{
    public abstract class AbstractTemplateContextHandler : ITemplateContextHandler
    {
        public abstract String getStartContextStringWrapper();

        public abstract String getEndContextStringWrapper();

        public abstract bool isStartContextAndEndContextAnEntireWord();

        public String getTemplateHandlerSignature()
        {
            return getStartContextStringWrapper() + getEndContextStringWrapper();
        }

        public String TrimContextFromContextWrapper(String stringContext)
        {
		
	    if(!stringContext.StartsWith(getStartContextStringWrapper(), StringComparison.Ordinal))
	    {
		    throw new
                Exception("The provided stringContext does not start with " + getStartContextStringWrapper());
        }
		
	    if(!stringContext.EndsWith(getEndContextStringWrapper(), StringComparison.Ordinal))
	    {
		    throw new
                Exception("The provided stringContext does not end with " + getEndContextStringWrapper());
        }

        String result = stringContext.Substring(
            getStartContextStringWrapper().Length,
                stringContext.Length - getEndContextStringWrapper().Length);
		
	        return result;
        }
	
        public abstract String processContext(String StringContext);

        public abstract String HandleTrimedContext(String StringTrimedContext);
    }
}
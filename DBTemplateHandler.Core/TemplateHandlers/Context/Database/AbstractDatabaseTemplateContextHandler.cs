using System;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Database
{
    public abstract class AbstractDatabaseTemplateContextHandler : AbstractTemplateContextHandler, IDatabaseTemplateContextHandler
    {
        public AbstractDatabaseTemplateContextHandler(ITemplateHandler templateHandlerNew):base(templateHandlerNew){}
	    
        protected override void ControlContext(string StringContext, IDatabaseContext databaseContext)
        {
            if (StringContext == null) throw new Exception($"The provided {nameof(StringContext)} is null");
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (databaseContext.Database == null)throw new Exception($"The {nameof(databaseContext.Database)} is not set");
        }

        public override string HandleTrimedContext(string StringTrimedContext,IDatabaseContext databaseContext)
        {
		    if(StringTrimedContext == null) return null;
            IDatabaseModel database = databaseContext.Database;
		    if(database == null) return StringTrimedContext;
		    return TemplateHandler.HandleTemplate(StringTrimedContext, databaseContext);
        }
    }
}

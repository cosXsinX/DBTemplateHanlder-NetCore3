﻿using System;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Database
{
    public abstract class AbstractDatabaseTemplateContextHandler : AbstractTemplateContextHandler, IDatabaseTemplateContextHandler
    {
        public IDatabaseModel DatabaseModel { get; set; }
	    
        public override string HandleTrimedContext(string StringTrimedContext)
        {
		    if(StringTrimedContext == null) return null;
            IDatabaseModel database = DatabaseModel;
		    if(database == null) return StringTrimedContext;
		    return TemplateHandlerNew.
				    HandleTemplate(StringTrimedContext, database,null, null);
        }
    }
}

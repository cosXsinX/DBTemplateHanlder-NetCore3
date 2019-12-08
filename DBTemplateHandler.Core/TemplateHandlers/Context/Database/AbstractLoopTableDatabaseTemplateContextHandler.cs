using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Database
{
    public abstract class AbstractLoopTableDatabaseTemplateContextHandler : AbstractDatabaseTemplateContextHandler
    {
        public AbstractLoopTableDatabaseTemplateContextHandler(TemplateHandlerNew templateHandlerNew) : base(templateHandlerNew)
        {

        }
    }
}

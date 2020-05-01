using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public abstract class AbstractLoopColumnTableTemplateContextHandler : AbstractTableTemplateContextHandler
    {

        public AbstractLoopColumnTableTemplateContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
    }
}

using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public abstract class AbstractLoopColumnTableTemplateContextHandler : AbstractTableTemplateContextHandler
    {

        public AbstractLoopColumnTableTemplateContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }

        protected override void ControlContext(string StringContext, IDatabaseContext databaseContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (databaseContext.Table == null) throw new Exception($"The {nameof(databaseContext.Table)} is not set");
        }
    }
}

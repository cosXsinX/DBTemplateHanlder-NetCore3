using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public abstract class AbstractLoopConstraintTableTemplateContextHandler : AbstractTableTemplateContextHandler
    {
        public AbstractLoopConstraintTableTemplateContextHandler(ITemplateHandler templateHandler): base(templateHandler) { }

        protected override void ControlContext(string StringContext, IDatabaseContext databaseContext)
        {
            if (StringContext == null) throw new Exception($"The provided {nameof(StringContext)} is null");
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (databaseContext.Table == null) throw new ArgumentNullException($"The {nameof(databaseContext.Table)} is not set");
        }
    }
}

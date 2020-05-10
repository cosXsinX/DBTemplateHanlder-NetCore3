using System;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;

namespace DBTemplateHandler.Core.TemplateHandlers.Columns
{
    public abstract class AbstractConstraintTemplateContextHandler : AbstractTemplateContextHandler, IColumnTemplateContextHandler
    {
        public AbstractConstraintTemplateContextHandler(ITemplateHandler templateHandler):base(templateHandler)
        {
        }

        public override string HandleTrimedContext(string StringTrimedContext,IDatabaseContext databaseContext)
        {
            if (StringTrimedContext == null) return null;
            IForeignKeyConstraintModel constraint = databaseContext.ForeignKeyConstraint;
            if (constraint == null) return StringTrimedContext; //TODO Strange
            return TemplateHandler.HandleTemplate(StringTrimedContext, databaseContext);
        }

        protected override void ControlContext(string StringContext,IDatabaseContext databaseContext)
        {
            if (databaseContext == null)
                throw new ArgumentNullException(nameof(databaseContext));
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            if (databaseContext.ForeignKeyConstraint == null)
                throw new Exception($"The {nameof(databaseContext.ForeignKeyConstraint)} is not set");
        }

        protected void ControlContextContent(string ContextContent)
        {
            if (isStartContextAndEndContextAnEntireWord && !ContextContent.Equals(string.Empty)) 
                throw new Exception($"No value allowed between '{StartContext}' and '{EndContext}' => got {StartContext}{ContextContent}{EndContext}");
        }
    }
}

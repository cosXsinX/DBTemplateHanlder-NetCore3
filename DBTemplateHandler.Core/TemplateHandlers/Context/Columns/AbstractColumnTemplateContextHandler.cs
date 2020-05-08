using System;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;

namespace DBTemplateHandler.Core.TemplateHandlers.Columns
{
    public abstract class AbstractColumnTemplateContextHandler : AbstractTemplateContextHandler, IColumnTemplateContextHandler
    {
        public AbstractColumnTemplateContextHandler(ITemplateHandler templateHandlerNew):base(templateHandlerNew)
        {
        }

        public override string HandleTrimedContext(string StringTrimedContext,IDatabaseContext databaseContext)
        {
            if (StringTrimedContext == null) return null;
            IColumnModel columnModel = databaseContext.Column;
            if (columnModel == null) return StringTrimedContext;
            return TemplateHandler.
                HandleTemplate(StringTrimedContext, databaseContext);
        }


        protected override void ControlContext(string StringContext, IDatabaseContext databaseContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            if (databaseContext == null)
                throw new ArgumentNullException($"{nameof(databaseContext)} the provided database context is null");
            if (databaseContext.Column == null)
                throw new ArgumentException($"{nameof(databaseContext)}.{nameof(databaseContext.Column)} is null");
        }

        protected void ControlContextContent(string ContextContent)
        {
            if (isStartContextAndEndContextAnEntireWord && !ContextContent.Equals(string.Empty)) 
                throw new Exception($"No value allowed between '{StartContext}' and '{EndContext}' => got {StartContext}{ContextContent}{EndContext}");
        }
    }
}

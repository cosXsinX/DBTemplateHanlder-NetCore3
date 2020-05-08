using System;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Functions
{
    public abstract class AbstractFunctionTemplateContextHandler : AbstractTemplateContextHandler, IFunctionTemplateContextHandler
    {
        public AbstractFunctionTemplateContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew){}

        public override string HandleTrimedContext(string StringTrimedContext, IDatabaseContext databaseContext)
        {
		    if(StringTrimedContext == null) return null;
		    return TemplateHandler.HandleTemplate(StringTrimedContext, databaseContext);
        }

        protected override void ControlContext(string StringContext, IDatabaseContext databaseContext)
        {
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (StringContext == null) throw new Exception($"The provided {nameof(StringContext)} is null");
        }
    }
}

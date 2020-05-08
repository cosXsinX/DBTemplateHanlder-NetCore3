using System;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public abstract class AbstractTableTemplateContextHandler : AbstractTemplateContextHandler, ITableTemplateContextHandler
    {
        public AbstractTableTemplateContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }

        public override string HandleTrimedContext(string StringTrimedContext, IDatabaseContext databaseContext)
        {
		    if(StringTrimedContext == null) return null;
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
		    ITableModel table = databaseContext.Table;
		    if(table == null) return StringTrimedContext;
		    IDatabaseModel database = table.ParentDatabase;
		    return TemplateHandler.
                    HandleTemplate(StringTrimedContext, database,
                            table, null,null);
	    }

        protected override void ControlContext(string StringContext, IDatabaseContext databaseContext)
        {

            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            if (databaseContext.Table == null)
                throw new Exception($"The {nameof(databaseContext.Table)} is not set");
        }
    }
}

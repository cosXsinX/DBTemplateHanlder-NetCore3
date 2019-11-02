using System;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Functions
{
    public abstract class AbstractFunctionTemplateContextHandler : AbstractTemplateContextHandler, IFunctionTemplateContextHandler
    {
        public DatabaseModel DatabaseModel { get; set; }
        public TableModel TableModel { get; set; }
        public ColumnModel ColumnModel { get; set; }


        public override string HandleTrimedContext(string StringTrimedContext)
        {
		    if(StringTrimedContext == null) return null;
		    return TemplateHandlerNew.HandleTemplate(StringTrimedContext, 
                DatabaseModel, TableModel, ColumnModel);
        }
    }
}

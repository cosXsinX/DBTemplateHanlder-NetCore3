using System;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;

namespace DBTemplateHandler.Core.TemplateHandlers.Columns
{
    public abstract class AbstractColumnTemplateContextHandler : AbstractTemplateContextHandler, IColumnTemplateContextHandler
    {
        public AbstractColumnTemplateContextHandler()
        {

        }

        public ColumnModel ColumnModel{get;set;}

        public override string HandleTrimedContext(string StringTrimedContext)
        {
            if (StringTrimedContext == null) return null;
            ColumnModel descriptionPojo = ColumnModel;
            if (descriptionPojo == null) return StringTrimedContext;
            TableModel tableDescriptionPojo = descriptionPojo.ParentTable;
            DatabaseModel databaseDescriptionPojo = null;
            if (tableDescriptionPojo != null)
                databaseDescriptionPojo = tableDescriptionPojo.ParentDatabase;
            return TemplateHandlerNew.
                HandleTemplate(StringTrimedContext, databaseDescriptionPojo,
                        tableDescriptionPojo, descriptionPojo);
        }
    }
}

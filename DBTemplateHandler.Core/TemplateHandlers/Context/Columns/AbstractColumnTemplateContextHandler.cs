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

        public IColumnModel ColumnModel{get;set;}

        public override string HandleTrimedContext(string StringTrimedContext)
        {
            if (StringTrimedContext == null) return null;
            IColumnModel columnModel = ColumnModel;
            if (columnModel == null) return StringTrimedContext;
            ITableModel tableModel = columnModel.ParentTable;
            IDatabaseModel databaseModel = null;
            if (tableModel != null)
                databaseModel = tableModel.ParentDatabase;
            return TemplateHandlerNew.
                HandleTemplate(StringTrimedContext, databaseModel,
                        tableModel, columnModel);
        }
    }
}

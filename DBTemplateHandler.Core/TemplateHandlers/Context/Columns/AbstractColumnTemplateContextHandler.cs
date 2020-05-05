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
            return TemplateHandler.
                HandleTemplate(StringTrimedContext, databaseModel,
                        tableModel, columnModel,null);
        }

        protected void ControlContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            if (ColumnModel == null)
                throw new Exception($"The {nameof(ColumnModel)} is not set");
        }

        protected void ControlContextContent(string ContextContent)
        {
            if (isStartContextAndEndContextAnEntireWord && !ContextContent.Equals(string.Empty)) 
                throw new Exception($"No value allowed between '{StartContext}' and '{EndContext}' => got {StartContext}{ContextContent}{EndContext}");
        }
    }
}

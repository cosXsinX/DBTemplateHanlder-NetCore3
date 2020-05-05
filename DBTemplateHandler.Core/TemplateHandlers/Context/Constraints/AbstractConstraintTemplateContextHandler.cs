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

        public IForeignKeyConstraintModel ForeignKeyConstraintModel{get;set;}

        public override string HandleTrimedContext(string StringTrimedContext)
        {
            if (StringTrimedContext == null) return null;
            IForeignKeyConstraintModel constraintModel = ForeignKeyConstraintModel;
            if (constraintModel == null) return StringTrimedContext;
            ITableModel tableModel = constraintModel.ParentTable;
            IDatabaseModel databaseModel = null;
            if (tableModel != null)
                databaseModel = tableModel.ParentDatabase;
            return TemplateHandler.
                HandleTemplate(StringTrimedContext, databaseModel,
                        tableModel, null,ForeignKeyConstraintModel);
        }

        protected void ControlContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            if (ForeignKeyConstraintModel == null)
                throw new Exception($"The {nameof(ForeignKeyConstraintModel)} is not set");
        }

        protected void ControlContextContent(string ContextContent)
        {
            if (isStartContextAndEndContextAnEntireWord && !ContextContent.Equals(string.Empty)) 
                throw new Exception($"No value allowed between '{StartContext}' and '{EndContext}' => got {StartContext}{ContextContent}{EndContext}");
        }
    }
}

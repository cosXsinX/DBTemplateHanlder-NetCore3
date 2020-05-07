using System;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Functions
{
    public abstract class AbstractFunctionTemplateContextHandler : AbstractTemplateContextHandler, IFunctionTemplateContextHandler
    {
        public AbstractFunctionTemplateContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew){}

        public IDatabaseModel DatabaseModel { get; set; }
        public ITableModel TableModel { get; set; }
        public IColumnModel ColumnModel { get; set; }
        public IForeignKeyConstraintModel ConstraintModel { get; set; }

        public override string HandleTrimedContext(string StringTrimedContext)
        {
		    if(StringTrimedContext == null) return null;
		    return TemplateHandler.HandleTemplate(StringTrimedContext, 
                DatabaseModel, TableModel, ColumnModel,ConstraintModel);
        }

        public string processContext(string StringContext, IDatabaseContext databaseContext)
        {
            return ProcessContext(StringContext, 
                new ProcessorDatabaseContext() {
                    Column = ColumnModel, 
                    Database = DatabaseModel, 
                    ForeignKeyConstraint = ConstraintModel, 
                    Table = TableModel });
        }
    }
}

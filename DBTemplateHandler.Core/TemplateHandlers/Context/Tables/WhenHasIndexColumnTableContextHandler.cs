using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public class WhenHasIndexColumnTableContextHandler : AbstractTableTemplateContextHandler
    {
        public WhenHasIndexColumnTableContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:CURRENT:WHEN:HAS:INDEX(";

        public override string EndContext => ")::}";

        public override bool isStartContextAndEndContextAnEntireWord => false;

        public override string ContextActionDescription => "Is replaced by the processed content value when the current table has at least one or more columns which are indexed";

        public override string processContext(string StringContext)
        {
            return ProcessContext(StringContext, new ProcessorDatabaseContext() { Table = TableModel });
        }
        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            ITableModel table = databaseContext.Table;
            if (table == null)
                throw new Exception($"The {nameof(TableModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            
            if (!(table?.Columns ?? new List<IColumnModel>()).Any(m => m.IsIndexed))
                return String.Empty;
            
            var result = TemplateHandler.
                            HandleFunctionTemplate
                                            (TrimedStringContext, TableModel.ParentDatabase,TableModel, null,null);
            return result;
        }
    }
}

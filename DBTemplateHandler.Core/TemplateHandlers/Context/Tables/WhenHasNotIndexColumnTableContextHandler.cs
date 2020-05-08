using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public class WhenHasNotIndexColumnTableContextHandler : AbstractTableTemplateContextHandler
    {
        public WhenHasNotIndexColumnTableContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:CURRENT:WHEN:HAS:NOT:INDEX(";

        public override string EndContext => ")::}";

        public override bool isStartContextAndEndContextAnEntireWord => false;

        public override string ContextActionDescription => "Is replaced by the processed content value when the current table has not one or more columns which are indexed";

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            ITableModel table = databaseContext.Table;

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            
            if ((table?.Columns ?? new List<IColumnModel>()).Any(m => m.IsIndexed))
                return string.Empty;

            var result = TemplateHandler.
                            HandleFunctionTemplate
                                            (TrimedStringContext, DatabaseContextCopier.CopyWithOverride(databaseContext, table));
            return result;
        }
    }
}

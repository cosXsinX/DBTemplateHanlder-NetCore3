using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class ColumnIndexColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public ColumnIndexColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:INDEX";
        public override string EndContext => "::}";
        public override bool isStartContextAndEndContextAnEntireWord => true;
        public int DefaultIndex => 0;
        public override string ContextActionDescription => "Is replaced by the current column index in the current table column collection iterated";
        private string DefaultAutoIndexAsString => $"{DefaultIndex}";

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            base.ControlContext(StringContext, databaseContext);
            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            base.ControlContextContent(TrimedStringContext);
            if (databaseContext.Table == null) return DefaultAutoIndexAsString;
            IColumnModel columnModel = databaseContext.Column;
            IList<IColumnModel> parentColumn = databaseContext.Table.Columns;
            if (parentColumn == null) return DefaultAutoIndexAsString;
            if (parentColumn.Any(m => columnModel == m)) return parentColumn.IndexOf(columnModel).ToString();
            return DefaultAutoIndexAsString;
        }
    }
}

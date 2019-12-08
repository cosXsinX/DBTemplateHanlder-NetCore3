using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System.Collections.Generic;
using System.Linq;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class ColumnIndexColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public ColumnIndexColumnContextHandler(TemplateHandlerNew templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:INDEX";
        public override string EndContext => "::}";
        public override bool isStartContextAndEndContextAnEntireWord => true;
        public int DefaultIndex => 0;
        public override string ContextActionDescription => "Is replaced by the current column index in the current table column collection iterated";
        private string DefaultAutoIndexAsString => $"{DefaultIndex}";
        public override string processContext(string StringContext)
        {
            base.ControlContext(StringContext);
            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            base.ControlContextContent(TrimedStringContext);
            if (ColumnModel.ParentTable == null) return DefaultAutoIndexAsString;
            IColumnModel columnModel = ColumnModel;
            IList<IColumnModel> parentColumn = columnModel.ParentTable.Columns;
            if (parentColumn == null) return DefaultAutoIndexAsString;
            if (parentColumn.Any(m => columnModel == m)) return parentColumn.IndexOf(columnModel).ToString();
            return DefaultAutoIndexAsString;
        }
    }
}

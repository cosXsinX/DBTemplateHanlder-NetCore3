using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public class ForEachPrimaryKeyColumnTableContextHandler : AbstractLoopColumnTableTemplateContextHandler
    {

        public ForEachPrimaryKeyColumnTableContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:TABLE:COLUMN:PRIMARY:FOREACH["; }
        public override string EndContext { get => "]::}"; }
        public override string ContextActionDescription => "Is replaced by the intern context as many time as there is primary key column in the table";
        public override bool isStartContextAndEndContextAnEntireWord => false;

        public override string ProcessContext(String StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            ITableModel table = databaseContext.Table;
            if (table.Columns == null)
                throw new Exception($"The {nameof(TableModel.Columns)} are not set in {nameof(TableModel)}");
            if (table.Columns.Any(m => m == null))
                throw new Exception($"There is a null reference in the {nameof(TableModel.Columns)} from {nameof(TableModel)}");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            var indexedColumns = table.Columns.Where(m => m.IsPrimaryKey);
            var eachIndexedcolumnResult = indexedColumns
                .Select(currentColumn => TemplateHandler.HandleTemplate(TrimedStringContext, DatabaseContextCopier.CopyWithOverride(databaseContext, currentColumn)));
            var result = string.Join(string.Empty, eachIndexedcolumnResult);
            return result;
        }
    }
}

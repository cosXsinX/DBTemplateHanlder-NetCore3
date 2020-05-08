using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsIndexedColumnNotAFirstIndexedColumnContextHandler : AbstractColumnTemplateContextHandler
    {

        public IsIndexedColumnNotAFirstIndexedColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:INDEXED:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN(";
        public override string EndContext => "):::}";
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the inner context when the current column is not the first column from the iterated indexed column collection";


        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            IColumnModel columnModel = databaseContext.Column;
            if (columnModel == null)
                throw new Exception($"The {nameof(ColumnModel)} is not set");
            if (databaseContext.Table == null)
                throw new Exception("The provided column has no parent table");
            IList<IColumnModel> columnList = databaseContext.Table.Columns;
            if (columnList == null || !columnList.Any())
                throw new Exception("The provided column's parent table has no column associated to");

            if (columnModel.IsIndexed == false) return string.Empty;
            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            var indexedColumn = columnList.FirstOrDefault(currentColumn => currentColumn.IsIndexed);
            if (indexedColumn == default(IColumnModel)) return string.Empty;
            if (!indexedColumn.Equals(columnModel)) return HandleTrimedContext(TrimedStringContext, databaseContext);
            return string.Empty;
        }
    }
}

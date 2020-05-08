using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsNotIndexedColumnAFirstIndexedColumnContextHandler : AbstractColumnTemplateContextHandler
    {

        public IsNotIndexedColumnAFirstIndexedColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:NOT:INDEXED:FOREACH:CURRENT:IS:FIRST:COLUMN(";
        public override string EndContext => "):::}";
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the inner context when the current column is the first column from the iterated not indexed column collection";

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            IColumnModel columnModel = databaseContext.Column;
            if (databaseContext.Table == null)
                throw new Exception("The provided column has no parent table");
            IList<IColumnModel> columnList = databaseContext.Table.Columns;
            if (columnList == null || !columnList.Any())
                throw new Exception("The provided column's parent table has no column associated to");

            if (columnModel.IsIndexed) return string.Empty;
            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            var indexedColumn = columnList.FirstOrDefault(currentColumn => !currentColumn.IsIndexed);
            if (indexedColumn == default(IColumnModel)) return string.Empty;
            if (indexedColumn.Equals(columnModel)) return HandleTrimedContext(TrimedStringContext,databaseContext);
            return string.Empty;
        }
    }
}

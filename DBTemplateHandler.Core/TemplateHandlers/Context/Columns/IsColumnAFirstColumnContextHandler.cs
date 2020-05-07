using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsColumnAFirstColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public IsColumnAFirstColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:FIRST:COLUMN(";
        public override string EndContext => "):::}";
        public override string ContextActionDescription => "Is replaced by empty value or by the inner context when the current column is the first column from the current table column collection";


        public override string processContext(string StringContext)
        {
            return ProcessContext(StringContext, new ProcessorDatabaseContext() { Column = ColumnModel });
        }

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            base.ControlContext(StringContext);
            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            var columnModel = ColumnModel;
            if (columnModel.ParentTable == null)
                throw new Exception("The provided column has no parent table");
            IList<IColumnModel> columnList = columnModel.ParentTable.Columns;
            if (columnList == null || !(columnList.Count > 0))
                throw new Exception("The provided column's parent table has no column associated to");
            if (columnModel.Equals(columnList[0]))
            {
                return HandleTrimedContext(TrimedStringContext);
            }
            else return "";
        }

        public override bool isStartContextAndEndContextAnEntireWord => false;

    }
}

using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsColumnALastColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public IsColumnALastColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by empty value or by the inner context when the current column is the last column from the current table column collection";
        public override string StartContext=> "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:LAST:COLUMN(";
        public override string EndContext => "):::}";

        public override string processContext(string StringContext)
        {
            return ProcessContext(StringContext, new ProcessorDatabaseContext() {Column = ColumnModel });
        }

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            IColumnModel columnModel = ColumnModel;
            if (columnModel == null)
                throw new Exception($"The {nameof(columnModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (columnModel.ParentTable == null)
                throw new Exception("The provided column has no parent table");
            IList<IColumnModel> columnList = columnModel.ParentTable.Columns;
            if (columnList == null || !(columnList.Count > 0))
                throw new Exception("The provided column's parent table has no column associated to");
            if (columnModel.Equals(columnList[(columnList.Count - 1)]))
            {
                return HandleTrimedContext(TrimedStringContext);
            }
            else return "";
        }
    }
}

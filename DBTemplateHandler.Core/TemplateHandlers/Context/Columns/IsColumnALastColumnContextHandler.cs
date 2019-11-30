using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsColumnALastColumnContextHandler : AbstractColumnTemplateContextHandler
    {


        public const String START_CONTEXT_WORD = "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:LAST:COLUMN(";
        public const String END_CONTEXT_WORD = "):::}";

        public override string StartContext
        {
            get => START_CONTEXT_WORD;
        }

        public override string EndContext
        {
            get => END_CONTEXT_WORD;
        }

        public override String processContext(String StringContext)
        {
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

        public override bool isStartContextAndEndContextAnEntireWord => false;
    }
}

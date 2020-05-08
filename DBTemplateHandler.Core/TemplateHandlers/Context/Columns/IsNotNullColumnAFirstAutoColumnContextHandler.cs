using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsNotNullColumnAFirstAutoColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public IsNotNullColumnAFirstAutoColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:NOT:NULL:CURRENT:IS:FIRST:COLUMN(";
        public override string EndContext  => "):::}";
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the inner context when the current column is the first column from the iterated not nullable value column collection";
        
        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            IColumnModel columnModel = databaseContext.Column;
            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (databaseContext.Table == null)
                throw new Exception("The provided column has no parent table");
            IList<IColumnModel> columnList = databaseContext.Table.Columns;
            if (columnList == null || !(columnList.Count > 0))
                throw new Exception("The provided column's parent table has no column associated to");

            foreach (IColumnModel currentColumn in columnList)
            {
                if (currentColumn.IsNotNull)
                {
                    if (currentColumn.Equals(columnModel))
                    {
                        return HandleTrimedContext(TrimedStringContext,databaseContext);
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            return "";
        }
    }
}
